﻿namespace RTC
{
    partial class RTC_StockpilePlayer_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_StockpilePlayer_Form));
            this.btnNextItem = new System.Windows.Forms.Button();
            this.btnReloadItem = new System.Windows.Forms.Button();
            this.btnPreviousItem = new System.Windows.Forms.Button();
            this.btnLoadStockpile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBlastToggle = new System.Windows.Forms.Button();
            this.dgvStockpile = new System.Windows.Forms.DataGridView();
            this.Item = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GameName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SystemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SystemCore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Note = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockpile)).BeginInit();
            this.SuspendLayout();
            // 
            // btnNextItem
            // 
            this.btnNextItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNextItem.BackColor = System.Drawing.Color.LightGreen;
            this.btnNextItem.FlatAppearance.BorderSize = 0;
            this.btnNextItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnNextItem.ForeColor = System.Drawing.Color.Black;
            this.btnNextItem.Location = new System.Drawing.Point(582, 17);
            this.btnNextItem.Name = "btnNextItem";
            this.btnNextItem.Size = new System.Drawing.Size(57, 32);
            this.btnNextItem.TabIndex = 127;
            this.btnNextItem.Text = "Next";
            this.btnNextItem.UseVisualStyleBackColor = false;
            this.btnNextItem.Click += new System.EventHandler(this.btnNextItem_Click);
            // 
            // btnReloadItem
            // 
            this.btnReloadItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReloadItem.BackColor = System.Drawing.Color.LightGreen;
            this.btnReloadItem.FlatAppearance.BorderSize = 0;
            this.btnReloadItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReloadItem.ForeColor = System.Drawing.Color.Black;
            this.btnReloadItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.reboot;
            this.btnReloadItem.Location = new System.Drawing.Point(538, 17);
            this.btnReloadItem.Name = "btnReloadItem";
            this.btnReloadItem.Size = new System.Drawing.Size(35, 32);
            this.btnReloadItem.TabIndex = 126;
            this.btnReloadItem.UseVisualStyleBackColor = false;
            this.btnReloadItem.Click += new System.EventHandler(this.btnReloadItem_Click);
            // 
            // btnPreviousItem
            // 
            this.btnPreviousItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPreviousItem.BackColor = System.Drawing.Color.LightGreen;
            this.btnPreviousItem.FlatAppearance.BorderSize = 0;
            this.btnPreviousItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPreviousItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnPreviousItem.ForeColor = System.Drawing.Color.Black;
            this.btnPreviousItem.Location = new System.Drawing.Point(455, 17);
            this.btnPreviousItem.Name = "btnPreviousItem";
            this.btnPreviousItem.Size = new System.Drawing.Size(74, 32);
            this.btnPreviousItem.TabIndex = 124;
            this.btnPreviousItem.Text = "Previous";
            this.btnPreviousItem.UseVisualStyleBackColor = false;
            this.btnPreviousItem.Click += new System.EventHandler(this.btnPreviousItem_Click);
            // 
            // btnLoadStockpile
            // 
            this.btnLoadStockpile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadStockpile.BackColor = System.Drawing.Color.Orange;
            this.btnLoadStockpile.FlatAppearance.BorderSize = 0;
            this.btnLoadStockpile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadStockpile.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnLoadStockpile.ForeColor = System.Drawing.Color.Black;
            this.btnLoadStockpile.Location = new System.Drawing.Point(368, 17);
            this.btnLoadStockpile.Name = "btnLoadStockpile";
            this.btnLoadStockpile.Size = new System.Drawing.Size(78, 32);
            this.btnLoadStockpile.TabIndex = 123;
            this.btnLoadStockpile.Text = "Load";
            this.btnLoadStockpile.UseVisualStyleBackColor = false;
            this.btnLoadStockpile.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnLoadStockpile_MouseDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 26.25F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.Bisque;
            this.label2.Location = new System.Drawing.Point(11, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(271, 47);
            this.label2.TabIndex = 83;
            this.label2.Text = "Stockpile Player";
            // 
            // btnBlastToggle
            // 
            this.btnBlastToggle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBlastToggle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnBlastToggle.FlatAppearance.BorderSize = 0;
            this.btnBlastToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBlastToggle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnBlastToggle.ForeColor = System.Drawing.Color.White;
            this.btnBlastToggle.Location = new System.Drawing.Point(13, 469);
            this.btnBlastToggle.Name = "btnBlastToggle";
            this.btnBlastToggle.Size = new System.Drawing.Size(627, 32);
            this.btnBlastToggle.TabIndex = 131;
            this.btnBlastToggle.TabStop = false;
            this.btnBlastToggle.Tag = "color:darker";
            this.btnBlastToggle.Text = "BlastLayer : OFF    (Attempts to uncorrupt/recorrupt in real-time)";
            this.btnBlastToggle.UseVisualStyleBackColor = false;
            this.btnBlastToggle.Click += new System.EventHandler(this.btnBlastToggle_Click);
            // 
            // dgvStockpile
            // 
            this.dgvStockpile.AllowUserToAddRows = false;
            this.dgvStockpile.AllowUserToDeleteRows = false;
            this.dgvStockpile.AllowUserToResizeRows = false;
            this.dgvStockpile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvStockpile.BackgroundColor = System.Drawing.Color.Gray;
            this.dgvStockpile.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvStockpile.ColumnHeadersHeight = 21;
            this.dgvStockpile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvStockpile.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Item,
            this.GameName,
            this.SystemName,
            this.SystemCore,
            this.Note});
            this.dgvStockpile.GridColor = System.Drawing.Color.Black;
            this.dgvStockpile.Location = new System.Drawing.Point(13, 64);
            this.dgvStockpile.MultiSelect = false;
            this.dgvStockpile.Name = "dgvStockpile";
            this.dgvStockpile.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dgvStockpile.RowHeadersVisible = false;
            this.dgvStockpile.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStockpile.Size = new System.Drawing.Size(627, 391);
            this.dgvStockpile.TabIndex = 142;
            this.dgvStockpile.Tag = "color:normal";
            this.dgvStockpile.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockpile_CellClick);
            this.dgvStockpile.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvStockpile_MouseDown);
            // 
            // Item
            // 
            this.Item.HeaderText = "Item Name";
            this.Item.Name = "Item";
            this.Item.ReadOnly = true;
            this.Item.Width = 260;
            // 
            // GameName
            // 
            this.GameName.HeaderText = "Game";
            this.GameName.Name = "GameName";
            this.GameName.ReadOnly = true;
            this.GameName.Width = 106;
            // 
            // SystemName
            // 
            this.SystemName.HeaderText = "System";
            this.SystemName.Name = "SystemName";
            this.SystemName.ReadOnly = true;
            this.SystemName.Width = 45;
            // 
            // SystemCore
            // 
            this.SystemCore.HeaderText = "Core";
            this.SystemCore.Name = "SystemCore";
            this.SystemCore.Width = 150;
            // 
            // Note
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            this.Note.DefaultCellStyle = dataGridViewCellStyle1;
            this.Note.HeaderText = "Note";
            this.Note.Name = "Note";
            this.Note.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Note.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Note.Width = 35;
            // 
            // RTC_SP_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(655, 515);
            this.Controls.Add(this.btnBlastToggle);
            this.Controls.Add(this.btnNextItem);
            this.Controls.Add(this.dgvStockpile);
            this.Controls.Add(this.btnReloadItem);
            this.Controls.Add(this.btnPreviousItem);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnLoadStockpile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(655, 515);
            this.Name = "RTC_SP_Form";
            this.Tag = "color:dark";
            this.Text = "RTC : Stockpile Player";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RTC_BE_Form_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockpile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnLoadStockpile;
		private System.Windows.Forms.Button btnNextItem;
		private System.Windows.Forms.Button btnReloadItem;
		private System.Windows.Forms.Button btnPreviousItem;
		public System.Windows.Forms.Button btnBlastToggle;
		public System.Windows.Forms.DataGridView dgvStockpile;
        private System.Windows.Forms.DataGridViewTextBoxColumn Item;
        private System.Windows.Forms.DataGridViewTextBoxColumn GameName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SystemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SystemCore;
        private System.Windows.Forms.DataGridViewButtonColumn Note;
    }
}