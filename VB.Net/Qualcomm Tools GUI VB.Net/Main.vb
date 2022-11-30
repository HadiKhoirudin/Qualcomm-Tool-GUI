Imports System.Xml
Imports System.IO
Imports Microsoft.VisualBasic.CompilerServices
Imports System.Management
Imports System.ComponentModel

Public Class Main
    Public Typememory As String = "emmc"
    Public SectorSize As String = "512"

    Public bool_1 As Boolean
    Public textBox1 As New TextBox
    Public textBox11 As New TextBox
    Public emmcdl_output As String
    Public PortQcom As Integer = 0
    Public WaktuCari As Integer = 60
    Public foldersave As String = Application.StartupPath & "\Data\Process"
    Public StringXml As String = ""
    Public xmlpatch As String = ""
    Public totalchecked As Integer = 0
    Public tot As Integer = 0
    Public MSM_HW_ID As String = ""
    Public OEM_PK_HASH As String = ""
    Public WithEvents QcomWorkerFlash As BackgroundWorker
#Region "UI"
    Private Sub RichTextBox_TextChanged(sender As Object, e As EventArgs) Handles RichTextBox.TextChanged
        RichTextBox.Invoke(Sub()
                               RichTextBox.SelectionStart = RichTextBox.Text.Length
                               RichTextBox.ScrollToCaret()
                           End Sub)
    End Sub
    Public Sub RichLogs(msg As String, colour As Color, isBold As Boolean, Optional NextLine As Boolean = False)
        RichTextBox.Invoke(Sub()
                               RichTextBox.SelectionStart = RichTextBox.Text.Length
                               Dim selectionColor As Color = RichTextBox.SelectionColor
                               RichTextBox.SelectionColor = colour
                               If isBold Then
                                   RichTextBox.SelectionFont = New Font(RichTextBox.Font, FontStyle.Bold)
                               Else
                                   RichTextBox.SelectionFont = New Font(RichTextBox.Font, FontStyle.Regular)
                               End If
                               RichTextBox.AppendText(msg)
                               RichTextBox.SelectionColor = selectionColor
                               If NextLine Then
                                   If RichTextBox.TextLength > 0 Then
                                       RichTextBox.AppendText(vbCrLf)
                                   End If
                               End If
                           End Sub)
    End Sub

    Public Sub Logs(ByVal msg_0 As String, ByVal color_0 As Color, ByVal msg_1 As String, ByVal color_1 As Color)
        RichTextBox.Invoke(New Action(Sub()
                                          RichTextBox.SelectionFont = New Font(RichTextBox.Font, FontStyle.Bold)
                                          RichTextBox.SelectionColor = color_0
                                          RichTextBox.AppendText(msg_0)
                                          RichTextBox.SelectionColor = color_1
                                          RichTextBox.AppendText(msg_1)
                                          RichTextBox.Refresh()
                                          RichTextBox.ScrollToCaret()
                                      End Sub))
    End Sub
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RichLogs("<+++++++++++      Qualcomm Tools GUI      +++++++++++>", Color.DarkOrange, True, True)
        RichLogs("► Software  " & vbTab & ": ", Color.DarkOrange, True, False)
        RichLogs("Qualcomm Tool", Color.DarkOrange, True, True)
        RichLogs("► Version Tool  " & vbTab & ": ", Color.DarkOrange, True, False)
        RichLogs("20-11-2022", Color.DarkOrange, True, True)
        RichLogs("► License  " & vbTab & ": ", Color.DarkOrange, True, False)
        RichLogs("Maintainer", Color.DarkOrange, True, True)
        RichLogs("► Version Base " & vbTab & ": ", Color.DarkOrange, True, False)
        RichLogs("Alpha I based [ 20-11-2022 ] Version", Color.DarkOrange, True, False)
        RichLogs("  ==========================================", Color.DarkOrange, True, True)
        RichLogs("► Websites  " & vbTab & ":  https://facebook.com/f.hadikhoir/", Color.DarkOrange, True, False)
        RichLogs("  ==========================================", Color.DarkOrange, True, True)
        RichLogs("", Color.DarkOrange, True, True)

    End Sub
    Public Sub ProcessBar1(Process As Long, total As Long)
        ProgressBar1.Invoke(New Action(Sub()
                                           ProgressBar1.Value = CInt(Math.Round((Process * 100L) / total))
                                       End Sub))
    End Sub

    Public Sub ProcessBar2(Process As Long, total As Long)
        ProgressBar2.Invoke(New Action(Sub()
                                           ProgressBar2.Value = CInt(Math.Round((Process * 100L) / total))
                                       End Sub))
    End Sub
#End Region
#Region "Function"
    Private Sub cbstorage_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbstorage.SelectedIndexChanged
        If cbstorage.SelectedIndex = 0 Then
            SectorSize = "512"
            Typememory = "emmc"
        Else
            SectorSize = "4096"
            Typememory = "ufs"
        End If
    End Sub

    Private Sub cblistdataview_CheckedChanged(sender As Object, e As EventArgs) Handles cblistdataview.CheckedChanged
        If DataView.Rows.Count > 0 Then
            If cblistdataview.Checked Then
                For Each item As DataGridViewRow In DataView.Rows
                    item.Cells(0).Value = True
                Next
            Else
                For Each item As DataGridViewRow In DataView.Rows
                    item.Cells(0).Value = False
                Next
            End If
        End If
    End Sub
    Private Sub DataView_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataView.CellDoubleClick
        If DataView.Rows.Count > 0 Then
            If e.ColumnIndex = 6 Then
                Dim openFileDialog As New OpenFileDialog()
                openFileDialog.Title = "Select File Partition " + DataView.CurrentRow.Cells(3).Value
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)
                openFileDialog.FileName = "*.*"
                openFileDialog.Filter = "ALL FILE  (*.*)|*.*"
                openFileDialog.FilterIndex = 2
                openFileDialog.RestoreDirectory = True
                If openFileDialog.ShowDialog() = DialogResult.OK Then
                    DataView.CurrentRow.Cells(6).Value = openFileDialog.FileName
                End If
            End If
        End If
    End Sub

    Private Sub btngpt_Click(sender As Object, e As EventArgs) Handles btngpt.Click
        Dim flags As Boolean = False

        If CheckBoxAutoLoader.Checked Then
            flags = True
        Else
            If txtloader.Text = "" Then
                MsgBox("Please select / browse loader file.")
            Else
                flags = True
            End If
        End If

        If flags Then
            WaktuCari = 60
            RichTextBox.Clear()
            CariPorts()
            If PortQcom > 0 Then

                QcomWorkerFlash = New BackgroundWorker()
                QcomWorkerFlash.WorkerSupportsCancellation = True
                AddHandler QcomWorkerFlash.DoWork, AddressOf ReadInfoDevice
                AddHandler QcomWorkerFlash.RunWorkerCompleted, AddressOf AllIsDone
                QcomWorkerFlash.RunWorkerAsync()
                QcomWorkerFlash.Dispose()

            End If
        End If
    End Sub
    Public Sub ReadInfoDevice(sender As Object, e As DoWorkEventArgs)
        getinfodevice()
        Scan_PartTable()
    End Sub

    Private Sub btnbackup_Click(sender As Object, e As EventArgs) Handles btnbackup.Click
        Dim flags As Boolean = False

        If CheckBoxAutoLoader.Checked Then
            flags = True
        Else
            If txtloader.Text = "" Then
                MsgBox("Please select / browse loader file.")
            Else
                flags = True
            End If
        End If

        If flags Then
            WaktuCari = 60
            RichTextBox.Clear()

            Dim flag As Boolean
            For Each item As DataGridViewRow In DataView.Rows
                If item.Cells(0).Value = True Then
                    flag = True
                End If
            Next
            If flag Then
                Dim folderBrowserDialog As New FolderBrowserDialog() With
        {
            .ShowNewFolderButton = True
        }
                If (folderBrowserDialog.ShowDialog() = DialogResult.OK) Then
                    CariPorts()
                    If PortQcom > 0 Then
                        foldersave = folderBrowserDialog.SelectedPath
                        StringXml = ""
                        StringXml = String.Concat(StringXml, "<?xml version=""1.0"" ?>" & vbCrLf & "")
                        StringXml = String.Concat(StringXml, "<data>" & vbCrLf & "")
                        totalchecked = 0
                        For Each item As DataGridViewRow In DataView.Rows
                            If item.Cells(DataView.Columns(0).Index).Value = True Then
                                totalchecked += 1

                                StringXml = String.Concat(StringXml, String.Format("<read SECTOR_SIZE_IN_BYTES=""{0}"" file_sector_offset=""0"" filename=""{1}"" label=""{2}"" num_partition_sectors=""{3}"" physical_partition_number=""{4}"" start_sector=""{5}""/>", New Object() {
                                                                                           SectorSize, '512, 4096
                                                                                           item.Cells(DataView.Columns(6).Index).Value,
                                                                                           item.Cells(DataView.Columns(3).Index).Value,
                                                                                           item.Cells(DataView.Columns(5).Index).Value,
                                                                                           item.Cells(DataView.Columns(1).Index).Value,
                                                                                           item.Cells(DataView.Columns(4).Index).Value
                                                                                           }),
                                                                  "" & vbCrLf & "")

                            End If
                        Next
                        StringXml = String.Concat(StringXml, "</data>")

                        QcomWorkerFlash = New BackgroundWorker()
                        QcomWorkerFlash.WorkerSupportsCancellation = True
                        AddHandler QcomWorkerFlash.DoWork, AddressOf XmlRead
                        AddHandler QcomWorkerFlash.RunWorkerCompleted, AddressOf AllIsDone
                        QcomWorkerFlash.RunWorkerAsync()
                        QcomWorkerFlash.Dispose()

                    End If
                End If
            End If
        End If
    End Sub

    Public Sub XmlRead(sender As Object, e As DoWorkEventArgs)
        getinfodevice()
        Try
            RichLogs(" ", Color.White, True, True)
            Dim totaldo As Integer
            totaldo = totalchecked
            Dim doprosess = 0
            Dim xr1 As XmlTextReader
            xr1 = New XmlTextReader(New StringReader(StringXml))

            If File.Exists(foldersave & "\rawprogram.xml") Then
                File.Delete(foldersave & "\rawprogram.xml")
            End If

            Dim files As StreamWriter
            files = My.Computer.FileSystem.OpenTextFileWriter(foldersave & "\rawprogram.xml", True)
            files.WriteLine("<?xml version=""1.0"" ?>")
            files.WriteLine("<data>")
            files.WriteLine("<!--NOTE: Genererate by HadiK IT **-->")
            ProcessBar2(doprosess, totaldo)
            Do While xr1.Read()
                If xr1.NodeType = XmlNodeType.Element AndAlso xr1.Name = "read" Then
                    Dim SectSize = xr1.GetAttribute("SECTOR_SIZE_IN_BYTES")
                    Dim SectorSizeStorage = SectSize
                    Dim numPartSect = xr1.GetAttribute("num_partition_sectors")
                    Dim label = xr1.GetAttribute("label")
                    Dim PhysicalPartition = xr1.GetAttribute("physical_partition_number")
                    Dim StartSector = xr1.GetAttribute("start_sector")

                    If numPartSect < 1 Then
                        numPartSect = ReadWriteEraseSize(numPartSect, label)
                    End If

                    Dim num As Double = Convert.ToDouble(numPartSect) / 2
                    RichLogs("Reading ", Color.White, True, False)
                    RichLogs(label & " " & GetFileCalculator(num) & " :   ", Color.DeepSkyBlue, True, False)


                    Dim Status = Read(StartSector, numPartSect, SectSize, PhysicalPartition, label)
                    If Status Then
                        RichLogs("Done  ✓", Color.Yellow, True, True)
                        files.WriteLine("<program SECTOR_SIZE_IN_BYTES=""" & SectorSize & """ file_sector_offset=""0"" filename=""" & label & ".img"" label=""" & label & """ num_partition_sectors=""" & numPartSect & """ physical_partition_number=""" & PhysicalPartition & """ start_sector=""" & StartSector & """/>")
                        doprosess += 1

                    Else
                        RichLogs("Failed  ", Color.Red, True, True)
                        doprosess += 1

                    End If
                    ProcessBar2(doprosess, totaldo)
                End If
            Loop
            files.WriteLine("</data>")
            files.Close()
            If cbreboot.Checked Then
                RichLogs(" ", Color.Yellow, True, True)
                RichLogs("Auto Reboot       : ", Color.White, True, False)
                RichLogs("Done  ✓", Color.Yellow, True, True)
                Dim status = RunFHCmdNoWaitForExit(String.Concat(New String() {"--port=\\.\COM", PortQcom, " --sendxml=""", Application.StartupPath & "\Data\Process\Reboot.xml", """", " --noprompt --loglevel=2 --showpercentagecomplete --zlpawarehost=1", """"}))
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub btnflash_Click(sender As Object, e As EventArgs) Handles btnflash.Click
        Dim flags As Boolean = False

        If CheckBoxAutoLoader.Checked Then
            flags = True
        Else
            If txtloader.Text = "" Then
                MsgBox("Please select / browse loader file.")
            Else
                flags = True
            End If
        End If

        If flags Then
            WaktuCari = 60
            RichTextBox.Clear()

            Dim flag As Boolean

            For Each item As DataGridViewRow In DataView.Rows
                If item.Cells(0).Value = True Then
                    flag = True
                End If
            Next

            If flag Then
                CariPorts()
                If PortQcom > 0 Then
                    StringXml = ""
                    StringXml = String.Concat(StringXml, "<?xml version=""1.0"" ?>" & vbCrLf & "")
                    StringXml = String.Concat(StringXml, "<data>" & vbCrLf & "")
                    totalchecked = 0
                    For Each item As DataGridViewRow In DataView.Rows
                        If item.Cells(DataView.Columns(0).Index).Value = True Then
                            totalchecked += 1

                            StringXml = String.Concat(StringXml, String.Format("<program SECTOR_SIZE_IN_BYTES=""{0}"" file_sector_offset=""0"" filename=""{1}"" label=""{2}"" num_partition_sectors=""{3}"" physical_partition_number=""{4}"" start_sector=""{5}""/>", New Object() {
                                                                                       SectorSize, '512, 4096
                                                                                       item.Cells(DataView.Columns(6).Index).Value,
                                                                                       item.Cells(DataView.Columns(3).Index).Value,
                                                                                       item.Cells(DataView.Columns(5).Index).Value,
                                                                                       item.Cells(DataView.Columns(1).Index).Value,
                                                                                       item.Cells(DataView.Columns(4).Index).Value
                                                                                       }),
                                                              "" & vbCrLf & "")

                        End If
                    Next
                    StringXml = String.Concat(StringXml, "</data>")
                    If Not txtpatchxml.Text = "" Then
                        totalchecked += 1
                    End If

                    QcomWorkerFlash = New BackgroundWorker()
                    QcomWorkerFlash.WorkerSupportsCancellation = True
                    AddHandler QcomWorkerFlash.DoWork, AddressOf XmlWrite
                    AddHandler QcomWorkerFlash.RunWorkerCompleted, AddressOf AllIsDone
                    QcomWorkerFlash.RunWorkerAsync()
                    QcomWorkerFlash.Dispose()

                End If
            End If
        End If
    End Sub

    Public Sub XmlWrite(sender As Object, e As DoWorkEventArgs)
        getinfodevice()
        Try
            RichLogs(" ", Color.White, True, True)
            Dim totaldo As Integer
            totaldo = totalchecked
            Dim doprosess = 0
            Dim xr1 As XmlTextReader
            xr1 = New XmlTextReader(New StringReader(StringXml))
            ProcessBar2(doprosess, totaldo)
            xr1 = New XmlTextReader(New StringReader(StringXml))
            Do While xr1.Read()
                If xr1.NodeType = XmlNodeType.Element AndAlso xr1.Name = "program" Then
                    SectorSize = xr1.GetAttribute("SECTOR_SIZE_IN_BYTES")
                    Dim numPartSect = xr1.GetAttribute("num_partition_sectors")
                    Dim label = xr1.GetAttribute("label")
                    Dim filename = xr1.GetAttribute("filename")
                    Dim PhysicalPartition = xr1.GetAttribute("physical_partition_number")
                    Dim StartSector = xr1.GetAttribute("start_sector")
                    If filename = "" Then
                        doprosess += 1
                    Else

                        If File.Exists(filename) Then
                            doprosess += 1

                            If numPartSect < 1 Then
                                numPartSect = ReadWriteEraseSize(numPartSect, label)
                            End If

                            Dim num As Double = Convert.ToDouble(numPartSect) / 2

                            RichLogs("Writing ", Color.White, True, False)

                            RichLogs(label & " " & GetFileCalculator(num) & " :   ", Color.DeepSkyBlue, True, False)

                            Dim status = Write(StartSector, numPartSect, SectorSize, PhysicalPartition, filename, label)

                            If Not status Then
                                RichLogs("Failed", Color.Red, True, True)
                            Else
                                RichLogs("Done  ✓", Color.Yellow, True, True)
                            End If

                        Else

                            RichLogs("File Not exist : ", Color.White, True, False)
                            RichLogs("skiping", Color.Red, True, True)
                            doprosess += 1
                        End If

                    End If


                    ProcessBar2(doprosess, totaldo)
                Else

                End If
            Loop
            If Not txtpatchxml.Text = "" Then

                RichLogs("Apply Patch       : ", Color.White, True, False)

                Dim StatusPatch = WritePatch()

                If Not StatusPatch Then
                    RichLogs("Failed", Color.Red, True, True)
                    Return
                End If

                RichLogs("Done  ✓", Color.Yellow, True, True)

                ProcessBar2(totaldo, totaldo)
            End If
            If cbreboot.Checked Then
                RichLogs(" ", Color.Yellow, True, True)
                RichLogs("Auto Reboot       : ", Color.White, True, False)
                RichLogs("Done  ✓", Color.Yellow, True, True)
                Dim status = RunFHCmdNoWaitForExit(String.Concat(New String() {"--port=\\.\COM", PortQcom, " --sendxml=""", Application.StartupPath & "\Data\Process\Reboot.xml", """", " --noprompt --loglevel=2 --showpercentagecomplete --zlpawarehost=1", """"}))
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub btnerase_Click(sender As Object, e As EventArgs) Handles btnerase.Click
        Dim flags As Boolean = False

        If CheckBoxAutoLoader.Checked Then
            flags = True
        Else
            If txtloader.Text = "" Then
                MsgBox("Please select / browse loader file.")
            Else
                flags = True
            End If
        End If

        If flags Then
            WaktuCari = 60
            RichTextBox.Clear()

            Dim flag As Boolean

            For Each item As DataGridViewRow In DataView.Rows
                If item.Cells(0).Value = True Then
                    flag = True
                End If
            Next

            If flag Then
                CariPorts()
                If PortQcom > 0 Then
                    StringXml = ""
                    StringXml = String.Concat(StringXml, "<?xml version=""1.0"" ?>" & vbCrLf & "")
                    StringXml = String.Concat(StringXml, "<data>" & vbCrLf & "")
                    totalchecked = 0
                    For Each item As DataGridViewRow In DataView.Rows
                        If item.Cells(DataView.Columns(0).Index).Value = True Then
                            totalchecked += 1

                            StringXml = String.Concat(StringXml, String.Format("<erase SECTOR_SIZE_IN_BYTES=""{0}"" file_sector_offset=""0"" filename=""{1}"" label=""{2}"" num_partition_sectors=""{3}"" physical_partition_number=""{4}"" start_sector=""{5}""/>", New Object() {
                                                                                       SectorSize, '512, 4096
                                                                                       item.Cells(DataView.Columns(6).Index).Value,
                                                                                       item.Cells(DataView.Columns(3).Index).Value,
                                                                                       item.Cells(DataView.Columns(5).Index).Value,
                                                                                       item.Cells(DataView.Columns(1).Index).Value,
                                                                                       item.Cells(DataView.Columns(4).Index).Value
                                                                                       }),
                                                              "" & vbCrLf & "")

                        End If
                    Next
                    StringXml = String.Concat(StringXml, "</data>")
                    If Not txtpatchxml.Text = "" Then
                        totalchecked += 1
                    End If

                    QcomWorkerFlash = New BackgroundWorker()
                    QcomWorkerFlash.WorkerSupportsCancellation = True
                    AddHandler QcomWorkerFlash.DoWork, AddressOf XmlErase
                    AddHandler QcomWorkerFlash.RunWorkerCompleted, AddressOf AllIsDone
                    QcomWorkerFlash.RunWorkerAsync()
                    QcomWorkerFlash.Dispose()

                End If
            End If
        End If
    End Sub

    Public Sub XmlErase(sender As Object, e As DoWorkEventArgs)
        getinfodevice()
        Try
            RichLogs(" ", Color.White, True, True)
            Dim totaldo As Integer
            totaldo = totalchecked
            Dim doprosess = 0
            Dim xr1 As XmlTextReader
            xr1 = New XmlTextReader(New StringReader(StringXml))
            ProcessBar2(doprosess, totaldo)
            xr1 = New XmlTextReader(New StringReader(StringXml))
            Do While xr1.Read()
                If xr1.NodeType = XmlNodeType.Element AndAlso xr1.Name = "erase" Then
                    SectorSize = xr1.GetAttribute("SECTOR_SIZE_IN_BYTES")
                    Dim numPartSect = xr1.GetAttribute("num_partition_sectors")
                    Dim label = xr1.GetAttribute("label")
                    Dim filename = xr1.GetAttribute("filename")
                    Dim PhysicalPartition = xr1.GetAttribute("physical_partition_number")
                    Dim StartSector = xr1.GetAttribute("start_sector")

                    doprosess += 1

                    If numPartSect < 1 Then
                        numPartSect = ReadWriteEraseSize(numPartSect, label)
                    End If

                    Dim num As Double = Convert.ToDouble(numPartSect) / 2

                    RichLogs("Erasing ", Color.White, True, False)

                    RichLogs(label & " " & GetFileCalculator(num) & " :   ", Color.DeepSkyBlue, True, False)

                    Dim status = Eraser(StartSector, numPartSect, SectorSize, PhysicalPartition, filename, label)

                    If Not status Then
                        RichLogs("Failed", Color.Red, True, True)
                    Else
                        RichLogs("Done  ✓", Color.Yellow, True, True)
                    End If


                    ProcessBar2(doprosess, totaldo)
                Else

                End If
            Loop
            If Not txtpatchxml.Text = "" Then

                RichLogs("Apply Patch       : ", Color.White, True, False)

                Dim StatusPatch = WritePatch()

                If Not StatusPatch Then
                    RichLogs("Failed", Color.Red, True, True)
                    Return
                End If

                RichLogs("Done  ✓", Color.Yellow, True, True)

                ProcessBar2(totaldo, totaldo)
            End If
            If cbreboot.Checked Then
                RichLogs(" ", Color.Yellow, True, True)
                RichLogs("Auto Reboot       : ", Color.White, True, False)
                RichLogs("Done  ✓", Color.Yellow, True, True)
                Dim status = RunFHCmdNoWaitForExit(String.Concat(New String() {"--port=\\.\COM", PortQcom, " --sendxml=""", Application.StartupPath & "\Data\Process\Reboot.xml", """", " --noprompt --loglevel=2 --showpercentagecomplete --zlpawarehost=1", """"}))
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub btnloader_Click(sender As Object, e As EventArgs) Handles btnloader.Click
        txtloader.Text = ""
        Dim openFileDialog As New OpenFileDialog() With
        {
            .Title = "loader",
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer),
            .FileName = "*.*",
            .Filter = "all file |*.*;*.* ",
            .FilterIndex = 2,
            .RestoreDirectory = True
        }
        If (openFileDialog.ShowDialog() = DialogResult.OK) Then
            txtloader.Text = openFileDialog.FileName
            Dim fileInfo As New FileInfo(openFileDialog.FileName)
        End If
    End Sub

    Private Sub btnrawxml_Click(sender As Object, e As EventArgs) Handles btnrawxml.Click

        Dim enumerator As IEnumerator = Nothing
        Dim openFileDialog As New OpenFileDialog() With
        {
            .Title = "Raw Program",
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer),
            .FileName = "*.xml",
            .Filter = "all file |*.*;*.* ",
            .FilterIndex = 2,
            .RestoreDirectory = True
        }
        If (openFileDialog.ShowDialog() = DialogResult.OK) Then
            txtrawxml.Text = openFileDialog.FileName
            DataView.Rows.Clear()
            Dim attribute As String = ""
            Dim fileInfo As New FileInfo(openFileDialog.FileName)
            Dim xmlReader As XmlReader = XmlReader.Create(txtrawxml.Text)
            Dim text As String = Nothing
            Dim chkd As Boolean = False
            While xmlReader.Read()
                If (xmlReader.NodeType <> XmlNodeType.Element OrElse Operators.CompareString(xmlReader.Name, "program", False) <> 0) Then
                    Continue While
                End If
                If (Operators.CompareString(xmlReader.GetAttribute("filename"), "", False) <> 0) Then
                    text = If(String.Concat(Path.GetDirectoryName(openFileDialog.FileName), "\", xmlReader.GetAttribute("filename")), "")
                    chkd = True
                Else
                    text = "none"
                    chkd = False
                End If
                DataView.Rows.Add(chkd,
                                  xmlReader.GetAttribute("physical_partition_number"),
                                  xmlReader.GetAttribute("SECTOR_SIZE_IN_BYTES"),
                                  xmlReader.GetAttribute("label"),
                                  xmlReader.GetAttribute("start_sector"),
                                  xmlReader.GetAttribute("num_partition_sectors"),
                                  text)

                attribute = xmlReader.GetAttribute("SECTOR_SIZE_IN_BYTES")
            End While

            If (attribute.Contains("512")) Then
                cbstorage.SelectedItem = "emmc"
                Typememory = "emmc"
                SectorSize = "512"
            ElseIf (attribute.Contains("4096")) Then
                cbstorage.SelectedItem = "ufs"
                Typememory = "ufs"
                SectorSize = "4096"
            End If
        End If
    End Sub
    Private Sub btnpatchxml_Click(sender As Object, e As EventArgs) Handles btnpatchxml.Click
        Dim openFileDialog As New OpenFileDialog() With
            {
            .Title = "Patch XML",
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer),
            .FileName = "*.xml",
            .Filter = "all file |*.*;*.* ",
            .FilterIndex = 2,
            .RestoreDirectory = True
        }
        If openFileDialog.ShowDialog() = DialogResult.OK Then
            txtpatchxml.Text = openFileDialog.FileName
            xmlpatch = File.ReadAllText(txtpatchxml.Text)
        End If
    End Sub
    Private Sub nono1()
        RichLogs(".", Color.FromArgb(224, 224, 224), True, False)
        Delay(0.1)
        RichLogs("..", Color.FromArgb(224, 224, 224), True, False)
        Delay(0.1)
    End Sub

    Public Sub Delay(ByVal dblSecs As Double)
        DateAndTime.Now.AddSeconds(0.0000115740740740741)
        Dim dateTime As System.DateTime = DateAndTime.Now.AddSeconds(0.0000115740740740741)
        Dim dateTime1 As System.DateTime = dateTime.AddSeconds(dblSecs)
        While System.DateTime.Compare(DateAndTime.Now, dateTime1) <= 0
            Application.DoEvents()
        End While
    End Sub
    Public Sub AllIsDone(sender As Object, e As RunWorkerCompletedEventArgs)
        RichLogs(" ", Color.Red, True, True)
        RichLogs("_______________________________________________________", Color.WhiteSmoke, True, True)
        RichLogs("All Progress Completed ... ", Color.WhiteSmoke, False, True)
        PortQcom = 0
    End Sub
    Private Sub data_info()
        RichLogs("Waiting For Device Connection", Color.FromArgb(224, 224, 224), True, False)
        Delay(1)
        nono1()
        RichLogs(" OK", Color.FromArgb(144, 255, 89), True, True)
        RichLogs("Waiting", Color.FromArgb(224, 224, 224), True, False)
        RichLogs(" HS-USB QDLoader 9008", Color.FromArgb(255, 171, 0), False, False)
        nono1()
        Delay(1)
        RichLogs("COM" & PortQcom, Color.FromArgb(144, 255, 89), True, True)
        RichLogs("Handshaking... ", Color.FromArgb(224, 224, 224), True, False)
        Delay(1)
        RichLogs("OK", Color.FromArgb(144, 255, 89), True, True)
        RichLogs("Writing flash programmer... ", Color.FromArgb(224, 224, 224), True, False)
        Delay(1)
        RichLogs(" OK", Color.FromArgb(144, 255, 89), True, True)
        RichLogs("Connecting to flash programmer", Color.FromArgb(224, 224, 224), True, False)
        Delay(1)
        nono1()
        RichLogs(" OK", Color.FromArgb(144, 255, 89), True, True)
        RichLogs("Firehose config : ", Color.FromArgb(224, 224, 224), True, False)
        RichLogs("Storage : ", Color.Tomato, True, False)
        RichLogs(String.Concat(Typememory.ToUpper, " "), Color.Yellow, True, True)
        RichLogs("Reading partition map", Color.FromArgb(224, 224, 224), True, False)
        Delay(1)
        nono1()
        RichLogs(" OK", Color.FromArgb(144, 255, 89), True, True)
    End Sub

    Public Sub CariPorts()
        RichLogs("Searching USB Devices...", Color.White, True, False)
        While Not PortQcom > 0
            If Not WaktuCari = 0 Then

                Delay(1)

                labeltimer.Invoke(New Action(Sub()
                                                 labeltimer.Text = WaktuCari - 1
                                             End Sub))

                Dim enumerator As ManagementObjectCollection.ManagementObjectEnumerator = Nothing
                Using managementObjectSearcher As New ManagementObjectSearcher("root\cimv2", "SELECT * FROM Win32_PnPEntity  WHERE Name LIKE '%9008%'  ")
                    enumerator = managementObjectSearcher.[Get]().GetEnumerator()
                    While enumerator.MoveNext()
                        Dim current As ManagementObject = DirectCast(enumerator.Current, ManagementObject)
                        Dim str As String = current("Name").ToString()
                        Dim str1 As String = current("Name").ToString().Substring(current("Name").ToString().IndexOf("(COM") + 4)
                        PortQcom = Conversions.ToInteger(str1.TrimEnd(New Char() {")"c}))

                        Comboboxport.Invoke(New Action(Sub()
                                                           Comboboxport.Text = str
                                                       End Sub))


                    End While
                End Using

                Dim ptr As Integer = WaktuCari
                WaktuCari = ptr - 1
            Else
                Exit While
            End If
        End While
        If Not PortQcom > 0 Then
            RichLogs(" Not Found!", Color.Red, True, True)
        Else
            RichTextBox.Invoke(New Action(Sub()
                                              RichTextBox.Clear()
                                          End Sub))
        End If
    End Sub

    Private Function CheckMSM(ByVal MSM_ID As String) As String
        Dim str As String

        If (MSM_ID.Contains("0x007050e1")) Then
            str = "MSM8916"
        ElseIf (If(MSM_ID.Contains("0x0004f0e1"), True, MSM_ID.Contains("0x0006b0e1"))) Then
            str = "MSM8937"
        ElseIf (MSM_ID.Contains("0x008050e1")) Then
            str = "MSM8926"
        ElseIf (If(MSM_ID.Contains("0x008110e1") OrElse MSM_ID.Contains("0x008120E1") OrElse MSM_ID.Contains("0x008150e1"), True, MSM_ID.Contains("0x008150e1"))) Then
            str = "MSM8x10"
        ElseIf (MSM_ID.Contains("0x008140e1")) Then
            str = "MSM8x12"
        ElseIf (MSM_ID.Contains("0x009690e1")) Then
            str = "MSM8992"
        ElseIf (If(MSM_ID.Contains("0x009470e1"), True, MSM_ID.Contains("0x0005f0e1"))) Then
            str = "MSM8996"
        ElseIf (MSM_ID.Contains("0x009400e1")) Then
            str = "MSM8994"
        ElseIf (MSM_ID.Contains("0x007B80E1")) Then
            str = "MSM8974"
        ElseIf (MSM_ID.Contains("0x008A30E1")) Then
            str = "MSM8930"
        ElseIf (MSM_ID.Contains("0x0091b0e1")) Then
            str = "MSM8929"
        ElseIf (MSM_ID.Contains("0x009180e1")) Then
            str = "MSM8928"
        ElseIf (MSM_ID.Contains("0x008050e2")) Then
            str = "MSM8926"
        ElseIf (MSM_ID.Contains("0x000560e1")) Then
            str = "MSM8917"
        ElseIf (MSM_ID.Contains("0x0090b0e1")) Then
            str = "MSM8936"
        ElseIf (MSM_ID.Contains("0x000460e1")) Then
            str = "MSM8953"
        ElseIf (If(MSM_ID.Contains("0x009600e1"), True, MSM_ID.Contains("0x009610e1"))) Then
            str = "MSM8909"
        ElseIf (Not MSM_ID.Contains("0x009900e1")) Then
            str = If(MSM_ID.Contains("0x009b00e1"), "MSM8976", "Unknown")
        Else
            str = "MSM8976"
        End If
        Return str
    End Function

    Public Sub emmcdl_cmd(ByVal cmd As String)
        Console.WriteLine(cmd)
        Try
            Dim process As System.Diagnostics.Process = New System.Diagnostics.Process() With
            {
                .StartInfo = New ProcessStartInfo() With
                {
                    .WindowStyle = ProcessWindowStyle.Hidden,
                    .Verb = "runas",
                    .FileName = "cmd.exe",
                    .Arguments = String.Concat("/c emmcdl.exe ", cmd),
                    .RedirectStandardOutput = True,
                    .UseShellExecute = False,
                    .CreateNoWindow = True,
                    .WorkingDirectory = "Data\Process\"
                }
            }
            process.Start()
            Dim standardOutput As StreamReader = process.StandardOutput
            emmcdl_output = standardOutput.ReadToEnd().Replace("===============Device Class:sahara Protocol:firehose Emergency:false================", "").Replace("Status: 0 The operation completed successfully.", "").Replace("Version 2.15", "")
            process.WaitForExit()
            standardOutput.Close()
        Catch exception As System.Exception
            ProjectData.SetProjectError(exception)
            ProjectData.ClearProjectError()
        End Try
    End Sub

    Public Sub getinfodevice()
        textBox11.Clear()
        emmcdl_cmd(String.Concat("emmcdl.exe -p COM", PortQcom, " -info"))
        If (Not emmcdl_output.Contains("Did not receive Sahara hello packet from device")) Then
            data_info()
            textBox11.Text = emmcdl_output
            Dim strs As List(Of String) = New List(Of String)()
            Dim lines As String() = textBox11.Lines
            Dim num As Integer = 0
            While num < CInt(lines.Length)
                Dim str As String = lines(num)
                bool_1 = Operators.CompareString(str.Trim(), "", False) <> 0
                If (bool_1) Then
                    strs.Add(str)
                End If
                num = num + 1
            End While
            textBox11.Lines = strs.ToArray()
            Dim strArrays As String() = textBox11.Lines
            Dim num1 As Integer = 0
            While num1 < CInt(strArrays.Length)
                Dim str1 As String = strArrays(num1)
                If (Operators.CompareString(str1, "", False) <> 0) Then
                    If (str1.Contains("SerialNumber:")) Then
                        RichLogs(" ", Color.White, False, True)
                        Logs("  Serial :", Color.FromArgb(224, 224, 224), str1.Replace("SerialNumber:", ""), Color.FromArgb(244, 114, 208))
                    End If
                    If (str1.Contains("MSM_HW_ID:")) Then
                        Logs(String.Concat("  MSM_HW_ID : ", CheckMSM(str1)), Color.FromArgb(224, 224, 224), str1.Replace("MSM_HW_ID:", ""), Color.FromArgb(30, 136, 229))
                        MSM_HW_ID = str1.Replace("MSM_HW_ID:", "").Substring(3)

                    End If
                    If (str1.Contains("OEM_PK_HASH:")) Then
                        textBox1.Text = str1
                        textBox1.Text = textBox1.Text.Replace("OEM_PK_HASH:", "")
                        Dim str2 As String = textBox1.Text.Substring(17)
                        Dim str3 As String = textBox1.Text.Substring(0, 50)
                        textBox1.Text = str2
                        Delay(1)
                        RichLogs("  PK_HASH[0] : ", Color.FromArgb(224, 224, 224), True, False)
                        RichLogs(textBox1.Text, Color.Orange, False, True)
                        textBox1.Text = str3.Substring(3)
                        RichLogs("  PK_HASH[1] : ", Color.FromArgb(224, 224, 224), True, False)
                        RichLogs(textBox1.Text, Color.Orange, False, False)
                        OEM_PK_HASH = textBox1.Text.Substring(0, 16)
                    End If
                    If (str1.Contains("SBL SW Version:")) Then
                        Delay(0.6)
                        Logs("  SBL_SW : ", Color.FromArgb(224, 224, 224), str1.Replace("SBL SW Version:", ""), Color.Orange)
                        RichLogs(" ", Color.White, False, True)
                    Else
                        RichLogs(" ", Color.White, False, True)
                    End If
                    If CheckBoxAutoLoader.Checked Then
                        If MSM_HW_ID.Length < 16 Then
                            Dim Str As String
                            Do
                                Dim sb As New Text.StringBuilder()
                                Str = sb.Append("0").ToString()

                                MSM_HW_ID = MSM_HW_ID & Str

                                If MSM_HW_ID.Length = 16 Then
                                    Exit Do
                                End If
                            Loop
                        End If

                        Dim folder = Application.StartupPath & "\Data\Autoloader"
                        Dim string_to_find = MSM_HW_ID & "_" & OEM_PK_HASH
                        Console.WriteLine(string_to_find)
                        Dim di = New DirectoryInfo(folder)

                        Dim results = di.EnumerateFileSystemInfos("*", SearchOption.AllDirectories).Where(Function(i) i.Name.IndexOf(string_to_find, StringComparison.InvariantCultureIgnoreCase) >= 0)
                        Do
                            For Each r In results

                                If TypeOf r Is DirectoryInfo Then
                                    Console.WriteLine("Directory: {0}", r.Name)
                                Else
                                    Console.WriteLine("File: {0}", r.FullName)
                                End If
                                If r.Name <> String.Empty Then
                                    txtloader.Invoke(New Action(Sub()
                                                                    txtloader.Text = r.FullName
                                                                End Sub))
                                    Exit Do
                                End If
                            Next
                        Loop
                    End If
                End If
                num1 = num1 + 1
            End While
            emmcdl_cmd(String.Concat({"emmcdl.exe -p COM", PortQcom, " -f ", """" & txtloader.Text & """", " -info "}))
        Else
            RichLogs("Device Already in Programmer Mode...", Color.White, True, True)
            emmcdl_cmd(String.Concat({"emmcdl.exe -p COM", PortQcom, " -f ", """" & txtloader.Text & """", " -info "}))
        End If
    End Sub

    Public Function GetFileCalculator(ByVal byteCount As Double) As String
        Dim str As String = "0 Bytes"
        If (byteCount >= 1073741824) Then
            str = String.Concat(String.Format("{0:##.##}", byteCount / 1073741824), " TB")
        ElseIf (byteCount >= 1048576) Then
            str = String.Concat(String.Format("{0:##.##}", byteCount / 1048576), " GB")
        ElseIf (byteCount >= 1024) Then
            str = String.Concat(String.Format("{0:##.##}", byteCount / 1024), " MB")
        ElseIf (If(byteCount <= 0, False, byteCount < 1024)) Then
            str = String.Concat(byteCount.ToString(), " KB")
        End If
        Return str
    End Function

    Public Function emmcdl_class(ByVal cmd As String)
        Console.WriteLine(cmd)
        Dim process As New Process()
        process.StartInfo = New ProcessStartInfo
        With process.StartInfo
            .UseShellExecute = False
            .CreateNoWindow = True
            .FileName = "Data/Process/emmcdl.exe"
            .Arguments = cmd
            .RedirectStandardOutput = True
        End With
        Dim edl_Renamed As Process = process
        edl_Renamed.Start()
        Return edl_Renamed.StandardOutput.ReadToEnd()
    End Function

    Public Sub Scan_PartTable()
        DataView.Invoke(New Action(Sub()
                                       DataView.Rows.Clear()
                                   End Sub))

        Dim str As String = emmcdl_class(String.Concat(New String() {" -p COM", PortQcom, " -f ", """" & txtloader.Text & """", " -gpt "}))
        If str.Contains("SECTOR_SIZE_IN_BYTES=""512""") Then
            Typememory = "emmc"
            SectorSize = "512"
            cbstorage.Invoke(New Action(Sub()
                                            cbstorage.SelectedIndex = 0
                                        End Sub))
        ElseIf str.Contains("SECTOR_SIZE_IN_BYTES=""4096""") Then
            Typememory = "ufs"
            SectorSize = "4096"
            cbstorage.Invoke(New Action(Sub()
                                            cbstorage.SelectedIndex = 1
                                        End Sub))
        End If
        str = str.Substring(str.LastIndexOf(">") + 1)
        str = str.Substring(0, str.LastIndexOf("Status:"))
        Using stringReader As System.IO.StringReader = New System.IO.StringReader(str)
            While stringReader.Peek() <> -1
                Dim str1 As String = stringReader.ReadLine()
                If str1.Contains("Partition Name") Then

                    str1 = str1.Replace("Partition Name:", "")
                    str1 = str1.Replace("Start LBA:", "")
                    str1 = str1.Replace("Size in LBA:", "")

                    Dim strArrays As String() = str1.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)

                    DataView.Invoke(New Action(Sub()
                                                   DataView.Rows.Add(False, "0", SectorSize, strArrays(1), strArrays(2), strArrays(3))
                                               End Sub))


                End If
            End While
            If str.Contains("The handle Is invalid.") Then
                RichLogs("FAILED! Please Re-Enter to EDL mode.", Color.FromArgb(239, 83, 80), True, False)
                PortQcom = 0
            End If
        End Using
    End Sub
    Public Function ReadWriteEraseSize(NumPartition As String, pname As String) As String
        Delay(1)
        Dim str As String = emmcdl_class(String.Concat(New String() {" -p COM", PortQcom, " -f ", """" & txtloader.Text & """", " -gpt "}))
        If str.Contains("SECTOR_SIZE_IN_BYTES=""512""") Then
            Typememory = "emmc"
            SectorSize = "512"
            cbstorage.Invoke(New Action(Sub()
                                            cbstorage.SelectedIndex = 0
                                        End Sub))
        ElseIf str.Contains("SECTOR_SIZE_IN_BYTES=""4096""") Then
            Typememory = "ufs"
            SectorSize = "4096"
            cbstorage.Invoke(New Action(Sub()
                                            cbstorage.SelectedIndex = 1
                                        End Sub))
        End If
        str = str.Substring(str.LastIndexOf(">") + 1)
        str = str.Substring(0, str.LastIndexOf("Status:"))
        Using stringReader As System.IO.StringReader = New System.IO.StringReader(str)
            While stringReader.Peek() <> -1
                Dim str1 As String = stringReader.ReadLine()
                If str1.Contains("Partition Name") Then
                    str1 = str1.Replace("Partition Name:", "")
                    str1 = str1.Replace("Start LBA:", "")
                    str1 = str1.Replace("Size in LBA:", "")
                    Dim strArrays As String() = str1.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                    If strArrays(1).Contains(pname) Then
                        Return strArrays(3)
                    End If
                End If
            End While
        End Using
        Return 0L
    End Function

    Public Function Read(Startsector As String, NumPartition As String, ByRef SectSize As String, physical As String, pname As String) As Boolean

        tot = 0

        Dim tmp As String = $"<?xml version = ""1.0""?><data><program SECTOR_SIZE_IN_BYTES=""{SectorSize}"" file_sector_offset=""0"" filename=""{pname & ".img"}"" label=""{pname}"" num_partition_sectors=""{NumPartition}""  physical_partition_number=""{physical}"" start_sector=""{Startsector}""/></data>"

        If File.Exists(foldersave & "\tmp.xml") Then
            File.Delete(foldersave & "\tmp.xml")
        End If

        File.WriteAllText(foldersave & "\tmp.xml", tmp)

        Return RunFHCmdRead(String.Concat(New String() {"--port=\\.\COM", PortQcom, " --sendxml=""", foldersave & "\tmp.xml", """", " --search_path=""", foldersave, """", " --mainoutputdir=""", foldersave, """", " --convertprogram2read --noprompt --loglevel=2 --showpercentagecomplete --zlpawarehost=1", """"}), NumPartition, SectSize)


    End Function
    Public Function RunFHCmdRead(ByVal cmd As String, NumPartition As String, SectSize As String) As Boolean
        Dim flag As Boolean = False
        Dim Progress As New ProgressBar()
        Progress.Minimum = 0
        Progress.Maximum = 100
        Dim resultprogress As Integer = 0
        Dim now As Integer = 0
        Dim thn As Integer = 0
        Dim startInfo As ProcessStartInfo = New ProcessStartInfo(Path.Combine(Application.StartupPath & "\Data\Process\fh_loader.exe"), cmd) With {
            .CreateNoWindow = True,
            .WindowStyle = ProcessWindowStyle.Hidden,
            .UseShellExecute = False,
            .Verb = "runas",
            .WorkingDirectory = foldersave,
            .RedirectStandardError = True,
            .RedirectStandardOutput = True
        }
        Using process As Process = Process.Start(startInfo)
            Console.WriteLine(cmd)
            process.BeginOutputReadLine()
            process.BeginErrorReadLine()

            AddHandler process.OutputDataReceived, Sub(sender As Object, e As DataReceivedEventArgs)
                                                       Dim text As String = If(e.Data, String.Empty)
                                                       Console.WriteLine(text)
                                                       Dim fileOffset As Long = 0
                                                       Dim totalprogress As Long = 0
                                                       If text <> String.Empty Then
                                                           If text.Contains("FileSizeNumSectorsLeft") Then
                                                               Dim str As String = text.Replace(": DEBUG: FileSizeNumSectorsLeft =", "").Replace(" ", "").Replace("-", "")
                                                               str = str.Substring(str.IndexOf(str.Substring(8))).Replace(":", "")


                                                               fileOffset += str
                                                               totalprogress += NumPartition

                                                               If fileOffset > 0 Then
                                                                   Progress.Maximum = fileOffset * 100L
                                                                   Progress.Value = CInt(Math.Round(fileOffset * 100L / totalprogress))
                                                               Else
                                                                   Progress.Maximum = 100
                                                                   Progress.Value = 0
                                                               End If

                                                               now = Progress.Value

                                                               If Progress.Value > 0 AndAlso resultprogress < 99 Then
                                                                   If now < thn Then
                                                                       resultprogress += 1
                                                                       Console.WriteLine(resultprogress)
                                                                       ProcessBar1(resultprogress, 100)
                                                                   End If
                                                               End If

                                                               thn = Progress.Value

                                                           ElseIf text.Contains("{All Finished Successfully}") Then
                                                               ProgressBar1.Invoke(New Action(Sub()
                                                                                                  ProgressBar1.Value = 100
                                                                                              End Sub))
                                                               If File.Exists(foldersave & "\tmp.xml") Then
                                                                   File.Delete(foldersave & "\tmp.xml")
                                                               End If
                                                               flag = True
                                                           End If
                                                       End If
                                                   End Sub
            process.WaitForExit()
        End Using
        Return flag
    End Function


    Public Function Write(Startsector As String, NumPartition As String, ByRef SectSize As String, physical As String, pname As String, label As String) As Boolean

        tot = 0
        foldersave = Path.Combine(New String() {Path.GetDirectoryName(pname)})
        Dim filenya As String = Path.Combine(New String() {Path.GetFileName(pname)})
        Dim tmp As String = $"<?xml version = ""1.0""?><data><program SECTOR_SIZE_IN_BYTES=""{SectorSize}"" file_sector_offset=""0"" filename=""{filenya}"" label=""{pname}"" num_partition_sectors=""{NumPartition}""  physical_partition_number=""{physical}"" start_sector=""{Startsector}""/></data>"

        If File.Exists(foldersave & "\tmp.xml") Then
            File.Delete(foldersave & "\tmp.xml")
        End If

        File.WriteAllText(foldersave & "\tmp.xml", tmp)

        Return RunFHCmdWrite(String.Concat(New String() {"--port=\\.\COM", PortQcom, " --sendxml=""", foldersave & "\tmp.xml", """", " --noprompt --loglevel=2 --showpercentagecomplete --zlpawarehost=1", """"}))

    End Function

    Public Function WritePatch() As Boolean
        tot = 0

        foldersave = Path.Combine(New String() {Path.GetDirectoryName(txtpatchxml.Text)})

        If File.Exists(foldersave & "\tmp.xml") Then
            File.Delete(foldersave & "\tmp.xml")
        End If

        Return RunFHCmdWrite(String.Concat(New String() {"--port=\\.\COM", PortQcom, " --sendxml=""", txtpatchxml.Text, """", " --noprompt --loglevel=2 --showpercentagecomplete --zlpawarehost=1", """"}))

    End Function

    Public Function Eraser(Startsector As String, NumPartition As String, ByRef SectSize As String, physical As String, pname As String, label As String) As Boolean
        tot = 0

        Dim tmp As String = $"<?xml version = ""1.0""?><data><erase SECTOR_SIZE_IN_BYTES=""{SectorSize}"" file_sector_offset=""0"" filename=""{pname}"" label=""{pname}"" num_partition_sectors=""{NumPartition}""  physical_partition_number=""{physical}"" start_sector=""{Startsector}""/></data>"

        If File.Exists(foldersave & "\tmp.xml") Then
            File.Delete(foldersave & "\tmp.xml")
        End If

        File.WriteAllText(foldersave & "\tmp.xml", tmp)

        Return RunFHCmdWrite(String.Concat(New String() {"--port=\\.\COM", PortQcom, " --sendxml=""", foldersave & "\tmp.xml", """", " --noprompt --loglevel=2 --showpercentagecomplete --zlpawarehost=1", """"}))

    End Function
    Public Function RunFHCmdWrite(ByVal cmd As String) As Boolean
        Delay(0.6)
        Dim flag As Boolean = False
        Dim Progress As New ProgressBar()
        Progress.Minimum = 0
        Progress.Maximum = 100
        Dim resultprogress As Integer = 0
        Dim now As Integer = 0
        Dim thn As Integer = 0
        Dim startInfo As ProcessStartInfo = New ProcessStartInfo(Path.Combine(Application.StartupPath & "\Data\Process\fh_loader.exe"), cmd) With {
            .CreateNoWindow = True,
            .WindowStyle = ProcessWindowStyle.Hidden,
            .UseShellExecute = False,
            .Verb = "runas",
            .WorkingDirectory = foldersave,
            .RedirectStandardError = True,
            .RedirectStandardOutput = True
        }
        Using process As Process = Process.Start(startInfo)
            Console.WriteLine(cmd)
            process.BeginOutputReadLine()
            process.BeginErrorReadLine()

            AddHandler process.OutputDataReceived, Sub(sender As Object, e As DataReceivedEventArgs)
                                                       Dim text As String = If(e.Data, String.Empty)
                                                       Console.WriteLine(text)
                                                       Dim fileOffset As Long = 0
                                                       Dim totalprogress As Long = 0
                                                       If text <> String.Empty Then
                                                           If (text.Contains("percent files transferred")) Then
                                                               Dim str As String = text.Replace(": INFO: {percent files transferred", "").Replace("%}", "").Replace(" ", "")
                                                               str = str.Substring(str.IndexOf(str.Substring(8)))
                                                               str = str.Substring(0, str.IndexOf("."c))

                                                               ProgressBar1.Invoke(New Action(Sub()
                                                                                                  ProgressBar1.Value = CInt(Convert.ToInt64(str))
                                                                                              End Sub))


                                                           ElseIf (text.Contains("All Finished Successfully")) Then

                                                               ProgressBar1.Invoke(New Action(Sub()
                                                                                                  ProgressBar1.Value = 100
                                                                                              End Sub))

                                                               If File.Exists(foldersave & "\tmp.xml") Then
                                                                   File.Delete(foldersave & "\tmp.xml")
                                                               End If
                                                               flag = True

                                                           End If


                                                       End If
                                                   End Sub
            process.WaitForExit()
        End Using
        Return flag
    End Function

    Public Function RunFHCmdNoWaitForExit(ByVal cmd As String) As Boolean
        Delay(0.6)
        Dim flag As Boolean = False
        Dim Progress As New ProgressBar()
        Progress.Minimum = 0
        Progress.Maximum = 100
        Dim resultprogress As Integer = 0
        Dim now As Integer = 0
        Dim thn As Integer = 0
        Dim startInfo As ProcessStartInfo = New ProcessStartInfo(Path.Combine(Application.StartupPath & "\Data\Process\fh_loader.exe"), cmd) With {
            .CreateNoWindow = True,
            .WindowStyle = ProcessWindowStyle.Hidden,
            .UseShellExecute = False,
            .Verb = "runas",
            .WorkingDirectory = foldersave,
            .RedirectStandardError = True,
            .RedirectStandardOutput = True
        }
        Using process As Process = Process.Start(startInfo)
            Console.WriteLine(cmd)
            process.BeginOutputReadLine()
            process.BeginErrorReadLine()

            AddHandler process.OutputDataReceived, Sub(sender As Object, e As DataReceivedEventArgs)
                                                       Dim text As String = If(e.Data, String.Empty)
                                                       Console.WriteLine(text)
                                                       Dim fileOffset As Long = 0
                                                       Dim totalprogress As Long = 0
                                                       If text <> String.Empty Then
                                                           If (text.Contains("percent files transferred")) Then
                                                               Dim str As String = text.Replace(": INFO: {percent files transferred", "").Replace("%}", "").Replace(" ", "")
                                                               str = str.Substring(str.IndexOf(str.Substring(8)))
                                                               str = str.Substring(0, str.IndexOf("."c))

                                                               ProgressBar1.Invoke(New Action(Sub()
                                                                                                  ProgressBar1.Value = CInt(Convert.ToInt64(str))
                                                                                              End Sub))


                                                           ElseIf (text.Contains("All Finished Successfully")) Then

                                                               ProgressBar1.Invoke(New Action(Sub()
                                                                                                  ProgressBar1.Value = 100
                                                                                              End Sub))

                                                               If File.Exists(foldersave & "\tmp.xml") Then
                                                                   File.Delete(foldersave & "\tmp.xml")
                                                               End If
                                                               flag = True

                                                           End If


                                                       End If
                                                   End Sub
        End Using
        Return flag
    End Function

    Private Sub CheckBoxAutoLoader_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxAutoLoader.CheckedChanged
        If CheckBoxAutoLoader.Checked Then
            txtloader.Text = ""
            btnloader.Enabled = False
        Else
            txtloader.Text = ""
            btnloader.Enabled = True
        End If
    End Sub


#End Region
End Class
