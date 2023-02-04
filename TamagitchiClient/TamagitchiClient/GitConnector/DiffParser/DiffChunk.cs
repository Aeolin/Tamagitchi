using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamagitchiClient.GitConnector.DiffParser
{
  public class DiffChunk
  {
    public int BeforeLineStart { get; set; }
    public int BeforeLineCount { get; set; }
    public int AfterLineStart { get; set; }
    public int AfterLineCount { get; set; }

    public List<Line> Lines { get; init; } = new List<Line>();
  }
}
