namespace Tanji.Services.Options
{
    partial class SettingsPage
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
            this.GenerateMessageHashesBtn = new Tangine.Controls.TangineButton();
            this.SuspendLayout();
            // 
            // GenerateMessageHashesBtn
            // 
            this.GenerateMessageHashesBtn.Location = new System.Drawing.Point(91, 76);
            this.GenerateMessageHashesBtn.Name = "GenerateMessageHashesBtn";
            this.GenerateMessageHashesBtn.Size = new System.Drawing.Size(223, 20);
            this.GenerateMessageHashesBtn.TabIndex = 0;
            this.GenerateMessageHashesBtn.Text = "Generate message hashes";
            this.GenerateMessageHashesBtn.Click += new System.EventHandler(this.GenerateMessageHashesBtn_Click);
            // 
            // SettingsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GenerateMessageHashesBtn);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SettingsPage";
            this.Size = new System.Drawing.Size(402, 299);
            this.ResumeLayout(false);

        }

        #endregion

        private Tangine.Controls.TangineButton GenerateMessageHashesBtn;
    }
}
