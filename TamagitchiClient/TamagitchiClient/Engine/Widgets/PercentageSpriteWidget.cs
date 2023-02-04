using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamagitchiClient.Engine.Positioning;
using TamagitchiClient.Engine.Scaling;

namespace TamagitchiClient.Engine.Widgets
{
  public class PercentageSpriteWidget : IWidget
  {
    public IContainer Container { get; init; }
    public IPosition Position { get; set; }
    public IScaling Scale { get; set; }
    public Vector2 Size => Scale.Scale(this)*OriginalSize;
    public int FrameSize { get; init; }
    public int FrameCount { get; init; }
    public FrameSizeMode Mode { get; init; }
    public Func<float> PercentageGetter { get; init; }
    public float? MsPerPercent { get; set; }
    public Vector2 OriginalSize => Mode == FrameSizeMode.Column ? new Vector2(SpriteSheet.Width, FrameSize) : new Vector2(FrameSize, SpriteSheet.Height);
    public bool Visible { get; set; }
    public Texture2D SpriteSheet { get; init; }

    private float _lastValue;
    private TimeSpan? _lastValueTimestamp;

    public PercentageSpriteWidget(IContainer container, Texture2D texture, FrameSizeMode mode, Func<float> percentageGetter, int? frameSize = null)
    {
      FrameSize = frameSize ?? mode switch
      {
        FrameSizeMode.Column => texture.Width,
        FrameSizeMode.Row => texture.Height,
        FrameSizeMode.Detect => Math.Min(texture.Width, texture.Height),
      };

      FrameCount = mode switch
      {
        FrameSizeMode.Column => texture.Height / FrameSize,
        FrameSizeMode.Row => texture.Width / FrameSize,
        FrameSizeMode.Detect => Math.Max(texture.Height, texture.Width) / FrameSize,
      };

      if (mode == FrameSizeMode.Detect)
        mode = texture.Width > texture.Height ? FrameSizeMode.Row : FrameSizeMode.Column;

      Mode = mode;
      Container=container;
      SpriteSheet=texture;
      PercentageGetter=percentageGetter;
      _lastValue = percentageGetter();
      Visible =true;
    }

    public void Render(GameTime time, SpriteBatch batch)
    {
      var value = PercentageGetter();
      var toDisplay = value;

      if (MsPerPercent.HasValue)
      {
        if (value != _lastValue && _lastValueTimestamp.HasValue == false)
          _lastValueTimestamp = time.TotalGameTime;

        if (_lastValueTimestamp.HasValue)
        {
          var diff = (value - _lastValue);
          var progress = (float)(time.TotalGameTime - _lastValueTimestamp.Value).TotalMilliseconds / (MsPerPercent.Value * 100);
          toDisplay = diff > 0 ? Math.Min(value, _lastValue + progress) : Math.Max(value, _lastValue - progress);
          if (toDisplay == value)
          {
            _lastValue = value;
            _lastValueTimestamp = null;
          }
        }
      }

      var index = (int)(toDisplay * (FrameCount - 1));
      var pos = Position.GetPosition(this);
      var scale = Scale.Scale(this);
      var rect = Mode == FrameSizeMode.Row ? new Rectangle(index*FrameSize, 0, FrameSize, SpriteSheet.Height) : new Rectangle(0, index*FrameSize, SpriteSheet.Width, FrameSize);
      batch.Draw(SpriteSheet, pos, rect, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
    }
  }
}
