namespace CxViewerAction2022.Views.DockedView
{
    partial class PerspectiveCtrl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PerspectiveCtrl));
            this.tvPerspective = new System.Windows.Forms.TreeView();
            this.lblLoading = new System.Windows.Forms.Label();
            this.contextTreeViewMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemShowDescription = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemExpand = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCollapse = new System.Windows.Forms.ToolStripMenuItem();
            this.tvImages = new System.Windows.Forms.ImageList(this.components);
            this.cbScans = new System.Windows.Forms.ComboBox();
            this.pnlScans = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.contextTreeViewMenu.SuspendLayout();
            this.pnlScans.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvPerspective
            // 
            this.tvPerspective.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvPerspective.Location = new System.Drawing.Point(0, 0);
            this.tvPerspective.Name = "tvPerspective";
            this.tvPerspective.Size = new System.Drawing.Size(572, 342);
            this.tvPerspective.TabIndex = 0;
            this.tvPerspective.Visible = false;
            this.tvPerspective.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvPerspective_NodeMouseDoubleClick);
            this.tvPerspective.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tvPerspective_MouseUp);
            this.tvPerspective.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvPerspective_NodeMouseClick);
            // 
            // lblLoading
            // 
            this.lblLoading.AutoSize = true;
            this.lblLoading.Location = new System.Drawing.Point(9, 3);
            this.lblLoading.Name = "lblLoading";
            this.lblLoading.Size = new System.Drawing.Size(0, 13);
            this.lblLoading.TabIndex = 1;
            // 
            // contextTreeViewMenu
            // 
            this.contextTreeViewMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemShowDescription,
            this.menuItemExpand,
            this.menuItemCollapse});
            this.contextTreeViewMenu.Name = "contextTreeViewMneu";
            this.contextTreeViewMenu.Size = new System.Drawing.Size(167, 92);
            // 
            // menuItemShowDescription
            // 
            this.menuItemShowDescription.Name = "menuItemShowDescription";
            this.menuItemShowDescription.Size = new System.Drawing.Size(166, 22);
            this.menuItemShowDescription.Text = "Show Description";
            this.menuItemShowDescription.Click += new System.EventHandler(this.menuItemShowDescription_Click);
            // 
            // menuItemExpand
            // 
            this.menuItemExpand.Name = "menuItemExpand";
            this.menuItemExpand.Size = new System.Drawing.Size(166, 22);
            this.menuItemExpand.Text = "Expand All";
            this.menuItemExpand.Click += new System.EventHandler(this.menuItemExpand_Click);
            // 
            // menuItemCollapse
            // 
            this.menuItemCollapse.Name = "menuItemCollapse";
            this.menuItemCollapse.Size = new System.Drawing.Size(166, 22);
            this.menuItemCollapse.Text = "Collapse All";
            this.menuItemCollapse.Click += new System.EventHandler(this.menuItemCollapse_Click);
            // 
            // tvImages
            // 
            this.tvImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("tvImages.ImageStream")));
            this.tvImages.TransparentColor = System.Drawing.Color.Transparent;
            this.tvImages.Images.SetKeyName(0, "help_16x16.gif");
            this.tvImages.Images.SetKeyName(1, "exclamation.png");
            this.tvImages.Images.SetKeyName(2, "attention2_16x16.gif");
            this.tvImages.Images.SetKeyName(3, "middle.jpg");
            this.tvImages.Images.SetKeyName(4, "cross_octagon.png");
            // 
            // cbScans
            // 
            this.cbScans.FormattingEnabled = true;
            this.cbScans.Location = new System.Drawing.Point(12, 2);
            this.cbScans.Name = "cbScans";
            this.cbScans.Size = new System.Drawing.Size(165, 21);
            this.cbScans.TabIndex = 2;
            this.cbScans.SelectedValueChanged += new System.EventHandler(this.cbScans_SelectedValueChanged);
            // 
            // pnlScans
            // 
            this.pnlScans.Controls.Add(this.cbScans);
            this.pnlScans.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlScans.Location = new System.Drawing.Point(0, 0);
            this.pnlScans.Name = "pnlScans";
            this.pnlScans.Size = new System.Drawing.Size(572, 26);
            this.pnlScans.TabIndex = 3;
            this.pnlScans.Visible = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tvPerspective);
            this.panel2.Controls.Add(this.lblLoading);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 26);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(572, 342);
            this.panel2.TabIndex = 4;
            // 
            // PerspectiveCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pnlScans);
            this.Name = "PerspectiveCtrl";
            this.Size = new System.Drawing.Size(572, 368);
            this.contextTreeViewMenu.ResumeLayout(false);
            this.pnlScans.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvPerspective;
        private System.Windows.Forms.Label lblLoading;
        private System.Windows.Forms.ContextMenuStrip contextTreeViewMenu;
        private System.Windows.Forms.ToolStripMenuItem menuItemShowDescription;
        private System.Windows.Forms.ToolStripMenuItem menuItemExpand;
        private System.Windows.Forms.ToolStripMenuItem menuItemCollapse;
        private System.Windows.Forms.ImageList tvImages;
        private System.Windows.Forms.ComboBox cbScans;
        private System.Windows.Forms.Panel pnlScans;
        private System.Windows.Forms.Panel panel2;
    }
}
