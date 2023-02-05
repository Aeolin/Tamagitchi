using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TamagitchiClient.GitConnector.DiffParser
{
  public class Parser
  {
    private static readonly Regex MatchHeader = new Regex("@@.*@@(.*)\n", RegexOptions.Compiled);
    public static List<Diff> GetDiffs(string diffString) => new Parser().ParseDiffString(diffString).ToList();
    public static Diff[] GetDiffs(string diffString, string beforeFile, string afterFile) => new Parser().ParseDiffString(diffString, beforeFile, afterFile).ToArray();

    public IEnumerable<Diff> ParseDiffString(string diffString)
    {
      var queue = new Queue<string>(diffString.Split('\n'));
      while (queue.Count > 0)
        yield return parseDiff(queue);
    }

    public IEnumerable<Diff> ParseDiffString(string diffString, string beforeFile, string afterFile)
    {
      var match = MatchHeader.Match(diffString);
      if(match.Success && match.Groups[1].Value != "")
      {
        var index = match.Groups[1].Index;
        diffString = diffString.Insert(index, "\n");
      }

      var queue = new Queue<string>(diffString.Split('\n'));
      var chunk = parseDiffChunk(queue);
      var diff = new Diff { AfterFile= afterFile, BeforeFile = beforeFile, Chunks = new List<DiffChunk> { chunk } };
      yield return diff;
    }

    private Diff parseDiff(Queue<string> queue)
    {
      var diff = new Diff();
      var before = parseBeforeMarker(queue);
      diff.BeforeFile = before;
      if (tryParseAfterMarker(queue, out var after))
      {
        diff.AfterFile = after;

      }

      return diff;
    }

    private IEnumerable<DiffChunk> parseDiffChunks(Queue<string> queue)
    {
      while (queue.Count > 0 && queue.Peek().StartsWith("@@"))
        yield return parseDiffChunk(queue);
    }

    private DiffChunk parseDiffChunk(Queue<string> queue)
    {
      var header = queue.Dequeue();
      var result = new DiffChunk();
      if (header.StartsWith("@@ ") == false)
        throw new InvalidOperationException("Invalid chunk header expected to start with @@");

      header = parseRange(header, '-', out var beforeIndex, out var beforeCount);
      result.BeforeLineStart = beforeIndex;
      result.BeforeLineCount = beforeCount;

      header = parseRange(header, '+', out var afterIndex, out var afterCount);
      result.AfterLineStart = afterIndex;
      result.AfterLineCount = afterCount;

      if (header.EndsWith("@@") == false)
        throw new InvalidOperationException("Invalid chunk header expected to end with @@");

      int index = 0;
      while (queue.Count > 0 && queue.Peek().StartsWith("--- ") == false)
        result.Lines.Add(parseLine(queue, ref index));

      return result;
    }

    private Line parseLine(Queue<string> queue, ref int index)
    {
      var line = queue.Dequeue();
      var mode = line.StartsWith("+") ? LineMode.Added : (line.StartsWith("-") ? LineMode.Removed : LineMode.Unmodified);

      return new Line(index++, mode, mode == LineMode.Unmodified ? line : line.Substring(1));
    }

    private string parseRange(string remaining, char indicator, out int index, out int lines)
    {
      var indicatorIndex = remaining.IndexOf(indicator);
      if (indicatorIndex == -1)
        throw new InvalidOperationException($"Invalid range didnt find range indicator {indicator}");

      remaining = remaining.Substring(indicatorIndex + 1);
      var spaceIndex = remaining.IndexOf(' ');
      var comaIndex = remaining.IndexOf(',');
      if (comaIndex == -1 && spaceIndex == -1)
        throw new InvalidOperationException($"Invalid range, didnt find a coma after the index");

      if (spaceIndex != -1 && (spaceIndex < comaIndex || comaIndex == -1))
      {
        var indexNum = remaining[..spaceIndex];
        index = int.Parse(indexNum);
        lines = 0;
        remaining = remaining.Substring(spaceIndex+1);
      }
      else
      {
        var indexNum = remaining[..comaIndex];
        index = int.Parse(indexNum);
        remaining = remaining.Substring(comaIndex+1);
        var endIndex = remaining.IndexOf(' ');
        if (endIndex == -1)
          throw new InvalidOperationException($"Invalid range, didnt find a space after the range");

        var lineNum = remaining[..endIndex];
        lines = int.Parse(lineNum);
        remaining = remaining.Substring(endIndex);
      }
      return remaining;
    }

    private string parseBeforeMarker(Queue<string> queue)
    {
      var line = queue.Dequeue();
      if (line.StartsWith("--- ") == false)
        throw new InvalidOperationException("Expected diff to start with --- marker");

      return line.Substring(4);
    }

    private bool tryParseAfterMarker(Queue<string> queue, out string path)
    {
      path = null;
      if (queue.Count == 0 || queue.Peek().StartsWith("--- "))
        return false;

      var line = queue.Dequeue();
      if (line.StartsWith("+++ ") == false)
        throw new InvalidOperationException("Expected diff to start with +++ marker");

      path = line.Substring(4);
      return true;
    }
  }
}
