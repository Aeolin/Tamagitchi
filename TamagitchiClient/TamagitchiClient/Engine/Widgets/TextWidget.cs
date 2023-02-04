using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using TamagitchiClient.Engine.Positioning;
using TamagitchiClient.Engine.Scaling;

namespace TamagitchiClient.Engine.Widgets
{
  public class TextWidget : IWidget
  {
    public IContainer Container { get; init; }
    public IPosition Position { get; set; }
    public IScaling Scale { get; set; }

    public Vector2 Size => OriginalSize*Scale.Scale(this);

    public Vector2 OriginalSize { get; set; }

    public bool Visible { get; set; }

    public Color Color { get; set; }

    private SpriteFont _font;

    private Func<string> _textGetter;
    private readonly List<string> _currentTextLines = new List<string>();
    private string _currentText;
    private float _maxLineHeight;

    public TextWidget(IContainer container, Func<string> textGetter, SpriteFont font, Vector2 size, Color color)
    {
      _font = font;
      Container = container;
      OriginalSize = size;
      _textGetter = textGetter;
      Visible = true;
      Color = color;
    }

    private void generateLines(string newString)
    {
      _currentText = newString;
      _currentTextLines.Clear();
      var words = newString.Split(' ', '\r', '\n');
      var currentString = string.Empty;
      foreach (var word in words)
      {
        var size = _font.MeasureString($"{currentString} {word}");
        _maxLineHeight = Math.Max(size.Y, _maxLineHeight);
        var width = size.X;
        if(width > Size.X)
        {
          _currentTextLines.Add(currentString.TrimEnd());
          currentString = word + " ";
        }
        else
        {
          currentString += word + " ";
        }
      }
      _currentTextLines.Add(currentString.TrimStart());
    }

    public void Render(GameTime time, SpriteBatch batch)
    {
      var text = _textGetter();
      if(text != _currentText) 
        generateLines(text);
      
      var pos = Position.GetPosition(this);
      foreach(var line in _currentTextLines)
      {
        batch.DrawString(_font, line, pos, Color);
        pos.Y += _maxLineHeight + 1;
      }
    }
  }
}
