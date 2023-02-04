using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamagitchiClient.Database.Models
{
  public class TamagitchiProject : EntityBase
  {
    public long WebhookId { get; set; } 
    public long GitlabId { get; set; }
    public string Name { get; set; }
  }
}
