namespace CxViewerAction2022.Views
{
  partial class UploadFrm
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
            this.components = new System.ComponentModel.Container();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblPreset = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtProjectName = new System.Windows.Forms.TextBox();
            this.lblProject = new System.Windows.Forms.Label();
            this.cmbPreset = new System.Windows.Forms.ComboBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.cbPublic = new System.Windows.Forms.CheckBox();
            this.labelTeam = new System.Windows.Forms.Label();
            this.cmbTeams = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(278, 183);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(359, 183);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblPreset
            // 
            this.lblPreset.AutoSize = true;
            this.lblPreset.Location = new System.Drawing.Point(12, 119);
            this.lblPreset.Name = "lblPreset";
            this.lblPreset.Size = new System.Drawing.Size(40, 13);
            this.lblPreset.TabIndex = 12;
            this.lblPreset.Text = "Preset:";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(122, 32);
            this.txtDescription.MaxLength = 255;
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(312, 78);
            this.txtDescription.TabIndex = 11;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(12, 35);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(63, 13);
            this.lblDescription.TabIndex = 10;
            this.lblDescription.Text = "Description:";
            // 
            // txtProjectName
            // 
            this.txtProjectName.Location = new System.Drawing.Point(122, 6);
            this.txtProjectName.MaxLength = 200;
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.Size = new System.Drawing.Size(312, 20);
            this.txtProjectName.TabIndex = 9;
            this.txtProjectName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtProjectName_KeyUp);
            // 
            // lblProject
            // 
            this.lblProject.AutoSize = true;
            this.lblProject.Location = new System.Drawing.Point(12, 9);
            this.lblProject.Name = "lblProject";
            this.lblProject.Size = new System.Drawing.Size(74, 13);
            this.lblProject.TabIndex = 8;
            this.lblProject.Text = "Project Name:";
            // 
            // cmbPreset
            // 
            this.cmbPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPreset.FormattingEnabled = true;
            this.cmbPreset.Items.AddRange(new object[] {
            "All"});
            this.cmbPreset.Location = new System.Drawing.Point(122, 116);
            this.cmbPreset.Name = "cmbPreset";
            this.cmbPreset.Size = new System.Drawing.Size(312, 21);
            this.cmbPreset.TabIndex = 16;
            this.cmbPreset.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cmbPreset_KeyUp);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // cbPublic
            // 
            this.cbPublic.AutoSize = true;
            this.cbPublic.Checked = true;
            this.cbPublic.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPublic.Location = new System.Drawing.Point(15, 187);
            this.cbPublic.Name = "cbPublic";
            this.cbPublic.Size = new System.Drawing.Size(187, 17);
            this.cbPublic.TabIndex = 17;
            this.cbPublic.Text = "Make project visible to other users";
            this.cbPublic.UseVisualStyleBackColor = true;
            // 
            // labelTeam
            // 
            this.labelTeam.AutoSize = true;
            this.labelTeam.Location = new System.Drawing.Point(12, 147);
            this.labelTeam.Name = "labelTeam";
            this.labelTeam.Size = new System.Drawing.Size(37, 13);
            this.labelTeam.TabIndex = 18;
            this.labelTeam.Text = "Team:";
            // 
            // cmbTeams
            // 
            this.cmbTeams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTeams.FormattingEnabled = true;
            this.cmbTeams.Items.AddRange(new object[] {
            "All"});
            this.cmbTeams.Location = new System.Drawing.Point(122, 144);
            this.cmbTeams.Name = "cmbTeams";
            this.cmbTeams.Size = new System.Drawing.Size(312, 21);
            this.cmbTeams.TabIndex = 19;
            this.cmbTeams.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cmbTeams_KeyUp);
            // 
            // UploadFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(446, 216);
            this.Controls.Add(this.cmbTeams);
            this.Controls.Add(this.labelTeam);
            this.Controls.Add(this.cbPublic);
            this.Controls.Add(this.cmbPreset);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblPreset);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtProjectName);
            this.Controls.Add(this.lblProject);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UploadFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Upload Source";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UploadFrm_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Label lblPreset;
    private System.Windows.Forms.TextBox txtDescription;
    private System.Windows.Forms.Label lblDescription;
    private System.Windows.Forms.TextBox txtProjectName;
    private System.Windows.Forms.Label lblProject;
    private System.Windows.Forms.ComboBox cmbPreset;
    private System.Windows.Forms.ErrorProvider errorProvider;
    private System.Windows.Forms.CheckBox cbPublic;
    private System.Windows.Forms.ComboBox cmbTeams;
    private System.Windows.Forms.Label labelTeam;
  }
}