using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlideSketch.Models {
  public class ItemType : TreeNode {
    public ItemType() { }
    private int _id;
    public int TypeId {
      get { return _id; }
      set {
        _id = value;
        switch (this.TypeId) {
          case (int)ItemTypeEnum.Null:
            this.ImageIndex = (int)ImageIndexEnum.Null;
            this.SelectedImageIndex = (int)ImageIndexEnum.Null;
            break;
          case (int)ItemTypeEnum.Frame:
            this.ImageIndex = (int)ImageIndexEnum.Frame;
            this.SelectedImageIndex = (int)ImageIndexEnum.Frame;
            break;
          case (int)ItemTypeEnum.Element:
            this.ImageIndex = (int)ImageIndexEnum.Element;
            this.SelectedImageIndex = (int)ImageIndexEnum.Element;
            break;
        }
      }
    }
    public int OwnerTypeId { get; set; } = 0;
    public int CatagoryTypeId { get; set; } = 0;
    public int EditorTypeId { get; set; } = 0;
    public int TypeRank { get; set; } = 0;
    public int TypeEnum { get; set; } = 0;
    public new string Name { get { return base.Text; } set { base.Text = value; } }
    public bool Visible { get; set; } = false;
    public string Desc { get; set; } = "";
    public bool Readonly { get; set; } = false;

  }
}
