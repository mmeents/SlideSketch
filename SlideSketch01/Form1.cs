
using SlideSketch.Models;
using System.Collections.Concurrent;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace SlideSketch {
  public partial class Form1 : Form {
    private BitmapCache _bitmapCache = new BitmapCache();
    private SettingsFile _settingsPack { get; set; }
    private Settings _settings;
    private Types _types;
    private FilePackage _filePackage;
    private ItemService _itemCaster;
    private Item? _inEditItem = null;
    private bool _inReorder = false;
    private bool _inReset = false;
    private bool _isInResize = false;

    private string _fileName = "No File Open";
    private string _folder;
    private bool _drawTimerRunning = false;
    private Item? _copiedItem;
    private int _flashCounter = 0;
    private bool _flashLine = false;
    public bool DrawTimerRunning {
      get { return _drawTimerRunning; }
      set {
        _drawTimerRunning = value;
        if (_drawTimerRunning) {
          if (!timerDraw.Enabled) timerDraw.Enabled = true;
        } else {
          if (timerDraw.Enabled) timerDraw.Enabled = false;
        }
      }
    }
    public Form1() {
      InitializeComponent();
      _types = new Types();
      _settingsPack = new SettingsFile(SettingsExt.SettingsFileName, this);
      _settings = _settingsPack.Settings;
      cbAspectRatio.SelectedIndex = 0;
      this.Text = "SlideSketch name a file and click design to continue";
      label4.Text = "";
      ConfigureSliders();
      UpdateCbTypeWithItemTypes();
    }

    private void ConfigureSliders() {
      tbWeight.MouseEnter += Slider_MouseEnter;
      tbWeight.MouseLeave += Slider_MouseLeave;
      tbLeft.MouseEnter += Slider_MouseEnter;
      tbLeft.MouseLeave += Slider_MouseLeave;
      tbTop.MouseEnter += Slider_MouseEnter;
      tbTop.MouseLeave += Slider_MouseLeave;
      tbWidth.MouseEnter += Slider_MouseEnter;
      tbWidth.MouseLeave += Slider_MouseLeave;
      tbHeight.MouseEnter += Slider_MouseEnter;
      tbHeight.MouseLeave += Slider_MouseLeave;
      AngleATrackBar.MouseEnter += Slider_MouseEnter;
      AngleATrackBar.MouseLeave += Slider_MouseLeave;
      AngleBTrackBar.MouseEnter += Slider_MouseEnter;
      AngleBTrackBar.MouseLeave += Slider_MouseLeave;
    }
    private void Form1_Shown(object sender, EventArgs e) {
      SetSettings();
    }

    delegate void SetLogMsgCallback(string msg);
    public void LogMsg(string msg) {
      if (this.edLogMsg.InvokeRequired) {
        SetLogMsgCallback d = new(LogMsg);
        this.BeginInvoke(d, new object[] { msg });
      } else {
        this.edLogMsg.Text = msg + Environment.NewLine + edLogMsg.Text;
      }
    }

    private void SetSettings() {
      if (_settings == null) return;
      if (_isInResize) return;
      _isInResize = true;
      if (_settings.ContainsKey("FormTop")) {
        this.Top = _settings["FormTop"].Value.AsInt();
      }
      if (_settings.ContainsKey("FormLeft")) {
        this.Left = _settings["FormLeft"].Value.AsInt();
      }
      if (_settings.ContainsKey("FormHeight")) {
        this.Height = _settings["FormHeight"].Value.AsInt();
      }
      if (_settings.ContainsKey("FormWidth")) {
        this.Width = _settings["FormWidth"].Value.AsInt();
      }

      string smrul = _settings["MRUL"].Value;
      if (!String.IsNullOrEmpty(smrul)) {
        cbProjectFileName.Items.Clear();
        cbProjectFileName.Items.AddRange(smrul.Parse(Environment.NewLine));
        cbProjectFileName.SelectedIndex = 0;
      }
      _isInResize = false;
    }

    private void SaveSettings() {
      if (_settings == null) return;
      if (_isInResize) return;

      _isInResize = true;
      if (!_settings.ContainsKey("FormTop")) {
        _settings["FormTop"] = new SettingProperty() { Key = "FormTop", Value = this.Top.AsString() };
      } else {
        _settings["FormTop"].Value = this.Top.AsString();
      }

      if (!_settings.ContainsKey("FormLeft")) {
        _settings["FormLeft"] = new SettingProperty() { Key = "FormLeft", Value = this.Left.AsString() };
      } else {
        _settings["FormLeft"].Value = this.Left.AsString();
      }

      if (!_settings.ContainsKey("FormHeight")) {
        _settings["FormHeight"] = new SettingProperty() { Key = "FormHeight", Value = this.Height.AsString() };
      } else {
        _settings["FormHeight"].Value = this.Height.AsString();
      }

      if (!_settings.ContainsKey("FormWidth")) {
        _settings["FormWidth"] = new SettingProperty() { Key = "FormWidth", Value = this.Width.AsString() };
      } else {
        _settings["FormWidth"].Value = this.Width.AsString();
      }

      _settingsPack.Settings = _settings;
      _settingsPack.Save();
      _isInResize = false;

    }

    private void UpdateCbTypeWithItemTypes() {
      cbType.Items.Clear();
      foreach (var itemType in Enum.GetValues(typeof(ItemTypeEnum))) {
        if ((int)itemType >= 2) {
          cbType.Items.Add(itemType);
        }
      }
    }
    private void AddFileToMRUL(string fileName) {
      if (!_settings.Contains("MRUL")) {
        _settings["MRUL"] = new SettingProperty() { Key = "MRUL", Value = fileName };
      } else {
        var mrul = _settings["MRUL"].Value.Parse(Environment.NewLine);
        string newMRUL = (mrul.Length > 0 ? mrul[0] : "")
          + (mrul.Length > 1 ? Environment.NewLine + mrul[1] : "")
          + (mrul.Length > 2 ? Environment.NewLine + mrul[2] : "");
        StringDict mruld = newMRUL.AsDict(Environment.NewLine);
        mruld.Add(fileName);
        _settings["MRUL"].Value = fileName + Environment.NewLine + mruld.AsString();
        _settingsPack.Settings = _settings;
        _settingsPack.Save();
      }
    }

    private void BrowseFileNameButton_Click(object sender, EventArgs e) {
      string file = String.IsNullOrEmpty(cbProjectFileName.Text) ? "DrawingFile" : cbProjectFileName.Text;
      openFileDialog1.FileName = file;
      openFileDialog1.InitialDirectory = SettingsExt.DefaultPath;
      if (openFileDialog1.ShowDialog() == DialogResult.OK) {
        cbProjectFileName.Text = openFileDialog1.FileName;
      }
    }

    private void TabCtrlRoot_Selecting(object sender, TabControlCancelEventArgs e) {

      if (e.TabPage == SetupTab) {
        FormCloseFileName();
      } else if (e.TabPage == DesignTab) {
        if (cbProjectFileName.Text.Length > 0) {
          _fileName = cbProjectFileName.Text;
          FormLoadFileName();
        } else {
          e.Cancel = false;
        }
      }

    }

    private void FormLoadFileName() {
      this.Text = "SlideSketch working on " + _fileName;
      AddFileToMRUL(_fileName);
      _filePackage = new FilePackage(_fileName, this);
      _itemCaster = new ItemService(this, treeView1, _filePackage, _types);
      _itemCaster.LoadTreeviewItemsAsync(treeView1);
      treeView1.ExpandAll();
      DrawTimerRunning = true;
    }

    private void FormCloseFileName() {
      DrawTimerRunning = false;
      if (_inEditItem?.Dirty ?? false) {
        SaveInEditChanges();
      }
      treeView1.Nodes.Clear();
      this.Text = "SlideSketch name a file and click design to continue";
    }

    private void MenuAddFrame_Click(object sender, EventArgs e) {
      _ = _itemCaster.SaveNewChildItemsFromText(null, _types[(int)ItemTypeEnum.Frame], "Frame");
    }

    private void MenuAddElement_Click(object sender, EventArgs e) {
      if (_inEditItem != null) {
        var element = _itemCaster.SaveNewChildItemsFromText(_inEditItem, _types[(int)ItemTypeEnum.Element], "Element");
        element.ColorA = "#000000";
      }
    }

    private void MenuTree_Opening(object sender, System.ComponentModel.CancelEventArgs e) {
      if (_inEditItem == null) {
        MenuAddElement.Enabled = false;
        MenuDeleteItem.Enabled = false;
        MenuMoveUpItem.Enabled = false;
        MenuDeleteItem.Enabled = false;
        copyItemToolStripMenuItem.Enabled = false;
        pasteItemToolStripMenuItem.Enabled = false;
      } else {
        MenuAddElement.Enabled = true;
        MenuDeleteItem.Enabled = true;
        MenuMoveUpItem.Enabled = true;
        MenuDeleteItem.Enabled = true;
        copyItemToolStripMenuItem.Enabled = true;
        pasteItemToolStripMenuItem.Enabled = (_copiedItem != null && treeView1.SelectedNode is Item selectedItem);
      }
    }

    private void treeView1_AfterSelect(object sender, TreeViewEventArgs e) {
      if (!_inReorder) {
        try {
          if (e?.Node == null) return;
          _inEditItem = e.Node as Item;
          if (_inEditItem == null) return;
          ResetPropEditors(_inEditItem);
        } finally {
        }
      }
    }

    private void MenuDeleteItem_Click(object sender, EventArgs e) {
      if (_inEditItem == null) return;
      try {
        Item item = _inEditItem;
        Item ParentItem = (Item)item.Parent;
        if (ParentItem != null) {
          treeView1.SelectedNode = ParentItem;
          ParentItem.Nodes.Remove(item);
        }
        _itemCaster.RemoveItem(item);
        treeView1_AfterSelect(sender, new TreeViewEventArgs(ParentItem));
      } finally {

      }
    }

    private void MenuMoveUpItem_Click(object sender, EventArgs e) {
      if (_inEditItem == null) return;
      if (!_inEditItem.CanSwitchUp()) return;
      var otherItem = _inEditItem.GetSwitchUpItem();
      _inEditItem.SwitchRankUp();
      if (_inEditItem.Dirty) { _itemCaster.SaveItem(_inEditItem); }
      if (otherItem != null && otherItem.Dirty) _itemCaster.SaveItem(otherItem);

      var opItem = _inEditItem;
      _inReorder = true;
      try {
        var parentItem = (Item)_inEditItem.Parent;
        var otherItemIndex = parentItem.Nodes.IndexOf(otherItem);
        if (otherItemIndex >= 0) {
          parentItem.Nodes.Remove(opItem);
          parentItem.Nodes.Insert(otherItemIndex, opItem);
        }
      } finally {
        _inReorder = false;
      }
      treeView1.SelectedNode = opItem;
    }

    private void treeView1_BeforeExpand(object? sender, TreeViewCancelEventArgs e) {
      Item? tn = e.Node as Item;
      if (tn != null) {
        var items = _itemCaster.GetOwnersItemsAsync(tn);
        e.Cancel = false;
      }
    }

    private void ResetPropEditors(Item item) {
      if (item != null) {
        _inReset = true;
        label4.Text = $"Focused: {item.Name}";
        tbWeight.Value = item.Weight;
        tbLeft.Value = item.Left;
        tbTop.Value = 100 - item.Top;
        tbWidth.Value = item.Width;
        tbHeight.Value = 100 - item.Height;

        // Find the index of the enum string that matches item.TypeId
        var typeName = ((ItemTypeEnum)item.TypeId).ToString();
        var index = cbType.Items.Cast<object>().ToList().FindIndex(x => x.ToString() == typeName);
        if (index >= 0) {
          cbType.SelectedIndex = index;
        } else {
          // Handle the case where the type is not found
          cbType.SelectedIndex = -1; // or any other appropriate action
        }
        if (cbType.SelectedItem.AsString().Contains("Bitmap")) {
          BmpBrowse.Visible = true;
        } else {
          BmpBrowse.Visible = false;
        }
        ColorAButton.BackColor = item.ColorA.AsColor();
        textFore.Text = item.ColorA.AsString();
        ColorBButton.BackColor = item.ColorB.AsColor();
        textBack.Text = item.ColorB.AsString();
        AngleATrackBar.Value = item.AngleA;
        AngleBTrackBar.Value = item.AngleB;
        edCaption.Text = item.Caption;
        btnFont.Font = item.Font;
        _inReset = false;
        _flashLine = true;
        _flashCounter = 0;
      }
    }

    private void SaveInEditChanges() {
      if (_inEditItem == null) return;
      if (_inEditItem.Dirty) {
        try {
          _itemCaster.SaveItem(_inEditItem);
        } finally {
          //SetInProgress(0);
          ResetPropEditors(_inEditItem);
        }
      }
    }

    private void cbType_SelectedIndexChanged(object sender, EventArgs e) {
      if (_inEditItem == null || _inReset || cbType.SelectedItem == null) return;
      string selectedType = cbType.SelectedItem?.ToString() ?? string.Empty;
      int selectedIndex = (Enum.TryParse(selectedType, out ItemTypeEnum itemType)) ? (int)itemType : 0;
      if (selectedIndex > 1) {
        _inEditItem.TypeId = selectedIndex;
        SaveInEditChanges();
      }
    }
    private void ColorAButton_Click(object sender, EventArgs e) {
      if (_inEditItem == null) return;
      colorDialog1.Color = ColorAButton.BackColor;
      if (colorDialog1.ShowDialog() == DialogResult.OK) {
        textFore.Text = colorDialog1.Color.AsString();
        ColorAButton.BackColor = colorDialog1.Color;
        _inEditItem.ColorA = ColorAButton.BackColor.AsString();
        SaveInEditChanges();
      }
    }

    private void ColorBButton_Click(object sender, EventArgs e) {
      if (_inEditItem == null) return;
      colorDialog1.Color = ColorBButton.BackColor;
      if (colorDialog1.ShowDialog() == DialogResult.OK) {
        textBack.Text = colorDialog1.Color.AsString();
        ColorBButton.BackColor = colorDialog1.Color;
        _inEditItem.ColorB = ColorBButton.BackColor.AsString();
        SaveInEditChanges();
      }
    }

    private void tbLeft_ValueChanged(object sender, EventArgs e) {
      if (_inEditItem == null || _inReset) return;
      _inEditItem.Left = tbLeft.Value;
      SaveInEditChanges();
      _flashLine = true;
      _flashCounter = 0;
    }

    private void tbTop_ValueChanged(object sender, EventArgs e) {
      if (_inEditItem == null || _inReset) return;
      _inEditItem.Top = 100 - tbTop.Value;
      SaveInEditChanges();
      _flashLine = true;
      _flashCounter = 0;
    }

    private void tbHeight_ValueChanged(object sender, EventArgs e) {
      if (_inEditItem == null || _inReset) return;
      _inEditItem.Height = 100 - tbHeight.Value;
      SaveInEditChanges();
    }

    private void tbWidth_ValueChanged(object sender, EventArgs e) {
      if (_inEditItem == null || _inReset) return;
      _inEditItem.Width = tbWidth.Value;
      SaveInEditChanges();
    }

    private void edCaption_TextChanged(object sender, EventArgs e) {
      if (_inEditItem == null || _inReset) return;
      _inEditItem.Caption = edCaption.Text;
      SaveInEditChanges();
    }

    private void AngleATrackBar_ValueChanged(object sender, EventArgs e) {
      if (_inEditItem == null || _inReset) return;
      _inEditItem.AngleA = AngleATrackBar.Value;
      SaveInEditChanges();
    }

    private void AngleBTrackBar_ValueChanged(object sender, EventArgs e) {
      if (_inEditItem == null || _inReset) return;
      _inEditItem.AngleB = AngleBTrackBar.Value;
      SaveInEditChanges();
    }

    private void tbWeight_ValueChanged(object sender, EventArgs e) {
      if (_inEditItem == null || _inReset) return;
      _inEditItem.Weight = tbWeight.Value;
      SaveInEditChanges();
    }

    private void btnFont_Click(object sender, EventArgs e) {
      if (_inEditItem == null) return;
      fontDialog1.Font = _inEditItem.Font;
      if (fontDialog1.ShowDialog() == DialogResult.OK) {
        _inEditItem.Font = fontDialog1.Font;
        SaveInEditChanges();
      }
    }

    private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e) {
      try {
        if (_inEditItem != (Item)e.Node) {
          _inEditItem = (Item)e.Node;
        }
        if (_inEditItem == null || e.Label == null) {
          e.CancelEdit = true;
          return;
        }
        if (_inEditItem.Name != e.Label) { _inEditItem.Name = e.Label; }
        if (_inEditItem.Dirty) _itemCaster.SaveItem(_inEditItem);
        ResetPropEditors(_inEditItem);
      } finally {
        //SetInProgress(0);
      }
    }

    private void timerDraw_Tick(object sender, EventArgs e) {
      timerDraw.Enabled = false;
      try {
        var g = splitContainer4.Panel1.CreateGraphics();
        DrawTreeOnPictureBox(g);
      } finally {
        timerDraw.Enabled = _drawTimerRunning;
      }
    }


    private Item? FindFrameNode() {
      if (_inEditItem == null) return null;
      if (_inEditItem.TypeId == 1) return _inEditItem;
      Item? parentItem = null;
      if (_inEditItem.TypeId > 1) {
        parentItem = _inEditItem.Parent as Item;
        if (parentItem == null) {
          return null;
        } else {
          while ((parentItem != null) && (parentItem.TypeId > 1)) {
            parentItem = parentItem.Parent as Item;
          }
          if (parentItem != null && parentItem.TypeId == 1) {
            return parentItem;
          }
        }
      }
      return null;
    }
    private void DrawTreeOnPictureBox(Graphics graphics) {
      Item? frame = this.FindFrameNode();
      if (frame == null) return;
      BufferedGraphics bg = BufferedGraphicsManager.Current.Allocate(graphics, splitContainer4.Panel1.DisplayRectangle);
      SurfaceProps surface = new SurfaceProps() {
        BitmapCache = _bitmapCache,
        ContainerWidth = splitContainer4.Panel1.Width,
        ContainerHeight = splitContainer4.Panel1.Height,
        ContainerAspectRatio = cbAspectRatio.SelectedItem.AsString().GetAspectRatio(),
        Frame = frame,
        FocusedItem = _inEditItem,
        FrameForgroundBrush = new SolidBrush(frame.ColorA.AsColor()),
        FrameBackgroundBrush = new SolidBrush(frame.ColorB.AsColor()),
        ItemBrushA = new SolidBrush(frame.ColorA.AsColor()),
        ItemBrushB = new SolidBrush(frame.ColorB.AsColor()),
        bg = bg
      };
      try {
        Rectangle drawRectangle = surface.FrameRectangle();
        Rectangle displayRectangle = surface.DisplayRectangle();

        Pen framePen = new Pen(surface.FrameForgroundBrush);
        framePen.Width = 3;

        Region displayReg = new Region(displayRectangle);
        Region drawReg = new Region(drawRectangle);

        bg.Graphics.FillRegion(SystemBrushes.ButtonFace, displayReg);

        bg.Graphics.FillRegion(surface.FrameBackgroundBrush, drawReg);
        bg.Graphics.DrawRectangle(framePen, 0, 0, drawRectangle.Width.AsInt() - 1, drawRectangle.Height.AsInt() - 1);

        foreach (var element in frame.Nodes) {
          Item? elementItem = element as Item;
          if (elementItem != null) {
            elementItem.DrawElement(surface);
          }
        }

        if (_flashLine && _flashCounter < 8 && _inEditItem != null) {

          // Get the current values of Left and Top          
          int left = ((_inEditItem.Left * surface.ContainerWidth) / 100).AsInt();
          int top = ((_inEditItem.Top * surface.ContainerHeight) / 100).AsInt();

          // Draw the flashing line
          using (Pen pen = new Pen(Color.Red, 1)) {
            bg.Graphics.DrawLine(pen, left - 5, top, left + 10, top); // Horizontal line
            bg.Graphics.DrawLine(pen, left, top - 5, left, top + 10); // Vertical line
          }

          _flashCounter++;
          if (_flashCounter >= 8) {
            _flashLine = false;
          }
        }

        bg.Render(graphics);
      } catch (Exception ex) {
        LogMsg($"Render Exception: " + ex.Message);
      } finally {
        bg.Dispose();
        graphics.Dispose();
        _bitmapCache.CleanupBitmapCache();
      }
    }
    private void Slider_MouseEnter(object sender, EventArgs e) {
      this.Cursor = Cursors.Hand; // Change to slider icon
    }

    private void Slider_MouseLeave(object sender, EventArgs e) {
      this.Cursor = Cursors.Default; // Revert to default cursor
    }

    private void copyItemToolStripMenuItem_MouseDown(object sender, MouseEventArgs e) {
      if (treeView1.SelectedNode is Item selectedItem) {
        _copiedItem = selectedItem; // Assuming AsClone creates a deep copy of the item
        LogMsg($"Item '{selectedItem.Name}' copied.");
      }
    }

    private void pasteItemToolStripMenuItem_Click(object sender, EventArgs e) {
      if (_copiedItem != null && treeView1.SelectedNode is Item selectedItem) {
        try {
          _itemCaster.CopyItemTo(selectedItem, _copiedItem);
        } catch (Exception ex) {
          LogMsg($"Error pasting item: {ex.Message}");
          MessageBox.Show($"Error pasting item: {ex.Message}");
        }
        LogMsg($"Item '{_copiedItem.Name}' pasted under '{selectedItem.Name}'.");
      }
    }

    private void takeSnapshotToolStripMenuItem_Click(object sender, EventArgs e) {
      // Find the Frame node
      Item? frameNode = FindFrameNode();
      if (frameNode == null) {
        MessageBox.Show("Frame node not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      // Get the dimensions from the Frame node
      decimal designSize = frameNode.Width / (100).AsDecimal();
      decimal frameWidth = (splitContainer4.Panel1.Width * designSize);
      decimal ContainerAspectRatio = cbAspectRatio.SelectedItem.AsString().GetAspectRatio();
      decimal frameHeight = (frameWidth * ContainerAspectRatio);
      int width = frameWidth.AsInt();
      int height = frameHeight.AsInt();

      // Create a bitmap with the dimensions of the Frame node
      using (Bitmap bitmap = new Bitmap(width, height)) {
        using (Graphics g = Graphics.FromImage(bitmap)) {
          DrawTreeOnPictureBox(g);
        }

        // Save the bitmap to a file
        using (SaveFileDialog saveFileDialog = new SaveFileDialog()) {
          saveFileDialog.Filter = "Bitmap Image|*.bmp";
          saveFileDialog.Title = "Save Snapshot";
          if (saveFileDialog.ShowDialog() == DialogResult.OK) {
            bitmap.Save(saveFileDialog.FileName);
            MessageBox.Show("Snapshot saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
          }
        }
      }
    }

    private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
      _bitmapCache.Dispose();
    }

    private void BmpBrowse_Click(object sender, EventArgs e) {
      openBmpDialog.FileName = edCaption.Text;
      if (openBmpDialog.ShowDialog() == DialogResult.OK) {
        edCaption.Text = openBmpDialog.FileName;
      }
    }

    private void label11_Click(object sender, EventArgs e) {

    }

    private void treeView1_DragEnter(object sender, DragEventArgs e) {
      e.Effect = DragDropEffects.Move;
    }

    private void treeView1_DragOver(object sender, DragEventArgs e) {
      if (e.Data != null) {
        Item sni = (Item)e.Data.GetData(typeof(Item));
        if (sni != null) {
          Point targetPt = treeView1.PointToClient(new Point(e.X, e.Y));
          Item tn = (Item)treeView1.GetNodeAt(targetPt);
          if (tn != sni) {
            if (tn != null) {
              e.Effect = DragDropEffects.Move;
              if (!tn.IsExpanded) tn.Expand();
            } else {
              if ((sni.TypeId >= (int)ItemTypeEnum.Element) && (sni.TypeId <= 199)) {
                e.Effect = DragDropEffects.Move;
              } else {
                e.Effect = DragDropEffects.None;
              }
            }
          }
        }
      }
    }

    private void treeView1_DragDrop(object sender, DragEventArgs e) {
      try {
        Point targetPt = treeView1.PointToClient(new Point(e.X, e.Y));
        Item tn = (Item)treeView1.GetNodeAt(targetPt);

        if (tn != null && e.Data != null) {
          Item dragNode = (Item)e.Data.GetData(typeof(Item));
          if (dragNode != null) {
            var fnn = _itemCaster.MoveItemSave(tn, dragNode);
          }
        }

      } finally {
      }
    }

    private void treeView1_ItemDrag(object sender, ItemDragEventArgs e) {
      if (e.Button == MouseButtons.Left) {
        DoDragDrop(e.Item, DragDropEffects.Move);
      }
    }

    private void textFore_TextChanged(object sender, EventArgs e) {
      if (_inEditItem != null) {
        try {
          // Parse the color from the text box
          var color = ColorTranslator.FromHtml(textFore.Text);
          _inEditItem.ColorA = ColorTranslator.ToHtml(color);

          // Optionally, update the UI or other related properties
          ColorAButton.BackColor = color;
        } catch (Exception ex) {
          // Handle invalid color format
          LogMsg($"Invalid color format: {ex.Message}");
        }
      }
    }

    private void textBack_TextChanged(object sender, EventArgs e) {
      if (_inEditItem != null) {
        try {
          // Parse the color from the text box
          var color = ColorTranslator.FromHtml(textBack.Text);
          _inEditItem.ColorB = ColorTranslator.ToHtml(color);

          // Optionally, update the UI or other related properties
          ColorBButton.BackColor = color;
        } catch (Exception ex) {
          // Handle invalid color format
          LogMsg($"Invalid color format: {ex.Message}");
        }
      }
    }

    private void button1_Click(object sender, EventArgs e) {
      treeView1.ExpandAll();
    }
  }
}