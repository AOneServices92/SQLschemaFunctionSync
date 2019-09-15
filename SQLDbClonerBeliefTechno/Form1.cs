using Microsoft.Data.ConnectionUI;
using SQLDbClonerBeliefTechno.Core.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace SQLDbClonerBeliefTechno
{
    public partial class frmMain : Form
    {
        private SqlTransfer transfer;
        private List<SqlObject> sItems;
        private List<SqlObject> dItems;
        private int ErrCount = 0;
        private List<SqlObject> SelItems;
        private List<SqlObject> GrdItems;
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnSelectSourceCon_Click(object sender, EventArgs e)
        {
            using (DataConnectionDialog dialog = new DataConnectionDialog())
            {
                DataSource.AddStandardDataSources(dialog);
                if (DataConnectionDialog.Show(dialog) == System.Windows.Forms.DialogResult.OK)
                {
                    txtSourceConn.Text = dialog.ConnectionString;
                }
            }
        }

        private void btnSelectDestConn_Click(object sender, EventArgs e)
        {
            using (DataConnectionDialog dialog = new DataConnectionDialog())
            {
                DataSource.AddStandardDataSources(dialog);
                if (DataConnectionDialog.Show(dialog) == System.Windows.Forms.DialogResult.OK)
                {
                    txtDestConn.Text = dialog.ConnectionString;
                }
            }
        }

        private void btnSelectItems_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtSourceConn.Text)|| string.IsNullOrWhiteSpace(txtSourceConn.Text))
            {
                MessageBox.Show("Any of the connections could not be empty.","Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                this.Cursor = Cursors.WaitCursor;
                transfer = new SqlTransfer(txtSourceConn.Text, txtDestConn.Text);
                sItems = transfer.SourceObjects;
                dItems = transfer.DestinationObjects;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                return;
            }
            prgBar.Value = 0;
            lblProgressPercent.Text = "0.00 %";
            dgvProgressStatus.DataSource = null;
            if (sItems != null && sItems.Count > 0)
            {
                treeViewSource.Nodes.Clear();
                var sourceNodes = treeViewSource.Nodes;
                var sRoot = sourceNodes.Add("All");
                string[] sItemTypes = sItems.Select(i => i.Type).Distinct().ToArray();
                string[] dItemTypes = dItems.Select(i => i.Type).Distinct().ToArray();
                for (int i = 0; i < sItemTypes.Count(); i++)
                {
                    TreeNode tnType = new TreeNode { Checked = !dItemTypes.Contains(sItemTypes[i]), Name = sItemTypes[i], Text = sItemTypes[i] };
                    sRoot.Nodes.Add(tnType);
                    var sObjs = sItems.Where(j => j.Type == sItemTypes[i]);
                    var dObjs = dItems.Where(j => j.Type == sItemTypes[i]);
                    foreach (var sObj in sObjs)
                    {
                        bool sContainsDobj = false;
                        SqlObject dObj = null;
                        foreach (var d in dObjs)
                        {
                            if (d.Name == sObj.Name)
                            {
                                sContainsDobj = true;
                                dObj = d;
                                break;
                            }
                        }

                        TreeNode tnObj = new TreeNode { Checked = !sContainsDobj, Name = sObj.Name, Text = sObj.Name };
                        tnType.Nodes.Add(tnObj);
                        if (sObj.Type.ToUpper() == "TABLE" && dObj != null)
                        {
                            var sTable = sObj.SubObject;
                            var dTable = dObj.SubObject;

                            foreach (var sColumn in sTable)
                            {
                                bool sContainsDcols = false;
                                foreach (var d in dTable)
                                {
                                    if (d.Name == sColumn.Name)
                                    {
                                        sContainsDcols = true;
                                        break;
                                    }
                                }
                                TreeNode tnCol = new TreeNode { Checked = !sContainsDcols, Name = sColumn.Name, Text = sColumn.Name };
                                tnObj.Nodes.Add(tnCol);
                            }
                        }
                    }
                }
                sRoot.ExpandAll();
            }
            else
            {
                MessageBox.Show("No SQL Objects found in source.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (dItems != null && dItems.Count > 0)
            {
                //treeViewSource.Nodes.Clear();
                treeViewDest.Nodes.Clear();
                var destNodes = treeViewDest.Nodes;
                var dRoot = destNodes.Add("All");
                foreach (var itm in dItems.Select(i => i.Type).Distinct())
                {
                    var dChild = dRoot.Nodes.Add(itm);
                    foreach (var dChld in dItems.Where(j => j.Type == itm))
                    {
                        var sObj = dChild.Nodes.Add(dChld.Name);
                        if (dChld.Type.ToUpper() == "TABLE")
                        {
                            var dTable = dChld.SubObject;
                            foreach (var dCols in dTable)
                            {
                                sObj.Nodes.Add(dCols.ToString());
                            }
                        }
                    }
                }
                dRoot.ExpandAll();
            }

            this.Cursor = Cursors.Default;
        }

        private void btnStartProcess_Click(object sender, EventArgs e)
        {
            btnSelectDestConn.Enabled = false;
            btnSelectSourceCon.Enabled = false;
            btnSelectItems.Enabled = false;
            btnStartProcess.Enabled = false;
            SelItems = new List<SqlObject>();
            GrdItems = new List<SqlObject>();
            foreach (TreeNode type in treeViewSource.Nodes[0].Nodes)
            {
                foreach (TreeNode Sobject in type.Nodes)
                {
                    if (Sobject.Checked && type.Text.ToUpper() != "TABLE")
                    {
                        SqlObject objd = sItems.Single(i => i.Name == Sobject.Text);
                        objd.Status = Properties.Resources.Unknown;
                        SelItems.Add(objd);
                    }
                    else if (Sobject.Checked && type.Text.ToUpper() == "TABLE")
                    {
                        SqlObject objd = sItems.Single(i => i.Name == Sobject.Text);
                        objd.Status = Properties.Resources.Unknown;
                        objd.SubObject = null;
                        SelItems.Add(objd);
                    }
                    else if (type.Text.ToUpper() == "TABLE")
                    {
                        bool isAddToColl = false;
                        SqlObject objs = sItems.Single(i => i.Name == Sobject.Text);
                        SqlObject objd = new SqlObject
                        {
                            Name = objs.Name,
                            Object = objs.Object,
                            SubObject = new List<Microsoft.SqlServer.Management.Smo.NamedSmoObject>(),
                            Type = objs.Type,
                            Status = Properties.Resources.Unknown
                        };
                        foreach (TreeNode Scolumn in Sobject.Nodes)
                        {
                            var subcolobj = objs.SubObject.Single(j => j.Name == Scolumn.Text);

                            if (Scolumn.Checked)
                            {
                                objd.SubObject.Add(subcolobj);
                                isAddToColl = true;
                            }
                        }
                        if (isAddToColl)
                            SelItems.Add(objd);
                    }
                }
            }
            foreach (var item in SelItems)
            {
                GrdItems.Add(item);
            }
            foreach (var item in GrdItems.Where(i => i.Type == "Table"))
            {
                SqlObject ob = new SqlObject { Name = item.Name, Type = "PrimaryKey" };
                ob.Status = Properties.Resources.Unknown;
                ob.Object = item.Object;
                SelItems.Add(ob);
            }
            foreach (var item in GrdItems.Where(i => i.Type == "Table"))
            {
                SqlObject ob = new SqlObject { Name = item.Name, Type = "ForeignKey" };
                ob.Status = Properties.Resources.Unknown;
                ob.Object = item.Object;
                SelItems.Add(ob);
            }
            foreach (var item in GrdItems.Where(i => i.Type == "Table"))
            {
                SqlObject ob = new SqlObject { Name = item.Name, Type = "Constraints" };
                ob.Status = Properties.Resources.Unknown;
                ob.Object = item.Object;
                SelItems.Add(ob);
            }
            if (SelItems.Count == 0)
            {
                MessageBox.Show("Need to select Items to sync.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            transfer.Refresh();
            dgvProgressStatus.DataSource = SelItems;
            dgvProgressStatus.Columns[0].Width = 40;
            bgWorker.DoWork += bgWorker_DoWork;
            bgWorker.RunWorkerCompleted += bgWorker_RunWorkerCompleted;
            bgWorker.ProgressChanged += bgWorker_ProgressChanged;
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.WorkerReportsProgress = true;
            bgWorker.RunWorkerAsync();
        }

        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            prgBar.Value = e.ProgressPercentage;
            lblProgressPercent.Text = e.ProgressPercentage + " %";
            dgvProgressStatus.Refresh();
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = Cursors.Default;
            btnSelectDestConn.Enabled = true;
            btnSelectSourceCon.Enabled = true;
            btnSelectItems.Enabled = true;
            btnStartProcess.Enabled = true;
            txtDestConn.Text = "";
            txtSourceConn.Text = "";
            treeViewDest.Nodes.Clear();
            treeViewSource.Nodes.Clear();
            string MessageText = "Operation Completed " + (ErrCount == 0 ? "Successfully" : "with " + ErrCount + " errors.");
            MessageBox.Show(MessageText, "Process Completed.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            double current = 0;
            double max = SelItems.Count;
            foreach (var item in SelItems.Where(i=>i.Type == "Table" || i.Type.ToUpper() == "STOREDPROCEDURE" || i.Type.ToUpper() == "USERDEFINEDFUNCTION"))
            {
                if (bgWorker.CancellationPending)
                    break;
                try
                {
                    if (item.Type.ToUpper() == "STOREDPROCEDURE" || item.Type.ToUpper() == "USERDEFINEDFUNCTION")
                    {
                        transfer.DropAndCreateObject(item.Object);
                    }
                    else if (item.Type.ToUpper() == "TABLE" && (item.SubObject == null || item.SubObject.Count < 1))
                    {
                        transfer.CreateObject(item.Object);
                    }
                    else if (item.Type.ToUpper() == "TABLE")
                    {
                        if ((item.SubObject != null) && (item.SubObject.Count > 0))
                        {
                            foreach (var col in item.SubObject)
                            {
                                //transfer.CreateObject(col);
                                transfer.CreateColumns(item.Object, col);
                            }
                        }
                    }
                    item.Status = Properties.Resources.success;
                }
                catch (Exception ex)
                {
                    item.Status = Properties.Resources.failure;
                    item.Error = ex.Message;
                    if (ex.InnerException != null)
                    {
                        item.Error += " => " + ex.InnerException.Message;
                    }
                    ErrCount++;
                }
                bgWorker.ReportProgress((int)(((++current) / max) * 100.0));
            }

            transfer = new SqlTransfer(txtSourceConn.Text, txtDestConn.Text);
            
            foreach (var item in SelItems.Where(i => i.Type == "PrimaryKey"))
            {
                try
                {
                    transfer.ApplyIndexes(item.Object);
                    item.Status = Properties.Resources.success;
                }
                catch(Exception ex)
                {
                    item.Status = Properties.Resources.failure;
                    item.Error = ex.Message;
                    if (ex.InnerException != null)
                    {
                        item.Error += " => " + ex.InnerException.Message;
                    }
                    ErrCount++;
                }
                bgWorker.ReportProgress((int)(((++current) / max) * 100.0));
            }

            foreach (var item in SelItems.Where(i => i.Type == "ForeignKey"))
            {
                try
                {
                    transfer.ApplyForeignKeys(item.Object);
                    item.Status = Properties.Resources.success;
                }
                catch(Exception ex)
                {
                    item.Status = Properties.Resources.failure;
                    item.Error = ex.Message;
                    if (ex.InnerException != null)
                    {
                        item.Error += " => " + ex.InnerException.Message;
                    }
                    ErrCount++;
                }
                bgWorker.ReportProgress((int)(((++current) / max) * 100.0));
            }

            foreach (var item in SelItems.Where(i => i.Type == "Constraints"))
            {
                try
                {
                    transfer.ApplyChecks(item.Object);
                    item.Status = Properties.Resources.success;
                }
                catch(Exception ex)
                {
                    item.Status = Properties.Resources.failure;
                    item.Error = ex.Message;
                    if (ex.InnerException != null)
                    {
                        item.Error += " => " + ex.InnerException.Message;
                    }
                    ErrCount++;
                }
                bgWorker.ReportProgress((int)(((++current) / max) * 100.0));
            }
        }

        private void treeViewSource_AfterCheck(object sender, TreeViewEventArgs e)
        {
            foreach (TreeNode node in e.Node.Nodes)
            {
                node.Checked = e.Node.Checked;
            }
        }
    }
}