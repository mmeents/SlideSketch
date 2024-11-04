using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlideSketch.Models {
  public class BitmapCache : ConcurrentDictionary<string, BitmapCacheEntry> {
    public void IncrementReferenceCount(string key) {
      if (TryGetValue(key, out var cachedBitmap)) {
        Interlocked.Increment(ref cachedBitmap.ReferenceCount);
      }
    }

    public void CleanupBitmapCache() {
      foreach (var key in Keys.ToList()) {
        if (TryGetValue(key, out var cachedBitmap) && cachedBitmap.ReferenceCount <= 0) {
          if (TryRemove(key, out var removedBitmap)) {
            removedBitmap.Bitmap.Dispose();
          }
        } else if (TryGetValue(key, out var cachedBitmap2)) {
          Interlocked.Decrement(ref cachedBitmap2.ReferenceCount);
        }
      }
    }

    public void Dispose() {
      foreach (var key in Keys.ToList()) {
        if (TryGetValue(key, out var cachedBitmap)) {
          if (TryRemove(key, out var removedBitmap)) {
            removedBitmap.Bitmap.Dispose();
          }
        }
      }
    }
  }

  public class BitmapCacheEntry {
    public Bitmap Bitmap { get; }
    public int ReferenceCount;

    public BitmapCacheEntry(Bitmap bitmap) {
      Bitmap = bitmap;
      ReferenceCount = 1;
    }
  }
}
