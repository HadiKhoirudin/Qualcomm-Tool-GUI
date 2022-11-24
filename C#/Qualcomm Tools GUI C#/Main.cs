using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Management;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Xml;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Qualcomm_Tools_GUI
{

    public partial class Main
    {
        public string Typememory = "emmc";
        public string SectorSize = "512";

        public bool bool_1;
        public TextBox textBox1 = new TextBox();
        public TextBox textBox11 = new TextBox();
        public string emmcdl_output;
        public int PortQcom = 0;
        public int WaktuCari = 60;
        public string foldersave = Application.StartupPath + @"\Data\Process";
        public string StringXml = "";
        public string xmlpatch = "";
        public int totalchecked = 0;
        public int tot = 0;
        private BackgroundWorker _QcomWorkerFlash;

        public virtual BackgroundWorker QcomWorkerFlash
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _QcomWorkerFlash;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _QcomWorkerFlash = value;
            }
        }

        public Main()
        {
            InitializeComponent();
        }
        #region UI
        private void RichTextBox_TextChanged(object sender, EventArgs e)
        {
            RichTextBox.Invoke(new Action(() =>
                {
                    RichTextBox.SelectionStart = RichTextBox.Text.Length;
                    RichTextBox.ScrollToCaret();
                }));
        }
        public void RichLogs(string msg, Color colour, bool isBold, bool NextLine = false)
        {
            RichTextBox.Invoke(new Action(() =>
                {
                    RichTextBox.SelectionStart = RichTextBox.Text.Length;
                    var selectionColor = RichTextBox.SelectionColor;
                    RichTextBox.SelectionColor = colour;
                    if (isBold)
                    {
                        RichTextBox.SelectionFont = new Font(RichTextBox.Font, FontStyle.Bold);
                    }
                    else
                    {
                        RichTextBox.SelectionFont = new Font(RichTextBox.Font, FontStyle.Regular);
                    }
                    RichTextBox.AppendText(msg);
                    RichTextBox.SelectionColor = selectionColor;
                    if (NextLine)
                    {
                        if (RichTextBox.TextLength > 0)
                        {
                            RichTextBox.AppendText(Constants.vbCrLf);
                        }
                    }
                }));
        }

        public void Logs(string msg_0, Color color_0, string msg_1, Color color_1)
        {
            RichTextBox.Invoke(new Action(() =>
                {
                    RichTextBox.SelectionFont = new Font(RichTextBox.Font, FontStyle.Bold);
                    RichTextBox.SelectionColor = color_0;
                    RichTextBox.AppendText(msg_0);
                    RichTextBox.SelectionColor = color_1;
                    RichTextBox.AppendText(msg_1);
                    RichTextBox.Refresh();
                    RichTextBox.ScrollToCaret();
                }));
        }
        private void Main_Load(object sender, EventArgs e)
        {
            RichLogs("<+++++++++++      Qualcomm Tools GUI      +++++++++++>", Color.DarkOrange, true, true);
            RichLogs("► Software  " + Constants.vbTab + ": ", Color.DarkOrange, true, false);
            RichLogs("Qualcomm Tool", Color.DarkOrange, true, true);
            RichLogs("► Version Tool  " + Constants.vbTab + ": ", Color.DarkOrange, true, false);
            RichLogs("20-11-2022", Color.DarkOrange, true, true);
            RichLogs("► License  " + Constants.vbTab + ": ", Color.DarkOrange, true, false);
            RichLogs("Maintainer", Color.DarkOrange, true, true);
            RichLogs("► Version Base " + Constants.vbTab + ": ", Color.DarkOrange, true, false);
            RichLogs("Alpha I based [ 20-11-2022 ] Version", Color.DarkOrange, true, false);
            RichLogs("  ==========================================", Color.DarkOrange, true, true);
            RichLogs("► Websites  " + Constants.vbTab + ":  https://facebook.com/f.hadikhoir/", Color.DarkOrange, true, false);
            RichLogs("  ==========================================", Color.DarkOrange, true, true);
            RichLogs("", Color.DarkOrange, true, true);
        }
        public void ProcessBar1(long Process, long total)
        {
            ProgressBar1.Invoke(new Action(() => ProgressBar1.Value = (int)Math.Round(Math.Round(Process * 100L / (double)total))));
        }

        public void ProcessBar2(long Process, long total)
        {
            ProgressBar2.Invoke(new Action(() => ProgressBar2.Value = (int)Math.Round(Math.Round(Process * 100L / (double)total))));
        }
        #endregion
        #region Function
        private void cbstorage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbstorage.SelectedIndex == 0)
            {
                SectorSize = "512";
                Typememory = "emmc";
            }
            else
            {
                SectorSize = "4096";
                Typememory = "ufs";
            }
        }

        private void cblistdataview_CheckedChanged(object sender, EventArgs e)
        {
            if (DataView.Rows.Count > 0)
            {
                if (cblistdataview.Checked)
                {
                    foreach (DataGridViewRow item in DataView.Rows)
                        item.Cells[0].Value = true;
                }
                else
                {
                    foreach (DataGridViewRow item in DataView.Rows)
                        item.Cells[0].Value = false;
                }
            }
        }
        private void DataView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DataView.Rows.Count > 0)
            {
                if (e.ColumnIndex == 6)
                {
                    var openFileDialog = new OpenFileDialog();
                    openFileDialog.Title = Conversions.ToString(Operators.AddObject("Select File Partition ", DataView.CurrentRow.Cells[3].Value));
                    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
                    openFileDialog.FileName = "*.*";
                    openFileDialog.Filter = "ALL FILE  (*.*)|*.*";
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.RestoreDirectory = true;
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        DataView.CurrentRow.Cells[6].Value = openFileDialog.FileName;
                    }
                }
            }
        }

        private void btngpt_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtloader.Text))
            {
                Interaction.MsgBox("Please select / browse loader file.");
            }
            else
            {
                WaktuCari = 60;
                RichTextBox.Clear();
                CariPorts();
                if (PortQcom > 0)
                {

                    QcomWorkerFlash = new BackgroundWorker();
                    QcomWorkerFlash.WorkerSupportsCancellation = true;
                    QcomWorkerFlash.DoWork += ReadInfoDevice;
                    QcomWorkerFlash.RunWorkerCompleted += AllIsDone;
                    QcomWorkerFlash.RunWorkerAsync();
                    QcomWorkerFlash.Dispose();

                }
            }
        }
        public void ReadInfoDevice(object sender, DoWorkEventArgs e)
        {
            getinfodevice();
            Scan_PartTable();
        }

        private void btnbackup_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtloader.Text))
            {
                Interaction.MsgBox("Please select / browse loader file.");
            }
            else
            {
                WaktuCari = 60;
                RichTextBox.Clear();

                var flag = default(bool);
                foreach (DataGridViewRow item in DataView.Rows)
                {
                    if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(item.Cells[0].Value, true, false)))
                    {
                        flag = true;
                    }
                }
                if (flag)
                {
                    var folderBrowserDialog = new FolderBrowserDialog() { ShowNewFolderButton = true };
                    if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    {
                        CariPorts();
                        if (PortQcom > 0)
                        {
                            foldersave = folderBrowserDialog.SelectedPath;
                            StringXml = "";
                            StringXml = string.Concat(StringXml, "<?xml version=\"1.0\" ?>" + Constants.vbCrLf + "");
                            StringXml = string.Concat(StringXml, "<data>" + Constants.vbCrLf + "");
                            totalchecked = 0;
                            foreach (DataGridViewRow item in DataView.Rows)
                            {
                                if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(item.Cells[DataView.Columns[0].Index].Value, true, false)))
                                {
                                    totalchecked += 1;

                                    StringXml = string.Concat(StringXml, string.Format("<read SECTOR_SIZE_IN_BYTES=\"{0}\" file_sector_offset=\"0\" filename=\"{1}\" label=\"{2}\" num_partition_sectors=\"{3}\" physical_partition_number=\"{4}\" start_sector=\"{5}\"/>", new object[] { SectorSize, item.Cells[DataView.Columns[6].Index].Value, item.Cells[DataView.Columns[3].Index].Value, item.Cells[DataView.Columns[5].Index].Value, item.Cells[DataView.Columns[1].Index].Value, item.Cells[DataView.Columns[4].Index].Value }), "" + Constants.vbCrLf + ""); // 512, 4096

                                }
                            }
                            StringXml = string.Concat(StringXml, "</data>");

                            QcomWorkerFlash = new BackgroundWorker();
                            QcomWorkerFlash.WorkerSupportsCancellation = true;
                            QcomWorkerFlash.DoWork += XmlRead;
                            QcomWorkerFlash.RunWorkerCompleted += AllIsDone;
                            QcomWorkerFlash.RunWorkerAsync();
                            QcomWorkerFlash.Dispose();

                        }
                    }
                }
            }
        }

        public void XmlRead(object sender, DoWorkEventArgs e)
        {
            getinfodevice();
            try
            {
                RichLogs(" ", Color.White, true, true);
                int totaldo;
                totaldo = totalchecked;
                int doprosess = 0;
                XmlTextReader xr1;
                xr1 = new XmlTextReader(new StringReader(StringXml));

                if (File.Exists(foldersave + @"\rawprogram.xml"))
                {
                    File.Delete(foldersave + @"\rawprogram.xml");
                }

                StreamWriter files;
                files = My.MyProject.Computer.FileSystem.OpenTextFileWriter(foldersave + @"\rawprogram.xml", true);
                files.WriteLine("<?xml version=\"1.0\" ?>");
                files.WriteLine("<data>");
                files.WriteLine("<!--NOTE: Genererate by HadiK IT **-->");
                ProcessBar2(doprosess, totaldo);
                while (xr1.Read())
                {
                    if (xr1.NodeType == XmlNodeType.Element && xr1.Name == "read")
                    {
                        string SectSize = xr1.GetAttribute("SECTOR_SIZE_IN_BYTES");
                        string SectorSizeStorage = SectSize;
                        string numPartSect = xr1.GetAttribute("num_partition_sectors");
                        string label = xr1.GetAttribute("label");
                        string PhysicalPartition = xr1.GetAttribute("physical_partition_number");
                        string StartSector = xr1.GetAttribute("start_sector");

                        if (Conversions.ToDouble(numPartSect) < 1d)
                        {
                            numPartSect = ReadWriteEraseSize(numPartSect, label);
                        }

                        double num = Convert.ToDouble(numPartSect) / 2d;
                        RichLogs("Reading ", Color.White, true, false);
                        RichLogs(label + " " + GetFileCalculator(num) + " :   ", Color.DeepSkyBlue, true, false);


                        bool Status = Read(StartSector, numPartSect, ref SectSize, PhysicalPartition, label);
                        if (Status)
                        {
                            RichLogs("Done  ✓", Color.Yellow, true, true);
                            files.WriteLine("<program SECTOR_SIZE_IN_BYTES=\"" + SectorSize + "\" file_sector_offset=\"0\" filename=\"" + label + ".img\" label=\"" + label + "\" num_partition_sectors=\"" + numPartSect + "\" physical_partition_number=\"" + PhysicalPartition + "\" start_sector=\"" + StartSector + "\"/>");
                            doprosess += 1;
                        }

                        else
                        {
                            RichLogs("Failed  ", Color.Red, true, true);
                            doprosess += 1;

                        }
                        ProcessBar2(doprosess, totaldo);
                    }
                }
                files.WriteLine("</data>");
                files.Close();
                if (cbreboot.Checked)
                {
                    RichLogs(" ", Color.Yellow, true, true);
                    RichLogs("Auto Reboot       : ", Color.White, true, false);
                    RichLogs("Done  ✓", Color.Yellow, true, true);
                    bool status = RunFHCmdNoWaitForExit(string.Concat(new string[] { @"--port=\\.\COM", PortQcom.ToString(), " --sendxml=\"", Application.StartupPath + @"\Data\Process\Reboot.xml", "\"", " --noprompt --loglevel=2 --showpercentagecomplete --zlpawarehost=1", "\"" }));
                }
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.ToString());
            }
        }
        private void btnflash_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtloader.Text))
            {
                Interaction.MsgBox("Please select / browse loader file.");
            }
            else
            {
                WaktuCari = 60;
                RichTextBox.Clear();

                var flag = default(bool);

                foreach (DataGridViewRow item in DataView.Rows)
                {
                    if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(item.Cells[0].Value, true, false)))
                    {
                        flag = true;
                    }
                }

                if (flag)
                {
                    CariPorts();
                    if (PortQcom > 0)
                    {
                        StringXml = "";
                        StringXml = string.Concat(StringXml, "<?xml version=\"1.0\" ?>" + Constants.vbCrLf + "");
                        StringXml = string.Concat(StringXml, "<data>" + Constants.vbCrLf + "");
                        totalchecked = 0;
                        foreach (DataGridViewRow item in DataView.Rows)
                        {
                            if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(item.Cells[DataView.Columns[0].Index].Value, true, false)))
                            {
                                totalchecked += 1;

                                StringXml = string.Concat(StringXml, string.Format("<program SECTOR_SIZE_IN_BYTES=\"{0}\" file_sector_offset=\"0\" filename=\"{1}\" label=\"{2}\" num_partition_sectors=\"{3}\" physical_partition_number=\"{4}\" start_sector=\"{5}\"/>", new object[] { SectorSize, item.Cells[DataView.Columns[6].Index].Value, item.Cells[DataView.Columns[3].Index].Value, item.Cells[DataView.Columns[5].Index].Value, item.Cells[DataView.Columns[1].Index].Value, item.Cells[DataView.Columns[4].Index].Value }), "" + Constants.vbCrLf + ""); // 512, 4096

                            }
                        }
                        StringXml = string.Concat(StringXml, "</data>");
                        if (!string.IsNullOrEmpty(txtpatchxml.Text))
                        {
                            totalchecked += 1;
                        }

                        QcomWorkerFlash = new BackgroundWorker();
                        QcomWorkerFlash.WorkerSupportsCancellation = true;
                        QcomWorkerFlash.DoWork += XmlWrite;
                        QcomWorkerFlash.RunWorkerCompleted += AllIsDone;
                        QcomWorkerFlash.RunWorkerAsync();
                        QcomWorkerFlash.Dispose();

                    }
                }
            }
        }

        public void XmlWrite(object sender, DoWorkEventArgs e)
        {
            getinfodevice();
            try
            {
                RichLogs(" ", Color.White, true, true);
                int totaldo;
                totaldo = totalchecked;
                int doprosess = 0;
                XmlTextReader xr1;
                xr1 = new XmlTextReader(new StringReader(StringXml));
                ProcessBar2(doprosess, totaldo);
                xr1 = new XmlTextReader(new StringReader(StringXml));
                while (xr1.Read())
                {
                    if (xr1.NodeType == XmlNodeType.Element && xr1.Name == "program")
                    {
                        SectorSize = xr1.GetAttribute("SECTOR_SIZE_IN_BYTES");
                        string numPartSect = xr1.GetAttribute("num_partition_sectors");
                        string label = xr1.GetAttribute("label");
                        string filename = xr1.GetAttribute("filename");
                        string PhysicalPartition = xr1.GetAttribute("physical_partition_number");
                        string StartSector = xr1.GetAttribute("start_sector");
                        if (string.IsNullOrEmpty(filename))
                        {
                            doprosess += 1;
                        }

                        else if (File.Exists(filename))
                        {
                            doprosess += 1;

                            if (Conversions.ToDouble(numPartSect) < 1d)
                            {
                                numPartSect = ReadWriteEraseSize(numPartSect, label);
                            }

                            double num = Convert.ToDouble(numPartSect) / 2d;

                            RichLogs("Writing ", Color.White, true, false);

                            RichLogs(label + " " + GetFileCalculator(num) + " :   ", Color.DeepSkyBlue, true, false);

                            bool status = Write(StartSector, numPartSect, ref SectorSize, PhysicalPartition, filename, label);

                            if (!status)
                            {
                                RichLogs("Failed", Color.Red, true, true);
                            }
                            else
                            {
                                RichLogs("Done  ✓", Color.Yellow, true, true);
                            }
                        }

                        else
                        {

                            RichLogs("File Not exist : ", Color.White, true, false);
                            RichLogs("skiping", Color.Red, true, true);
                            doprosess += 1;

                        }


                        ProcessBar2(doprosess, totaldo);
                    }
                    else
                    {

                    }
                }
                if (!string.IsNullOrEmpty(txtpatchxml.Text))
                {

                    RichLogs("Apply Patch       : ", Color.White, true, false);

                    bool StatusPatch = WritePatch();

                    if (!StatusPatch)
                    {
                        RichLogs("Failed", Color.Red, true, true);
                        return;
                    }

                    RichLogs("Done  ✓", Color.Yellow, true, true);

                    ProcessBar2(totaldo, totaldo);
                }
                if (cbreboot.Checked)
                {
                    RichLogs(" ", Color.Yellow, true, true);
                    RichLogs("Auto Reboot       : ", Color.White, true, false);
                    RichLogs("Done  ✓", Color.Yellow, true, true);
                    bool status = RunFHCmdNoWaitForExit(string.Concat(new string[] { @"--port=\\.\COM", PortQcom.ToString(), " --sendxml=\"", Application.StartupPath + @"\Data\Process\Reboot.xml", "\"", " --noprompt --loglevel=2 --showpercentagecomplete --zlpawarehost=1", "\"" }));
                }
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.ToString());
            }
        }

        private void btnerase_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtloader.Text))
            {
                Interaction.MsgBox("Please select / browse loader file.");
            }
            else
            {
                WaktuCari = 60;
                RichTextBox.Clear();

                var flag = default(bool);

                foreach (DataGridViewRow item in DataView.Rows)
                {
                    if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(item.Cells[0].Value, true, false)))
                    {
                        flag = true;
                    }
                }

                if (flag)
                {
                    CariPorts();
                    if (PortQcom > 0)
                    {
                        StringXml = "";
                        StringXml = string.Concat(StringXml, "<?xml version=\"1.0\" ?>" + Constants.vbCrLf + "");
                        StringXml = string.Concat(StringXml, "<data>" + Constants.vbCrLf + "");
                        totalchecked = 0;
                        foreach (DataGridViewRow item in DataView.Rows)
                        {
                            if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(item.Cells[DataView.Columns[0].Index].Value, true, false)))
                            {
                                totalchecked += 1;

                                StringXml = string.Concat(StringXml, string.Format("<erase SECTOR_SIZE_IN_BYTES=\"{0}\" file_sector_offset=\"0\" filename=\"{1}\" label=\"{2}\" num_partition_sectors=\"{3}\" physical_partition_number=\"{4}\" start_sector=\"{5}\"/>", new object[] { SectorSize, item.Cells[DataView.Columns[6].Index].Value, item.Cells[DataView.Columns[3].Index].Value, item.Cells[DataView.Columns[5].Index].Value, item.Cells[DataView.Columns[1].Index].Value, item.Cells[DataView.Columns[4].Index].Value }), "" + Constants.vbCrLf + ""); // 512, 4096

                            }
                        }
                        StringXml = string.Concat(StringXml, "</data>");
                        if (!string.IsNullOrEmpty(txtpatchxml.Text))
                        {
                            totalchecked += 1;
                        }

                        QcomWorkerFlash = new BackgroundWorker();
                        QcomWorkerFlash.WorkerSupportsCancellation = true;
                        QcomWorkerFlash.DoWork += XmlErase;
                        QcomWorkerFlash.RunWorkerCompleted += AllIsDone;
                        QcomWorkerFlash.RunWorkerAsync();
                        QcomWorkerFlash.Dispose();

                    }
                }
            }
        }

        public void XmlErase(object sender, DoWorkEventArgs e)
        {
            getinfodevice();
            try
            {
                RichLogs(" ", Color.White, true, true);
                int totaldo;
                totaldo = totalchecked;
                int doprosess = 0;
                XmlTextReader xr1;
                xr1 = new XmlTextReader(new StringReader(StringXml));
                ProcessBar2(doprosess, totaldo);
                xr1 = new XmlTextReader(new StringReader(StringXml));
                while (xr1.Read())
                {
                    if (xr1.NodeType == XmlNodeType.Element && xr1.Name == "erase")
                    {
                        SectorSize = xr1.GetAttribute("SECTOR_SIZE_IN_BYTES");
                        string numPartSect = xr1.GetAttribute("num_partition_sectors");
                        string label = xr1.GetAttribute("label");
                        string filename = xr1.GetAttribute("filename");
                        string PhysicalPartition = xr1.GetAttribute("physical_partition_number");
                        string StartSector = xr1.GetAttribute("start_sector");

                        doprosess += 1;

                        if (Conversions.ToDouble(numPartSect) < 1d)
                        {
                            numPartSect = ReadWriteEraseSize(numPartSect, label);
                        }

                        double num = Convert.ToDouble(numPartSect) / 2d;

                        RichLogs("Erasing ", Color.White, true, false);

                        RichLogs(label + " " + GetFileCalculator(num) + " :   ", Color.DeepSkyBlue, true, false);

                        bool status = Eraser(StartSector, numPartSect, ref SectorSize, PhysicalPartition, filename, label);

                        if (!status)
                        {
                            RichLogs("Failed", Color.Red, true, true);
                        }
                        else
                        {
                            RichLogs("Done  ✓", Color.Yellow, true, true);
                        }


                        ProcessBar2(doprosess, totaldo);
                    }
                    else
                    {

                    }
                }
                if (!string.IsNullOrEmpty(txtpatchxml.Text))
                {

                    RichLogs("Apply Patch       : ", Color.White, true, false);

                    bool StatusPatch = WritePatch();

                    if (!StatusPatch)
                    {
                        RichLogs("Failed", Color.Red, true, true);
                        return;
                    }

                    RichLogs("Done  ✓", Color.Yellow, true, true);

                    ProcessBar2(totaldo, totaldo);
                }
                if (cbreboot.Checked)
                {
                    RichLogs(" ", Color.Yellow, true, true);
                    RichLogs("Auto Reboot       : ", Color.White, true, false);
                    RichLogs("Done  ✓", Color.Yellow, true, true);
                    bool status = RunFHCmdNoWaitForExit(string.Concat(new string[] { @"--port=\\.\COM", PortQcom.ToString(), " --sendxml=\"", Application.StartupPath + @"\Data\Process\Reboot.xml", "\"", " --noprompt --loglevel=2 --showpercentagecomplete --zlpawarehost=1", "\"" }));
                }
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.ToString());
            }
        }

        private void btnloader_Click(object sender, EventArgs e)
        {
            txtloader.Text = "";
            var openFileDialog = new OpenFileDialog()
            {
                Title = "loader",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer),
                FileName = "*.*",
                Filter = "all file |*.*;*.* ",
                FilterIndex = 2,
                RestoreDirectory = true
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtloader.Text = openFileDialog.FileName;
                var fileInfo = new FileInfo(openFileDialog.FileName);
            }
        }

        private void btnrawxml_Click(object sender, EventArgs e)
        {

            var openFileDialog = new OpenFileDialog()
            {
                Title = "Raw Program",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer),
                FileName = "*.xml",
                Filter = "all file |*.*;*.* ",
                FilterIndex = 2,
                RestoreDirectory = true
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtrawxml.Text = openFileDialog.FileName;
                DataView.Rows.Clear();
                string attribute = "";
                var fileInfo = new FileInfo(openFileDialog.FileName);
                var xmlReader = XmlReader.Create(txtrawxml.Text);
                string text = null;
                bool chkd = false;
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType != XmlNodeType.Element || Operators.CompareString(xmlReader.Name, "program", false) != 0)
                    {
                        continue;
                    }
                    if (Operators.CompareString(xmlReader.GetAttribute("filename"), "", false) != 0)
                    {
                        text = string.Concat(Path.GetDirectoryName(openFileDialog.FileName), @"\", xmlReader.GetAttribute("filename")) ?? "";
                        chkd = true;
                    }
                    else
                    {
                        text = "none";
                        chkd = false;
                    }
                    DataView.Rows.Add(chkd, xmlReader.GetAttribute("physical_partition_number"), xmlReader.GetAttribute("SECTOR_SIZE_IN_BYTES"), xmlReader.GetAttribute("label"), xmlReader.GetAttribute("start_sector"), xmlReader.GetAttribute("num_partition_sectors"), text);

                    attribute = xmlReader.GetAttribute("SECTOR_SIZE_IN_BYTES");
                }

                if (attribute.Contains("512"))
                {
                    cbstorage.SelectedItem = "emmc";
                    Typememory = "emmc";
                    SectorSize = "512";
                }
                else if (attribute.Contains("4096"))
                {
                    cbstorage.SelectedItem = "ufs";
                    Typememory = "ufs";
                    SectorSize = "4096";
                }
            }
        }
        private void btnpatchxml_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog()
            {
                Title = "Patch XML",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer),
                FileName = "*.xml",
                Filter = "all file |*.*;*.* ",
                FilterIndex = 2,
                RestoreDirectory = true
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtpatchxml.Text = openFileDialog.FileName;
                xmlpatch = File.ReadAllText(txtpatchxml.Text);
            }
        }
        private void nono1()
        {
            RichLogs(".", Color.FromArgb(224, 224, 224), true, false);
            Delay(0.1d);
            RichLogs("..", Color.FromArgb(224, 224, 224), true, false);
            Delay(0.1d);
        }

        public void Delay(double dblSecs)
        {
            DateTime.Now.AddSeconds(0.0000115740740740741d);
            var dateTime = DateTime.Now.AddSeconds(0.0000115740740740741d);
            var dateTime1 = dateTime.AddSeconds(dblSecs);
            while (DateTime.Compare(DateTime.Now, dateTime1) <= 0)
                Application.DoEvents();
        }
        public void AllIsDone(object sender, RunWorkerCompletedEventArgs e)
        {
            RichLogs(" ", Color.Red, true, true);
            RichLogs("_______________________________________________________", Color.WhiteSmoke, true, true);
            RichLogs("All Progress Completed ... ", Color.WhiteSmoke, false, true);
            PortQcom = 0;
        }
        private void data_info()
        {
            RichLogs("Waiting For Device Connection", Color.FromArgb(224, 224, 224), true, false);
            Delay(1d);
            nono1();
            RichLogs(" OK", Color.FromArgb(144, 255, 89), true, true);
            RichLogs("Waiting", Color.FromArgb(224, 224, 224), true, false);
            RichLogs(" HS-USB QDLoader 9008", Color.FromArgb(255, 171, 0), false, false);
            nono1();
            Delay(1d);
            RichLogs("COM" + PortQcom, Color.FromArgb(144, 255, 89), true, true);
            RichLogs("Handshaking... ", Color.FromArgb(224, 224, 224), true, false);
            Delay(1d);
            RichLogs("OK", Color.FromArgb(144, 255, 89), true, true);
            RichLogs("Writing flash programmer... ", Color.FromArgb(224, 224, 224), true, false);
            Delay(1d);
            RichLogs(" OK", Color.FromArgb(144, 255, 89), true, true);
            RichLogs("Connecting to flash programmer", Color.FromArgb(224, 224, 224), true, false);
            Delay(1d);
            nono1();
            RichLogs(" OK", Color.FromArgb(144, 255, 89), true, true);
            RichLogs("Firehose config : ", Color.FromArgb(224, 224, 224), true, false);
            RichLogs("Storage : ", Color.Tomato, true, false);
            RichLogs(string.Concat(Typememory.ToUpper(), " "), Color.Yellow, true, true);
            RichLogs("Reading partition map", Color.FromArgb(224, 224, 224), true, false);
            Delay(1d);
            nono1();
            RichLogs(" OK", Color.FromArgb(144, 255, 89), true, true);
        }

        public void CariPorts()
        {
            RichLogs("Searching USB Devices...", Color.White, true, false);
            while (!(PortQcom > 0))
            {
                if (!(WaktuCari == 0))
                {

                    Delay(1d);

                    labeltimer.Invoke(new Action(() => labeltimer.Text = (WaktuCari - 1).ToString()));

                    ManagementObjectCollection.ManagementObjectEnumerator enumerator = null;
                    using (var managementObjectSearcher = new ManagementObjectSearcher(@"root\cimv2", "SELECT * FROM Win32_PnPEntity  WHERE Name LIKE '%9008%'  "))
                    {
                        enumerator = managementObjectSearcher.Get().GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            ManagementObject current = (ManagementObject)enumerator.Current;
                            string str = current["Name"].ToString();
                            string str1 = current["Name"].ToString().Substring(current["Name"].ToString().IndexOf("(COM") + 4);
                            PortQcom = Conversions.ToInteger(str1.TrimEnd(new char[] { ')' }));

                            Comboboxport.Invoke(new Action(() => Comboboxport.Text = str));


                        }
                    }

                    int ptr = WaktuCari;
                    WaktuCari = ptr - 1;
                }
                else
                {
                    break;
                }
            }
            if (!(PortQcom > 0))
            {
                RichLogs(" Not Found!", Color.Red, true, true);
            }
            else
            {
                RichTextBox.Invoke(new Action(() => RichTextBox.Clear()));
            }
        }

        private string CheckMSM(string MSM_ID)
        {
            string str;

            if (MSM_ID.Contains("0x007050e1"))
            {
                str = "MSM8916";
            }
            else if (MSM_ID.Contains("0x0004f0e1") ? true : MSM_ID.Contains("0x0006b0e1"))
            {
                str = "MSM8937";
            }
            else if (MSM_ID.Contains("0x008050e1"))
            {
                str = "MSM8926";
            }
            else if (MSM_ID.Contains("0x008110e1") || MSM_ID.Contains("0x008120E1") || MSM_ID.Contains("0x008150e1") ? true : MSM_ID.Contains("0x008150e1"))
            {
                str = "MSM8x10";
            }
            else if (MSM_ID.Contains("0x008140e1"))
            {
                str = "MSM8x12";
            }
            else if (MSM_ID.Contains("0x009690e1"))
            {
                str = "MSM8992";
            }
            else if (MSM_ID.Contains("0x009470e1") ? true : MSM_ID.Contains("0x0005f0e1"))
            {
                str = "MSM8996";
            }
            else if (MSM_ID.Contains("0x009400e1"))
            {
                str = "MSM8994";
            }
            else if (MSM_ID.Contains("0x007B80E1"))
            {
                str = "MSM8974";
            }
            else if (MSM_ID.Contains("0x008A30E1"))
            {
                str = "MSM8930";
            }
            else if (MSM_ID.Contains("0x0091b0e1"))
            {
                str = "MSM8929";
            }
            else if (MSM_ID.Contains("0x009180e1"))
            {
                str = "MSM8928";
            }
            else if (MSM_ID.Contains("0x008050e2"))
            {
                str = "MSM8926";
            }
            else if (MSM_ID.Contains("0x000560e1"))
            {
                str = "MSM8917";
            }
            else if (MSM_ID.Contains("0x0090b0e1"))
            {
                str = "MSM8936";
            }
            else if (MSM_ID.Contains("0x000460e1"))
            {
                str = "MSM8953";
            }
            else if (MSM_ID.Contains("0x009600e1") ? true : MSM_ID.Contains("0x009610e1"))
            {
                str = "MSM8909";
            }
            else if (!MSM_ID.Contains("0x009900e1"))
            {
                str = MSM_ID.Contains("0x009b00e1") ? "MSM8976" : "Unknown";
            }
            else
            {
                str = "MSM8976";
            }
            return str;
        }

        public void emmcdl_cmd(string cmd)
        {
            Console.WriteLine(cmd);
            try
            {
                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Verb = "runas",
                        FileName = "cmd.exe",
                        Arguments = string.Concat("/c emmcdl.exe ", cmd),
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        WorkingDirectory = @"Data\Process\"
                    }
                };
                process.Start();
                var standardOutput = process.StandardOutput;
                emmcdl_output = standardOutput.ReadToEnd().Replace("===============Device Class:sahara Protocol:firehose Emergency:false================", "").Replace("Status: 0 The operation completed successfully.", "").Replace("Version 2.15", "");
                process.WaitForExit();
                standardOutput.Close();
            }
            catch (Exception exception)
            {
                ProjectData.SetProjectError(exception);
                ProjectData.ClearProjectError();
            }
        }

        public void getinfodevice()
        {
            textBox11.Clear();
            emmcdl_cmd(string.Concat("emmcdl.exe -p COM", PortQcom, " -info"));
            if (!emmcdl_output.Contains("Did not receive Sahara hello packet from device"))
            {
                data_info();
                textBox11.Text = emmcdl_output;
                var strs = new List<string>();
                var lines = textBox11.Lines;
                int num = 0;
                while (num < lines.Length)
                {
                    string str = lines[num];
                    bool_1 = Operators.CompareString(str.Trim(), "", false) != 0;
                    if (bool_1)
                    {
                        strs.Add(str);
                    }
                    num = num + 1;
                }
                textBox11.Lines = strs.ToArray();
                var strArrays = textBox11.Lines;
                int num1 = 0;
                while (num1 < strArrays.Length)
                {
                    string str1 = strArrays[num1];
                    if (Operators.CompareString(str1, "", false) != 0)
                    {
                        if (str1.Contains("SerialNumber:"))
                        {
                            RichLogs(" ", Color.White, false, true);
                            Logs("  Serial :", Color.FromArgb(224, 224, 224), str1.Replace("SerialNumber:", ""), Color.FromArgb(244, 114, 208));
                        }
                        if (str1.Contains("MSM_HW_ID:"))
                        {
                            Logs(string.Concat("  MSM_HW_ID : ", CheckMSM(str1)), Color.FromArgb(224, 224, 224), str1.Replace("MSM_HW_ID:", ""), Color.FromArgb(30, 136, 229));
                        }
                        if (str1.Contains("OEM_PK_HASH:"))
                        {
                            textBox1.Text = str1;
                            textBox1.Text = textBox1.Text.Replace("OEM_PK_HASH:", "");
                            string str2 = textBox1.Text.Substring(17);
                            string str3 = textBox1.Text.Substring(0, 50);
                            textBox1.Text = str2;
                            Delay(1d);
                            RichLogs("  PK_HASH[0] : ", Color.FromArgb(224, 224, 224), true, false);
                            RichLogs(textBox1.Text, Color.Orange, false, true);
                            textBox1.Text = str3;
                            RichLogs("  PK_HASH[1] : ", Color.FromArgb(224, 224, 224), true, false);
                            RichLogs(textBox1.Text, Color.Orange, false, false);
                        }
                        if (str1.Contains("SBL SW Version:"))
                        {
                            Delay(0.6d);
                            Logs("  SBL_SW : ", Color.FromArgb(224, 224, 224), str1.Replace("SBL SW Version:", ""), Color.Orange);
                            RichLogs(" ", Color.White, false, true);
                        }
                        else
                        {
                            RichLogs(" ", Color.White, false, true);
                        }
                    }
                    num1 = num1 + 1;
                }
                emmcdl_cmd(string.Concat(new[] { "emmcdl.exe -p COM", (object)PortQcom, " -f ", "\"" + txtloader.Text + "\"", " -info " }));
            }
            else
            {
                RichLogs("Device Already in Programmer Mode...", Color.White, true, true);
                emmcdl_cmd(string.Concat(new[] { "emmcdl.exe -p COM", (object)PortQcom, " -f ", "\"" + txtloader.Text + "\"", " -info " }));
            }
        }

        public string GetFileCalculator(double byteCount)
        {
            string str = "0 Bytes";
            if (byteCount >= 1073741824d)
            {
                str = string.Concat(string.Format("{0:##.##}", byteCount / 1073741824d), " TB");
            }
            else if (byteCount >= 1048576d)
            {
                str = string.Concat(string.Format("{0:##.##}", byteCount / 1048576d), " GB");
            }
            else if (byteCount >= 1024d)
            {
                str = string.Concat(string.Format("{0:##.##}", byteCount / 1024d), " MB");
            }
            else if (byteCount <= 0d ? false : byteCount < 1024d)
            {
                str = string.Concat(byteCount.ToString(), " KB");
            }
            return str;
        }

        public object emmcdl_class(string cmd)
        {
            Console.WriteLine(cmd);
            var process = new Process();
            process.StartInfo = new ProcessStartInfo();
            {
                var withBlock = process.StartInfo;
                withBlock.UseShellExecute = false;
                withBlock.CreateNoWindow = true;
                withBlock.FileName = "Data/Process/emmcdl.exe";
                withBlock.Arguments = cmd;
                withBlock.RedirectStandardOutput = true;
            }
            var edl_Renamed = process;
            edl_Renamed.Start();
            return edl_Renamed.StandardOutput.ReadToEnd();
        }

        public void Scan_PartTable()
        {
            DataView.Invoke(new Action(() => DataView.Rows.Clear()));

            string str = Conversions.ToString(emmcdl_class(string.Concat(new string[] { " -p COM", PortQcom.ToString(), " -f ", "\"" + txtloader.Text + "\"", " -gpt " })));
            if (str.Contains("SECTOR_SIZE_IN_BYTES=\"512\""))
            {
                Typememory = "emmc";
                SectorSize = "512";
                cbstorage.Invoke(new Action(() => cbstorage.SelectedIndex = 0));
            }
            else if (str.Contains("SECTOR_SIZE_IN_BYTES=\"4096\""))
            {
                Typememory = "ufs";
                SectorSize = "4096";
                cbstorage.Invoke(new Action(() => cbstorage.SelectedIndex = 1));
            }
            str = str.Substring(str.LastIndexOf(">") + 1);
            str = str.Substring(0, str.LastIndexOf("Status:"));
            using (var stringReader = new StringReader(str))
            {
                while (stringReader.Peek() != -1)
                {
                    string str1 = stringReader.ReadLine();
                    if (str1.Contains("Partition Name"))
                    {

                        str1 = str1.Replace("Partition Name:", "");
                        str1 = str1.Replace("Start LBA:", "");
                        str1 = str1.Replace("Size in LBA:", "");

                        var strArrays = str1.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                        DataView.Invoke(new Action(() => DataView.Rows.Add(false, "0", SectorSize, strArrays[1], strArrays[2], strArrays[3])));


                    }
                }
                if (str.Contains("The handle Is invalid."))
                {
                    RichLogs("FAILED! Please Re-Enter to EDL mode.", Color.FromArgb(239, 83, 80), true, false);
                    PortQcom = 0;
                }
            }
        }
        public string ReadWriteEraseSize(string NumPartition, string pname)
        {
            Delay(1d);
            string str = Conversions.ToString(emmcdl_class(string.Concat(new string[] { " -p COM", PortQcom.ToString(), " -f ", "\"" + txtloader.Text + "\"", " -gpt " })));
            if (str.Contains("SECTOR_SIZE_IN_BYTES=\"512\""))
            {
                Typememory = "emmc";
                SectorSize = "512";
                cbstorage.Invoke(new Action(() => cbstorage.SelectedIndex = 0));
            }
            else if (str.Contains("SECTOR_SIZE_IN_BYTES=\"4096\""))
            {
                Typememory = "ufs";
                SectorSize = "4096";
                cbstorage.Invoke(new Action(() => cbstorage.SelectedIndex = 1));
            }
            str = str.Substring(str.LastIndexOf(">") + 1);
            str = str.Substring(0, str.LastIndexOf("Status:"));
            using (var stringReader = new StringReader(str))
            {
                while (stringReader.Peek() != -1)
                {
                    string str1 = stringReader.ReadLine();
                    if (str1.Contains("Partition Name"))
                    {
                        str1 = str1.Replace("Partition Name:", "");
                        str1 = str1.Replace("Start LBA:", "");
                        str1 = str1.Replace("Size in LBA:", "");
                        var strArrays = str1.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (strArrays[1].Contains(pname))
                        {
                            return strArrays[3];
                        }
                    }
                }
            }
            return 0L.ToString();
        }

        public bool Read(string Startsector, string NumPartition, ref string SectSize, string physical, string pname)
        {

            tot = 0;

            string tmp = $"<?xml version = \"1.0\"?><data><program SECTOR_SIZE_IN_BYTES=\"{SectorSize}\" file_sector_offset=\"0\" filename=\"{pname + ".img"}\" label=\"{pname}\" num_partition_sectors=\"{NumPartition}\"  physical_partition_number=\"{physical}\" start_sector=\"{Startsector}\"/></data>";

            if (File.Exists(foldersave + @"\tmp.xml"))
            {
                File.Delete(foldersave + @"\tmp.xml");
            }

            File.WriteAllText(foldersave + @"\tmp.xml", tmp);

            return RunFHCmdRead(string.Concat(new string[] { @"--port=\\.\COM", PortQcom.ToString(), " --sendxml=\"", foldersave + @"\tmp.xml", "\"", " --search_path=\"", foldersave, "\"", " --mainoutputdir=\"", foldersave, "\"", " --convertprogram2read --noprompt --loglevel=2 --showpercentagecomplete --zlpawarehost=1", "\"" }), NumPartition, SectSize);


        }
        public bool RunFHCmdRead(string cmd, string NumPartition, string SectSize)
        {
            bool flag = false;
            var Progress = new ProgressBar();
            Progress.Minimum = 0;
            Progress.Maximum = 100;
            int resultprogress = 0;
            int now = 0;
            int thn = 0;
            var startInfo = new ProcessStartInfo(Path.Combine(Application.StartupPath + @"\Data\Process\fh_loader.exe"), cmd)
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                Verb = "runas",
                WorkingDirectory = foldersave,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
            using (var process = Process.Start(startInfo))
            {
                Console.WriteLine(cmd);
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.OutputDataReceived += (sender, e) =>
                    {
                        string text = e.Data ?? string.Empty;
                        Console.WriteLine(text);
                        long fileOffset = 0L;
                        long totalprogress = 0L;
                        if (!string.IsNullOrEmpty(text))
                        {
                            if (text.Contains("FileSizeNumSectorsLeft"))
                            {
                                string str = text.Replace(": DEBUG: FileSizeNumSectorsLeft =", "").Replace(" ", "").Replace("-", "");
                                str = str.Substring(str.IndexOf(str.Substring(8))).Replace(":", "");

                                fileOffset = (long)Math.Round(fileOffset + Conversions.ToDouble(str));
                                totalprogress = (long)Math.Round(totalprogress + Conversions.ToDouble(NumPartition));
                                Progress.Maximum = (int)(fileOffset * 100L);
                                Progress.Value = (int)Math.Round(Math.Round(fileOffset * 100L / (double)totalprogress));
                                now = Progress.Value;

                                if (Progress.Value > 0 && resultprogress < 99)
                                {
                                    if (now < thn)
                                    {
                                        resultprogress += 1;
                                        Console.WriteLine(resultprogress);
                                        ProcessBar1(resultprogress, 100L);
                                    }
                                }

                                thn = Progress.Value;
                            }

                            else if (text.Contains("{All Finished Successfully}"))
                            {
                                ProgressBar1.Invoke(new Action(() => ProgressBar1.Value = 100));
                                if (File.Exists(foldersave + @"\tmp.xml"))
                                {
                                    File.Delete(foldersave + @"\tmp.xml");
                                }
                                flag = true;
                            }
                        }
                    };
                process.WaitForExit();
            }
            return flag;
        }


        public bool Write(string Startsector, string NumPartition, ref string SectSize, string physical, string pname, string label)
        {

            tot = 0;
            foldersave = Path.Combine(new string[] { Path.GetDirectoryName(pname) });
            string filenya = Path.Combine(new string[] { Path.GetFileName(pname) });
            string tmp = $"<?xml version = \"1.0\"?><data><program SECTOR_SIZE_IN_BYTES=\"{SectorSize}\" file_sector_offset=\"0\" filename=\"{filenya}\" label=\"{pname}\" num_partition_sectors=\"{NumPartition}\"  physical_partition_number=\"{physical}\" start_sector=\"{Startsector}\"/></data>";

            if (File.Exists(foldersave + @"\tmp.xml"))
            {
                File.Delete(foldersave + @"\tmp.xml");
            }

            File.WriteAllText(foldersave + @"\tmp.xml", tmp);

            return RunFHCmdWrite(string.Concat(new string[] { @"--port=\\.\COM", PortQcom.ToString(), " --sendxml=\"", foldersave + @"\tmp.xml", "\"", " --noprompt --loglevel=2 --showpercentagecomplete --zlpawarehost=1", "\"" }));

        }

        public bool WritePatch()
        {
            tot = 0;

            foldersave = Path.Combine(new string[] { Path.GetDirectoryName(txtpatchxml.Text) });

            if (File.Exists(foldersave + @"\tmp.xml"))
            {
                File.Delete(foldersave + @"\tmp.xml");
            }

            return RunFHCmdWrite(string.Concat(new string[] { @"--port=\\.\COM", PortQcom.ToString(), " --sendxml=\"", txtpatchxml.Text, "\"", " --noprompt --loglevel=2 --showpercentagecomplete --zlpawarehost=1", "\"" }));

        }

        public bool Eraser(string Startsector, string NumPartition, ref string SectSize, string physical, string pname, string label)
        {
            tot = 0;

            string tmp = $"<?xml version = \"1.0\"?><data><erase SECTOR_SIZE_IN_BYTES=\"{SectorSize}\" file_sector_offset=\"0\" filename=\"{pname}\" label=\"{pname}\" num_partition_sectors=\"{NumPartition}\"  physical_partition_number=\"{physical}\" start_sector=\"{Startsector}\"/></data>";

            if (File.Exists(foldersave + @"\tmp.xml"))
            {
                File.Delete(foldersave + @"\tmp.xml");
            }

            File.WriteAllText(foldersave + @"\tmp.xml", tmp);

            return RunFHCmdWrite(string.Concat(new string[] { @"--port=\\.\COM", PortQcom.ToString(), " --sendxml=\"", foldersave + @"\tmp.xml", "\"", " --noprompt --loglevel=2 --showpercentagecomplete --zlpawarehost=1", "\"" }));

        }
        public bool RunFHCmdWrite(string cmd)
        {
            Delay(0.6d);
            bool flag = false;
            var Progress = new ProgressBar();
            Progress.Minimum = 0;
            Progress.Maximum = 100;

            var startInfo = new ProcessStartInfo(Path.Combine(Application.StartupPath + @"\Data\Process\fh_loader.exe"), cmd)
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                Verb = "runas",
                WorkingDirectory = foldersave,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
            using (var process = Process.Start(startInfo))
            {
                Console.WriteLine(cmd);
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.OutputDataReceived += (sender, e) =>
                    {
                        string text = e.Data ?? string.Empty;
                        Console.WriteLine(text);

                        if (!string.IsNullOrEmpty(text))
                        {
                            if (text.Contains("percent files transferred"))
                            {
                                string str = text.Replace(": INFO: {percent files transferred", "").Replace("%}", "").Replace(" ", "");
                                str = str.Substring(str.IndexOf(str.Substring(8)));
                                str = str.Substring(0, str.IndexOf('.'));

                                ProgressBar1.Invoke(new Action(() => ProgressBar1.Value = (int)Convert.ToInt64(str)));
                            }


                            else if (text.Contains("All Finished Successfully"))
                            {

                                ProgressBar1.Invoke(new Action(() => ProgressBar1.Value = 100));

                                if (File.Exists(foldersave + @"\tmp.xml"))
                                {
                                    File.Delete(foldersave + @"\tmp.xml");
                                }
                                flag = true;

                            }


                        }
                    };
                process.WaitForExit();
            }
            return flag;
        }

        public bool RunFHCmdNoWaitForExit(string cmd)
        {
            Delay(0.6d);
            bool flag = false;
            var Progress = new ProgressBar();
            Progress.Minimum = 0;
            Progress.Maximum = 100;

            var startInfo = new ProcessStartInfo(Path.Combine(Application.StartupPath + @"\Data\Process\fh_loader.exe"), cmd)
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                Verb = "runas",
                WorkingDirectory = foldersave,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
            using (var process = Process.Start(startInfo))
            {
                Console.WriteLine(cmd);
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.OutputDataReceived += (sender, e) =>
                    {
                        string text = e.Data ?? string.Empty;

                        Console.WriteLine(text);

                        if (!string.IsNullOrEmpty(text))
                        {
                            if (text.Contains("percent files transferred"))
                            {
                                string str = text.Replace(": INFO: {percent files transferred", "").Replace("%}", "").Replace(" ", "");
                                str = str.Substring(str.IndexOf(str.Substring(8)));
                                str = str.Substring(0, str.IndexOf('.'));

                                ProgressBar1.Invoke(new Action(() => ProgressBar1.Value = (int)Convert.ToInt64(str)));
                            }


                            else if (text.Contains("All Finished Successfully"))
                            {

                                ProgressBar1.Invoke(new Action(() => ProgressBar1.Value = 100));

                                if (File.Exists(foldersave + @"\tmp.xml"))
                                {
                                    File.Delete(foldersave + @"\tmp.xml");
                                }
                                flag = true;

                            }


                        }
                    };
            }
            return flag;
        }


        #endregion
    }
}