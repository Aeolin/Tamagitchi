using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamagitchiClient.Engine.Widgets;

namespace TamagitchiClient.Engine.Positioning
{
  public class RelativeAnchoredPosition : IPosition
  {
    public float? Left { get; set; }
    public float? Top { get; set; }
    public float? Right { get; set; }
    public float? Bottom { get; set; }

    public RelativeAnchoredPosition(float? left, float? top, float? right, float? bottom)
    {
      Left=left;
      Top=top;
      Right=right;
      Bottom=bottom;
    }

    public Vector2 GetPosition(IWidget widget)
    {
      if ((Left == null && Right == null) || (Top == null && Bottom == null))
        return Vector2.Zero;

      var containerSize = widget.Container.Size;
      var size = widget.Size;
      var maxWidth = containerSize.X;
      var maxHeight = containerSize.Y;
      var x = Left.HasValue ? maxWidth * Left.Value : maxWidth - (maxWidth * Right.Value);
      var y = Top.HasValue ? maxHeight * Top.Value : maxHeight - (maxHeight * Bottom.Value);
      return new Vector2(x, y);
    }
  }
}
