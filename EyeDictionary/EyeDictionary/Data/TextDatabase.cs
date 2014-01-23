using System;
using System.Collections.Generic;
using System.Text;

namespace EyeDictionary.Data
{
    public static class TextDatabase
    {
        /// <summary>
        /// Read textDatabase, extract properIndex, keys and values and then  
        /// </summary>
        /// <returns>return Core.DictionaryPack instance</returns>
        public static Core.DictionaryPack LoadDictionary(Core.TranslatingLanguages language)
        {
            // Create reader for reading all lines
            System.IO.StreamReader reader;
            if (language == EyeDictionary.Core.TranslatingLanguages.EnglishToFarsi)
                reader = new System.IO.StreamReader(Global.Settings.Dictionary.En_Fa_TextDatabasePath);
            else
                reader = new System.IO.StreamReader(Global.Settings.Dictionary.De_En_TextDatabasePath);

            return GetDictionary(reader);
        }


        public static Core.DictionaryPack LoadDictionary()
        {
            // Create reader for reading all lines
            System.IO.StreamReader reader = new System.IO.StreamReader(Global.Settings.Dictionary.De_En_TextDatabasePath);

            return GetDictionary(reader);
        }


        private static EyeDictionary.Core.DictionaryPack GetDictionary(System.IO.StreamReader reader)
        {
            Core.DictionaryPack pack = new EyeDictionary.Core.DictionaryPack();

            // Read each line
            for (int index = 0; reader.Peek() != -1; index++)
            {
                string line = reader.ReadLine();
                int separatorIndex = line.IndexOf(Global.Settings.Dictionary.Separator);

                // Key is right string and value is left string of TextDatabaseModifier._separator                
                string key = line.Substring(0, separatorIndex).Trim().ToLower();
                string value = line.Substring(separatorIndex + Global.Settings.Dictionary.Separator.Length);

                // We add key ,properIndex and value as Word to _dictionaryPack
                pack.Add(index, key, value);
            }

            reader.Close();
            reader.Dispose();
            return pack;
        }
    }
}
