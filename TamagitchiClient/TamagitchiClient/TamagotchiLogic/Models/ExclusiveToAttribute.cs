using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamagitchiClient.TamagotchiLogic.Models
{
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
  public class ExclusiveToAttribute : Attribute
  {
    public HashSet<CharacterTrait> Exclusive { get; init; } = new HashSet<CharacterTrait>();
    
    public ExclusiveToAttribute(params CharacterTrait[] traits)
    {
      foreach (var trait in traits)
        Exclusive.Add(trait);
    }
  }
}
