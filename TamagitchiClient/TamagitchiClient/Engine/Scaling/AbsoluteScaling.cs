using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamagitchiClient.Engine.Widgets;

namespace TamagitchiClient.Engine.Scaling
{
  public class AbsoluteScaling : IScaling
  {
    public float Factor { get; set; }
    public AbsoluteScaling(float factor = 1F)
    {
      this.Factor = factor;
    }

    public float Scale(IWidget widget) => Factor;

    public static implicit operator AbsoluteScaling(float factor) => new AbsoluteScaling(factor);
  }
}
