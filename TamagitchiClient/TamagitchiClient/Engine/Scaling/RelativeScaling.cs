using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamagitchiClient.Engine.Widgets;

namespace TamagitchiClient.Engine.Scaling
{
  public class RelativeScaling : IScaling
  {
    public float Percentage { get; set; }
    public RelativeScaling(float percentage)
    {
      Percentage=percentage;
    }

    public float Scale(IWidget widget)      
    {
      var size = widget.OriginalSize;
      var containerSize = widget.Container.Size;
      var min = Math.Min(containerSize.X/size.X, containerSize.Y/size.Y)*Percentage;
      return min;
    }
  }
}
