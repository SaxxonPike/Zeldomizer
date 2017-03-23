namespace Zeldomizer.UI.Forms
{
    partial class MasterForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.romTab = new System.Windows.Forms.TabPage();
            this.randomizationTab = new System.Windows.Forms.TabPage();
            this.spritesTab = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.romTab);
            this.tabControl1.Controls.Add(this.randomizationTab);
            this.tabControl1.Controls.Add(this.spritesTab);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(6, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(472, 278);
            this.tabControl1.TabIndex = 0;
            // 
            // romTab
            // 
            this.romTab.Location = new System.Drawing.Point(4, 22);
            this.romTab.Name = "romTab";
            this.romTab.Padding = new System.Windows.Forms.Padding(3);
            this.romTab.Size = new System.Drawing.Size(464, 252);
            this.romTab.TabIndex = 0;
            this.romTab.Text = "Rom";
            this.romTab.UseVisualStyleBackColor = true;
            // 
            // randomizationTab
            // 
            this.randomizationTab.Location = new System.Drawing.Point(4, 22);
            this.randomizationTab.Name = "randomizationTab";
            this.randomizationTab.Padding = new System.Windows.Forms.Padding(3);
            this.randomizationTab.Size = new System.Drawing.Size(556, 412);
            this.randomizationTab.TabIndex = 1;
            this.randomizationTab.Text = "Randomization";
            this.randomizationTab.UseVisualStyleBackColor = true;
            // 
            // spritesTab
            // 
            this.spritesTab.Location = new System.Drawing.Point(4, 22);
            this.spritesTab.Name = "spritesTab";
            this.spritesTab.Size = new System.Drawing.Size(556, 412);
            this.spritesTab.TabIndex = 2;
            this.spritesTab.Text = "Sprites";
            this.spritesTab.UseVisualStyleBackColor = true;
            // 
            // MasterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 290);
            this.Controls.Add(this.tabControl1);
            this.Name = "MasterForm";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.Text = "MasterForm";
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage romTab;
        private System.Windows.Forms.TabPage randomizationTab;
        private System.Windows.Forms.TabPage spritesTab;
    }
}