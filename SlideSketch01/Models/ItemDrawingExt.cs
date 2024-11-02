using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Playground.Models {
  public static class ItemDrawingExt {

    public static decimal GetAspectRatio(this string ratioString) {
      var aspectParts = ratioString.Parse(":");
      int aspectWidth = aspectParts[0].AsInt();
      decimal aspectHeight = aspectParts[1].AsDecimal();
      decimal aspectRatio = aspectWidth / aspectHeight;
      return aspectRatio;
    } 

    public static Font DefaultFont { get {
      string FontName = "Segoe UI";
      float FontSize = 9f;
      return new Font(FontName, FontSize);
    } } 
    

    public static string ToFontChunkStr(this Font font) { 
      if (font == null) return "<null>".AsBase64Encoded();
      var fontChunk = new StringBuilder();
      fontChunk.AppendLine(font.Name);
      fontChunk.AppendLine(font.Size.AsString());
      fontChunk.AppendLine(font.Bold.AsString());
      fontChunk.AppendLine(font.Italic.AsString());
      fontChunk.AppendLine(font.Underline.AsString());
      fontChunk.AppendLine(font.Strikeout.AsString());      
      return fontChunk.ToString().AsBase64Encoded();
    }
    public static Font FromFontChunkStr(this string chunkStr) { 
      var values = chunkStr.AsBase64Decoded().Parse(Environment.NewLine);
      if (values.Length != 6) { 
        return ItemDrawingExt.DefaultFont;
      }
      string FontName = values[0];
      float FontSize = values[1].AsFloat();
      bool isBold = values[2].AsBool();
      bool isItalic = values[3].AsBool();
      bool isUnderline = values[4].AsBool();
      bool isStrikeout = values[5].AsBool();
      FontStyle thisFontStyle = FontStyle.Regular;
      if (isBold) { thisFontStyle = thisFontStyle & FontStyle.Bold; }
      if (isItalic) { thisFontStyle = thisFontStyle & FontStyle.Italic; }
      if (isUnderline) { thisFontStyle = thisFontStyle & FontStyle.Underline; }
      if (isStrikeout) { thisFontStyle = thisFontStyle & FontStyle.Strikeout; }
      return new Font(FontName, FontSize, thisFontStyle);
    }
    
    public static int DrawElement(this Item node, ContainerProps props) {
      if (node == null || node.TypeId < 2 || props?.bg == null) return 0;
      Brush brushForground = new SolidBrush(node.ColorA.AsColor());
      Brush brushBackground = new SolidBrush(node.ColorB.AsColor());
      Pen itemPen = new Pen(node.ColorA.AsColor()){
        Width = node.Weight
      };
      Font workingFont = node.Font;
      SizeF wfSize = props.bg.Graphics.MeasureString("W0", workingFont);
      itemPen.Width = 3;
      var nodeType = node.TypeId;
      switch (nodeType) {
        case (int)ItemTypeEnum.Element: 
          var PrintRegion = props.FocusedRectangle(node);
          props.bg.Graphics.FillRectangle(brushBackground, PrintRegion);
          if (node.Caption.Length > 0) { 
            props.bg.Graphics.DrawString(node.Caption, workingFont, brushForground, 
              PrintRegion.Left.AsFloat(), PrintRegion.Top.AsFloat() + (PrintRegion.Height/2) - (wfSize.Height/2) );
          }
          break;
        case (int)ItemTypeEnum.Rectangle:
          var PrintRRegion = props.FocusedRectangle(node);
          props.bg.Graphics.FillRectangle(brushBackground, PrintRRegion);
          props.bg.Graphics.DrawRectangle(itemPen, PrintRRegion);
          break;
        case (int)ItemTypeEnum.Oval:
          node.DrawOval(props);          
          break;
        case (int)ItemTypeEnum.Arc:
          node.DrawArc(props);
          break;
        case (int)ItemTypeEnum.Line:
          node.DrawLine(props);
          break;
        case (int)ItemTypeEnum.FloodFill:
          node.FloodFill(props);
          break;
      }
      foreach (var child in node.Nodes) {
        Item? childItem = child as Item;
        if (childItem != null) {
          DrawElement(childItem, props);
        }
      }

      return 0;
    }

    public static void DrawOval(this Item node, ContainerProps props) {
      if (props == null || node == null || props.bg == null) return;
      var brush = new SolidBrush(node.ColorA.AsColor());
      float left = ((props.ContainerWidth.AsFloat() * node.Left.AsFloat()) / 100);
      float top = ((props.ContainerHeight.AsFloat() * node.Top.AsFloat()) / 100);
      float width = (props.ContainerWidth.AsFloat() * node.Width.AsFloat()) / 100;
      float height = (props.ContainerHeight.AsFloat() * node.Height.AsFloat()) / 100;
      float startAngle = node.AngleA.AsFloat() * 360 / 100;
      float sweepAngle = node.AngleB.AsFloat() * 360 / 100;
      props.bg.Graphics.FillEllipse(brush, left, top, width, height);      
    }

    public static void DrawArc(this Item node, ContainerProps props) { 
      if (props == null || node == null || props.bg == null) return;
      Pen pen = new Pen(props.FrameForgroundBrush, node.Weight);
      float left = ((props.ContainerWidth.AsFloat() * node.Left.AsFloat())/100);
      float top = ((props.ContainerHeight.AsFloat() * node.Top.AsFloat()) / 100);
      float width = (props.ContainerWidth.AsFloat() * node.Width.AsFloat()) / 100;
      float height = (props.ContainerHeight.AsFloat() * node.Height.AsFloat()) / 100;
      float startAngle = node.AngleA.AsFloat() * 360/100;
      float sweepAngle = node.AngleB.AsFloat() * 360/100; 
      props.bg.Graphics.DrawArc(pen, left, top, width, height, startAngle, sweepAngle );
    }

    public static void DrawLine(this Item node, ContainerProps props) {
      if (props == null || node == null || props.bg == null) return;

      Pen pen = new Pen(props.FrameForgroundBrush, node.Weight);
      float startX = (props.ContainerWidth * node.Left) / 100;
      float startY = (props.ContainerHeight * node.Top) / 100;
      float endX = (props.ContainerWidth * node.Width) / 100;
      float endY = (props.ContainerHeight * node.Height) / 100;

      props.bg.Graphics.DrawLine(pen, startX, startY, endX, endY);
    }

    public static void FloodFill(this Item node, ContainerProps props) {
      if (node == null || props?.bg == null) return;
      var colorA = node.ColorA.AsColor().ToArgb();
      var colorB = node.ColorB.AsColor().ToArgb();
      int left = ((props.ContainerWidth.AsFloat() * node.Left.AsFloat()) / 100).AsInt();
      int top = ((props.ContainerHeight.AsFloat() * node.Top.AsFloat()) / 100).AsInt() ;

      var bmp = props.ToBitmap();

      int targetColor = bmp.GetPixel(left, top).ToArgb();
      if (targetColor == colorB)
        return;

      Queue<Point> pixels = new Queue<Point>();
      pixels.Enqueue(new Point(left, top));

      // Lock bitmap data
      BitmapData data = bmp.LockBits(
          new Rectangle(0, 0, bmp.Width, bmp.Height),
          ImageLockMode.ReadWrite,
          PixelFormat.Format32bppArgb);

      int bytesPerPixel = 4;  // 32 bits per pixel (ARGB format)
      int heightInPixels = data.Height;
      int widthInBytes = data.Width * bytesPerPixel;
      byte[] pixelBuffer = new byte[data.Stride * heightInPixels];
      Marshal.Copy(data.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            

      while (pixels.Count > 0) {
        Point point = pixels.Dequeue();
        int pixelIndex = (point.Y * data.Stride) + (point.X * bytesPerPixel);

        // Bounds check
        if (point.X < 0 || point.X >= data.Width || point.Y < 0 || point.Y >= heightInPixels)
          continue;

        // Check if the current pixel color matches the target color
        int currentPixelColorArgb = BitConverter.ToInt32(pixelBuffer, pixelIndex);
        if (currentPixelColorArgb != targetColor || currentPixelColorArgb == colorA || currentPixelColorArgb == colorB)
          continue;

        // Set pixel to fill color
        byte[] fillColorBytes = BitConverter.GetBytes(colorB);
        Buffer.BlockCopy(fillColorBytes, 0, pixelBuffer, pixelIndex, bytesPerPixel);

        // Enqueue neighboring pixels
        pixels.Enqueue(new Point(point.X - 1, point.Y));
        pixels.Enqueue(new Point(point.X + 1, point.Y));
        pixels.Enqueue(new Point(point.X, point.Y - 1));
        pixels.Enqueue(new Point(point.X, point.Y + 1));
      }

      // Copy modified pixel data back to the bitmap
      Marshal.Copy(pixelBuffer, 0, data.Scan0, pixelBuffer.Length);
      bmp.UnlockBits(data);

      props.bg.Graphics.DrawImage(bmp, 0, 0);

    }
   

    public static Bitmap ToBitmap(this ContainerProps props) { 
      Bitmap bitmap = new Bitmap(props.ContainerWidth, props.ContainerHeight);
      using (Graphics g = Graphics.FromImage(bitmap)) {
        IntPtr hdc = g.GetHdc();
        try {
          props.bg?.Render(hdc);          
        } finally {
          g.ReleaseHdc(hdc);
        }
      }
      return bitmap;
    }

  }

  public class ContainerProps {
    public int ContainerWidth { get; set; } = 0;
    public int ContainerHeight { get; set; } = 0;
    public decimal ContainerAspectRatio { get; set; } = 1;
    public BufferedGraphics? bg { get; set; } = null;
    public Brush FrameForgroundBrush { get; set; }
    public Brush FrameBackgroundBrush { get; set; }
    public Item Frame { get; set; }
    public Item? FocusedItem { get; set;}
    public ContainerProps() { }
    public Rectangle DisplayRectangle() { 
      return new Rectangle(0,0,ContainerWidth, ContainerHeight);
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
