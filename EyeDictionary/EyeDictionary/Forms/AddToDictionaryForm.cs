using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EyeDictionary.Forms
{
    public partial class AddToDictionaryForm : Form
    {
        #region
        private Core.DictionaryPack _dictionaryPack;
        #endregion


        public AddToDictionaryForm(string key, Core.DictionaryPack pack)
        {
            InitializeComponent();
            textBoxKey.Text = key;
            _dictionaryPack = pack;
        }


        private void buttonOK_Click(object sender, EventArgs e)
        {
            System.Threading.Thread addinThread = new System.Threading.Thread(new System.Threading.ThreadStart(AddToDictioanry));
            addinThread.Start();

            MessageBox.Show(Global.Settings.TextDatabaseModifier.AddToDictionarySuccessMessage);
        }

        private void AddToDictioanry()
        {
            string key = textBoxKey.Text;
            string value = textBoxValue.Text;

            if (key != string.Empty && value != string.Empty)
                Data.TextDatabaseModifier.AddToDictionary(_dictionaryPack, key, value, true);            
        }
    }
}