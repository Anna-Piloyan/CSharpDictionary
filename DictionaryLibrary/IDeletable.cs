using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryLibrary
{
    interface IDeletable
    {
        public void DeleteWord(string word);
        public void DeleteTranslation(string word, string remove);
    }
}
