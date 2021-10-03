using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryLibrary
{
    interface IChangable
    {
        public void ChangeWord(string newword, string oldword);
        public void ChangeTranslation(string newtr, string oldtr);
    }
}
