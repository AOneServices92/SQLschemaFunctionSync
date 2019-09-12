namespace SQLDbClonerBeliefTechno
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.txtSourceConn = new System.Windows.Forms.TextBox();
            this.txtDestConn = new System.Windows.Forms.TextBox();
            this.lblSourceCon = new System.Windows.Forms.Label();
            this.lblDestCon = new System.Windows.Forms.Label();
            this.btnSelectSourceCon = new System.Windows.Forms.Button();
            this.btnSelectDestConn = new System.Windows.Forms.Button();
            this.btnSelectItems = new System.Windows.Forms.Button();
            this.treeViewSource = new System.Windows.Forms.TreeView();
            this.treeViewDest = new System.Windows.Forms.TreeView();
            this.btnStartProcess = new System.Windows.Forms.Button();
            this.prgBar = new System.Windows.Forms.ProgressBar();
            this.dgvProgressStatus = new System.Windows.Forms.DataGridView();
            this.bgWorker = new System.ComponentModel.BackgroundWorker();
            this.lblSource = new System.Windows.Forms.Label();
            this.lblDestination = new System.Windows.Forms.Label();
            this.lblProgressPercent = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProgressStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSourceConn
            // 
            this.txtSourceConn.Location = new System.Drawing.Point(141, 12);
            this.txtSourceConn.Name = "txtSourceConn";
            this.txtSourceConn.Size = new System.Drawing.Size(327, 20);
            this.txtSourceConn.TabIndex = 0;
            // 
            // txtDestConn
            // 
            this.txtDestConn.Location = new System.Drawing.Point(141, 38);
            this.txtDestConn.Name = "txtDestConn";
            this.txtDestConn.Size = new System.Drawing.Size(327, 20);
            this.txtDestConn.TabIndex = 1;
            // 
            // lblSourceCon
            // 
            this.lblSourceCon.AutoSize = true;
            this.lblSourceCon.Location = new System.Drawing.Point(12, 15);
            this.lblSourceCon.Name = "lblSourceCon";
            this.lblSourceCon.Size = new System.Drawing.Size(104, 13);
            this.lblSourceCon.TabIndex = 2;
            this.lblSourceCon.Text = "Source Conncection";
            // 
            // lblDestCon
            // 
            this.lblDestCon.AutoSize = true;
            this.lblDestCon.Location = new System.Drawing.Point(12, 41);
            this.lblDestCon.Name = "lblDestCon";
            this.lblDestCon.Size = new System.Drawing.Size(123, 13);
            this.lblDestCon.TabIndex = 3;
            this.lblDestCon.Text = "Destination Conncection";
            // 
            // btnSelectSourceCon
            // 
            this.btnSelectSourceCon.Location = new System.Drawing.Point(474, 10);
            this.btnSelectSourceCon.Name = "btnSelectSourceCon";
            this.btnSelectSourceCon.Size = new System.Drawing.Size(102, 23);
            this.btnSelectSourceCon.TabIndex = 4;
            this.btnSelectSourceCon.Text = "<= Select Conn";
            this.btnSelectSourceCon.UseVisualStyleBackColor = true;
            this.btnSelectSourceCon.Click += new System.EventHandler(this.btnSelectSourceCon_Click);
            // 
            // btnSelectDestConn
            // 
            this.btnSelectDestConn.Location = new System.Drawing.Point(474, 36);
            this.btnSelectDestConn.Name = "btnSelectDestConn";
            this.btnSelectDestConn.Size = new System.Drawing.Size(102, 23);
            this.btnSelectDestConn.TabIndex = 5;
            this.btnSelectDestConn.Text = "<= Select Conn";
            this.btnSelectDestConn.UseVisualStyleBackColor = true;
            this.btnSelectDestConn.Click += new System.EventHandler(this.btnSelectDestConn_Click);
            // 
            // btnSelectItems
            // 
            this.btnSelectItems.Location = new System.Drawing.Point(229, 64);
            this.btnSelectItems.Name = "btnSelectItems";
            this.btnSelectItems.Size = new System.Drawing.Size(75, 23);
            this.btnSelectItems.TabIndex = 6;
            this.btnSelectItems.Text = "Load Items";
            this.btnSelectItems.UseVisualStyleBackColor = true;
            this.btnSelectItems.Click += new System.EventHandler(this.btnSelectItems_Click);
            // 
            // treeViewSource
            // 
            this.treeViewSource.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.treeViewSource.CheckBoxes = true;
            this.treeViewSource.Location = new System.Drawing.Point(15, 109);
            this.treeViewSource.Name = "treeViewSource";
            this.treeViewSource.Size = new System.Drawing.Size(246, 155);
            this.treeViewSource.TabIndex = 7;
            this.treeViewSource.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewSource_AfterCheck);
            // 
            // treeViewDest
            // 
            this.treeViewDest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.treeViewDest.Location = new System.Drawing.Point(267, 109);
            this.treeViewDest.Name = "treeViewDest";
            this.treeViewDest.Size = new System.Drawing.Size(247, 155);
            this.treeViewDest.TabIndex = 8;
            // 
            // btnStartProcess
            // 
            this.btnStartProcess.Location = new System.Drawing.Point(520, 123);
            this.btnStartProcess.Name = "btnStartProcess";
            this.btnStartProcess.Size = new System.Drawing.Size(56, 118);
            this.btnStartProcess.TabIndex = 9;
            this.btnStartProcess.Text = "Go";
            this.btnStartProcess.UseVisualStyleBackColor = true;
            this.btnStartProcess.Click += new System.EventHandler(this.btnStartProcess_Click);
            // 
            // prgBar
            // 
            this.prgBar.Location = new System.Drawing.Point(16, 270);
            this.prgBar.Name = "prgBar";
            this.prgBar.Size = new System.Drawing.Size(560, 23);
            this.prgBar.TabIndex = 10;
            // 
            // dgvProgressStatus
            // 
            this.dgvProgressStatus.AllowUserToAddRows = false;
            this.dgvProgressStatus.AllowUserToDeleteRows = false;
            this.dgvProgressStatus.AllowUserToResizeRows = false;
            this.dgvProgressStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvProgressStatus.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProgressStatus.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvProgressStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvProgressStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProgressStatus.Location = new System.Drawing.Point(16, 300);
            this.dgvProgressStatus.Name = "dgvProgressStatus";
            this.dgvProgressStatus.ReadOnly = true;
            this.dgvProgressStatus.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProgressStatus.ShowEditingIcon = false;
            this.dgvProgressStatus.Size = new System.Drawing.Size(560, 154);
            this.dgvProgressStatus.TabIndex = 11;
            this.dgvProgressStatus.VirtualMode = true;
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new System.Drawing.Point(13, 93);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(109, 13);
            this.lblSource.TabIndex = 12;
            this.lblSource.Text = "Source (Server Items)";
            // 
            // lblDestination
            // 
            this.lblDestination.AutoSize = true;
            this.lblDestination.Location = new System.Drawing.Point(267, 93);
            this.lblDestination.Name = "lblDestination";
            this.lblDestination.Size = new System.Drawing.Size(123, 13);
            this.lblDestination.TabIndex = 13;
            this.lblDestination.Text = "Destination (Local Items)";
            // 
            // lblProgressPercent
            // 
            this.lblProgressPercent.AutoSize = true;
            this.lblProgressPercent.BackColor = System.Drawing.Color.Transparent;
            this.lblProgressPercent.Location = new System.Drawing.Point(249, 275);
            this.lblProgressPercent.Name = "lblProgressPercent";
            this.lblProgressPercent.Size = new System.Drawing.Size(36, 13);
            this.lblProgressPercent.TabIndex = 14;
            this.lblProgressPercent.Text = "100 %";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 469);
            this.Controls.Add(this.lblProgressPercent);
            this.Controls.Add(this.lblDestination);
            this.Controls.Add(this.lblSource);
            this.Controls.Add(this.dgvProgressStatus);
            this.Controls.Add(this.prgBar);
            this.Controls.Add(this.btnStartProcess);
            this.Controls.Add(this.treeViewDest);
            this.Controls.Add(this.treeViewSource);
            this.Controls.Add(this.btnSelectItems);
            this.Controls.Add(this.btnSelectDestConn);
            this.Controls.Add(this.btnSelectSourceCon);
            this.Controls.Add(this.lblDestCon);
            this.Controls.Add(this.lblSourceCon);
            this.Controls.Add(this.txtDestConn);
            this.Controls.Add(this.txtSourceConn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "SQL - DB - Sync - Belief Techno";
            ((System.ComponentModel.ISupportInitialize)(this.dgvProgressStatus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSourceConn;
        private System.Windows.Forms.TextBox txtDestConn;
        private System.Windows.Forms.Label lblSourceCon;
        private System.Windows.Forms.Label lblDestCon;
        private System.Windows.Forms.Button btnSelectSourceCon;
        private System.Windows.Forms.Button btnSelectDestConn;
        private System.Windows.Forms.Button btnSelectItems;
        private System.Windows.Forms.TreeView treeViewSource;
        private System.Windows.Forms.TreeView treeViewDest;
        private System.Windows.Forms.Button btnStartProcess;
        private System.Windows.Forms.ProgressBar prgBar;
        private System.Windows.Forms.DataGridView dgvProgressStatus;
        private System.ComponentModel.BackgroundWorker bgWorker;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.Label lblDestination;
        private System.Windows.Forms.Label lblProgressPercent;
    }
}

