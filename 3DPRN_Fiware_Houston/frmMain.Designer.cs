namespace _3DPRN_Fiware
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtprnstatus = new System.Windows.Forms.TextBox();
            this.btndelete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtprnstatus
            // 
            this.txtprnstatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtprnstatus.Location = new System.Drawing.Point(0, 0);
            this.txtprnstatus.Multiline = true;
            this.txtprnstatus.Name = "txtprnstatus";
            this.txtprnstatus.Size = new System.Drawing.Size(841, 403);
            this.txtprnstatus.TabIndex = 0;
            this.txtprnstatus.Text = "Test";
            // 
            // btndelete
            // 
            this.btndelete.Location = new System.Drawing.Point(748, 409);
            this.btndelete.Name = "btndelete";
            this.btndelete.Size = new System.Drawing.Size(81, 24);
            this.btndelete.TabIndex = 1;
            this.btndelete.Text = "delete";
            this.btndelete.UseVisualStyleBackColor = true;
            this.btndelete.Click += new System.EventHandler(this.btndelete_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 438);
            this.Controls.Add(this.btndelete);
            this.Controls.Add(this.txtprnstatus);
            this.Name = "frmMain";
            this.Text = "3Dprn Fiware";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtprnstatus;
      private System.Windows.Forms.Button btndelete;
   }
}
