using SlideSketch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlideSketch.Models {
  public class SurfaceProps {
    public int ContainerWidth { get; set; } = 0;
    public int ContainerHeight { get; set; } = 0;
    public decimal ContainerAspectRatio { get; set; } = 1;
    public BufferedGraphics? bg { get; set; } = null;
    public Brush FrameForgroundBrush { get; set; }
    public Brush FrameBackgroundBrush { get; set; }
    public Brush ItemBrushA { get; set; }
    public Brush ItemBrushB { get; set; }
    public Item Frame { get; set; }
    public Item? FocusedItem { get; set; }
    public SurfaceProps() { }
    public Rectangle DisplayRectangle() {
      return new Rectangle(0, 0, ContainerWidth, ContainerHeight);
    }
    public Rectangle FrameRectangle() {
      decimal designSize = Frame.Width / (100).AsDecimal();
      decimal frameWidth = (ContainerWidth * designSize);
      decimal frameHeight = (frameWidth * ContainerAspectRatio);
      return new Rectangle(0, 0, frameWidth.AsInt(), frameHeight.AsInt());
    }
    public Point FocusedTopLeft(Item item) {
      Rectangle bounds = FrameRectangle();
      decimal designTop = item.Top / (100).AsDecimal() * bounds.Height;
      decimal designLeft = item.Left / (100).AsDecimal() * bounds.Width;
      var aPoint = new Point(designLeft.AsInt(), designTop.AsInt());
      return aPoint;
    }
    public Rectangle FocusedRectangle(Item item) {
      Rectangle bounds = FrameRectangle();
      decimal designTop = item.Top / (100).AsDecimal() * bounds.Height;
      decimal designLeft = item.Left / (100).AsDecimal() * bounds.Width;
      decimal designHeight = item.Height / (100).AsDecimal() * bounds.Height;
      decimal designWidth = item.Width / (100).AsDecimal() * bounds.Width;
      var aRect = new Rectangle(designLeft.AsInt(), designTop.AsInt(), designWidth.AsInt(), designHeight.AsInt());
      return aRect;
    }

  }
}
