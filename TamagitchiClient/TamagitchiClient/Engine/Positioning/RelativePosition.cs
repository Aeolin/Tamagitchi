using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using TamagitchiClient.Engine.Widgets;

namespace TamagitchiClient.Engine.Positioning
{
  public class RelativePosition : IPosition
  {
    public VerticalAlignment VerticalAlignment { get; set; } 
    public HorizontalAlignment HorizontalAlignment { get; set; } 


    public RelativePosition() : this(VerticalAlignment.Center, HorizontalAlignment.Center)
    {
    }

    public RelativePosition(VerticalAlignment verticalAlignment, HorizontalAlignment horizontalAlignment)
    {
      VerticalAlignment=verticalAlignment;
      HorizontalAlignment=horizontalAlignment;
    }



    public Vector2 GetPosition(IWidget widget)
    {
      var space = widget.Container.Size;
      var size = widget.Size;
      var yPos = VerticalAlignment switch
      {
        VerticalAlignment.Top => 0,
        VerticalAlignment.Center => (space.Y / 2) - (size.Y / 2),
        VerticalAlignment.Bottom => space.Y - size.Y,
        _ => 0
      };

      var xPos = HorizontalAlignment switch
      {
        HorizontalAlignment.Left => 0,
        HorizontalAlignment.Center => (space.X / 2) - (size.X / 2),
        HorizontalAlignment.Right => space.X - size.X,
        _ => 0
      };

      return new Vector2(xPos, yPos);
    }
  }
}
