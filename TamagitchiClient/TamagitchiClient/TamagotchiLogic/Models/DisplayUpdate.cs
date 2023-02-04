using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamagitchiClient.Database.Models;

namespace TamagitchiClient.TamagotchiLogic.Models
{
  public class DisplayUpdate
  {
    public string Text { get; set; }
    public TamagitchiPet Pet { get; set; }
    public string Animation { get; set; }
    public DateTime Timestamp { get; set; }
  }
}
