using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DictionaryLibrary
{
    [Serializable]
    public class DictionaryWord
    {
        public string Word { get; set; }
        public List<string> Translation { get; set; } = new List<string>();
        public DictionaryWord() { }
        public DictionaryWord(string word, List<string> translation)
        {
            Word = word;
            Translation = translation;
        }
        public override string ToString()
        {
            return $" {Word} - {string.Join(",", Translation)}";
        }

    }
}
