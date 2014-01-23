using System;
using System.Collections.Generic;
using System.Text;

namespace EyeDictionary.Core
{
    public class DictionaryKeyIndexValue : IComparable
    {
        #region Members
        private int _index;
        private string _value;
        private string _key;
        #endregion


        #region Properties
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }


        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }


        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
        #endregion


        public DictionaryKeyIndexValue(int index, string key, string value)
        {
            this._index = index;
            this._key = key;
            this._value = value;
        }


        public int CompareTo(Object obj)
        {
            DictionaryKeyIndexValue item = (DictionaryKeyIndexValue)obj;

            return string.Compare(this.Key.Replace(' ', '_'), item.Key.Replace(' ', '_'));
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool full)
        {
            if (full)
                return this.Key + Global.Settings.Dictionary.Separator + this.Value;
            else
                return this.Value;
        }

        #region IComparable Members

        int IComparable.CompareTo(object obj)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
