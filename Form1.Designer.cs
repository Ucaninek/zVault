namespace zVault
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.MidTxt = new System.Windows.Forms.Label();
            this.Title = new System.Windows.Forms.Label();
            this.Panel = new System.Windows.Forms.Panel();
            this.PassBox = new System.Windows.Forms.MaskedTextBox();
            this.InnerPanel = new System.Windows.Forms.Panel();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnProceed = new System.Windows.Forms.Button();
            this.Panel.SuspendLayout();
            this.InnerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MidTxt
            // 
            this.MidTxt.AllowDrop = true;
            this.MidTxt.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.MidTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MidTxt.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MidTxt.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.MidTxt.Location = new System.Drawing.Point(8, 53);
            this.MidTxt.Name = "MidTxt";
            this.MidTxt.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.MidTxt.Size = new System.Drawing.Size(268, 183);
            this.MidTxt.TabIndex = 1;
            this.MidTxt.Text = "Drag a file to get started";
            this.MidTxt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.MidTxt.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.MidTxt.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.MidTxt.DragLeave += new System.EventHandler(this.Form1_DragLeave);
            // 
            // Title
            // 
            this.Title.AllowDrop = true;
            this.Title.Dock = System.Windows.Forms.DockStyle.Top;
            this.Title.Font = new System.Drawing.Font("Segoe UI Semibold", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title.Location = new System.Drawing.Point(8, 8);
            this.Title.Name = "Title";
            this.Title.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Title.Size = new System.Drawing.Size(268, 45);
            this.Title.TabIndex = 0;
            this.Title.Text = "Welcome Back\r\n";
            this.Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Panel
            // 
            this.Panel.Controls.Add(this.PassBox);
            this.Panel.Controls.Add(this.InnerPanel);
            this.Panel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Panel.Location = new System.Drawing.Point(8, 166);
            this.Panel.Name = "Panel";
            this.Panel.Size = new System.Drawing.Size(268, 70);
            this.Panel.TabIndex = 3;
            this.Panel.Visible = false;
            // 
            // PassBox
            // 
            this.PassBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PassBox.Location = new System.Drawing.Point(3, 5);
            this.PassBox.Name = "PassBox";
            this.PassBox.PasswordChar = '•';
            this.PassBox.Size = new System.Drawing.Size(262, 26);
            this.PassBox.TabIndex = 0;
            // 
            // InnerPanel
            // 
            this.InnerPanel.Controls.Add(this.BtnCancel);
            this.InnerPanel.Controls.Add(this.BtnProceed);
            this.InnerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.InnerPanel.Location = new System.Drawing.Point(0, 34);
            this.InnerPanel.Margin = new System.Windows.Forms.Padding(0);
            this.InnerPanel.Name = "InnerPanel";
            this.InnerPanel.Size = new System.Drawing.Size(268, 36);
            this.InnerPanel.TabIndex = 4;
            // 
            // BtnCancel
            // 
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.BtnCancel.Location = new System.Drawing.Point(0, 0);
            this.BtnCancel.Margin = new System.Windows.Forms.Padding(0);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(90, 36);
            this.BtnCancel.TabIndex = 7;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnProceed
            // 
            this.BtnProceed.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnProceed.Location = new System.Drawing.Point(99, 0);
            this.BtnProceed.Margin = new System.Windows.Forms.Padding(0);
            this.BtnProceed.Name = "BtnProceed";
            this.BtnProceed.Size = new System.Drawing.Size(169, 36);
            this.BtnProceed.TabIndex = 6;
            this.BtnProceed.Text = "Proceed";
            this.BtnProceed.UseVisualStyleBackColor = true;
            this.BtnProceed.Click += new System.EventHandler(this.BtnProceed_Click);
            // 
            // Form1
            // 
            this.AcceptButton = this.BtnProceed;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtnCancel;
            this.ClientSize = new System.Drawing.Size(284, 244);
            this.Controls.Add(this.Panel);
            this.Controls.Add(this.MidTxt);
            this.Controls.Add(this.Title);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(300, 268);
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "zVault";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.DragLeave += new System.EventHandler(this.Form1_DragLeave);
            this.Panel.ResumeLayout(false);
            this.Panel.PerformLayout();
            this.InnerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Panel Panel;
        private System.Windows.Forms.Panel InnerPanel;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnProceed;
        private System.Windows.Forms.Label MidTxt;
        private System.Windows.Forms.MaskedTextBox PassBox;
    }
}

