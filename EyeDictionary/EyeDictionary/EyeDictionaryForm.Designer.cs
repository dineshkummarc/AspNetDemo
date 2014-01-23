namespace EyeDictionary
{
    partial class EyeDictionaryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EyeDictionaryForm));
            this.textBoxKey = new System.Windows.Forms.TextBox();
            this.textBoxMeanings = new System.Windows.Forms.TextBox();
            this.listBoxAutoCompleteWords = new System.Windows.Forms.ListBox();
            this.buttonGo = new System.Windows.Forms.Button();
            this.panel = new System.Windows.Forms.Panel();
            this.linkLabelSpellChecker = new System.Windows.Forms.LinkLabel();
            this.numericUpDownLevel = new System.Windows.Forms.NumericUpDown();
            this.linkLabelAbout = new System.Windows.Forms.LinkLabel();
            this.panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLevel)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxKey
            // 
            this.textBoxKey.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBoxKey.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBoxKey.Location = new System.Drawing.Point(12, 12);
            this.textBoxKey.Multiline = true;
            this.textBoxKey.Name = "textBoxKey";
            this.textBoxKey.Size = new System.Drawing.Size(211, 21);
            this.textBoxKey.TabIndex = 0;
            this.textBoxKey.TextChanged += new System.EventHandler(this.textBoxKey_TextChanged);
            this.textBoxKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxKey_KeyDown);
            // 
            // textBoxMeanings
            // 
            this.textBoxMeanings.Location = new System.Drawing.Point(254, 11);
            this.textBoxMeanings.Multiline = true;
            this.textBoxMeanings.Name = "textBoxMeanings";
            this.textBoxMeanings.ReadOnly = true;
            this.textBoxMeanings.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textBoxMeanings.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMeanings.Size = new System.Drawing.Size(220, 179);
            this.textBoxMeanings.TabIndex = 2;
            // 
            // listBoxAutoCompleteWords
            // 
            this.listBoxAutoCompleteWords.FormattingEnabled = true;
            this.listBoxAutoCompleteWords.Location = new System.Drawing.Point(12, 57);
            this.listBoxAutoCompleteWords.Name = "listBoxAutoCompleteWords";
            this.listBoxAutoCompleteWords.Size = new System.Drawing.Size(236, 134);
            this.listBoxAutoCompleteWords.TabIndex = 3;
            this.listBoxAutoCompleteWords.DoubleClick += new System.EventHandler(this.listBoxAutoCompleteWords_DoubleClick);
            this.listBoxAutoCompleteWords.Click += new System.EventHandler(this.listBoxAutoCompleteWords_Click);
            // 
            // buttonGo
            // 
            this.buttonGo.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.buttonGo.FlatAppearance.BorderSize = 0;
            this.buttonGo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.buttonGo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.buttonGo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonGo.Image = global::EyeDictionary.Properties.Resources.Front;
            this.buttonGo.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonGo.Location = new System.Drawing.Point(223, 12);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(25, 21);
            this.buttonGo.TabIndex = 1;
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // panel
            // 
            this.panel.Controls.Add(this.linkLabelSpellChecker);
            this.panel.Controls.Add(this.numericUpDownLevel);
            this.panel.Location = new System.Drawing.Point(12, 34);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(236, 22);
            this.panel.TabIndex = 5;
            // 
            // linkLabelSpellChecker
            // 
            this.linkLabelSpellChecker.AutoSize = true;
            this.linkLabelSpellChecker.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelSpellChecker.Location = new System.Drawing.Point(31, 4);
            this.linkLabelSpellChecker.Name = "linkLabelSpellChecker";
            this.linkLabelSpellChecker.Size = new System.Drawing.Size(131, 13);
            this.linkLabelSpellChecker.TabIndex = 6;
            this.linkLabelSpellChecker.TabStop = true;
            this.linkLabelSpellChecker.Text = "Spellchecker  (Level?)";
            this.linkLabelSpellChecker.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSpellChecker_LinkClicked);
            // 
            // numericUpDownLevel
            // 
            this.numericUpDownLevel.Location = new System.Drawing.Point(179, 0);
            this.numericUpDownLevel.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.numericUpDownLevel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownLevel.Name = "numericUpDownLevel";
            this.numericUpDownLevel.Size = new System.Drawing.Size(32, 21);
            this.numericUpDownLevel.TabIndex = 5;
            this.numericUpDownLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownLevel.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // linkLabelAbout
            // 
            this.linkLabelAbout.AutoSize = true;
            this.linkLabelAbout.Location = new System.Drawing.Point(326, 193);
            this.linkLabelAbout.Name = "linkLabelAbout";
            this.linkLabelAbout.Size = new System.Drawing.Size(148, 13);
            this.linkLabelAbout.TabIndex = 6;
            this.linkLabelAbout.TabStop = true;
            this.linkLabelAbout.Text = "About : Dictionary Databases";
            this.linkLabelAbout.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelAbout_LinkClicked);
            // 
            // EyeDictionaryForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(486, 214);
            this.Controls.Add(this.linkLabelAbout);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.listBoxAutoCompleteWords);
            this.Controls.Add(this.textBoxMeanings);
            this.Controls.Add(this.buttonGo);
            this.Controls.Add(this.textBoxKey);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EyeDictionaryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Eye Dictionary";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EyeDictionaryForm_KeyDown);
            this.Load += new System.EventHandler(this.EyeDictionaryForm_Load);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLevel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxKey;
        private System.Windows.Forms.Button buttonGo;
        private System.Windows.Forms.TextBox textBoxMeanings;
        private System.Windows.Forms.ListBox listBoxAutoCompleteWords;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.NumericUpDown numericUpDownLevel;
        private System.Windows.Forms.LinkLabel linkLabelSpellChecker;
        private System.Windows.Forms.LinkLabel linkLabelAbout;
    }
}

