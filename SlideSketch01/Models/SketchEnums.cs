using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlideSketch.Models {
  public enum ItemTypeEnum {
    Null = 0,
    Frame = 1,
    Element = 2,
    Rectangle = 3,
    Oval = 4,
    Arc = 5,
    Line = 6,
    FloodFill = 7,
    Bitmap = 8,
  }

  public enum ImageIndexEnum {
    Null = 0,
    Frame = 1,
    Element = 2,
    Rectangle = 3,
    Oval = 4,
  }
}
