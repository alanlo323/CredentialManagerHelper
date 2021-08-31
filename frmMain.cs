using ExcelDataReader;
using ExcelDataReader.Exceptions;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Action = System.Action;

using Excel = Microsoft.Office.Interop.Excel;

namespace CredentialManagerHelper
{
    public partial class frmMain : Form
    {
        private string _formName = string.Empty;
        private Dictionary<CredentialType, List<Credential>> credentialTypeList;
        private bool isSelectAllDetailTriggerByUser = true;

        public frmMain()
        {
            InitializeComponent();

            _formName = this.Text;
            credentialTypeList = new Dictionary<CredentialType, List<Credential>>();
            checkedListBoxCredentailType.Items.Add("Loading...");
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "Excel File|*.xlsx",
                Title = "Export to File",
                FileName = "CredentialManagerData",
                OverwritePrompt = false,
            };
            DialogResult dialogResult = sfd.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                btnImport.Enabled = false;
                btnExport.Enabled = false;
                textBoxPassword.Enabled = false;
                new Thread(new ThreadStart(() =>
                {
                    List<Credential> selectedCredentails = new List<Credential>();
                    foreach (var item in checkedListBoxCredentailDetail.CheckedItems)
                    {
                        selectedCredentails.Add((Credential)item);
                    }

                    Excel.Application oXL = null;
                    Excel.Workbooks oWBs = null;
                    Excel.Workbook oWB = null;
                    Excel.Worksheet oSheet;
                    Excel.Range oRng;

                    try
                    {
                        UI_UpdateTitleMessage("Openning Excel...");
                        UI_UpdateProgressBar(value: 0, max: selectedCredentails.Count + 5);

                        oXL = new Excel.Application
                        {
                            Visible = false
                        };

                        UI_UpdateProgressBar(addValue: 1);
                        UI_UpdateTitleMessage("Creating Workbook...");

                        //Get a new workbook.
                        oWBs = oXL.Workbooks;
                        oWB = (Excel.Workbook)(oWBs.Add(Missing.Value));

                        UI_UpdateProgressBar(addValue: 1);
                        UI_UpdateTitleMessage("Creating Worksheet...");

                        oSheet = (Excel.Worksheet)oWB.ActiveSheet;

                        UI_UpdateProgressBar(addValue: 1);
                        UI_UpdateTitleMessage("Inserting Header...");

                        //Add table headers going cell by cell.
                        int i = 1;
                        oSheet.Cells[i, 1] = "CredentialType";
                        oSheet.Cells[i, 2] = "ApplicationName";
                        oSheet.Cells[i, 3] = "UserName";
                        oSheet.Cells[i, 4] = "Password";
                        oSheet.get_Range("A1", "D1").Font.Bold = true;
                        oRng = oSheet.get_Range($@"A1", $@"D{i}");
                        oRng.EntireColumn.AutoFit();
                        Marshal.FinalReleaseComObject(oRng);

                        UI_UpdateProgressBar(addValue: 1);

                        foreach (var credential in selectedCredentails)
                        {
                            UI_UpdateTitleMessage($@"Inserting object(s) ({++i}/{selectedCredentails.Count})...");

                            oSheet.Cells[i, 1] = credential.CredentialType.ToString();
                            oSheet.Cells[i, 2] = credential.ApplicationName;
                            oSheet.Cells[i, 3] = credential.UserName;
                            oSheet.Cells[i, 4] = credential.Password;

                            oRng = oSheet.get_Range($@"A1", $@"D{i}");
                            oRng.EntireColumn.AutoFit();
                            Marshal.FinalReleaseComObject(oRng);

                            UI_UpdateProgressBar(addValue: 1);
                        }

                        UI_UpdateTitleMessage($@"Saving...");

                        Marshal.FinalReleaseComObject(oSheet);

                        FileInfo fileInfo = new FileInfo(sfd.FileName);
                        oWB.SaveAs(fileInfo.FullName, Password: textBoxPassword.Text ?? null);

                        UI_UpdateProgressBar(addValue: 1);
                        UI_UpdateTitleMessage("All Done");

                        int hWnd = oXL.Application.Hwnd;
                        oWB.Close();
                        oXL.Quit();
                        Marshal.FinalReleaseComObject(oWB);
                        Marshal.FinalReleaseComObject(oXL);
                        GC.Collect();
                        GC.WaitForPendingFinalizers();

                        ProcessHelper.TryKillProcessByMainWindowHwnd(hWnd);

                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show($@"Exported {selectedCredentails.Count} record(s) to {fileInfo.FullName}", "Success");
                        }));
                    }
                    catch (Exception ex)
                    {
                        int hWnd = oXL.Application.Hwnd;
                        oWB.Close();
                        oXL.Quit();
                        Marshal.FinalReleaseComObject(oWB);
                        Marshal.FinalReleaseComObject(oXL);
                        GC.Collect();
                        GC.WaitForPendingFinalizers();

                        ProcessHelper.TryKillProcessByMainWindowHwnd(hWnd);

                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show($@"Unhandled exception{Environment.NewLine}{ex}", "Error");
                        }));
                    }
                    finally
                    {
                        UI_UpdateTitleMessage();
                        UI_UpdateProgressBar(value: 1, max: 1);
                        this.Invoke(new Action(() =>
                        {
                            btnImport.Enabled = true;
                            btnExport.Enabled = true;
                            textBoxPassword.Enabled = true;
                        }));
                    }
                })).Start();
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "Excel File|*.xlsx",
                Title = "Open File",
                CheckFileExists = true,
            };
            DialogResult dialogResult = ofd.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                btnImport.Enabled = false;
                btnExport.Enabled = false;
                textBoxPassword.Enabled = false;
                new Thread(new ThreadStart(() =>
                {
                    List<Credential> loadedCredentials = new List<Credential>();

                    UI_UpdateTitleMessage("Openning Excel...");
                    UI_UpdateProgressBar(value: 0, 1);

                    using (var stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read))
                    {
                        try
                        {
                            UI_UpdateTitleMessage("Loading Worksheet...");

                            // Auto-detect format, supports:
                            //  - Binary Excel files (2.0-2003 format; *.xls)
                            //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                            using (var reader = ExcelReaderFactory.CreateReader(stream,
                                new ExcelReaderConfiguration()
                                {
                                    Password = textBoxPassword.Text ?? null,
                                }))
                            {
                                Dictionary<string, int> headerRelatedCol = new Dictionary<string, int>();
                                DataSet dataSet = reader.AsDataSet();
                                var dataTable = dataSet.Tables["Sheet1"];

                                UI_UpdateProgressBar(addValue: 1, max: (dataTable.Rows.Count - 1) * 2 + 4);

                                for (int i = 0; i < dataTable.Rows.Count; i++)
                                {
                                    //Thread.Sleep(new Random().Next(0, 250));
                                    DataRow dataRow = dataTable.Rows[i];
                                    if (i == 0)
                                    {
                                        for (int j = 0; j < dataRow.ItemArray.Length; j++)
                                        {
                                            var item = dataRow.ItemArray[j];
                                            headerRelatedCol.Add(item.ToString(), j);
                                        }

                                        UI_UpdateProgressBar(addValue: 1);
                                    }
                                    else
                                    {
                                        UI_UpdateTitleMessage($@"Loading object(s) ({i}/{dataTable.Rows.Count - 1})...");

                                        string strCredentialType = dataRow.ItemArray[headerRelatedCol["CredentialType"]].ToString();
                                        Enum.TryParse(strCredentialType, out CredentialType credentialType);
                                        string applicationName = dataRow.ItemArray[headerRelatedCol["ApplicationName"]].ToString();
                                        string userName = dataRow.ItemArray[headerRelatedCol["UserName"]].ToString();
                                        string password = dataRow.ItemArray[headerRelatedCol["Password"]].ToString();
                                        Credential credential = new Credential(credentialType, applicationName, userName, password);
                                        loadedCredentials.Add(credential);
                                    }

                                    UI_UpdateProgressBar(addValue: 1);
                                }
                            }

                            int imported = 0;
                            int skipped = 0;
                            for (int i = 0; i < loadedCredentials.Count; i++)
                            {
                                //Thread.Sleep(new Random().Next(0, 250));
                                UI_UpdateTitleMessage($@"Importing credential(s) ({i}/{loadedCredentials.Count})...");

                                var credential = loadedCredentials[i];
                                if (CredentialManager.ReadCredential(credential.ApplicationName) == null)
                                {
                                    CredentialManager.WriteCredential(credential.ApplicationName, credential.UserName, credential.Password);
                                    imported++;
                                }
                                else
                                {
                                    skipped++;
                                }

                                UI_UpdateProgressBar(addValue: 1);
                            }

                            UI_UpdateTitleMessage("All Done");

                            this.Invoke(new Action(() =>
                            {
                                MessageBox.Show($@"Imported {imported} new record(s), skipped {skipped} record(s) that already exist", "Success");
                            }));
                        }
                        catch (InvalidPasswordException ex)
                        {
                            this.Invoke(new Action(() =>
                            {
                                MessageBox.Show($@"{ex.Message}", "Error");
                            }));
                        }
                        catch (Exception ex)
                        {
                            this.Invoke(new Action(() =>
                            {
                                MessageBox.Show($@"Unhandled exception{Environment.NewLine}{ex}", "Error");
                            }));
                        }
                        finally
                        {
                            UI_UpdateTitleMessage();
                            UI_UpdateProgressBar(value: 1, max: 1);
                            this.Invoke(new Action(() =>
                            {
                                btnImport.Enabled = true;
                                btnExport.Enabled = true;
                                textBoxPassword.Enabled = true;
                            }));
                        }
                    }
                })).Start();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshCredentialList();
        }

        private void checkBoxSelectAllDetail_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSelectAllDetail.CheckState != CheckState.Indeterminate)
            {
                isSelectAllDetailTriggerByUser = false;
                for (int i = 0; i < checkedListBoxCredentailDetail.Items.Count; i++)
                {
                    checkedListBoxCredentailDetail.SetItemChecked(i, checkBoxSelectAllDetail.Checked);
                }
                isSelectAllDetailTriggerByUser = true;
            }
        }

        private void checkedListBoxCredentailDetail_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (isSelectAllDetailTriggerByUser)
            {
                new Thread(new ThreadStart(() =>
                {
                    this.Invoke(new Action(() =>
                    {
                        if (checkedListBoxCredentailDetail.CheckedItems.Count == 0)
                        {
                            //checkBoxSelectAllDetail.Checked = false;
                            checkBoxSelectAllDetail.CheckState = CheckState.Unchecked;
                        }
                        else if (checkedListBoxCredentailDetail.CheckedItems.Count == checkedListBoxCredentailDetail.Items.Count)
                        {
                            //checkBoxSelectAllDetail.Checked = true;
                            checkBoxSelectAllDetail.CheckState = CheckState.Checked;
                        }
                        else
                        {
                            checkBoxSelectAllDetail.CheckState = CheckState.Indeterminate;
                        }
                    }));
                })).Start();
            }
        }

        private void checkedListBoxCredentailDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxDetailInfo.Text = checkedListBoxCredentailDetail.SelectedItem?.ToString();
        }

        private void checkedListBoxCredentailType_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            new Thread(new ThreadStart(() =>
            {
                this.Invoke(new Action(() =>
                {
                    checkBoxSelectAllDetail.Checked = false;
                    checkedListBoxCredentailDetail.Items.Clear();
                    foreach (CredentialType type in checkedListBoxCredentailType.CheckedItems)
                    {
                        Enum.TryParse(type.ToString(), out CredentialType credentialType);
                        credentialTypeList[credentialType].ForEach(x => checkedListBoxCredentailDetail.Items.Add(x));
                    }
                }));
            })).Start();
        }

        private void DisplayCredentialList()
        {
            credentialTypeList.Clear();
            foreach (var credential in CredentialManager.EnumerateCrendentials())
            {
                if (!credentialTypeList.ContainsKey(credential.CredentialType))
                {
                    credentialTypeList.Add(credential.CredentialType, new List<Credential>());
                }
                credentialTypeList[credential.CredentialType].Add(credential);
            }

            this.Invoke(new Action(() =>
            {
                checkedListBoxCredentailType.Items.Clear();
                checkedListBoxCredentailDetail.Items.Clear();
                foreach (var item in credentialTypeList)
                {
                    CredentialType credentialType = item.Key;
                    checkedListBoxCredentailType.Items.Add(credentialType);
                }
                checkBoxSelectAllDetail.Checked = false;
            }));
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

            RefreshCredentialList();
        }

        private void RefreshCredentialList()
        {
            try
            {
                new Thread(new ThreadStart(DisplayCredentialList)).Start();
            }
            catch (Exception)
            {
            }
        }

        private void UI_UpdateProgressBar(int? value = null, int? addValue = null, int? max = null)
        {
            this.Invoke(new Action(() =>
            {
                if (max != null)
                    progressBarInfo.Maximum = (int)max;
                if (value != null)
                    progressBarInfo.Value = (int)value;
                progressBarInfo.Value += addValue ?? 0;
            }));
        }

        private void UI_UpdateTitleMessage(string message = null)
        {
            this.Invoke(new Action(() =>
            {
                if (string.IsNullOrWhiteSpace(message))
                {
                    this.Text = _formName;
                }
                else
                {
                    this.Text = $@"{_formName} ({message})";
                }
            }));
        }
    }
}