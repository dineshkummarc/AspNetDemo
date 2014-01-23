using System;
using System.Collections.Generic;
using System.Text;

namespace EyeDictionary.Data
{
    public static class TextDatabaseModifier
    {
        /// <summary>
        /// Add a pair of key and value to pack
        /// </summary>
        /// <param name="pack">DictionaryPack that word add to this</param>
        /// <param name="pair">a KeyValuePair</param>
        /// <param name="save">If we want save after add key and value, must be true when we use directly this method to add key and value</param>
        public static void AddToDictionary(Core.DictionaryPack pack, KeyValuePair<string, string> pair, bool save)
        {
            TextDatabaseModifier.AddToDictionary(pack, pair.Key, pair.Value, save);
        }


        /// <summary>
        /// Add a key and value to pack
        /// </summary>
        /// <param name="pack">DictionaryPack that word add to this</param>
        /// <param name="key">key word</param>
        /// <param name="value">value</param>
        /// <param name="save">If we want save after add key and value, must be true when we use directly this method to add key and value</param>
        public static void AddToDictionary(Core.DictionaryPack pack, string key, string value, bool save)
        {
            int index;
            string newKey;

            for (index = 0; ; index++)
            {
                if (index == 0)
                {
                    // If before we ha dont have the key
                    if (!pack.Dictionary.ContainsKey(key))
                    {
                        newKey = key;
                        break;
                    }
                }
                else
                {
                    // If before we ha have the key, then we search for ##(2) and ... then we must use (properIndex + 1) for number in ##(), because we wont have ##(1)
                    if (!pack.Dictionary.ContainsKey(key + "(" + (index + 1) + ")")) 
                    {
                        newKey = key + "(" + (index + 1) + ")";
                        break;
                    }
                }
            }            

            pack.Add(pack.Count, newKey, value);
            //pack.List.Add(new EyeDictionary.Core.DictionaryKeyIndexValue());
            //pack.Dictionary.Add(newKey, new EyeDictionary.Core.DictionaryKeyIndexValue(pack.Count, newKey, value));

            if (save) TextDatabaseModifier.Save(pack, true);
        }


        /// <summary>
        /// Add a rang of KeyValuePair to pack
        /// </summary>
        /// <param name="pack">DictionaryPack that word add to this</param>
        /// <param name="pairs">Pairs of KeyValuePair</param>
        public static void AddToDictionary(Core.DictionaryPack pack, KeyValuePair<string, string>[] pairs)
        {
            for (int index = 0; index < pack.Count; index++)
                TextDatabaseModifier.AddToDictionary(pack, new KeyValuePair<string, string>(pairs[index].Key, pairs[index].Value), false);
            TextDatabaseModifier.Save(pack, true);
        }

        
        /// <summary>
        /// Save DictionaryPack as new dictionary to textFile
        /// </summary>
        /// <param name="pack"></param>
        /// <param name="sort">If we want to sort pack.List</param>
        private static void Save(Core.DictionaryPack pack, bool sort)
        {
            try
            {
                System.IO.StreamWriter writer = new System.IO.StreamWriter(Global.Settings.Dictionary.En_Fa_TextDatabasePath);

                if (sort) pack.List.Sort();
                for (int index = 0; index < pack.Count; index++)
                {
                    writer.WriteLine(Global.Settings.Dictionary.DictionaryLine(pack.List[index]));
                }

                writer.Close();
                writer.Dispose();

                pack = Data.TextDatabase.LoadDictionary();
            }
            catch { }
        }        
    }
}
