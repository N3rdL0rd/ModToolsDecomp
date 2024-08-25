// Decompiled with JetBrains decompiler
// Type: ScriptTool.LevelMobForm
// Assembly: ScriptTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2726570-3B7A-4F0B-A465-A10C11BE2151
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\ScriptTool.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace ScriptTool
{
  public class LevelMobForm : Form
  {
    private IContainer components;
    private DataGridView dataGridView;
    private Button cmdClear;
    private Button cmdOk;

    public static BindingList<Mob> mobRoster { get; private set; } = new BindingList<Mob>();

    public LevelMobForm(bool _resetRoster)
    {
      this.InitializeComponent();
      if (!_resetRoster)
        return;
      LevelMobForm.mobRoster.Clear();
    }

    private void LevelMobForm_Load(object _sender, EventArgs _args)
    {
      DataGridViewComboBoxColumn viewComboBoxColumn = new DataGridViewComboBoxColumn();
      viewComboBoxColumn.DataSource = (object) MobList.ids;
      viewComboBoxColumn.HeaderText = "mobName";
      viewComboBoxColumn.DataPropertyName = "mobName";
      this.dataGridView.Columns.Add((DataGridViewColumn) viewComboBoxColumn);
      this.dataGridView.DataSource = (object) LevelMobForm.mobRoster;
      this.dataGridView.Refresh();
    }

    private void cmdClear_Click(object _sender, EventArgs _args)
    {
      if (MessageBox.Show("You are about to clear every mobs from the current roster, are you sure you want to remove everything (cannot revert)?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
        return;
      LevelMobForm.mobRoster.Clear();
      this.dataGridView.Refresh();
    }

    private void cmdOk_Click(object _sender, EventArgs _args)
    {
      this.Close();
      this.Dispose();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.dataGridView = new DataGridView();
      this.cmdClear = new Button();
      this.cmdOk = new Button();
      ((ISupportInitialize) this.dataGridView).BeginInit();
      this.SuspendLayout();
      this.dataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView.Location = new Point(13, 13);
      this.dataGridView.Name = "dataGridView";
      this.dataGridView.Size = new Size(965, 369);
      this.dataGridView.TabIndex = 0;
      this.cmdClear.Location = new Point(903, 388);
      this.cmdClear.Name = "cmdClear";
      this.cmdClear.Size = new Size(75, 23);
      this.cmdClear.TabIndex = 1;
      this.cmdClear.Text = "Clear";
      this.cmdClear.UseVisualStyleBackColor = true;
      this.cmdClear.Click += new EventHandler(this.cmdClear_Click);
      this.cmdOk.Location = new Point(822, 388);
      this.cmdOk.Name = "cmdOk";
      this.cmdOk.Size = new Size(75, 23);
      this.cmdOk.TabIndex = 2;
      this.cmdOk.Text = "Ok";
      this.cmdOk.UseVisualStyleBackColor = true;
      this.cmdOk.Click += new EventHandler(this.cmdOk_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(990, 421);
      this.Controls.Add((Control) this.cmdOk);
      this.Controls.Add((Control) this.cmdClear);
      this.Controls.Add((Control) this.dataGridView);
      this.Name = nameof (LevelMobForm);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = nameof (LevelMobForm);
      this.Load += new EventHandler(this.LevelMobForm_Load);
      ((ISupportInitialize) this.dataGridView).EndInit();
      this.ResumeLayout(false);
    }
  }
}
