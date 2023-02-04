using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TamagitchiClient.Engine.Positioning;
using TamagitchiClient.Engine.Scaling;

namespace TamagitchiClient.Engine.Widgets
{
  public class AnimatedSpriteWidget : IWidget
  {
    public IContainer Container { get; init; }

    public IPosition Position { get; set; }
    public IScaling Scale { get; set; }
    public int FrameSize { get; init; }
    public int FrameCount { get; init; }
    public Texture2D SpriteSheet { get; init; }
    public Vector2 Size => OriginalSize*Scale.Scale(this);
    public Vector2 OriginalSize => Mode == FrameSizeMode.Column ? new Vector2(SpriteSheet.Width, FrameSize) : new Vector2(FrameSize, SpriteSheet.Height);
    public FrameSizeMode Mode { get; set; }
    public bool Visible { get; set; }
    public float MsPerFrame { get; set; }
    public float Layer { get; set; } = 0;

    public AnimatedSpriteWidget(IContainer container, Texture2D spriteSheet, FrameSizeMode mode, float msPerFrame)
    {
      FrameSize = mode switch
      {
        FrameSizeMode.Column => spriteSheet.Width,
        FrameSizeMode.Row => spriteSheet.Height,
        FrameSizeMode.Detect => Math.Min(spriteSheet.Width, spriteSheet.Height),
      };

      FrameCount = mode switch
      {
        FrameSizeMode.Column => spriteSheet.Height / FrameSize,
        FrameSizeMode.Row => spriteSheet.Width / FrameSize,
        FrameSizeMode.Detect => Math.Max(spriteSheet.Height, spriteSheet.Width) / FrameSize,
      };

      if (mode == FrameSizeMode.Detect)
        mode = spriteSheet.Width > spriteSheet.Height ? FrameSizeMode.Row : FrameSizeMode.Column;

      Mode = mode;
      Container = container;
      SpriteSheet = spriteSheet;
      MsPerFrame = msPerFrame;
      Visible = true;
    }

    public void Render(GameTime time, SpriteBatch batch)
    {
      var index = (int)(time.TotalGameTime.TotalMilliseconds / MsPerFrame) % FrameCount;
      var pos = Position.GetPosition(this);
      var scale = Scale.Scale(this);
      var rect = Mode == FrameSizeMode.Row ? new Rectangle(index*FrameSize, 0, FrameSize, FrameSize) : new Rectangle(0, index*FrameSize, FrameSize, FrameSize);
      batch.Draw(SpriteSheet, pos, rect, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, Layer);
    }
  }
}
