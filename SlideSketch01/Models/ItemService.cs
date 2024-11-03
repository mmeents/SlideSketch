using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlideSketch.Models {
 
  public class ItemService {
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
    public ItemService(Form1 ownerform, System.Windows.Forms.TreeView tv, FilePackage package, Types types) {
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

    public void CopyItemTo(Item newOwnerItem, Item itemToCopy) {
      Item newItem = itemToCopy.AsClone();
      newItem.Id = _items.GetNextId();
      newItem.OwnerId = newOwnerItem.Id;
      _items[newItem.Id] = newItem;
      newOwnerItem.Nodes.Add(newItem);

      foreach (Item child in itemToCopy.Nodes) {
        CopyItemTo(newItem, child);
      }
    }

    private void AssignNewIds(Item newItem, int newOwnerId) {
      if (newItem.Id == 0) {
        newItem.Id = _items.GetNextId();
      }      
      newItem.OwnerId = newOwnerId;
      foreach (Item child in newItem.Nodes) {
        AssignNewIds(child, newItem.Id);
      }
    }

    private void AddItemToPackage(Item item) {
      _items[item.Id] = item;
      foreach (Item child in item.Nodes) {
        AddItemToPackage(child);
      }
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
