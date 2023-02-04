using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TamagitchiClient.Engine.Positioning;

namespace TamagitchiClient.Engine.Widgets
{
  public static class WidgetExtensions
  {
    public static SpriteWidget SpriteWidget(this ContentManager manager, string name, IContainer container)
    {
      var texture = manager.Load<Texture2D>(name);
      return new SpriteWidget(texture, container);
    }

    public static TextWidget TextWidget(this ContentManager manager, string fontFace, IContainer container, Func<string> textGetter, Vector2 boxSize, Color color) 
    {
      var font = manager.Load<SpriteFont>(fontFace);
      return new TextWidget(container, textGetter, font, boxSize, color);
    }

    public static AnimatedSpriteWidget AnimatedSpriteWidget(this ContentManager manager, string name, IContainer container, float msPerFrame, FrameSizeMode mode = FrameSizeMode.Detect)
    {
      var texture = manager.Load<Texture2D>(name);
      return new AnimatedSpriteWidget(container, texture, mode, msPerFrame);
    }

    public static PercentageSpriteWidget PercentageSpriteWidget(this ContentManager manager, string name, IContainer container, Func<float> percentageGetter, FrameSizeMode mode = FrameSizeMode.Detect, int? frameSize = null) 
    {
      var texture = manager.Load<Texture2D>(name);
      return new PercentageSpriteWidget(container, texture, mode, percentageGetter, frameSize);
    }
  }
}
