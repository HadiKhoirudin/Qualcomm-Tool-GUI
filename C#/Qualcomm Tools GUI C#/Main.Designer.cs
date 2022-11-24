using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Qualcomm_Tools_GUI
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class Main : Form
    {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is not null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.RichTextBox = new System.Windows.Forms.RichTextBox();
            this.btnrawxml = new System.Windows.Forms.Button();
            this.txtrawxml = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.cbstorage = new System.Windows.Forms.ComboBox();
            this.btngpt = new System.Windows.Forms.Button();
            this.Comboboxport = new System.Windows.Forms.ComboBox();
            this.labeltimer = new System.Windows.Forms.Label();
            this.btnloader = new System.Windows.Forms.Button();
            this.txtloader = new System.Windows.Forms.TextBox();
            this.btnbackup = new System.Windows.Forms.Button();
            this.btnflash = new System.Windows.Forms.Button();
            this.txtpatchxml = new System.Windows.Forms.TextBox();
            this.btnpatchxml = new System.Windows.Forms.Button();
            this.ProgressBar1 = new System.Windows.Forms.ProgressBar();
            this.ProgressBar2 = new System.Windows.Forms.ProgressBar();
            this.DataView = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cblistdataview = new System.Windows.Forms.CheckBox();
            this.cbreboot = new System.Windows.Forms.CheckBox();
            this.btnerase = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DataView)).BeginInit();
            this.SuspendLayout();
            // 
            // RichTextBox
            // 
            this.RichTextBox.BackColor = System.Drawing.Color.Black;
            this.RichTextBox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RichTextBox.Location = new System.Drawing.Point(650, 39);
            this.RichTextBox.Name = "RichTextBox";
            this.RichTextBox.Size = new System.Drawing.Size(392, 368);
            this.RichTextBox.TabIndex = 1;
            this.RichTextBox.Text = "";
            this.RichTextBox.TextChanged += new System.EventHandler(this.RichTextBox_TextChanged);
            // 
            // btnrawxml
            // 
            this.btnrawxml.Location = new System.Drawing.Point(12, 384);
            this.btnrawxml.Name = "btnrawxml";
            this.btnrawxml.Size = new System.Drawing.Size(75, 23);
            this.btnrawxml.TabIndex = 2;
            this.btnrawxml.Text = "Raw XML";
            this.btnrawxml.UseVisualStyleBackColor = true;
            this.btnrawxml.Click += new System.EventHandler(this.btnrawxml_Click);
            // 
            // txtrawxml
            // 
            this.txtrawxml.Location = new System.Drawing.Point(93, 386);
            this.txtrawxml.Name = "txtrawxml";
            this.txtrawxml.Size = new System.Drawing.Size(551, 20);
            this.txtrawxml.TabIndex = 3;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(480, 312);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(53, 13);
            this.Label1.TabIndex = 4;
            this.Label1.Text = "Storage : ";
            // 
            // cbstorage
            // 
            this.cbstorage.FormattingEnabled = true;
            this.cbstorage.Items.AddRange(new object[] {
            "emmc",
            "ufs"});
            this.cbstorage.Location = new System.Drawing.Point(544, 309);
            this.cbstorage.Name = "cbstorage";
            this.cbstorage.Size = new System.Drawing.Size(100, 21);
            this.cbstorage.TabIndex = 5;
            this.cbstorage.Text = "emmc";
            this.cbstorage.SelectedIndexChanged += new System.EventHandler(this.cbstorage_SelectedIndexChanged);
            // 
            // btngpt
            // 
            this.btngpt.Location = new System.Drawing.Point(104, 307);
            this.btngpt.Name = "btngpt";
            this.btngpt.Size = new System.Drawing.Size(88, 23);
            this.btngpt.TabIndex = 6;
            this.btngpt.Text = "Read GPT";
            this.btngpt.UseVisualStyleBackColor = true;
            this.btngpt.Click += new System.EventHandler(this.btngpt_Click);
            // 
            // Comboboxport
            // 
            this.Comboboxport.FormattingEnabled = true;
            this.Comboboxport.Location = new System.Drawing.Point(650, 12);
            this.Comboboxport.Name = "Comboboxport";
            this.Comboboxport.Size = new System.Drawing.Size(392, 21);
            this.Comboboxport.TabIndex = 7;
            // 
            // labeltimer
            // 
            this.labeltimer.AutoSize = true;
            this.labeltimer.Location = new System.Drawing.Point(1020, 15);
            this.labeltimer.Name = "labeltimer";
            this.labeltimer.Size = new System.Drawing.Size(19, 13);
            this.labeltimer.TabIndex = 8;
            this.labeltimer.Text = "[  ]";
            // 
            // btnloader
            // 
            this.btnloader.Location = new System.Drawing.Point(13, 332);
            this.btnloader.Name = "btnloader";
            this.btnloader.Size = new System.Drawing.Size(75, 23);
            this.btnloader.TabIndex = 9;
            this.btnloader.Text = "Loader";
            this.btnloader.UseVisualStyleBackColor = true;
            this.btnloader.Click += new System.EventHandler(this.btnloader_Click);
            // 
            // txtloader
            // 
            this.txtloader.Location = new System.Drawing.Point(93, 334);
            this.txtloader.Name = "txtloader";
            this.txtloader.Size = new System.Drawing.Size(551, 20);
            this.txtloader.TabIndex = 10;
            // 
            // btnbackup
            // 
            this.btnbackup.Location = new System.Drawing.Point(198, 307);
            this.btnbackup.Name = "btnbackup";
            this.btnbackup.Size = new System.Drawing.Size(88, 23);
            this.btnbackup.TabIndex = 11;
            this.btnbackup.Text = "Backup";
            this.btnbackup.UseVisualStyleBackColor = true;
            this.btnbackup.Click += new System.EventHandler(this.btnbackup_Click);
            // 
            // btnflash
            // 
            this.btnflash.Location = new System.Drawing.Point(386, 307);
            this.btnflash.Name = "btnflash";
            this.btnflash.Size = new System.Drawing.Size(88, 23);
            this.btnflash.TabIndex = 12;
            this.btnflash.Text = "Flash";
            this.btnflash.UseVisualStyleBackColor = true;
            this.btnflash.Click += new System.EventHandler(this.btnflash_Click);
            // 
            // txtpatchxml
            // 
            this.txtpatchxml.Location = new System.Drawing.Point(94, 360);
            this.txtpatchxml.Name = "txtpatchxml";
            this.txtpatchxml.Size = new System.Drawing.Size(550, 20);
            this.txtpatchxml.TabIndex = 14;
            // 
            // btnpatchxml
            // 
            this.btnpatchxml.Location = new System.Drawing.Point(13, 358);
            this.btnpatchxml.Name = "btnpatchxml";
            this.btnpatchxml.Size = new System.Drawing.Size(75, 23);
            this.btnpatchxml.TabIndex = 13;
            this.btnpatchxml.Text = "Patch XML";
            this.btnpatchxml.UseVisualStyleBackColor = true;
            this.btnpatchxml.Click += new System.EventHandler(this.btnpatchxml_Click);
            // 
            // ProgressBar1
            // 
            this.ProgressBar1.Location = new System.Drawing.Point(13, 284);
            this.ProgressBar1.Margin = new System.Windows.Forms.Padding(0);
            this.ProgressBar1.Name = "ProgressBar1";
            this.ProgressBar1.Size = new System.Drawing.Size(631, 10);
            this.ProgressBar1.TabIndex = 15;
            // 
            // ProgressBar2
            // 
            this.ProgressBar2.Location = new System.Drawing.Point(13, 294);
            this.ProgressBar2.Margin = new System.Windows.Forms.Padding(0);
            this.ProgressBar2.Name = "ProgressBar2";
            this.ProgressBar2.Size = new System.Drawing.Size(631, 10);
            this.ProgressBar2.TabIndex = 16;
            // 
            // DataView
            // 
            this.DataView.AllowUserToAddRows = false;
            this.DataView.AllowUserToDeleteRows = false;
            this.DataView.BackgroundColor = System.Drawing.Color.White;
            this.DataView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.DataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7});
            this.DataView.Location = new System.Drawing.Point(13, 12);
            this.DataView.Name = "DataView";
            this.DataView.RowHeadersVisible = false;
            this.DataView.Size = new System.Drawing.Size(631, 269);
            this.DataView.TabIndex = 17;
            this.DataView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataView_CellDoubleClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = " ";
            this.Column1.Name = "Column1";
            this.Column1.Width = 20;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Lun";
            this.Column2.Name = "Column2";
            this.Column2.Width = 50;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Block";
            this.Column3.Name = "Column3";
            this.Column3.Width = 50;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Partition";
            this.Column4.Name = "Column4";
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Sector Size";
            this.Column5.Name = "Column5";
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Partition Size";
            this.Column6.Name = "Column6";
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Location";
            this.Column7.Name = "Column7";
            this.Column7.Width = 300;
            // 
            // cblistdataview
            // 
            this.cblistdataview.AutoSize = true;
            this.cblistdataview.Location = new System.Drawing.Point(18, 17);
            this.cblistdataview.Name = "cblistdataview";
            this.cblistdataview.Size = new System.Drawing.Size(15, 14);
            this.cblistdataview.TabIndex = 18;
            this.cblistdataview.UseVisualStyleBackColor = true;
            this.cblistdataview.CheckedChanged += new System.EventHandler(this.cblistdataview_CheckedChanged);
            // 
            // cbreboot
            // 
            this.cbreboot.AutoSize = true;
            this.cbreboot.Location = new System.Drawing.Point(13, 311);
            this.cbreboot.Name = "cbreboot";
            this.cbreboot.Size = new System.Drawing.Size(86, 17);
            this.cbreboot.TabIndex = 19;
            this.cbreboot.Text = "Auto Reboot";
            this.cbreboot.UseVisualStyleBackColor = true;
            // 
            // btnerase
            // 
            this.btnerase.Location = new System.Drawing.Point(292, 307);
            this.btnerase.Name = "btnerase";
            this.btnerase.Size = new System.Drawing.Size(88, 23);
            this.btnerase.TabIndex = 20;
            this.btnerase.Text = "Erase";
            this.btnerase.UseVisualStyleBackColor = true;
            this.btnerase.Click += new System.EventHandler(this.btnerase_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1051, 417);
            this.Controls.Add(this.btnerase);
            this.Controls.Add(this.cbreboot);
            this.Controls.Add(this.cblistdataview);
            this.Controls.Add(this.DataView);
            this.Controls.Add(this.ProgressBar2);
            this.Controls.Add(this.ProgressBar1);
            this.Controls.Add(this.txtpatchxml);
            this.Controls.Add(this.btnpatchxml);
            this.Controls.Add(this.btnflash);
            this.Controls.Add(this.btnbackup);
            this.Controls.Add(this.txtloader);
            this.Controls.Add(this.btnloader);
            this.Controls.Add(this.labeltimer);
            this.Controls.Add(this.Comboboxport);
            this.Controls.Add(this.btngpt);
            this.Controls.Add(this.cbstorage);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.txtrawxml);
            this.Controls.Add(this.btnrawxml);
            this.Controls.Add(this.RichTextBox);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Qualcomm Tool GUI C#";
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DataView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        internal RichTextBox RichTextBox;
        internal Button btnrawxml;
        internal TextBox txtrawxml;
        internal Label Label1;
        internal ComboBox cbstorage;
        internal Button btngpt;
        internal ComboBox Comboboxport;
        internal Label labeltimer;
        internal Button btnloader;
        internal TextBox txtloader;
        internal Button btnbackup;
        internal Button btnflash;
        internal TextBox txtpatchxml;
        internal Button btnpatchxml;
        internal ProgressBar ProgressBar1;
        internal ProgressBar ProgressBar2;
        internal DataGridView DataView;
        internal DataGridViewCheckBoxColumn Column1;
        internal DataGridViewTextBoxColumn Column2;
        internal DataGridViewTextBoxColumn Column3;
        internal DataGridViewTextBoxColumn Column4;
        internal DataGridViewTextBoxColumn Column5;
        internal DataGridViewTextBoxColumn Column6;
        internal DataGridViewTextBoxColumn Column7;
        internal CheckBox cblistdataview;
        internal CheckBox cbreboot;
        internal Button btnerase;
    }
}