using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamagitchiClient.TamagotchiLogic.Models;

namespace TamagitchiClient.Database.Models
{
  public class TamagitchiUser : EntityBase
  {
    public long GitlabId { get; set; }
    public string Name { get; set; }
  }
}
