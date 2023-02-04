using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamagitchiClient.GitConnector.DiffParser
{
  public class Diff
  {
    public string BeforeFile { get; set; }
    public string AfterFile { get; set; }

    public bool Deleted => AfterFile == null;
    public bool Renamed => BeforeFile != AfterFile;
    public bool Added => BeforeFile == "/dev/null";

    public List<DiffChunk> Chunks { get; init; } = new List<DiffChunk>();
  }
}
