using System;
using System.Collections.Generic;
using System.Text;

namespace EyeDictionary.Global
{
    public static class Settings
    {
        public static class Dictionary
        {
            /// <summary>
            /// Filename of TextDatabase 
            /// </summary>
            public static readonly string En_Fa_TextDatabaseFilename = @"\En-Fa-TextDatabase.txt";
            public static readonly string De_En_TextDatabaseFilename = @"\De-En-TextDatabase.txt";

            /// <summary>
            /// Path of TextDatabase file
            /// </summary>
            private static readonly string _en_Fa_TextDatabasePath = System.Windows.Forms.Application.StartupPath + Dictionary.En_Fa_TextDatabaseFilename;
            private static readonly string _de_En_TextDatabasePath = System.Windows.Forms.Application.StartupPath + Dictionary.De_En_TextDatabaseFilename;


            public static string CurrentUsedDictionaryPath;


            public static readonly string Separator = "::";


            #region Properties
            public static string En_Fa_TextDatabasePath
            {
                get
                {
                    CurrentUsedDictionaryPath = Dictionary._en_Fa_TextDatabasePath;
                    return CurrentUsedDictionaryPath;
                }
            }


            public static string De_En_TextDatabasePath
            {
                get
                {
                    CurrentUsedDictionaryPath = Dictionary._de_En_TextDatabasePath;
                    return CurrentUsedDictionaryPath;
                }
            }
            #endregion


            public static string DictionaryLine(Core.DictionaryKeyIndexValue keyIndexValue)
            {
                return keyIndexValue.Key + Settings.Dictionary.Separator + keyIndexValue.Value;
            }
        }


        public static class TextDatabaseModifier
        {
            public static readonly string AddToDictionarySuccessMessage = "لغت به دیکشنری اضافه شد";
        }


        public static class Form
        {
            /// <summary>
            /// Determine items that will be represent before and after ListBox that associated to word of dictionary that we typed 
            /// </summary>
            public static readonly int MaxItemBoundary = 100;

            public static string[] ValidChars = { "a", "b", "c", "d", "e", "f", "g", "h",
                                                  "i", "j", "k", "l", "m", "n", "o", "p", 
                                                  "q", "r", "s", "t", "u", "v", "w", "x",
                                                  "y", "z", " ", ".", ",", "/", "\\", "!", "" };
        }
    }
}
