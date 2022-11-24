<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.RichTextBox = New System.Windows.Forms.RichTextBox()
        Me.btnrawxml = New System.Windows.Forms.Button()
        Me.txtrawxml = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cbstorage = New System.Windows.Forms.ComboBox()
        Me.btngpt = New System.Windows.Forms.Button()
        Me.Comboboxport = New System.Windows.Forms.ComboBox()
        Me.labeltimer = New System.Windows.Forms.Label()
        Me.btnloader = New System.Windows.Forms.Button()
        Me.txtloader = New System.Windows.Forms.TextBox()
        Me.btnbackup = New System.Windows.Forms.Button()
        Me.btnflash = New System.Windows.Forms.Button()
        Me.txtpatchxml = New System.Windows.Forms.TextBox()
        Me.btnpatchxml = New System.Windows.Forms.Button()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.ProgressBar2 = New System.Windows.Forms.ProgressBar()
        Me.DataView = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cblistdataview = New System.Windows.Forms.CheckBox()
        Me.cbreboot = New System.Windows.Forms.CheckBox()
        Me.btnerase = New System.Windows.Forms.Button()
        CType(Me.DataView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RichTextBox
        '
        Me.RichTextBox.BackColor = System.Drawing.Color.Black
        Me.RichTextBox.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RichTextBox.Location = New System.Drawing.Point(650, 39)
        Me.RichTextBox.Name = "RichTextBox"
        Me.RichTextBox.Size = New System.Drawing.Size(392, 368)
        Me.RichTextBox.TabIndex = 1
        Me.RichTextBox.Text = ""
        '
        'btnrawxml
        '
        Me.btnrawxml.Location = New System.Drawing.Point(12, 384)
        Me.btnrawxml.Name = "btnrawxml"
        Me.btnrawxml.Size = New System.Drawing.Size(75, 23)
        Me.btnrawxml.TabIndex = 2
        Me.btnrawxml.Text = "Raw XML"
        Me.btnrawxml.UseVisualStyleBackColor = True
        '
        'txtrawxml
        '
        Me.txtrawxml.Location = New System.Drawing.Point(93, 386)
        Me.txtrawxml.Name = "txtrawxml"
        Me.txtrawxml.Size = New System.Drawing.Size(551, 20)
        Me.txtrawxml.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(480, 312)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Storage : "
        '
        'cbstorage
        '
        Me.cbstorage.FormattingEnabled = True
        Me.cbstorage.Items.AddRange(New Object() {"emmc", "ufs"})
        Me.cbstorage.Location = New System.Drawing.Point(544, 309)
        Me.cbstorage.Name = "cbstorage"
        Me.cbstorage.Size = New System.Drawing.Size(100, 21)
        Me.cbstorage.TabIndex = 5
        Me.cbstorage.Text = "emmc"
        '
        'btngpt
        '
        Me.btngpt.Location = New System.Drawing.Point(104, 307)
        Me.btngpt.Name = "btngpt"
        Me.btngpt.Size = New System.Drawing.Size(88, 23)
        Me.btngpt.TabIndex = 6
        Me.btngpt.Text = "Read GPT"
        Me.btngpt.UseVisualStyleBackColor = True
        '
        'Comboboxport
        '
        Me.Comboboxport.FormattingEnabled = True
        Me.Comboboxport.Location = New System.Drawing.Point(650, 12)
        Me.Comboboxport.Name = "Comboboxport"
        Me.Comboboxport.Size = New System.Drawing.Size(392, 21)
        Me.Comboboxport.TabIndex = 7
        '
        'labeltimer
        '
        Me.labeltimer.AutoSize = True
        Me.labeltimer.Location = New System.Drawing.Point(1020, 15)
        Me.labeltimer.Name = "labeltimer"
        Me.labeltimer.Size = New System.Drawing.Size(19, 13)
        Me.labeltimer.TabIndex = 8
        Me.labeltimer.Text = "[  ]"
        '
        'btnloader
        '
        Me.btnloader.Location = New System.Drawing.Point(13, 332)
        Me.btnloader.Name = "btnloader"
        Me.btnloader.Size = New System.Drawing.Size(75, 23)
        Me.btnloader.TabIndex = 9
        Me.btnloader.Text = "Loader"
        Me.btnloader.UseVisualStyleBackColor = True
        '
        'txtloader
        '
        Me.txtloader.Location = New System.Drawing.Point(93, 334)
        Me.txtloader.Name = "txtloader"
        Me.txtloader.Size = New System.Drawing.Size(551, 20)
        Me.txtloader.TabIndex = 10
        '
        'btnbackup
        '
        Me.btnbackup.Location = New System.Drawing.Point(198, 307)
        Me.btnbackup.Name = "btnbackup"
        Me.btnbackup.Size = New System.Drawing.Size(88, 23)
        Me.btnbackup.TabIndex = 11
        Me.btnbackup.Text = "Backup"
        Me.btnbackup.UseVisualStyleBackColor = True
        '
        'btnflash
        '
        Me.btnflash.Location = New System.Drawing.Point(386, 307)
        Me.btnflash.Name = "btnflash"
        Me.btnflash.Size = New System.Drawing.Size(88, 23)
        Me.btnflash.TabIndex = 12
        Me.btnflash.Text = "Flash"
        Me.btnflash.UseVisualStyleBackColor = True
        '
        'txtpatchxml
        '
        Me.txtpatchxml.Location = New System.Drawing.Point(94, 360)
        Me.txtpatchxml.Name = "txtpatchxml"
        Me.txtpatchxml.Size = New System.Drawing.Size(550, 20)
        Me.txtpatchxml.TabIndex = 14
        '
        'btnpatchxml
        '
        Me.btnpatchxml.Location = New System.Drawing.Point(13, 358)
        Me.btnpatchxml.Name = "btnpatchxml"
        Me.btnpatchxml.Size = New System.Drawing.Size(75, 23)
        Me.btnpatchxml.TabIndex = 13
        Me.btnpatchxml.Text = "Patch XML"
        Me.btnpatchxml.UseVisualStyleBackColor = True
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(13, 284)
        Me.ProgressBar1.Margin = New System.Windows.Forms.Padding(0)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(631, 10)
        Me.ProgressBar1.TabIndex = 15
        '
        'ProgressBar2
        '
        Me.ProgressBar2.Location = New System.Drawing.Point(13, 294)
        Me.ProgressBar2.Margin = New System.Windows.Forms.Padding(0)
        Me.ProgressBar2.Name = "ProgressBar2"
        Me.ProgressBar2.Size = New System.Drawing.Size(631, 10)
        Me.ProgressBar2.TabIndex = 16
        '
        'DataView
        '
        Me.DataView.AllowUserToAddRows = False
        Me.DataView.AllowUserToDeleteRows = False
        Me.DataView.BackgroundColor = System.Drawing.Color.White
        Me.DataView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        Me.DataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.Column4, Me.Column5, Me.Column6, Me.Column7})
        Me.DataView.Location = New System.Drawing.Point(13, 12)
        Me.DataView.Name = "DataView"
        Me.DataView.RowHeadersVisible = False
        Me.DataView.Size = New System.Drawing.Size(631, 269)
        Me.DataView.TabIndex = 17
        '
        'Column1
        '
        Me.Column1.HeaderText = " "
        Me.Column1.Name = "Column1"
        Me.Column1.Width = 20
        '
        'Column2
        '
        Me.Column2.HeaderText = "Lun"
        Me.Column2.Name = "Column2"
        Me.Column2.Width = 50
        '
        'Column3
        '
        Me.Column3.HeaderText = "Block"
        Me.Column3.Name = "Column3"
        Me.Column3.Width = 50
        '
        'Column4
        '
        Me.Column4.HeaderText = "Partition"
        Me.Column4.Name = "Column4"
        '
        'Column5
        '
        Me.Column5.HeaderText = "Sector Size"
        Me.Column5.Name = "Column5"
        '
        'Column6
        '
        Me.Column6.HeaderText = "Partition Size"
        Me.Column6.Name = "Column6"
        '
        'Column7
        '
        Me.Column7.HeaderText = "Location"
        Me.Column7.Name = "Column7"
        Me.Column7.Width = 300
        '
        'cblistdataview
        '
        Me.cblistdataview.AutoSize = True
        Me.cblistdataview.Location = New System.Drawing.Point(18, 17)
        Me.cblistdataview.Name = "cblistdataview"
        Me.cblistdataview.Size = New System.Drawing.Size(15, 14)
        Me.cblistdataview.TabIndex = 18
        Me.cblistdataview.UseVisualStyleBackColor = True
        '
        'cbreboot
        '
        Me.cbreboot.AutoSize = True
        Me.cbreboot.Location = New System.Drawing.Point(13, 311)
        Me.cbreboot.Name = "cbreboot"
        Me.cbreboot.Size = New System.Drawing.Size(86, 17)
        Me.cbreboot.TabIndex = 19
        Me.cbreboot.Text = "Auto Reboot"
        Me.cbreboot.UseVisualStyleBackColor = True
        '
        'btnerase
        '
        Me.btnerase.Location = New System.Drawing.Point(292, 307)
        Me.btnerase.Name = "btnerase"
        Me.btnerase.Size = New System.Drawing.Size(88, 23)
        Me.btnerase.TabIndex = 20
        Me.btnerase.Text = "Erase"
        Me.btnerase.UseVisualStyleBackColor = True
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1051, 417)
        Me.Controls.Add(Me.btnerase)
        Me.Controls.Add(Me.cbreboot)
        Me.Controls.Add(Me.cblistdataview)
        Me.Controls.Add(Me.DataView)
        Me.Controls.Add(Me.ProgressBar2)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.txtpatchxml)
        Me.Controls.Add(Me.btnpatchxml)
        Me.Controls.Add(Me.btnflash)
        Me.Controls.Add(Me.btnbackup)
        Me.Controls.Add(Me.txtloader)
        Me.Controls.Add(Me.btnloader)
        Me.Controls.Add(Me.labeltimer)
        Me.Controls.Add(Me.Comboboxport)
        Me.Controls.Add(Me.btngpt)
        Me.Controls.Add(Me.cbstorage)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtrawxml)
        Me.Controls.Add(Me.btnrawxml)
        Me.Controls.Add(Me.RichTextBox)
        Me.MaximizeBox = False
        Me.Name = "Main"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Qualcomm Tool GUI"
        CType(Me.DataView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents RichTextBox As RichTextBox
    Friend WithEvents btnrawxml As Button
    Friend WithEvents txtrawxml As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents cbstorage As ComboBox
    Friend WithEvents btngpt As Button
    Friend WithEvents Comboboxport As ComboBox
    Friend WithEvents labeltimer As Label
    Friend WithEvents btnloader As Button
    Friend WithEvents txtloader As TextBox
    Friend WithEvents btnbackup As Button
    Friend WithEvents btnflash As Button
    Friend WithEvents txtpatchxml As TextBox
    Friend WithEvents btnpatchxml As Button
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents ProgressBar2 As ProgressBar
    Friend WithEvents DataView As DataGridView
    Friend WithEvents Column1 As DataGridViewCheckBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
    Friend WithEvents Column4 As DataGridViewTextBoxColumn
    Friend WithEvents Column5 As DataGridViewTextBoxColumn
    Friend WithEvents Column6 As DataGridViewTextBoxColumn
    Friend WithEvents Column7 As DataGridViewTextBoxColumn
    Friend WithEvents cblistdataview As CheckBox
    Friend WithEvents cbreboot As CheckBox
    Friend WithEvents btnerase As Button
End Class
