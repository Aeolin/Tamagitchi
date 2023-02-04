using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamagitchiClient.GitConnector.DiffParser
{
  public class Line
  {
    public int Index { get; init; }
    public LineMode Mode { get; init; }
    public string Content { get; init; }

    public Line(int index, LineMode mode, string content)
    {
      Index=index;
      Mode=mode;
      Content=content;
    }
  }
}
