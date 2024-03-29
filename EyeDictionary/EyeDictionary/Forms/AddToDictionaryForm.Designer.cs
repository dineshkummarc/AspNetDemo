namespace EyeDictionary.Forms
{
    partial class AddToDictionaryForm
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
            this.labelKeyCONST = new System.Windows.Forms.Label();
            this.labelValueCONST = new System.Windows.Forms.Label();
            this.textBoxKey = new System.Windows.Forms.TextBox();
            this.textBoxValue = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelKeyCONST
            // 
            this.labelKeyCONST.AutoSize = true;
            this.labelKeyCONST.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelKeyCONST.Location = new System.Drawing.Point(22, 9);
            this.labelKeyCONST.Name = "labelKeyCONST";
            this.labelKeyCONST.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelKeyCONST.Size = new System.Drawing.Size(37, 13);
            this.labelKeyCONST.TabIndex = 0;
            this.labelKeyCONST.Text = "Word";
            // 
            // labelValueCONST
            // 
            this.labelValueCONST.AutoSize = true;
            this.labelValueCONST.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelValueCONST.Location = new System.Drawing.Point(4, 36);
            this.labelValueCONST.Name = "labelValueCONST";
            this.labelValueCONST.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelValueCONST.Size = new System.Drawing.Size(55, 13);
            this.labelValueCONST.TabIndex = 0;
            this.labelValueCONST.Text = "Meaning";
            // 
            // textBoxKey
            // 
            this.textBoxKey.Location = new System.Drawing.Point(65, 6);
            this.textBoxKey.Name = "textBoxKey";
            this.textBoxKey.Size = new System.Drawing.Size(227, 21);
            this.textBoxKey.TabIndex = 1;
            // 
            // textBoxValue
            // 
            this.textBoxValue.Location = new System.Drawing.Point(65, 33);
            this.textBoxValue.Multiline = true;
            this.textBoxValue.Name = "textBoxValue";
            this.textBoxValue.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textBoxValue.Size = new System.Drawing.Size(227, 92);
            this.textBoxValue.TabIndex = 2;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(177, 131);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "Add";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(96, 131);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // AddToDictionaryForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(299, 160);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.textBoxValue);
            this.Controls.Add(this.textBoxKey);
            this.Controls.Add(this.labelValueCONST);
            this.Controls.Add(this.labelKeyCONST);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddToDictionaryForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add New Word To Dictionary";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelKeyCONST;
        private System.Windows.Forms.Label labelValueCONST;
        private System.Windows.Forms.TextBox textBoxKey;
        private System.Windows.Forms.TextBox textBoxValue;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
    }
}