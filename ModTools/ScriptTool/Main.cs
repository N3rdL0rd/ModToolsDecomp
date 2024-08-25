// Decompiled with JetBrains decompiler
// Type: ScriptTool.Main
// Assembly: ScriptTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2726570-3B7A-4F0B-A465-A10C11BE2151
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\ScriptTool.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace ScriptTool
{
  public class Main : Form
  {
    private Options m_Options;
    private string m_CDBJson = "";
    private string m_CurrentFileName = "";
    private IContainer components;
    private Panel panel;
    private RichTextBox richTextBox;
    private MenuStrip menuStrip;
    private ToolStripMenuItem fileToolStripMenuItem;
    private ToolStripMenuItem newToolStripMenuItem;
    private ToolStripMenuItem openToolStripMenuItem;
    private ToolStripMenuItem quitToolStripMenuItem;
    private Label lblCurrentFileName;
    private Button cmdNewScript;
    private Button cmdGenerateMobRoster;
    private Button cmdCopyToClipboard;
    private Button cmdPickColor;
    private ColorDialog colorDialog;

    public Main() => this.InitializeComponent();

    private void openToolStripMenuItem_Click(object _sender, EventArgs _args)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "hscript files|*.hx";
      if (openFileDialog.ShowDialog() != DialogResult.OK)
        return;
      this.Open(openFileDialog.FileName);
    }

    private void Open(string _fullFileName)
    {
      this.m_CurrentFileName = _fullFileName;
      this.lblCurrentFileName.Text = new FileInfo(this.m_CurrentFileName).Name;
      this.richTextBox.Text = File.ReadAllText(_fullFileName);
      this.richTextBox.Enabled = true;
      this.panel.Enabled = true;
    }

    private void Main_Load(object _sender, EventArgs _args)
    {
      this.Resize += new EventHandler(this.MainResized);
      try
      {
        this.m_Options = Options.FromJson(File.ReadAllText("options.json"));
      }
      catch (FileNotFoundException ex)
      {
        this.m_Options = (Options) null;
      }
      if (this.m_Options == null)
      {
        this.m_Options = new Options();
        this.SetRefCDBPath();
      }
      while (this.m_Options.refCDBPath == "" || !File.Exists(this.m_Options.refCDBPath))
        this.SetRefCDBPath();
      this.m_CDBJson = File.ReadAllText(this.m_Options.refCDBPath);
      MobList.InitMobList(this.m_CDBJson);
      this.MainResized((object) null, (EventArgs) null);
    }

    private void SetRefCDBPath()
    {
      int num = (int) MessageBox.Show("The reference CDB has not be found, please pick the reference CDB in the next dialog", "CDB not found", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "data.cdb|data.cdb";
      if (openFileDialog.ShowDialog() != DialogResult.OK)
        return;
      this.m_Options.refCDBPath = openFileDialog.FileName;
      this.SaveOptions();
    }

    private void SaveOptions()
    {
      if (this.m_Options == null)
        return;
      File.WriteAllText("options.json", this.m_Options.ToJson());
    }

    private void MainResized(object _sender, EventArgs _args)
    {
      int num = this.Width / 8;
      this.panel.Width = num - this.panel.Left - 2;
      int right = this.richTextBox.Right;
      this.richTextBox.Left = num + 2;
      this.richTextBox.Width = right - this.richTextBox.Left;
    }

    private void quitToolStripMenuItem_Click(object _sender, EventArgs _args) => Application.Exit();

    private void newToolStripMenuItem_Click(object _sender, EventArgs _args)
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = "hscript files|*.hx";
      if (saveFileDialog.ShowDialog() != DialogResult.OK)
        return;
      File.WriteAllText(saveFileDialog.FileName, ScriptWriter.instance.WriteWholeScript());
      this.Open(saveFileDialog.FileName);
    }

    private void generateDefaultScript_Click(object _sender, EventArgs _args)
    {
      this.lblCurrentFileName.Text = "";
      this.richTextBox.Text = ScriptWriter.instance.WriteWholeScript();
    }

    private void cmdGenerateMobRoster_Click(object _sender, EventArgs _args)
    {
      this.lblCurrentFileName.Text = "";
      int num = (int) new LevelMobForm(false).ShowDialog((IWin32Window) this);
      this.richTextBox.Text = ScriptWriter.instance.WriteWholeScript();
    }

    private void cmdCopyToClipboard_Click(object _sender, EventArgs _args)
    {
      Clipboard.SetText(this.richTextBox.Text);
    }

    private void cmdPickColor_Click(object _sender, EventArgs _args)
    {
      if (this.colorDialog.ShowDialog() != DialogResult.OK)
        return;
      Color color = this.colorDialog.Color;
      uint num1 = (uint) ((int) color.R << 16 | (int) color.G << 8) | (uint) color.B;
      Clipboard.SetText(num1.ToString());
      int num2 = (int) MessageBox.Show("The value is " + (object) num1 + " and has been copied to the clipboard. Use ctrl + V in your script to put the value", "Color copied", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel = new Panel();
      this.cmdCopyToClipboard = new Button();
      this.cmdGenerateMobRoster = new Button();
      this.cmdNewScript = new Button();
      this.richTextBox = new RichTextBox();
      this.menuStrip = new MenuStrip();
      this.fileToolStripMenuItem = new ToolStripMenuItem();
      this.newToolStripMenuItem = new ToolStripMenuItem();
      this.openToolStripMenuItem = new ToolStripMenuItem();
      this.quitToolStripMenuItem = new ToolStripMenuItem();
      this.lblCurrentFileName = new Label();
      this.cmdPickColor = new Button();
      this.colorDialog = new ColorDialog();
      this.panel.SuspendLayout();
      this.menuStrip.SuspendLayout();
      this.SuspendLayout();
      this.panel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.panel.Controls.Add((Control) this.cmdPickColor);
      this.panel.Controls.Add((Control) this.cmdCopyToClipboard);
      this.panel.Controls.Add((Control) this.cmdGenerateMobRoster);
      this.panel.Controls.Add((Control) this.cmdNewScript);
      this.panel.Location = new Point(12, 60);
      this.panel.Name = "panel";
      this.panel.Size = new Size(396, 609);
      this.panel.TabIndex = 0;
      this.cmdCopyToClipboard.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cmdCopyToClipboard.Location = new Point(256, 583);
      this.cmdCopyToClipboard.Name = "cmdCopyToClipboard";
      this.cmdCopyToClipboard.Size = new Size(137, 23);
      this.cmdCopyToClipboard.TabIndex = 2;
      this.cmdCopyToClipboard.Text = "Copy script to clipboard";
      this.cmdCopyToClipboard.UseVisualStyleBackColor = true;
      this.cmdCopyToClipboard.Click += new EventHandler(this.cmdCopyToClipboard_Click);
      this.cmdGenerateMobRoster.Location = new Point(5, 33);
      this.cmdGenerateMobRoster.Name = "cmdGenerateMobRoster";
      this.cmdGenerateMobRoster.Size = new Size(141, 23);
      this.cmdGenerateMobRoster.TabIndex = 1;
      this.cmdGenerateMobRoster.Text = "Generate mob roster";
      this.cmdGenerateMobRoster.UseVisualStyleBackColor = true;
      this.cmdGenerateMobRoster.Click += new EventHandler(this.cmdGenerateMobRoster_Click);
      this.cmdNewScript.Location = new Point(5, 4);
      this.cmdNewScript.Name = "cmdNewScript";
      this.cmdNewScript.Size = new Size(141, 23);
      this.cmdNewScript.TabIndex = 0;
      this.cmdNewScript.Text = "Generate default script";
      this.cmdNewScript.UseVisualStyleBackColor = true;
      this.cmdNewScript.Click += new EventHandler(this.generateDefaultScript_Click);
      this.richTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
      this.richTextBox.Font = new Font("Consolas", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.richTextBox.Location = new Point(418, 60);
      this.richTextBox.Name = "richTextBox";
      this.richTextBox.ReadOnly = true;
      this.richTextBox.Size = new Size(834, 609);
      this.richTextBox.TabIndex = 1;
      this.richTextBox.Text = "";
      this.richTextBox.WordWrap = false;
      this.menuStrip.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.fileToolStripMenuItem
      });
      this.menuStrip.Location = new Point(0, 0);
      this.menuStrip.Name = "menuStrip";
      this.menuStrip.Size = new Size(1264, 24);
      this.menuStrip.TabIndex = 2;
      this.menuStrip.Text = "menuStrip1";
      this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.newToolStripMenuItem,
        (ToolStripItem) this.openToolStripMenuItem,
        (ToolStripItem) this.quitToolStripMenuItem
      });
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new Size(37, 20);
      this.fileToolStripMenuItem.Text = "File";
      this.newToolStripMenuItem.Name = "newToolStripMenuItem";
      this.newToolStripMenuItem.ShortcutKeys = Keys.N | Keys.Control;
      this.newToolStripMenuItem.Size = new Size(146, 22);
      this.newToolStripMenuItem.Text = "New";
      this.newToolStripMenuItem.Click += new EventHandler(this.newToolStripMenuItem_Click);
      this.openToolStripMenuItem.Name = "openToolStripMenuItem";
      this.openToolStripMenuItem.ShortcutKeys = Keys.O | Keys.Control;
      this.openToolStripMenuItem.Size = new Size(146, 22);
      this.openToolStripMenuItem.Text = "Open";
      this.openToolStripMenuItem.Click += new EventHandler(this.openToolStripMenuItem_Click);
      this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
      this.quitToolStripMenuItem.Size = new Size(146, 22);
      this.quitToolStripMenuItem.Text = "Quit";
      this.quitToolStripMenuItem.Click += new EventHandler(this.quitToolStripMenuItem_Click);
      this.lblCurrentFileName.AutoSize = true;
      this.lblCurrentFileName.Font = new Font("Consolas", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblCurrentFileName.Location = new Point(13, 33);
      this.lblCurrentFileName.Name = "lblCurrentFileName";
      this.lblCurrentFileName.Size = new Size(0, 19);
      this.lblCurrentFileName.TabIndex = 3;
      this.cmdPickColor.Location = new Point(5, 62);
      this.cmdPickColor.Name = "cmdPickColor";
      this.cmdPickColor.Size = new Size(141, 23);
      this.cmdPickColor.TabIndex = 3;
      this.cmdPickColor.Text = "Get script color";
      this.cmdPickColor.UseVisualStyleBackColor = true;
      this.cmdPickColor.Click += new EventHandler(this.cmdPickColor_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1264, 681);
      this.Controls.Add((Control) this.lblCurrentFileName);
      this.Controls.Add((Control) this.richTextBox);
      this.Controls.Add((Control) this.panel);
      this.Controls.Add((Control) this.menuStrip);
      this.MainMenuStrip = this.menuStrip;
      this.Name = nameof (Main);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "ScriptTool";
      this.Load += new EventHandler(this.Main_Load);
      this.panel.ResumeLayout(false);
      this.menuStrip.ResumeLayout(false);
      this.menuStrip.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
