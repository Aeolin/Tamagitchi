using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TamagitchiClient.Engine.Positioning;
using TamagitchiClient.Engine.Scaling;

namespace TamagitchiClient.Engine.Widgets
{
  public class SceneBuilder
  {
    private readonly ContentManager _content;
    private readonly Stack<IContainer> _containers = new Stack<IContainer>();
    public IWidget CurrentWidget { get; private set; }
    private readonly Scene _scene;

    public static SceneBuilder New(ContentManager content, GraphicsDevice graphics) => new SceneBuilder(content, graphics);

    private SceneBuilder(ContentManager content, GraphicsDevice graphics)
    {
      _content = content;
      _scene = new Scene(graphics);
    }

    public Scene Build() => _scene;

    public SceneBuilder PushContainer(IContainer container)
    {
      _containers.Push(container);
      return this;
    }

    public SceneBuilder PushBaseContainer(GraphicsDeviceManager graphics) => PushContainer(new BaseContainer(graphics));

    public SceneBuilder PopContainer()
    {
      _containers.Pop();
      return this;
    }

    public SceneBuilder WithSprite(string name)
    {
      var container = _containers.Peek();
      var texture = _content.Load<Texture2D>(name);
      CurrentWidget = new SpriteWidget(texture, container);
      _scene.Add(CurrentWidget);
      return this;
    }

    public SceneBuilder WithTextSprite(string fontFace, Func<string> textGetter, Vector2 size, Color? color = null)
    {
      var container = _containers.Peek();
      CurrentWidget = _content.TextWidget(fontFace, container, textGetter, size, color ?? Color.Black);
      _scene.Add(CurrentWidget);
      return this;
    }

    public SceneBuilder WithAnimatedSprite(string name, float msPerFrame, FrameSizeMode mode = FrameSizeMode.Detect)
    {
      var container = _containers.Peek();
      CurrentWidget = _content.AnimatedSpriteWidget(name, container, msPerFrame, mode);
      _scene.Add(CurrentWidget);
      return this;
    }

    public SceneBuilder WithPercentageSprite(string name, Func<float> percentageGetter, float? msPerFrame = null, FrameSizeMode mode = FrameSizeMode.Detect, int? frameSize = null)
    {
      var container = _containers.Peek();
      var widget = _content.PercentageSpriteWidget(name, container, percentageGetter, mode, frameSize);
      widget.MsPerPercent = msPerFrame;
      CurrentWidget= widget;
      _scene.Add(CurrentWidget);
      return this;
    }

    public SceneBuilder WithRelativePosition(VerticalAlignment vertical, HorizontalAlignment horizontal) => WithPosition(new RelativePosition(vertical, horizontal));
    public SceneBuilder WithRelativePosition(float? left, float? top, float? right, float? bottom) => WithPosition(new RelativeAnchoredPosition(left, top, right, bottom));
    public SceneBuilder WithRelativeTo(IWidget relative, Direction direction) => WithPosition(new RelativeToWidgetPosition(relative, direction));

    public SceneBuilder WithPosition(IPosition position)
    {
      CurrentWidget.Position = position;
      return this;
    }

    public SceneBuilder WithScaling(IScaling scaling)
    {
      CurrentWidget.Scale = scaling;
      return this;
    }

    public SceneBuilder WithScaling(float scaling) => WithScaling((AbsoluteScaling)scaling);
    public SceneBuilder WithRelativeScaling(float scaling) => WithScaling(new RelativeScaling(scaling));
  }
}
