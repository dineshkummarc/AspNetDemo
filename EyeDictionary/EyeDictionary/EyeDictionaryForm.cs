using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;

using EyeDictionary.Core;

namespace EyeDictionary
{
    public partial class EyeDictionaryForm : Form
    {
        #region members
        private Core.DictionaryPack _dictionaryPack;
        private Boolean _sugesstionMode = false;
        #endregion


        public EyeDictionaryForm()
        {
            InitializeComponent();
        }


        private void EyeDictionaryForm_Load(object sender, EventArgs e)
        {
            _dictionaryPack = Core.DictionaryPack.LoadDictionary(EyeDictionary.Core.TranslatingLanguages.EnglishToFarsi);
        }


        #region Controls KeyDown
        private void textBoxKey_KeyDown(object sender, KeyEventArgs e)
        {
            _sugesstionMode = false;
            //panel.Enabled = false;

            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                Spellchecking();
            }
            else if (e.KeyCode == Keys.Up)
            {
                try
                {
                    listBoxAutoCompleteWords.SelectedIndex--;
                    listBoxAutoCompleteWords_Click(sender, e);
                }
                catch { }
            }
            else if (e.KeyCode == Keys.Down)
            {
                try
                {
                    listBoxAutoCompleteWords.SelectedIndex++;
                    listBoxAutoCompleteWords_Click(sender, e);
                }
                catch { }
            }
            else if (e.KeyCode == Keys.PageUp)
            {
                try
                {
                    listBoxAutoCompleteWords.SelectedIndex -= Global.Settings.Form.MaxItemBoundary - 1;
                    listBoxAutoCompleteWords_Click(sender, e);
                }
                catch { }
            }
            else if (e.KeyCode == Keys.PageDown)
            {
                try
                {
                    listBoxAutoCompleteWords.SelectedIndex += Global.Settings.Form.MaxItemBoundary - 1;
                    listBoxAutoCompleteWords_Click(sender, e);
                }
                catch { }
            }
        }


        private void listBoxAutoCompleteWords_KeyDown(object sender, KeyEventArgs e)
        {
            textBoxKey_KeyDown(sender, e);
        }


        private void EyeDictionaryForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Application.Exit();
        }
        #endregion


        private void textBoxKey_TextChanged(object sender, EventArgs e)
        {
            string key = textBoxKey.Text;
            if (!_sugesstionMode && key != string.Empty)
            {
                Core.DictionaryKeyIndexValue indexKeyValue = null;
                int properIndex;

                // Fill listBoxAutoCompleteWords with boundary listWords before and after the key if the key existed or not
                if (_dictionaryPack.ContainsKey(key))
                {
                    _dictionaryPack.TryGetValue(key, out indexKeyValue);

                    // Fill listBoxAutoCompleteWords
                    properIndex = -1;
                    if (indexKeyValue != null)
                    {
                        listBoxAutoCompleteWords.Items.Clear();
                        listBoxAutoCompleteWords.Items.AddRange(_dictionaryPack.GetAutoCompletedBoundaries(indexKeyValue.Index, out properIndex));
                    }

                    // Get meanings of the word
                    textBoxMeanings.Text = string.Empty;
                    foreach (string meaning in _dictionaryPack.GetMeanings(key))
                        textBoxMeanings.Text += meaning + "\r\n=======================";
                }
                else
                {
                    _sugesstionMode = false;
                    indexKeyValue = _dictionaryPack.GetAutoCompeleteWord(key, EyeDictionary.Core.AutoCompeleteLevel.Level3);

                    // Fill listBoxAutoCompleteWords
                    properIndex = -1;
                    if (indexKeyValue != null)
                    {
                        listBoxAutoCompleteWords.Items.Clear();
                        listBoxAutoCompleteWords.Items.AddRange(_dictionaryPack.GetAutoCompletedBoundaries(indexKeyValue.Index, out properIndex));
                        listBoxAutoCompleteWords.SelectedIndex = properIndex;
                    }
                }

                // Set proper SelectedIndex for listBoxAutoCompleteWords
                if (properIndex < Global.Settings.Form.MaxItemBoundary && properIndex != -1)
                    listBoxAutoCompleteWords.SelectedIndex = properIndex;
                else if (properIndex > _dictionaryPack.Count - Global.Settings.Form.MaxItemBoundary)
                    listBoxAutoCompleteWords.SelectedIndex = properIndex;
                else if (properIndex != -1)
                {
                    // Just adjust selected index
                    listBoxAutoCompleteWords.SelectedIndex = properIndex + 5;
                    listBoxAutoCompleteWords.SelectedIndex = properIndex;
                }
            }
        }


        private void listBoxAutoCompleteWords_Click(object sender, EventArgs e)
        {
            if (_sugesstionMode && listBoxAutoCompleteWords.SelectedIndex != -1)
            {
                textBoxMeanings.Text = string.Empty;

                foreach (string meaning in _dictionaryPack.GetMeanings(textBoxKey.Text))
                    textBoxMeanings.Text += meaning + "\r\n=======================";
            }
            else listBoxAutoCompleteWords_DoubleClick(sender, e);
        }


        private void listBoxAutoCompleteWords_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAutoCompleteWords.SelectedIndex != -1)
                textBoxKey.Text = listBoxAutoCompleteWords.SelectedItem.ToString();

            textBoxKey.SelectionStart = textBoxKey.TextLength;
        }


        private void buttonGo_Click(object sender, EventArgs e)
        {
            string key = textBoxKey.Text;
            if (_dictionaryPack.ContainsKey(key))
                textBoxKey_TextChanged(sender, e);
            else
                Spellchecking();
        }

  
        private void Spellchecking()
        {
            string key = textBoxKey.Text.Trim();
            textBoxMeanings.Text = string.Empty;

            if (_dictionaryPack.ContainsKey(key))
            {
                textBoxKey_TextChanged(null, null);
            }
            else
            {
                listBoxAutoCompleteWords.Items.Clear();
                _sugesstionMode = true;
                //panel.Enabled = true;
            }
        }


        private void linkLabelSpellChecker_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string key = textBoxKey.Text.Trim();
            textBoxMeanings.Text = string.Empty;

            listBoxAutoCompleteWords.Items.Clear();
            listBoxAutoCompleteWords.Items.AddRange(_dictionaryPack.GetSugesstionWords(key, (EyeDictionary.Core.SugesstionLevel)(numericUpDownLevel.Value - 1), true));
        }


        private void linkLabelAbout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Forms.AboutForm about = new EyeDictionary.Forms.AboutForm();
            about.ShowDialog();
        }
    }
}