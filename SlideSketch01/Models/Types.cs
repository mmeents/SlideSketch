using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlideSketch.Models {
 
  public class Types : ConcurrentDictionary<int, ItemType> {
    public Types() : base() {
      Load();
    }
    public void Load() {
      base.Clear();
      this[0] = new ItemType() { TypeId = 0, OwnerTypeId = 0, CatagoryTypeId = 0, EditorTypeId = 0, TypeRank = 0, TypeEnum = 0, Name = "none", Visible = true, Readonly = true, Desc = "" };

      this[1] = new ItemType() { TypeId = 1, OwnerTypeId = -1, CatagoryTypeId = 0, EditorTypeId = 1, TypeRank = 1, TypeEnum = 1, Visible = true, Readonly = false, Name = "Frame", Desc = "Root " };
      this[2] = new ItemType() { TypeId = 2, OwnerTypeId = 1, CatagoryTypeId = 2, EditorTypeId = 10, TypeRank = 1, TypeEnum = 2, Visible = true, Readonly = false, Name = "Element", Desc = "Element" };
      this[3] = new ItemType() { TypeId = 3, OwnerTypeId = 2, CatagoryTypeId = 2, EditorTypeId = 10, TypeRank = 2, TypeEnum = 2, Visible = true, Readonly = false, Name = "Rectangle", Desc = "Rectangle"};
      this[4] = new ItemType() { TypeId = 4, OwnerTypeId = 2, CatagoryTypeId = 2, EditorTypeId = 10, TypeRank = 3, TypeEnum = 2, Visible = true, Readonly = false, Name = "Oval", Desc = "Oval" };
      this[5] = new ItemType() { TypeId = 5, OwnerTypeId = 2, CatagoryTypeId = 2, EditorTypeId = 10, TypeRank = 4, TypeEnum = 2, Visible = true, Readonly = false, Name = "Arc", Desc = "Oval" };

    }

    public IEnumerable<ItemType> GetChildrenItemsNoDef(int id) {
      return this.Select(x => x.Value).Where(x => x.OwnerTypeId == id).OrderBy(x => x.TypeRank);
    }
    public IEnumerable<ItemType> GetChildrenItems(int id) {
      return this.Select(x => x.Value).Where(x => ((x.OwnerTypeId == id) || (x.OwnerTypeId == 0))).OrderBy(x => x.TypeRank);
    }


    public ItemType LoadSubtypes(ItemType item) {
      var items = GetOwnersTypes(item.TypeId);
      if (item.Nodes.Count > 0) item.Nodes.Clear();
      foreach (ItemType it in items) {
        item.Nodes.Add(it);
      }
      return item;
    }

    public IEnumerable<ItemType> GetOwnersTypes(int ownerTypeId) {
      try {
        IEnumerable<ItemType> result = GetChildrenItems(ownerTypeId);
        return result;
      } catch {
        return null;
      }
    }

    public virtual new ItemType this[int id] {
      get { return base.ContainsKey(id) ? base[id] : base.Values.First<ItemType>(x => x.TypeId > id); }
      set { if (value != null) { base[id] = value; } else { Remove(id); } }
    }

    public virtual void Remove(int id) { if (base.ContainsKey(id)) { _ = base.TryRemove(id, out _); } }
  }

}
