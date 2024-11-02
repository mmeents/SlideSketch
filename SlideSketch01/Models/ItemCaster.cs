using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground.Models {
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

      // Recursively clone child nodes
      foreach (Item child in this.Nodes) {        
        Item childClone = child.AsClone();
        childClone.OwnerId = item.Id; // Set the owner to the cloned item
        item.Nodes.Add(childClone);
      }
      return item;
    }


  }

  public class Items : ConcurrentDictionary<int, Item> {
    public Items() : base() { }
    public virtual Boolean Contains(int id) {
      try {
        return base.ContainsKey(id);
      } catch {
        return false;
      }
    }
    public virtual new Item this[int id] {
      get { return Contains(id) ? base[id] : null; }
      set { if (value != null) { base[id] = value; } else { Remove(id); } }
    }
    public virtual void Remove(int id) { if (Contains(id)) { _ = base.TryRemove(id, out _); } }

    public IEnumerable<Item> GetChildrenItems(int id) {
      return this.Select(x => x.Value).Where(x => x.OwnerId == id).OrderBy(x => x.ItemRank);
    }
    public int GetNextId() {
      int max = 0;
      if (this.Keys.Count > 0) {
        max = this.Select(x => x.Value).Max(x => x.Id);
      }
      return max + 1;
    }

    public ICollection<string> AsList {
      get {
        List<string> retList = new List<string>();
        foreach (Item i in Values) {
          retList.Add(i.AsChunk());
        }
        return retList;
      }
      set {
        base.Clear();
        foreach (var x in value) {
          try {
            Item n = new Item().FromChunk(x);
            this[n.Id] = n;
          } catch { }
        }
      }
    }
  }

  public class ItemCaster {
    private TreeView _tv;
    private FilePackage _package;
    private Items _items;
    private Form1 _ownerForm;
    private Types _types;
    private bool _inUpdate = false;

    public bool InUpdate {
      get { return _inUpdate; }
      set {
        _inUpdate = value;
        if (!_inUpdate) {
          _package.PackageItems = _items;
          _package.Save();
        }
      }
    }
    public ItemCaster(Form1 ownerform, System.Windows.Forms.TreeView tv, FilePackage package, Types types) {
      _package = package;
      _tv = tv;
      _ownerForm = ownerform;
      _items = package.PackageItems;
      _types = types;
    }



    public void LoadTreeviewItemsAsync(TreeView ownerItem) {
      ownerItem.Nodes.Clear();
      IEnumerable<Item> result = _items.GetChildrenItems(0);
      foreach (Item item in result) {
        ownerItem.Nodes.Add(LoadChildren(item));
      }     
    }

    public Item LoadChildren(Item item) {
      try {
        var items = GetOwnersItemsAsync(item.Id);
        if (item.Nodes.Count > 0) item.Nodes.Clear();
        foreach (Item it in items) {
          if (!item.Nodes.Contains(it)) item.Nodes.Add(it);
        }
        item.Dirty = false;
      } catch (Exception ex) {
        _ownerForm.LogMsg($"{DateTime.Now} Error LoadChildren {ex.Message}");
      }
      return item;
    }

    public IEnumerable<Item> GetOwnersItemsAsync(int ownerItemId) {
      IEnumerable<Item> result = _items.GetChildrenItems(ownerItemId);
      List<Item> cloned = new List<Item>();
      foreach (Item i in result) {
        cloned.Add(i.AsClone());
      }
      return cloned;
    }

    public Item GetOwnersItemsAsync(Item ownerItem) {
      foreach (Item item in ownerItem.Nodes) {
        ReloadChildren(LoadChildren(item));
      }
      return ownerItem;
    }

    public Item ReloadChildren(Item child) {
      List<Item> temp = new List<Item>();
      foreach (Item item in child.Nodes) {
        temp.Add(item);
      }
      foreach (Item item in temp) {
        child.Nodes.Remove(item);
        child.Nodes.Add(LoadChildren(item));
      }
      child.Dirty = false;
      return child;
    }

    public Item SaveItem(Item item) {
      if (item == null) return null;
      if (item.Id == 0) {
        item.Id = _items.GetNextId();
      }
      _items[item.Id] = item;
      if (!InUpdate) {
        _package.PackageItems = _items;
        _package.Save();
      }
      item.Dirty = false;
      return item;
    }

    private Item AddSubItemFromType(Item ownerItem, ItemType itemType) {

      int NextRank = ownerItem.Nodes.Count + 1;
      int NextId = _items.GetNextId();
      Item dbs = new Item() {
        Id = NextId,
        OwnerId = ownerItem.Id,
        ItemRank = NextRank,
        TypeId = itemType.TypeId,
        Name = itemType.Name
      };
      if (ownerItem != null) {
        ownerItem.Nodes.Add(dbs);
        ownerItem.Expand();
      }
      SaveItem(dbs);
      var components = _types.GetChildrenItemsNoDef(itemType.TypeId);
      foreach (ItemType i in components) {
        AddSubItemFromType(dbs, i);
      }
      return dbs;
    }

    public Item SaveNewItemFromType(Item ownerItem, ItemType itemType) {
      Item dbs;
      int NextId = _items.GetNextId();
      if (ownerItem == null) {
        dbs = new Item() {
          Id = NextId,
          OwnerId = 0,
          ItemRank = 1,
          TypeId = itemType.TypeId,
          Name = itemType.Name
        };
      } else {
        int NextRank = ownerItem.Nodes.Count + 1;
        dbs = new Item() {
          Id = NextId,
          OwnerId = ownerItem.Id,
          ItemRank = NextRank,
          TypeId = itemType.TypeId,
          Name = itemType.Name
        };
      }
      if (dbs == null) return null;
      SaveItem(dbs);
      this.InUpdate = true;
      var components = _types.GetChildrenItemsNoDef(itemType.TypeId);
      foreach (ItemType i in components) {
        AddSubItemFromType(dbs, i);
      }
      if (ownerItem != null) {
        ownerItem.Nodes.Add(dbs);
        ownerItem.Expand();
      }
      this.InUpdate = false;
      return dbs;
    }

    public Item MoveItemSave(Item newOwnerItem, Item DragItem) {
      bool SaveDragged = false;
      if (newOwnerItem == null) {
        if (!_tv.Nodes.Contains(DragItem)) {
          if (DragItem.Parent.Nodes.Contains(DragItem)) {
            DragItem.Parent.Nodes.Remove(DragItem);
          }
          _tv.Nodes.Add(DragItem);
          DragItem.OwnerId = 0;
          SaveDragged = true;
        } else {
        }
      } else {
        if (!newOwnerItem.Nodes.Contains(DragItem)) {
          if (DragItem.Parent.Nodes.Contains(DragItem)) {
            DragItem.Parent.Nodes.Remove(DragItem);
          }
          newOwnerItem.Nodes.Add(DragItem);
          DragItem.OwnerId = newOwnerItem.Id;
          SaveDragged = true;
        }
      }
      if (SaveDragged) SaveItem(DragItem);
      return DragItem;
    }

    public Item SaveNewChildItemsFromText(Item ownerItem, ItemType itemType, string text) {
      Item curParent = ownerItem;
      Item returnItem = ownerItem;
      string[] lines = text.Parse(Environment.NewLine);
      this.InUpdate = true;
      int newItemType = itemType.TypeId;
      if (lines.Count() > 0) {
        foreach (string line in lines) {
          bool goneNested = false;
          if (line.Trim().Last<char>() == ':') {
            newItemType = itemType.TypeId;
            curParent = ownerItem;
            goneNested = true;
          }
          int newID = 0;
          int newItemRank = 0;
          if (curParent != null) {
            newID = curParent.Id;
            newItemRank = curParent.Nodes.Count + 1;
          }
          Item dbs = new Item() {
            Id = _items.GetNextId(),
            OwnerId = newID,
            ItemRank = newItemRank,
            TypeId = newItemType,
            Name = line
          };
          if (curParent != null) {
            curParent.Nodes.Add(dbs);
          } else {
            _tv.Nodes.Add(dbs);
          }
          SaveItem(dbs);
          returnItem = dbs;

          if (goneNested) {
            curParent = dbs;
          }
        }
        if (!(ownerItem == null)) {
          ownerItem.Expand();
        }

      }
      this.InUpdate = false;
      return returnItem;
    }

    public void NestedRemoveItem(Item item) {
      if (item == null) return;
      if (item.Nodes.Count == 0) {
        _items.Remove(item.Id);
      } else {
        foreach (Item cItem in item.Nodes) {
          NestedRemoveItem(cItem);
        }
        _items.Remove(item.Id);
      }
    }

    public void RemoveItem(Item item) {
      try {
        if (item == null) return;
        NestedRemoveItem(item);
        _package.PackageItems = _items;
        _ = Task.Run(async () => await _package.SaveAsync().ConfigureAwait(false));
      } catch (Exception ex) {
        _ownerForm.LogMsg($"{DateTime.Now} Remove Error {ex.Message}");
      }

    }

  }
}
