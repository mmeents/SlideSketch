using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
      if (node == null || node.TypeId < 2) return 0;
      Brush brushForground = new SolidBrush(node.ColorA.AsColor());
      Brush brushBackground = new SolidBrush(node.ColorB.AsColor());
      Pen itemPen = new Pen(node.ColorA.AsColor());
      Font workingFont = node.Font;
      SizeF wfSize = props.bg.Graphics.MeasureString("W0", workingFont);
      itemPen.Width = 3;
      var nodeType = node.TypeId;
      switch (nodeType) {
        case (int)TnType.Element: 
          var PrintRegion = props.FocusedRectangle(node);
          props.bg.Graphics.FillRectangle(brushBackground, PrintRegion);
          if (node.Caption.Length > 0) { 
            props.bg.Graphics.DrawString(node.Caption, workingFont, brushForground, PrintRegion.Left.AsFloat(), PrintRegion.Top.AsFloat() + (PrintRegion.Height/2) - (wfSize.Height/2) );
          }
          break;
        case (int)TnType.Rectangle:
          var PrintRRegion = props.FocusedRectangle(node);
          props.bg.Graphics.FillRectangle(brushBackground, PrintRRegion);
          props.bg.Graphics.DrawRectangle(itemPen, PrintRRegion);
          break;
        case (int)TnType.Oval:
          var PrintORegion = props.FocusedRectangle(node);
          props.bg.Graphics.FillEllipse(brushBackground, PrintORegion);
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
