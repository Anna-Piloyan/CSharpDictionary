using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryLibrary
{
    interface ISavable
    {
        public void SaveResult(string path, ref string result);
        public void SaveHistory(string path, ref string result);
    }
}
