using System;
using System.Collections.Generic;
using System.Text;

namespace EyeDictionary.Core
{
    public class DictionaryPack
    {
        #region Fields
        private Dictionary<string, Core.DictionaryKeyIndexValue> _dictionary;
        private List<Core.DictionaryKeyIndexValue> _list;
        #endregion


        #region Properties
        public List<Core.DictionaryKeyIndexValue> List
        {
            get { return _list; }
        }


        public Dictionary<string, Core.DictionaryKeyIndexValue> Dictionary
        {
            get { return _dictionary; }
        }


        public int Count
        {
            get { return _dictionary.Count; }
        }
        #endregion


        public DictionaryPack()
        {
            _dictionary = new Dictionary<string, DictionaryKeyIndexValue>();
            _list = new List<DictionaryKeyIndexValue>();
        }



        #region Functionality Methods
        public void AddToDictionary(string key, string value)
        {
            Data.TextDatabaseModifier.AddToDictionary(this, new KeyValuePair<string, string>(key, value), true);
        }


        public override string ToString()
        {
            return "Count = " + Count;
        }


        public void Add(int index, string key, string value)
        {
            if (!_dictionary.ContainsKey(key))
            {
                _dictionary.Add(key, new DictionaryKeyIndexValue(index, key, value));
                _list.Add(new DictionaryKeyIndexValue(index, key, value));
            }
            else
            {
                for (int i = 2; ; i++)
                {
                    string newKey = key + "(" + i + ")";
                    if (!_dictionary.ContainsKey(newKey))
                    {                        
                        _dictionary.Add(key, new DictionaryKeyIndexValue(index, newKey, value));
                        _list.Add(new DictionaryKeyIndexValue(index, newKey, value));
                    }
                }
            }
        }


        public bool TryGetValue(string key, out DictionaryKeyIndexValue indexAndValue)
        {
            return _dictionary.TryGetValue(key.Trim().ToLower(), out indexAndValue);
        }


        public string GetValue(string key)
        {
            Core.DictionaryKeyIndexValue indexAndValue;
            if (_dictionary.TryGetValue(key.Trim().ToLower(), out indexAndValue)) return indexAndValue.Value;

            return null;
        }


        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key.Trim().ToLower());
        }


        public int IndexOf(string key)
        {
            Core.DictionaryKeyIndexValue indexKeyValue;

            if (ContainsKey(key))
            {
                TryGetValue(key, out indexKeyValue);
                return indexKeyValue.Index;
            }

            return -1;
        }
        #endregion


        #region AutoCompeleteSection
        public Core.DictionaryKeyIndexValue GetAutoCompeleteWord(string key, AutoCompeleteLevel level)
        {
            key = key.TrimStart().ToLower();

            switch (level)
            {
                case AutoCompeleteLevel.Level3:
                    {
                        return Level3AutoCompeleteWord(key);
                    }
                case AutoCompeleteLevel.Level2:
                    {
                        Core.DictionaryKeyIndexValue kiv = Level1AutoCompeleteWord(key);
                        if (kiv != null)
                            return Level1AutoCompeleteWord(key);
                        else
                            return Level2AutoCompeleteWord(key);
                    }
                case AutoCompeleteLevel.Level1:
                    {
                        return Level1AutoCompeleteWord(key);
                    }
                default:
                    {
                        return null;
                    }
            }
        }


        private Core.DictionaryKeyIndexValue Level1AutoCompeleteWord(string key)
        {
            Core.DictionaryKeyIndexValue indexKeyValue = null;

            // Search for all (key + alphabet)
            foreach (string alphabet in Enum.GetNames(typeof(Core.Alphabet)))
            {
                string newKey = key + alphabet;
                if (ContainsKey(newKey))
                {
                    TryGetValue(newKey, out indexKeyValue);
                    break;
                }
            }

            return indexKeyValue;
        }


        private Core.DictionaryKeyIndexValue Level2AutoCompeleteWord(string key)
        {
            Core.DictionaryKeyIndexValue indexKeyValue = null;

            // Search last existed word; e.g. for hellooo at first we search helooo then helloo then hello then return because hello existed
            while (key.Length != 0)
            {
                if (ContainsKey(key))
                {
                    TryGetValue(key, out indexKeyValue);
                    break;
                }
                else { key = key.Remove(key.Length - 1); }
            }

            if (indexKeyValue != null && (indexKeyValue.Index + 1) < this.Count)
                return new DictionaryKeyIndexValue(_list[indexKeyValue.Index + 1].Index, _list[indexKeyValue.Index + 1].Key, _list[indexKeyValue.Index + 1].Value);
            else
                return indexKeyValue;
        }


        private Core.DictionaryKeyIndexValue Level3AutoCompeleteWord(string key)
        {
            Core.DictionaryKeyIndexValue indexKeyValue = Level2AutoCompeleteWord(key);
            if (indexKeyValue == null) indexKeyValue = Level1AutoCompeleteWord(key);
            if (indexKeyValue == null) return null;

            // 100 maximum that must be check after key
            for (int counter = 0; counter < 100; counter++)
            {
                int index = indexKeyValue.Index + counter;
                if (index > this.Count)
                    break;

                // _list[counter].Key.IndexOf(key) == 0 Means that key must we first word
                if (_list[index].Key.IndexOf(key) == 0)
                    return _list[index];
            }

            if (indexKeyValue != null && (indexKeyValue.Index + 1) < this.Count)
                return new DictionaryKeyIndexValue(_list[indexKeyValue.Index + 1].Index, _list[indexKeyValue.Index + 1].Key, _list[indexKeyValue.Index + 1].Value);
            else
                return indexKeyValue;
        }


        public string[] GetAutoCompletedBoundaries(int index, out int properIndex)
        {
            string[] words = new string[Global.Settings.Form.MaxItemBoundary * 2];

            if (index < Global.Settings.Form.MaxItemBoundary)
            {
                // Add all first(Global.Settings.Form.MaxItemBoundary * 2) listWords to listBoxAutoCompleteWords
                for (int i = 0; i < Global.Settings.Form.MaxItemBoundary * 2; i++)
                    words[i] = this._list[i].Key;

                // Set proper position                
                properIndex = index;
            }
            else if (index > this.Count - Global.Settings.Form.MaxItemBoundary)
            {
                // Add all last (Global.Settings.Form.MaxItemBoundary * 2) listWords to listBoxAutoCompleteWords
                for (int i = -Global.Settings.Form.MaxItemBoundary * 2; i < 0; i++)
                    words[i + Global.Settings.Form.MaxItemBoundary * 2] = this._list[this._list.Count + i].Key;

                // Set proper position                
                properIndex = this.Count - (this._list.Count - index);
            }
            else
            {
                // Add -Global.Settings.Form.MaxItemBoundary to Global.Settings.Form.MaxItemBoundary listWords to listBoxAutoCompleteWords.Items
                for (int i = -Global.Settings.Form.MaxItemBoundary; i < Global.Settings.Form.MaxItemBoundary; i++)
                    words[i + Global.Settings.Form.MaxItemBoundary] = this._list[index + i].Key;

                // Set proper position                
                properIndex = Global.Settings.Form.MaxItemBoundary;
            }

            return words;
        }
        #endregion


        public string[] GetMeanings(string key)
        {
            List<string> listMeanings = new List<string>();
            listMeanings.Add(GetValue(key));

            for (int counter = 2; ; counter++)
            {
                string newKey = key + "(" + counter + ")";

                if (ContainsKey(newKey)) listMeanings.Add(GetValue(newKey));
                else break;
            }

            string[] stringWords = new string[listMeanings.Count];
            for (int index = 0; index < listMeanings.Count; index++)
                stringWords[index] = listMeanings[index];

            return stringWords;
        }


        #region MyRegion
        public string[] GetSugesstionWords(string key, Core.SugesstionLevel level, bool append)
        {
            List<string> listWords = new List<string>();

            switch (level)
            {
                case SugesstionLevel.Level1:
                    {
                        Level1SugesstionWords(key, ref listWords);
                        listWords.Sort();
                        break;
                    }
                case SugesstionLevel.Level2:
                    {
                        if (append)
                        {
                            Level1SugesstionWords(key, ref listWords);
                        }
                        Level2SugesstionWords(key, ref listWords);
                        break;
                    }
                case SugesstionLevel.Level3:
                    {
                        if (append)
                        {
                            Level1SugesstionWords(key, ref listWords);
                            Level2SugesstionWords(key, ref listWords);
                        }
                        Level3SugesstionWords(key, ref listWords);
                        listWords.Sort();
                        break;
                    }
                case SugesstionLevel.Level4:
                    {
                        if (append)
                        {
                            Level1SugesstionWords(key, ref listWords);
                            Level2SugesstionWords(key, ref listWords);
                            Level3SugesstionWords(key, ref listWords);
                        }
                        Level4SugesstionWords(key, ref listWords);
                        listWords.Sort();
                        break;
                    }
                case SugesstionLevel.Level5:
                    {
                        if (append)
                        {
                            Level1SugesstionWords(key, ref listWords);
                            Level2SugesstionWords(key, ref listWords);
                            Level3SugesstionWords(key, ref listWords);
                            Level4SugesstionWords(key, ref listWords);
                        }
                        Level5SugesstionWords(key, ref listWords);
                        listWords.Sort();
                        break;
                    }
                case SugesstionLevel.Level6:
                    {
                        if (append)
                        {
                            Level1SugesstionWords(key, ref listWords);
                            Level2SugesstionWords(key, ref listWords);
                            Level3SugesstionWords(key, ref listWords);
                            Level4SugesstionWords(key, ref listWords);
                            Level5SugesstionWords(key, ref listWords);
                        }
                        Level6SugesstionWords(key, ref listWords);
                        listWords.Sort();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            string[] stringWords = new string[listWords.Count];
            for (int index = 0; index < listWords.Count; index++)
                stringWords[index] = listWords[index];

            return stringWords;
        }


        private void Level1SugesstionWords(string key, ref List<string> listWord)
        {            
            for (int index = 0; index < key.Length; index++)
            {
                foreach (string charString in Global.Settings.Form.ValidChars)
                {
                    string newKey = key.Remove(index, 1);
                    newKey = newKey.Insert(index, charString);

                    if (ContainsKey(newKey) && listWord.IndexOf(newKey) == -1)
                        listWord.Add(newKey.Trim());
                }
            }
        }


        private void Level2SugesstionWords(string key, ref List<string> listWord)
        {            
            for (int index = 0; index < key.Length - 1; index++)
            {
                char leftChar = key[index];
                char rightChar = key[index + 1];

                string newKey = key;
                newKey = newKey.Remove(index, 2);
                newKey = newKey.Insert(index, rightChar.ToString());
                newKey = newKey.Insert(index + 1, leftChar.ToString());

                if (ContainsKey(newKey) && listWord.IndexOf(newKey) == -1)
                    listWord.Add(newKey.Trim());
            }
        }


        private void Level3SugesstionWords(string key, ref List<string> listWord)
        {            
            for (int index = 0; index < key.Length + 1; index++)
            {
                foreach (string charString in Global.Settings.Form.ValidChars)
                {
                    string newKey = key.Insert(index, charString).Trim();

                    if (ContainsKey(newKey) && listWord.IndexOf(newKey) == -1)
                        listWord.Add(newKey);
                }
            }
        }


        private void Level4SugesstionWords(string key, ref List<string> listWord)
        {            
            for (int index = 0; index < key.Length + 1; index++)
            {
                foreach (string charString in Global.Settings.Form.ValidChars)
                {
                    string newKey = key.Insert(index, charString);

                    for (int index2 = 0; index2 < newKey.Length + 1; index2++)
                    {
                        foreach (string charString2 in Global.Settings.Form.ValidChars)
                        {
                            string newNewKey = newKey.Insert(index2, charString2).Trim();

                            if (ContainsKey(newNewKey) && listWord.IndexOf(newNewKey) == -1)
                                listWord.Add(newNewKey);
                        }
                    }
                }
            }
        }


        private void Level5SugesstionWords(string key, ref List<string> listWord)
        {            
            for (int index = 0; index < key.Length - 1; index++)
            {
                char leftChar = key[index];
                char rightChar = key[index + 1];

                string newKey = key;
                newKey = newKey.Remove(index, 2);
                newKey = newKey.Insert(index, rightChar.ToString());
                newKey = newKey.Insert(index + 1, leftChar.ToString());

                Level1SugesstionWords(newKey, ref listWord);
            }
        }


        private void Level6SugesstionWords(string key, ref List<string> listWord)
        {
            for (int index = 0; index < this.Count; index++)
                if (_list[index].Key.IndexOf(key) != -1)
                    listWord.Add(_list[index].Key);
        }
        #endregion


        public static Core.DictionaryPack LoadDictionary(EyeDictionary.Core.TranslatingLanguages lang)
        {
            return Data.TextDatabase.LoadDictionary();
        }
    }
}
