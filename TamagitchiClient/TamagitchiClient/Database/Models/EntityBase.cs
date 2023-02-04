using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamagitchiClient.Database.Models
{
  public abstract class EntityBase
  {
    [Key]
    public Guid Id { get; set; }
  }
}
