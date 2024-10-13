namespace Playground {
  partial class Form1 {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      TabCtrlRoot = new TabControl();
      SetupTab = new TabPage();
      label8 = new Label();
      BrowseFileNameButton = new Button();
      cbProjectFileName = new ComboBox();
      DesignTab = new TabPage();
      splitContainer1 = new SplitContainer();
      label7 = new Label();
      label6 = new Label();
      label5 = new Label();
      label4 = new Label();
      cbAspectRatio = new ComboBox();
      trackBar6 = new TrackBar();
      tbFrame = new TrackBar();
      splitContainer2 = new SplitContainer();
      splitContainer3 = new SplitContainer();
      treeView1 = new TreeView();
      MenuTree = new ContextMenuStrip(components);
      MenuAddFrame = new ToolStripMenuItem();
      MenuAddElement = new ToolStripMenuItem();
      toolStripSeparator1 = new ToolStripSeparator();
      MenuMoveUpItem = new ToolStripMenuItem();
      toolStripSeparator2 = new ToolStripSeparator();
      MenuDeleteItem = new ToolStripMenuItem();
      imageList1 = new ImageList(components);
      splitContainer4 = new SplitContainer();
      label1 = new Label();
      tbHeight = new TrackBar();
      tbTop = new TrackBar();
      btnFont = new Button();
      label10 = new Label();
      cbType = new ComboBox();
      ColorBButton = new Button();
      ColorAButton = new Button();
      label9 = new Label();
      edCaption = new TextBox();
      label3 = new Label();
      label2 = new Label();
      tbWidth = new TrackBar();
      tbLeft = new TrackBar();
      LogsTab = new TabPage();
      edLogMsg = new TextBox();
      openFileDialog1 = new OpenFileDialog();
      colorDialog1 = new ColorDialog();
      fontDialog1 = new FontDialog();
      timerDraw = new System.Windows.Forms.Timer(components);
      TabCtrlRoot.SuspendLayout();
      SetupTab.SuspendLayout();
      DesignTab.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
      splitContainer1.Panel1.SuspendLayout();
      splitContainer1.Panel2.SuspendLayout();
      splitContainer1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)trackBar6).BeginInit();
      ((System.ComponentModel.ISupportInitialize)tbFrame).BeginInit();
      ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
      splitContainer2.Panel1.SuspendLayout();
      splitContainer2.Panel2.SuspendLayout();
      splitContainer2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)splitContainer3).BeginInit();
      splitContainer3.Panel1.SuspendLayout();
      splitContainer3.Panel2.SuspendLayout();
      splitContainer3.SuspendLayout();
      MenuTree.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)splitContainer4).BeginInit();
      splitContainer4.Panel2.SuspendLayout();
      splitContainer4.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)tbHeight).BeginInit();
      ((System.ComponentModel.ISupportInitialize)tbTop).BeginInit();
      ((System.ComponentModel.ISupportInitialize)tbWidth).BeginInit();
      ((System.ComponentModel.ISupportInitialize)tbLeft).BeginInit();
      LogsTab.SuspendLayout();
      SuspendLayout();
      // 
      // TabCtrlRoot
      // 
      TabCtrlRoot.Controls.Add(SetupTab);
      TabCtrlRoot.Controls.Add(DesignTab);
      TabCtrlRoot.Controls.Add(LogsTab);
      TabCtrlRoot.Dock = DockStyle.Fill;
      TabCtrlRoot.Location = new Point(0, 0);
      TabCtrlRoot.Name = "TabCtrlRoot";
      TabCtrlRoot.Padding = new Point(4, 2);
      TabCtrlRoot.SelectedIndex = 0;
      TabCtrlRoot.Size = new Size(1021, 680);
      TabCtrlRoot.TabIndex = 7;
      TabCtrlRoot.Selecting += TabCtrlRoot_Selecting;
      // 
      // SetupTab
      // 
      SetupTab.BackColor = SystemColors.ButtonFace;
      SetupTab.Controls.Add(label8);
      SetupTab.Controls.Add(BrowseFileNameButton);
      SetupTab.Controls.Add(cbProjectFileName);
      SetupTab.Location = new Point(4, 27);
      SetupTab.Name = "SetupTab";
      SetupTab.Padding = new Padding(3);
      SetupTab.Size = new Size(1013, 649);
      SetupTab.TabIndex = 0;
      SetupTab.Text = "Setup";
      // 
      // label8
      // 
      label8.AutoSize = true;
      label8.Location = new Point(37, 37);
      label8.Name = "label8";
      label8.Size = new Size(299, 20);
      label8.TabIndex = 2;
      label8.Text = "Choose the project file and switch to design";
      // 
      // BrowseFileNameButton
      // 
      BrowseFileNameButton.Location = new Point(640, 68);
      BrowseFileNameButton.Name = "BrowseFileNameButton";
      BrowseFileNameButton.Size = new Size(94, 29);
      BrowseFileNameButton.TabIndex = 1;
      BrowseFileNameButton.Text = "Browse";
      BrowseFileNameButton.UseVisualStyleBackColor = true;
      BrowseFileNameButton.Click += BrowseFileNameButton_Click;
      // 
      // cbProjectFileName
      // 
      cbProjectFileName.FormattingEnabled = true;
      cbProjectFileName.Location = new Point(60, 69);
      cbProjectFileName.Name = "cbProjectFileName";
      cbProjectFileName.Size = new Size(562, 28);
      cbProjectFileName.TabIndex = 0;
      // 
      // DesignTab
      // 
      DesignTab.BackColor = SystemColors.ButtonFace;
      DesignTab.Controls.Add(splitContainer1);
      DesignTab.Location = new Point(4, 27);
      DesignTab.Name = "DesignTab";
      DesignTab.Padding = new Padding(3);
      DesignTab.Size = new Size(1013, 649);
      DesignTab.TabIndex = 1;
      DesignTab.Text = "Design";
      // 
      // splitContainer1
      // 
      splitContainer1.Cursor = Cursors.HSplit;
      splitContainer1.Dock = DockStyle.Fill;
      splitContainer1.Location = new Point(3, 3);
      splitContainer1.Name = "splitContainer1";
      splitContainer1.Orientation = Orientation.Horizontal;
      // 
      // splitContainer1.Panel1
      // 
      splitContainer1.Panel1.Controls.Add(label7);
      splitContainer1.Panel1.Controls.Add(label6);
      splitContainer1.Panel1.Controls.Add(label5);
      splitContainer1.Panel1.Controls.Add(label4);
      splitContainer1.Panel1.Controls.Add(cbAspectRatio);
      splitContainer1.Panel1.Controls.Add(trackBar6);
      splitContainer1.Panel1.Controls.Add(tbFrame);
      // 
      // splitContainer1.Panel2
      // 
      splitContainer1.Panel2.Controls.Add(splitContainer2);
      splitContainer1.Size = new Size(1007, 643);
      splitContainer1.SplitterDistance = 82;
      splitContainer1.TabIndex = 0;
      // 
      // label7
      // 
      label7.AutoSize = true;
      label7.Location = new Point(505, 49);
      label7.Name = "label7";
      label7.Size = new Size(36, 20);
      label7.TabIndex = 6;
      label7.Text = "Size";
      // 
      // label6
      // 
      label6.AutoSize = true;
      label6.Location = new Point(246, 50);
      label6.Name = "label6";
      label6.Size = new Size(50, 20);
      label6.TabIndex = 5;
      label6.Text = "Frame";
      // 
      // label5
      // 
      label5.AutoSize = true;
      label5.Location = new Point(15, 50);
      label5.Name = "label5";
      label5.Size = new Size(54, 20);
      label5.TabIndex = 4;
      label5.Text = "Aspect";
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new Point(15, 12);
      label4.Name = "label4";
      label4.Size = new Size(50, 20);
      label4.TabIndex = 3;
      label4.Text = "label4";
      // 
      // cbAspectRatio
      // 
      cbAspectRatio.FormattingEnabled = true;
      cbAspectRatio.Items.AddRange(new object[] { "2:3", "3:2", "3:4", "4:3", "5:4", "4:5", "16:9", "9:16", "16:10", "10:16", "" });
      cbAspectRatio.Location = new Point(74, 47);
      cbAspectRatio.Name = "cbAspectRatio";
      cbAspectRatio.Size = new Size(151, 28);
      cbAspectRatio.TabIndex = 2;
      // 
      // trackBar6
      // 
      trackBar6.AutoSize = false;
      trackBar6.Location = new Point(547, 52);
      trackBar6.Maximum = 4000;
      trackBar6.Name = "trackBar6";
      trackBar6.Size = new Size(305, 23);
      trackBar6.TabIndex = 1;
      trackBar6.TickFrequency = 100;
      trackBar6.Value = 200;
      // 
      // tbFrame
      // 
      tbFrame.AutoSize = false;
      tbFrame.Location = new Point(302, 52);
      tbFrame.Maximum = 0;
      tbFrame.Name = "tbFrame";
      tbFrame.Size = new Size(145, 22);
      tbFrame.TabIndex = 0;
      // 
      // splitContainer2
      // 
      splitContainer2.Cursor = Cursors.HSplit;
      splitContainer2.Dock = DockStyle.Fill;
      splitContainer2.Location = new Point(0, 0);
      splitContainer2.Name = "splitContainer2";
      splitContainer2.Orientation = Orientation.Horizontal;
      // 
      // splitContainer2.Panel1
      // 
      splitContainer2.Panel1.Controls.Add(splitContainer3);
      // 
      // splitContainer2.Panel2
      // 
      splitContainer2.Panel2.Controls.Add(btnFont);
      splitContainer2.Panel2.Controls.Add(label10);
      splitContainer2.Panel2.Controls.Add(cbType);
      splitContainer2.Panel2.Controls.Add(ColorBButton);
      splitContainer2.Panel2.Controls.Add(ColorAButton);
      splitContainer2.Panel2.Controls.Add(label9);
      splitContainer2.Panel2.Controls.Add(edCaption);
      splitContainer2.Panel2.Controls.Add(label3);
      splitContainer2.Panel2.Controls.Add(label2);
      splitContainer2.Panel2.Controls.Add(tbWidth);
      splitContainer2.Panel2.Controls.Add(tbLeft);
      splitContainer2.Size = new Size(1007, 557);
      splitContainer2.SplitterDistance = 403;
      splitContainer2.TabIndex = 0;
      // 
      // splitContainer3
      // 
      splitContainer3.Cursor = Cursors.VSplit;
      splitContainer3.Dock = DockStyle.Fill;
      splitContainer3.Location = new Point(0, 0);
      splitContainer3.Name = "splitContainer3";
      // 
      // splitContainer3.Panel1
      // 
      splitContainer3.Panel1.Controls.Add(treeView1);
      // 
      // splitContainer3.Panel2
      // 
      splitContainer3.Panel2.Controls.Add(splitContainer4);
      splitContainer3.Size = new Size(1007, 403);
      splitContainer3.SplitterDistance = 242;
      splitContainer3.TabIndex = 0;
      // 
      // treeView1
      // 
      treeView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      treeView1.ContextMenuStrip = MenuTree;
      treeView1.ImageIndex = 0;
      treeView1.ImageList = imageList1;
      treeView1.LabelEdit = true;
      treeView1.Location = new Point(15, 13);
      treeView1.Name = "treeView1";
      treeView1.SelectedImageIndex = 0;
      treeView1.Size = new Size(224, 387);
      treeView1.TabIndex = 0;
      treeView1.AfterLabelEdit += treeView1_AfterLabelEdit;
      treeView1.BeforeExpand += treeView1_BeforeExpand;
      treeView1.AfterSelect += treeView1_AfterSelect;
      // 
      // MenuTree
      // 
      MenuTree.ImageScalingSize = new Size(20, 20);
      MenuTree.Items.AddRange(new ToolStripItem[] { MenuAddFrame, MenuAddElement, toolStripSeparator1, MenuMoveUpItem, toolStripSeparator2, MenuDeleteItem });
      MenuTree.Name = "MenuTree";
      MenuTree.Size = new Size(173, 112);
      MenuTree.Opening += MenuTree_Opening;
      // 
      // MenuAddFrame
      // 
      MenuAddFrame.Name = "MenuAddFrame";
      MenuAddFrame.Size = new Size(172, 24);
      MenuAddFrame.Text = "Add Frame";
      MenuAddFrame.Click += MenuAddFrame_Click;
      // 
      // MenuAddElement
      // 
      MenuAddElement.Name = "MenuAddElement";
      MenuAddElement.Size = new Size(172, 24);
      MenuAddElement.Text = "Add Element";
      MenuAddElement.Click += MenuAddElement_Click;
      // 
      // toolStripSeparator1
      // 
      toolStripSeparator1.Name = "toolStripSeparator1";
      toolStripSeparator1.Size = new Size(169, 6);
      // 
      // MenuMoveUpItem
      // 
      MenuMoveUpItem.Name = "MenuMoveUpItem";
      MenuMoveUpItem.Size = new Size(172, 24);
      MenuMoveUpItem.Text = "Move Item Up";
      MenuMoveUpItem.Click += MenuMoveUpItem_Click;
      // 
      // toolStripSeparator2
      // 
      toolStripSeparator2.Name = "toolStripSeparator2";
      toolStripSeparator2.Size = new Size(169, 6);
      // 
      // MenuDeleteItem
      // 
      MenuDeleteItem.Name = "MenuDeleteItem";
      MenuDeleteItem.Size = new Size(172, 24);
      MenuDeleteItem.Text = "Delete Item";
      MenuDeleteItem.Click += MenuDeleteItem_Click;
      // 
      // imageList1
      // 
      imageList1.ColorDepth = ColorDepth.Depth8Bit;
      imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
      imageList1.TransparentColor = Color.Transparent;
      imageList1.Images.SetKeyName(0, "folder-457-16.png");
      imageList1.Images.SetKeyName(1, "gift-551-16 (1).png");
      imageList1.Images.SetKeyName(2, "data-331-16.png");
      // 
      // splitContainer4
      // 
      splitContainer4.Cursor = Cursors.VSplit;
      splitContainer4.Dock = DockStyle.Fill;
      splitContainer4.Location = new Point(0, 0);
      splitContainer4.Name = "splitContainer4";
      // 
      // splitContainer4.Panel2
      // 
      splitContainer4.Panel2.Controls.Add(label1);
      splitContainer4.Panel2.Controls.Add(tbHeight);
      splitContainer4.Panel2.Controls.Add(tbTop);
      splitContainer4.Size = new Size(761, 403);
      splitContainer4.SplitterDistance = 582;
      splitContainer4.TabIndex = 0;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(11, 7);
      label1.Name = "label1";
      label1.Size = new Size(103, 20);
      label1.TabIndex = 2;
      label1.Text = " Top     Height";
      // 
      // tbHeight
      // 
      tbHeight.Location = new Point(61, 23);
      tbHeight.Maximum = 100;
      tbHeight.Name = "tbHeight";
      tbHeight.Orientation = Orientation.Vertical;
      tbHeight.Size = new Size(56, 190);
      tbHeight.TabIndex = 1;
      tbHeight.TickStyle = TickStyle.Both;
      tbHeight.ValueChanged += tbHeight_ValueChanged;
      // 
      // tbTop
      // 
      tbTop.Location = new Point(10, 23);
      tbTop.Maximum = 100;
      tbTop.Name = "tbTop";
      tbTop.Orientation = Orientation.Vertical;
      tbTop.Size = new Size(56, 190);
      tbTop.TabIndex = 0;
      tbTop.TickStyle = TickStyle.Both;
      tbTop.Value = 10;
      tbTop.ValueChanged += tbTop_ValueChanged;
      // 
      // btnFont
      // 
      btnFont.Location = new Point(798, 83);
      btnFont.Name = "btnFont";
      btnFont.Size = new Size(112, 62);
      btnFont.TabIndex = 10;
      btnFont.Text = "Font";
      btnFont.UseVisualStyleBackColor = true;
      btnFont.Click += btnFont_Click;
      // 
      // label10
      // 
      label10.AutoSize = true;
      label10.Location = new Point(15, 17);
      label10.Name = "label10";
      label10.Size = new Size(43, 20);
      label10.TabIndex = 9;
      label10.Text = "Type:";
      // 
      // cbType
      // 
      cbType.FormattingEnabled = true;
      cbType.Items.AddRange(new object[] { "None", "Frame", "Element", "Rectangle", "Oval" });
      cbType.Location = new Point(64, 14);
      cbType.Name = "cbType";
      cbType.Size = new Size(150, 28);
      cbType.TabIndex = 8;
      cbType.SelectedIndexChanged += cbType_SelectedIndexChanged;
      // 
      // ColorBButton
      // 
      ColorBButton.Location = new Point(122, 52);
      ColorBButton.Name = "ColorBButton";
      ColorBButton.Size = new Size(52, 37);
      ColorBButton.TabIndex = 7;
      ColorBButton.Text = "B";
      ColorBButton.UseVisualStyleBackColor = true;
      ColorBButton.Click += ColorBButton_Click;
      // 
      // ColorAButton
      // 
      ColorAButton.Location = new Point(64, 52);
      ColorAButton.Name = "ColorAButton";
      ColorAButton.Size = new Size(52, 37);
      ColorAButton.TabIndex = 6;
      ColorAButton.Text = "A";
      ColorAButton.UseVisualStyleBackColor = true;
      ColorAButton.Click += ColorAButton_Click;
      // 
      // label9
      // 
      label9.AutoSize = true;
      label9.Location = new Point(216, 105);
      label9.Name = "label9";
      label9.Size = new Size(61, 20);
      label9.TabIndex = 5;
      label9.Text = "Caption";
      // 
      // edCaption
      // 
      edCaption.Location = new Point(283, 102);
      edCaption.Name = "edCaption";
      edCaption.Size = new Size(509, 27);
      edCaption.TabIndex = 4;
      edCaption.TextChanged += edCaption_TextChanged;
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new Point(216, 66);
      label3.Name = "label3";
      label3.Size = new Size(49, 20);
      label3.TabIndex = 3;
      label3.Text = "Width";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(231, 17);
      label2.Name = "label2";
      label2.Size = new Size(34, 20);
      label2.TabIndex = 2;
      label2.Text = "Left";
      // 
      // tbWidth
      // 
      tbWidth.Location = new Point(271, 52);
      tbWidth.Maximum = 100;
      tbWidth.Name = "tbWidth";
      tbWidth.Size = new Size(521, 56);
      tbWidth.TabIndex = 1;
      tbWidth.TickStyle = TickStyle.Both;
      tbWidth.ValueChanged += tbWidth_ValueChanged;
      // 
      // tbLeft
      // 
      tbLeft.Location = new Point(271, 7);
      tbLeft.Maximum = 100;
      tbLeft.Name = "tbLeft";
      tbLeft.Size = new Size(521, 56);
      tbLeft.TabIndex = 0;
      tbLeft.TickStyle = TickStyle.Both;
      tbLeft.ValueChanged += tbLeft_ValueChanged;
      // 
      // LogsTab
      // 
      LogsTab.BackColor = SystemColors.ButtonFace;
      LogsTab.Controls.Add(edLogMsg);
      LogsTab.Location = new Point(4, 27);
      LogsTab.Name = "LogsTab";
      LogsTab.Size = new Size(1013, 649);
      LogsTab.TabIndex = 2;
      LogsTab.Text = "Logs ";
      // 
      // edLogMsg
      // 
      edLogMsg.Dock = DockStyle.Fill;
      edLogMsg.Location = new Point(0, 0);
      edLogMsg.Multiline = true;
      edLogMsg.Name = "edLogMsg";
      edLogMsg.Size = new Size(1013, 649);
      edLogMsg.TabIndex = 0;
      // 
      // openFileDialog1
      // 
      openFileDialog1.CheckFileExists = false;
      openFileDialog1.DefaultExt = "dfm";
      openFileDialog1.FileName = "openFileDialog1";
      openFileDialog1.Filter = "dfm|*.dfm|All|*.*";
      // 
      // timerDraw
      // 
      timerDraw.Interval = 333;
      timerDraw.Tick += timerDraw_Tick;
      // 
      // Form1
      // 
      AutoScaleDimensions = new SizeF(8F, 20F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(1021, 680);
      Controls.Add(TabCtrlRoot);
      Name = "Form1";
      Text = "Form1";
      Shown += Form1_Shown;
      TabCtrlRoot.ResumeLayout(false);
      SetupTab.ResumeLayout(false);
      SetupTab.PerformLayout();
      DesignTab.ResumeLayout(false);
      splitContainer1.Panel1.ResumeLayout(false);
      splitContainer1.Panel1.PerformLayout();
      splitContainer1.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
      splitContainer1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)trackBar6).EndInit();
      ((System.ComponentModel.ISupportInitialize)tbFrame).EndInit();
      splitContainer2.Panel1.ResumeLayout(false);
      splitContainer2.Panel2.ResumeLayout(false);
      splitContainer2.Panel2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
      splitContainer2.ResumeLayout(false);
      splitContainer3.Panel1.ResumeLayout(false);
      splitContainer3.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)splitContainer3).EndInit();
      splitContainer3.ResumeLayout(false);
      MenuTree.ResumeLayout(false);
      splitContainer4.Panel2.ResumeLayout(false);
      splitContainer4.Panel2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)splitContainer4).EndInit();
      splitContainer4.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)tbHeight).EndInit();
      ((System.ComponentModel.ISupportInitialize)tbTop).EndInit();
      ((System.ComponentModel.ISupportInitialize)tbWidth).EndInit();
      ((System.ComponentModel.ISupportInitialize)tbLeft).EndInit();
      LogsTab.ResumeLayout(false);
      LogsTab.PerformLayout();
      ResumeLayout(false);
    }

    #endregion
    private TabControl TabCtrlRoot;
    private TabPage SetupTab;
    private TabPage DesignTab;
    private SplitContainer splitContainer1;
    private SplitContainer splitContainer2;
    private SplitContainer splitContainer3;
    private TreeView treeView1;
    private SplitContainer splitContainer4;
    private Label label6;
    private Label label5;
    private Label label4;
    private ComboBox cbAspectRatio;
    private TrackBar tbFrame;
    private Label label1;
    private TrackBar tbHeight;
    private TrackBar tbTop;
    private Label label3;
    private Label label2;
    private TrackBar tbWidth;
    private TrackBar tbLeft;
    private ComboBox cbProjectFileName;
    private Label label8;
    private Button BrowseFileNameButton;
    private Label label7;
    private TrackBar trackBar6;
    private TabPage LogsTab;
    private TextBox edLogMsg;
    private OpenFileDialog openFileDialog1;
    private ImageList imageList1;
    private ContextMenuStrip MenuTree;
    private ToolStripMenuItem MenuAddFrame;
    private ToolStripMenuItem MenuAddElement;
    private ToolStripMenuItem MenuDeleteItem;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem MenuMoveUpItem;
    private ToolStripSeparator toolStripSeparator2;
    private ColorDialog colorDialog1;
    private Button ColorAButton;
    private Label label9;
    private TextBox edCaption;
    private FontDialog fontDialog1;
    private Button ColorBButton;
    private System.Windows.Forms.Timer timerDraw;
    private Label label10;
    private ComboBox cbType;
    private Button btnFont;
  }
}