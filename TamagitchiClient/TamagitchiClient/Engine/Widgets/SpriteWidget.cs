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
  public class SpriteWidget : IWidget
  {
    private Texture2D _sprite;
    private Vector2 _size;
    public IContainer Container { get; init; }
    public IPosition Position { get; set; }
    public IScaling Scale { get; set; } = (AbsoluteScaling)1;
    public bool Visible { get; set; }
    public float Layer { get; set; } = 0;
    public Rectangle? Clip { get; set; }
    public Vector2 Size => OriginalSize*Scale.Scale(this);
    public Vector2 OriginalSize => _size;

    public SpriteWidget(Texture2D sprite, IContainer container)
    {
      _sprite = sprite;
      Visible = true;
      _size = new Vector2(sprite.Width, sprite.Height);
      Container=container;
    }

    public void Render(GameTime time, SpriteBatch spriteBatch) 
    {
      var pos = Position.GetPosition(this);
      var scale = Scale.Scale(this);
      spriteBatch.Draw(_sprite, pos, Clip, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, Layer);
    }
  }
}