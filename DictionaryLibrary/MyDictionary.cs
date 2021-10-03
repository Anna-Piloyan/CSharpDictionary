using System;
using System.Collections.Generic;
using System.IO;

namespace DictionaryLibrary
{
   
    /// <summary>
    /// The <c>MyDictionary</c> class provides properties and methods
    /// for making actions and chanches in Dictionary
    /// </summary>

    [Serializable]
    public class MyDictionary : IChangable, ISavable, IDeletable
    {
        public delegate void DictionaryHandler(string message);
        public event DictionaryHandler Notify;
        /// <value>
        /// The <c>DictionaryType</c> contains name of the Dictionary
        /// </value>
        public string DictionaryType { get; set; }
        /// <value>
        /// The <c>Words</c> contains List of <c>DictionaryWord</c>
        /// which has word and translations of it.
        /// </value>
        public List<DictionaryWord> Words { get; set; } = new List<DictionaryWord>();
        /// <summary>
        /// Change old word to new one in the dictionary.
        /// </summary>
        /// <param name="oldword">
        /// Used to indicate a oldword.
        /// A <see cref="string"/>
        /// type representing an object of type String whose value is text
        /// </param>
        /// <param name="newword">
        /// Used to indicate a newword.
        /// A <see cref="string"/>
        /// type representing an object of type String whose value is text
        /// </param>
        /// <returns> method doesn't return a value.
        /// </returns>
        /// See<see cref="MyDictionary.ChangeWord(string, string)"/> to change words.
        public void ChangeWord(string newword, string oldword)
        {
            for (int i = 0; i < Words.Count; i++) 
            {
                if (Words[i].Word.Contains(oldword) == true) {
                    Words[i].Word = newword;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Notify?.Invoke($"Word was changed!");
                    Console.ResetColor();
                }
            }         
        }
        public void ChangeTranslation(string newtr, string oldtr)
        {
            for (int i = 0; i < Words.Count; i++)
            {
                for (int j = 0; j < Words[i].Translation.Count; j++)
                {
                    if (Words[i].Translation[j] == oldtr) {
                        Words[i].Translation[j] = newtr;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Notify?.Invoke($"Word was changed!");
                        Console.ResetColor();
                    }
                }
            }
        }
        /// <summary>
        /// Delete word from the dictionary.
        /// </summary>
        /// <param name="word">
        /// Used to indicate a word.
        /// A <see cref="string"/>
        /// type representing an object of type String whose value is text
        /// </param>
        public void DeleteWord(string word)
        {           
            for (int i = 0; i < Words.Count; i++)
            {
                if (Words[i].Word.Equals(word)) {                 
                    Words.RemoveAt(i);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Notify?.Invoke($"Word was delete!");
                    Console.ResetColor();
                }
            }           
        }
        public void DeleteTranslation(string word, string remove)
        {
            for (int i = 0; i < Words.Count; i++)
            {
                if (Words[i].Word.Equals(word)) {
                    if (Words[i].Translation.Count > 1)
                        Words[i].Translation.RemoveAt(Words[i].Translation.IndexOf(remove));
                    else if (Words[i].Translation.Count == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        throw new Exception("This is last translation! Word will not be delete!");
                    }
                }
            }         
        }

        public void SaveResult(string path, ref string result)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path + "Result.txt", true, System.Text.Encoding.Default))
                {
                    sw.WriteLine(result);
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(" Result saved!");
                Console.ResetColor();
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }

        public void SaveHistory(string path, ref string result)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path + "History.txt", true, System.Text.Encoding.Default))
                {
                    sw.WriteLine(result);
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(" History saved!");
                Console.ResetColor();
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }      
        public override string ToString()
        {
            return $" \t{DictionaryType} Vocabulary\n{string.Join("\n", Words)}";
        }
    }
}
