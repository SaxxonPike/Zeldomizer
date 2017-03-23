namespace Zeldomizer.UI.Controls
{
    partial class SpriteEditFrameControl
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
            this.spriteEditControl1 = new Zeldomizer.UI.Controls.SpriteEditControl();
            this.spriteDropdown = new System.Windows.Forms.ComboBox();
            this.nextSpriteButton = new System.Windows.Forms.Button();
            this.previousSpriteButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // spriteEditControl1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.spriteEditControl1, 3);
            this.spriteEditControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spriteEditControl1.Location = new System.Drawing.Point(3, 36);
            this.spriteEditControl1.Name = "spriteEditControl1";
            this.spriteEditControl1.Size = new System.Drawing.Size(728, 424);
            this.spriteEditControl1.TabIndex = 1;
            // 
            // spriteDropdown
            // 
            this.spriteDropdown.Dock = System.Windows.Forms.DockStyle.Top;
            this.spriteDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.spriteDropdown.FormattingEnabled = true;
            this.spriteDropdown.Location = new System.Drawing.Point(87, 7);
            this.spriteDropdown.Margin = new System.Windows.Forms.Padding(3, 7, 3, 5);
            this.spriteDropdown.Name = "spriteDropdown";
            this.spriteDropdown.Size = new System.Drawing.Size(560, 21);
            this.spriteDropdown.TabIndex = 2;
            // 
            // nextSpriteButton
            // 
            this.nextSpriteButton.Location = new System.Drawing.Point(653, 6);
            this.nextSpriteButton.Margin = new System.Windows.Forms.Padding(3, 6, 6, 3);
            this.nextSpriteButton.Name = "nextSpriteButton";
            this.nextSpriteButton.Size = new System.Drawing.Size(75, 23);
            this.nextSpriteButton.TabIndex = 3;
            this.nextSpriteButton.Text = ">>";
            this.nextSpriteButton.UseVisualStyleBackColor = true;
            // 
            // previousSpriteButton
            // 
            this.previousSpriteButton.Location = new System.Drawing.Point(6, 6);
            this.previousSpriteButton.Margin = new System.Windows.Forms.Padding(6, 6, 3, 3);
            this.previousSpriteButton.Name = "previousSpriteButton";
            this.previousSpriteButton.Size = new System.Drawing.Size(75, 23);
            this.previousSpriteButton.TabIndex = 4;
            this.previousSpriteButton.Text = "<<";
            this.previousSpriteButton.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.previousSpriteButton, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.nextSpriteButton, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.spriteDropdown, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.spriteEditControl1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(734, 463);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // SpriteEditFrameControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SpriteEditFrameControl";
            this.Size = new System.Drawing.Size(734, 463);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private SpriteEditControl spriteEditControl1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button previousSpriteButton;
        private System.Windows.Forms.Button nextSpriteButton;
        private System.Windows.Forms.ComboBox spriteDropdown;
    }
}
