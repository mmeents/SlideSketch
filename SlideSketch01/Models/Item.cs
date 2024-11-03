using System;
using System.Collections.Generic;

namespace SlideSketch.Models {

  public class Item : TreeNode {
    private int _TypeId = 0;
    private int _ItemRank = 0;
    private int _Left = 0;
    private int _Top = 0;
    private int _Width = 0;
    private int _Height = 0;
    private int _AngleA = 0;
    private int _AngleB = 0;
    private int _Weight = 0;

    private string _ColorA = "";
    private string _ColorB = "";
    private string _Font = "";
    private string _Caption = "";

    public bool Dirty = false;
    public Item() : base() { }
    public int Id { get; set; } = 0;
    public int OwnerId { get; set; } = 0;
    public int TypeId {
      get { return _TypeId; }
      set {
        Dirty = true;
        _TypeId = value;
        switch (this.TypeId) {
          case (int)ItemTypeEnum.Null:
            this.ImageIndex = (int)ImageIndexEnum.Null;
            this.SelectedImageIndex = (int)ImageIndexEnum.Null;
            break;
          case (int)ItemTypeEnum.Frame:
            this.ImageIndex = (int)ImageIndexEnum.Frame;
            this.SelectedImageIndex = (int)ImageIndexEnum.Frame;
            break;
          default:
            this.ImageIndex = (int)ImageIndexEnum.Element;
            this.SelectedImageIndex = (int)ImageIndexEnum.Element;
            break;
        }
      }
    }
    public int ItemRank { get { return _ItemRank; } set { _ItemRank = value; Dirty = true; } }
    public int Left { get { return _Left; } set { _Left = value; Dirty = true; } }
    public int Top { get { return _Top; } set { _Top = value; Dirty = true; } }
    public int Width { get { return _Width; } set { _Width = value; Dirty = true; } }
    public int Height { get { return _Height; } set { _Height = value; Dirty = true; } }
    public int AngleA { get { return _AngleA; } set { _AngleA = value; Dirty = true; } }
    public int AngleB { get { return _AngleB; } set { _AngleB = value; Dirty = true; } }
    public int Weight { get { return _Weight; } set { _Weight = value; Dirty = true; } }


    public string ColorA { get { return _ColorA; } set { _ColorA = value; Dirty = true; } }
    public string ColorB { get { return _ColorB; } set { _ColorB = value; Dirty = true; } }
    public Font Font { get { return _Font.FromFontChunkStr(); } set { _Font = value.ToFontChunkStr(); Dirty = true; } }
    public new string Name { get { return base.Text; } set { base.Text = value; Dirty = true; } }
    public string Caption { get { return _Caption; } set { _Caption = value; Dirty = true; } }


    public Item FromChunk(string chunk) {
      var base1 = chunk.AsBase64Decoded().Parse(" ");
      Id = base1[0].AsInt();    // index 0
      OwnerId = base1[1].AsInt();
      TypeId = base1[2].AsInt();
      _ItemRank = base1[3].AsInt();
      _Left = base1[4].AsInt();
      _Top = base1[5].AsInt();
      _Width = base1[6].AsInt();
      _Height = base1[7].AsInt();
      _AngleA = base1[8].AsInt();
      _AngleB = base1[9].AsInt();
      _Weight = base1[10].AsInt();

      _ColorA = base1[11].AsBase64Decoded();
      _ColorB = base1[12].AsBase64Decoded();
      _Font = base1[13].AsBase64Decoded();
      base.Text = base1[14].AsBase64Decoded();
      _Caption = base1[15].AsBase64Decoded();

      if (_ColorA == "<NULL>") _ColorA = "";
      if (_ColorB == "<NULL>") _ColorB = "";
      if (_Font == "<NULL>") _Font = "";
      if (_Caption == "<NULL>") _Caption = "";

      Dirty = false;
      return this;
    }
    public string AsChunk() {

      var aIAT = _ColorA;
      var aBC = _ColorB;
      var aC = _Font;
      var aRT = _Caption;

      if (string.IsNullOrEmpty(this._ColorA)) aIAT = "<NULL>";
      if (string.IsNullOrEmpty(this._ColorB)) aBC = "<NULL>";
      if (string.IsNullOrEmpty(this._Font)) aC = "<NULL>";
      if (string.IsNullOrEmpty(this._Caption)) aRT = "<NULL>";

      return ($"{Id} {OwnerId} {_TypeId} {_ItemRank} {_Left} {_Top} {_Width} {_Height} {_AngleA} {_AngleB} {_Weight} " +
             $"{aIAT.AsBase64Encoded()} {aBC.AsBase64Encoded()} {aC.AsBase64Encoded()} {base.Text.AsBase64Encoded()} {aRT.AsBase64Encoded()}").AsBase64Encoded();
    }

    public bool CanSwitchDown() {
      if (Parent == null) return false;
      var ImChildNo = Parent.Nodes.IndexOf(this);
      if (ImChildNo < 0) return false;
      return ImChildNo < Parent.Nodes.Count - 1;
    }
    public Item GetSwitchDownItem() {
      if (Parent == null) return null;
      var ImChildNo = Parent.Nodes.IndexOf(this);
      if (ImChildNo < 0) return null;
      if (ImChildNo + 1 <= Parent.Nodes.Count - 1) {
        return ((Item)Parent.Nodes[ImChildNo + 1]);
      }
      return null;
    }
    public bool SwitchRankDown() {
      if (Parent == null) return false;
      var ImChildNo = Parent.Nodes.IndexOf(this);
      if (ImChildNo < 0) return false;
      if (ImChildNo + 1 <= Parent.Nodes.Count - 1) {
        var rank = ItemRank;
        ItemRank = ((Item)Parent.Nodes[ImChildNo + 1]).ItemRank;
        ((Item)Parent.Nodes[ImChildNo + 1]).ItemRank = rank;
        return true;
      }
      return false;
    }

    public bool CanSwitchUp() {
      if (Parent == null) return false;
      return Parent.Nodes.IndexOf(this) >= 1;
    }

    public Item GetSwitchUpItem() {
      if (Parent == null) return null;
      var ImChildNo = Parent.Nodes.IndexOf(this);
      if (ImChildNo < 1) return null;
      return ((Item)Parent.Nodes[ImChildNo - 1]);
    }

    public bool SwitchRankUp() {
      if (Parent == null) return false;
      var ImChildNo = Parent.Nodes.IndexOf(this);
      if (ImChildNo < 1) return false;
      var rank = ItemRank;
      ItemRank = ((Item)Parent.Nodes[ImChildNo - 1]).ItemRank;
      ((Item)Parent.Nodes[ImChildNo - 1]).ItemRank = rank;
      return false;
    }

    public Item AsClone() {
      string chunk = AsChunk();
      Item item = new Item().FromChunk(chunk);
      return item;
    }


  }

}
