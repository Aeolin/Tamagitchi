using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamagitchiClient.Database.Models;
using TamagitchiClient.GitConnector.DiffParser;

namespace TamagitchiClient.GitConnector.Models
{
  public class CommitEvent
  {
    public TamagitchiProject Project { get; set; }
    public TamagitchiUser User { get; set; }
    public Diff[] Diffs { get; set; }
    public DateTime Timestamp { get; internal set; }

    public CommitEvent(TamagitchiProject project, TamagitchiUser user, IEnumerable<Diff> diffs)
    {
      Timestamp= DateTime.UtcNow;
      Project = project;
      User = user;
      Diffs = diffs.ToArray();
    }
  }
}
