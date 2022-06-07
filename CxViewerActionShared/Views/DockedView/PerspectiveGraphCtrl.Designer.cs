using System.Windows.Forms;
using CxViewerAction2022.Views.Shapes;
namespace CxViewerAction2022.Views.DockedView
{
    partial class PerspectiveGraphCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PerspectiveGraphCtrl));
            this.gViewer1 = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            this.tblLayout = new CxViewerAction2022.Views.Shapes.TableLayout();
            this.SuspendLayout();
            // 
            // gViewer1
            // 
            this.gViewer1.AsyncLayout = false;
            this.gViewer1.AutoScroll = true;
            this.gViewer1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gViewer1.BackColor = System.Drawing.Color.Transparent;
            this.gViewer1.BackwardEnabled = false;
            this.gViewer1.BuildHitTree = true;
            this.gViewer1.CurrentLayoutMethod = Microsoft.Msagl.GraphViewerGdi.LayoutMethod.SugiyamaScheme;
            this.gViewer1.ForwardEnabled = false;
            this.gViewer1.Graph = null;
            this.gViewer1.LayoutAlgorithmSettingsButtonVisible = true;
            this.gViewer1.LayoutEditingEnabled = false;
            this.gViewer1.Location = new System.Drawing.Point(0, 0);
            this.gViewer1.MouseHitDistance = 0.05;
            this.gViewer1.Name = "gViewer1";
            this.gViewer1.NavigationVisible = true;
            this.gViewer1.NeedToCalculateLayout = true;
            this.gViewer1.PanButtonPressed = false;
            this.gViewer1.SaveAsImageEnabled = true;
            this.gViewer1.SaveAsMsaglEnabled = true;
            this.gViewer1.SaveButtonVisible = true;
            this.gViewer1.SaveGraphButtonVisible = true;
            this.gViewer1.SaveInVectorFormatEnabled = true;
            this.gViewer1.Size = new System.Drawing.Size(100, 100);
            this.gViewer1.TabIndex = 0;
            this.gViewer1.ToolBarIsVisible = false;
            this.gViewer1.ZoomF = 1;
            this.gViewer1.ZoomFraction = 0.5;
            this.gViewer1.ZoomWindowThreshold = 0.05;
            // 
            // tblLayout
            // 
            this.tblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblLayout.Graph = null;
            this.tblLayout.Location = new System.Drawing.Point(20, 20);
            this.tblLayout.Name = "tblLayout";
            this.tblLayout.PathItemClick = null;     
            this.tblLayout.SelectedPath = null;
            this.tblLayout.Size = new System.Drawing.Size(100, 100);
            this.tblLayout.TabIndex = 0;
            // 
            // PerspectiveGraphCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(100, 30);
            this.AutoSize = true;
            this.Controls.Add(this.gViewer1);
            this.Name = "PerspectiveGraphCtrl";
            this.Size = new System.Drawing.Size(637, 590);
            this.Scroll += new System.Windows.Forms.ScrollEventHandler(this.PerspectiveGraphCtrl_Scroll);
            this.Resize += new System.EventHandler(this.PerspectiveGraphCtrl_Resize);
            this.SizeChanged += new System.EventHandler(this.PerspectiveGraphCtrl_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Msagl.GraphViewerGdi.GViewer gViewer1;
        private TableLayout tblLayout;

    }
}
