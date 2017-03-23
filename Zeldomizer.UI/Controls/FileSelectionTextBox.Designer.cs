﻿namespace Zeldomizer.UI.Controls
{
    partial class FileSelectionTextBox
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.fileNameTextBox = new System.Windows.Forms.TextBox();
            this.locateFileButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.fileNameTextBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.locateFileButton, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(393, 29);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // fileNameTextBox
            // 
            this.fileNameTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.fileNameTextBox.Location = new System.Drawing.Point(3, 4);
            this.fileNameTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 2);
            this.fileNameTextBox.Name = "fileNameTextBox";
            this.fileNameTextBox.Size = new System.Drawing.Size(355, 20);
            this.fileNameTextBox.TabIndex = 0;
            // 
            // locateFileButton
            // 
            this.locateFileButton.AutoSize = true;
            this.locateFileButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.locateFileButton.Location = new System.Drawing.Point(364, 3);
            this.locateFileButton.Name = "locateFileButton";
            this.locateFileButton.Size = new System.Drawing.Size(26, 23);
            this.locateFileButton.TabIndex = 1;
            this.locateFileButton.Text = "...";
            this.locateFileButton.UseVisualStyleBackColor = true;
            this.locateFileButton.Click += new System.EventHandler(this.LocateFileButtonClicked);
            // 
            // FileSelectionTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FileSelectionTextBox";
            this.Size = new System.Drawing.Size(393, 29);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox fileNameTextBox;
        private System.Windows.Forms.Button locateFileButton;
    }
}
