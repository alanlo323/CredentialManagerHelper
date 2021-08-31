
namespace CredentialManagerHelper
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
            this.checkedListBoxCredentailType = new System.Windows.Forms.CheckedListBox();
            this.checkedListBoxCredentailDetail = new System.Windows.Forms.CheckedListBox();
            this.checkBoxSelectAllDetail = new System.Windows.Forms.CheckBox();
            this.textBoxDetailInfo = new System.Windows.Forms.TextBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.progressBarInfo = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // checkedListBoxCredentailType
            // 
            this.checkedListBoxCredentailType.CheckOnClick = true;
            this.checkedListBoxCredentailType.Location = new System.Drawing.Point(12, 35);
            this.checkedListBoxCredentailType.Name = "checkedListBoxCredentailType";
            this.checkedListBoxCredentailType.Size = new System.Drawing.Size(138, 148);
            this.checkedListBoxCredentailType.TabIndex = 1;
            this.checkedListBoxCredentailType.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxCredentailType_ItemCheck);
            // 
            // checkedListBoxCredentailDetail
            // 
            this.checkedListBoxCredentailDetail.CheckOnClick = true;
            this.checkedListBoxCredentailDetail.Location = new System.Drawing.Point(156, 35);
            this.checkedListBoxCredentailDetail.Name = "checkedListBoxCredentailDetail";
            this.checkedListBoxCredentailDetail.Size = new System.Drawing.Size(732, 148);
            this.checkedListBoxCredentailDetail.TabIndex = 2;
            this.checkedListBoxCredentailDetail.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxCredentailDetail_ItemCheck);
            this.checkedListBoxCredentailDetail.SelectedIndexChanged += new System.EventHandler(this.checkedListBoxCredentailDetail_SelectedIndexChanged);
            // 
            // checkBoxSelectAllDetail
            // 
            this.checkBoxSelectAllDetail.AutoSize = true;
            this.checkBoxSelectAllDetail.Location = new System.Drawing.Point(156, 12);
            this.checkBoxSelectAllDetail.Name = "checkBoxSelectAllDetail";
            this.checkBoxSelectAllDetail.Size = new System.Drawing.Size(69, 17);
            this.checkBoxSelectAllDetail.TabIndex = 3;
            this.checkBoxSelectAllDetail.Text = "Select All";
            this.checkBoxSelectAllDetail.UseVisualStyleBackColor = true;
            this.checkBoxSelectAllDetail.CheckedChanged += new System.EventHandler(this.checkBoxSelectAllDetail_CheckedChanged);
            // 
            // textBoxDetailInfo
            // 
            this.textBoxDetailInfo.Location = new System.Drawing.Point(12, 189);
            this.textBoxDetailInfo.Multiline = true;
            this.textBoxDetailInfo.Name = "textBoxDetailInfo";
            this.textBoxDetailInfo.ReadOnly = true;
            this.textBoxDetailInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDetailInfo.Size = new System.Drawing.Size(876, 71);
            this.textBoxDetailInfo.TabIndex = 4;
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(732, 6);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 6;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(813, 6);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 7;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(587, 8);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '●';
            this.textBoxPassword.Size = new System.Drawing.Size(139, 21);
            this.textBoxPassword.TabIndex = 9;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(528, 13);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(53, 13);
            this.labelPassword.TabIndex = 10;
            this.labelPassword.Text = "Password";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(12, 8);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 11;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // progressBarInfo
            // 
            this.progressBarInfo.Location = new System.Drawing.Point(12, 266);
            this.progressBarInfo.Name = "progressBarInfo";
            this.progressBarInfo.Size = new System.Drawing.Size(876, 23);
            this.progressBarInfo.TabIndex = 12;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 296);
            this.Controls.Add(this.progressBarInfo);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.textBoxDetailInfo);
            this.Controls.Add(this.checkBoxSelectAllDetail);
            this.Controls.Add(this.checkedListBoxCredentailDetail);
            this.Controls.Add(this.checkedListBoxCredentailType);
            this.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Credential Manager Helper";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckedListBox checkedListBoxCredentailType;
        private System.Windows.Forms.CheckedListBox checkedListBoxCredentailDetail;
        private System.Windows.Forms.CheckBox checkBoxSelectAllDetail;
        private System.Windows.Forms.TextBox textBoxDetailInfo;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ProgressBar progressBarInfo;
    }
}

