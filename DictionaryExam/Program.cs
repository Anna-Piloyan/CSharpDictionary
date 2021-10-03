using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using DictionaryLibrary;

namespace DictionaryExam
{  
    class Program
	{        
        static void Main(string[] args)
        {
            string s = new string('-', 50);
            int choice = 0;
            string path = @"Folder\";
            string path1 = @"Folder1\";
            DirectoryInfo Info = new DirectoryInfo(path);
			List<string> historyList = new List<string>();
			MyDictionary dictionary = new MyDictionary();
            XmlSerializer formatter = new XmlSerializer(typeof(MyDictionary));
            do
            {
                Console.Clear();
                Console.WriteLine(s);
                Console.WriteLine("\tDICTIONARY LIBRARY\n");

                var files = Info.GetFiles("*.*", SearchOption.AllDirectories).Select(f => f).ToList();
                for (int i = 0; i < files.Count; i++)
                {
                    Console.WriteLine((i + 1) + ". " + files[i].Name.Remove(files[i].Name.Length - 4));
                }
                Console.WriteLine(s);
                Console.WriteLine("\t MENU.");
                Console.WriteLine(" 0 - EXIT.");
                Console.WriteLine(" 1 - Create dictionary");
                Console.WriteLine(" 2 - Add word and translation to the dictionary");
                Console.WriteLine(" 3 - Change word in dictionary");
                Console.WriteLine(" 4 - Delete word");
                Console.WriteLine(" 5 - Delete translation");
                Console.WriteLine(" 6 - Find translation");
                Console.WriteLine(" 7 - Sort dictionary from A-Z");
                Console.WriteLine(" 8 - Sort dictionary from Z-A");
                Console.WriteLine(" 9 - Dictionary checked history");
                Console.WriteLine(" 10 - Add translation");
                Console.WriteLine(s);
                Console.WriteLine(dictionary);
                Console.WriteLine(s);
                Console.Write(" Enter your choice: ");
                choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        {
                            Console.Write(" Enter Dictionary Type: ");
                            string dictionaryType = Console.ReadLine();
                            dictionary.DictionaryType = dictionaryType;
                            using (FileStream fs = new FileStream(path + dictionaryType + ".xml", FileMode.Create))
                            {
                                formatter.Serialize(fs, dictionary);
                            }
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(" Dictionary created!");
                            Console.ResetColor();
                            Console.ReadKey();
                            break;
                        }
                    case 2:
                        {
                            int amount = 0;
                            Console.Write(" Enter name of the dictionary: ");
                            string filename = Console.ReadLine();
                            DictionaryDeserialise(path, filename, ref dictionary);
                            Console.Clear();
                            Console.WriteLine(s);
                            Console.WriteLine(dictionary);
                            Console.WriteLine(s);
                            Console.Write(" Enter word: ");
                            string word = Console.ReadLine();
                            try
                            {
                                var result = dictionary.Words.Where(w => w.Word == word).Select(t => t).DefaultIfEmpty().First();
                                if (result == null) {
                                    Console.Write(" Enter amount of translation words: ");
                                    try { amount = Convert.ToInt32(Console.ReadLine()); }
                                    catch (FormatException ex) { Console.WriteLine(ex.Message); }
                                    List<string> translation = new List<string>();                                  
                                    for (int i = 0; i < amount; i++) {
                                        Console.Write(" Enter word: ");
                                        string tr = Console.ReadLine();
                                        string[] name = dictionary.DictionaryType.Split('-');
                                        if (name[1] == "Russian") {
                                            try
                                            {
                                                if (!MyStringExtension.IsValidCyrillicEnter(tr)){
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    throw new Exception("Enter text with cyrillic letters!");
                                                }
                                                else
                                                    translation.Add(tr);
                                            }
                                            catch (Exception ex) { Console.WriteLine(ex.Message); }
                                            Console.ResetColor();
                                        }
                                        if (name[1] == "Polish" || name[1] == "English" || name[1] == "Franch"){
                                            try
                                            {
                                                if (!MyStringExtension.IsValidLatinEnter(tr)) {
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    throw new Exception("Enter text with latin letters!");
                                                }
                                                else
                                                    translation.Add(tr);
                                            }
                                            catch (Exception ex) { Console.WriteLine(ex.Message); }
                                            Console.ResetColor();
                                        }
                                    }
                                    try
                                    {
                                        if (translation.Count == 0) {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            throw new Exception("Word was not Added! You didn't Enter Translation!");
                                        }
                                        else
                                        dictionary.Words.Add(new DictionaryWord(word, translation));
                                    }
                                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                                    Console.ResetColor();
                                    DictionarySerialise(path, filename, ref dictionary);
                                }
                                else {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Word already exsist!");
                                }
                                Console.ResetColor();
                            }
                            catch (Exception ex) { Console.WriteLine(ex.Message); }
                            Console.ReadKey();
                            break;
                            }
                    case 3:
                        {
                            Console.Write(" Enter name of the dictionary: ");
                            string filename = Console.ReadLine();
                            DictionaryDeserialise(path, filename, ref dictionary);
                            Console.Clear();
                            Console.WriteLine(s);
                            Console.WriteLine(dictionary);
                            Console.WriteLine(s);
                            Console.Write(" Enter old word: ");
                            string oldword = Console.ReadLine();
                            try
                            {
                                var result = dictionary.Words.Where(w => w.Word == oldword).Select(t => t).DefaultIfEmpty().First();
                                if (result != null) {
                                    Console.Write(" Enter new word: ");
                                    string newword = Console.ReadLine();
                                    dictionary.ChangeWord(newword, oldword);
                                    Console.Write(" Enter old translation word: ");
                                    string oldtr = Console.ReadLine();
                                    Console.Write(" Enter new translation word: ");
                                    string newtr = Console.ReadLine();
                                    dictionary.Notify += DisplayMessage;
                                    dictionary.ChangeTranslation(newtr, oldtr);
                                }
                                else {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Word Not Found!");
                                }
                                Console.ResetColor();
                            }
                            catch (Exception ex) { Console.WriteLine(ex.Message); }
                            DictionarySerialise(path, filename, ref dictionary);
                            Console.ReadKey();
                            break;
                        }
                    case 4:
                        {
                            Console.Write(" Enter name of the dictionary: ");
                            string filename = Console.ReadLine();
                            DictionaryDeserialise(path, filename, ref dictionary);
                            Console.Clear();
                            Console.WriteLine(s);
                            Console.WriteLine(dictionary);
                            Console.WriteLine(s);
                            Console.Write(" Enter word to delete: ");
                            string word = Console.ReadLine();
                            try
                            {
                                var result = dictionary.Words.Where(w => w.Word == word).Select(t => t).DefaultIfEmpty().First();
                                if (result != null) {
                                    dictionary.Notify += DisplayMessage;
                                    dictionary.DeleteWord(word);
                                }
                                else {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Word Not Found!");
                                }
                                Console.ResetColor();
                            }
                            catch (Exception ex) { Console.WriteLine(ex.Message); }
                            DictionarySerialise(path, filename, ref dictionary);
                            Console.ReadKey();
                            break;
                        }
                    case 5:
                        {
                            Console.Write(" Enter name of the dictionary: ");
                            string filename = Console.ReadLine();
                            DictionaryDeserialise(path, filename, ref dictionary);
                            Console.Clear();
                            Console.WriteLine(s);
                            Console.WriteLine(dictionary);
                            Console.WriteLine(s);
                            Console.Write(" Enter word to delete translations: ");
                            string word = Console.ReadLine();
                            Console.Write(" Enter translation which you want to delete: ");
                            string remove = Console.ReadLine();
                            try { dictionary.DeleteTranslation(word, remove); }
                            catch (Exception ex) { Console.WriteLine(ex.Message); }
                            Console.ResetColor();
                            DictionarySerialise(path, filename, ref dictionary);
                            Console.ReadKey();
                            break;
                        }
                    case 6:
                        {
                            Console.Write(" Enter name of the dictionary: ");
                            string filename = Console.ReadLine();
                            DictionaryDeserialise(path, filename, ref dictionary);
                            Console.Clear();
                            Console.WriteLine(s);
                            Console.WriteLine(dictionary);
                            Console.WriteLine(s);
                            Console.Write(" Enter word to find translation: ");
                            string word = Console.ReadLine();
                            try
                            {
                                var result = dictionary.Words.Where(w => w.Word == word).Select(t => t).DefaultIfEmpty().First();
                                if (result != null) {
                                    Console.WriteLine(result);
                                    historyList.Add(result.ToString());
                                    string res = result.ToString();
                                    dictionary.SaveResult(path1, ref res);
                                    dictionary.SaveHistory(path1, ref res);
                                }
                                else  {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine($" Word: \"{word}\" Not Found!");
                                }
                                Console.ResetColor();
                            }
                            catch (Exception ex) { Console.WriteLine(ex.Message); }
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine(" Last 2 checked words history:");
                            foreach (var item in historyList.TakeLast(2))
                            {
                                Console.WriteLine(item);
                            }
                            Console.ResetColor();
                            Console.ReadKey();
                            break;
                        }
                    case 7:
                        {
                            Console.Write(" Enter name of the dictionary: ");
                            string filename = Console.ReadLine();
                            DictionaryDeserialise(path, filename, ref dictionary);
                            Console.Clear();
                            Console.WriteLine(s);
                            Console.WriteLine($"\t{dictionary.DictionaryType}");
                            Console.WriteLine(s);
                            foreach (var item in dictionary.Words.OrderBy(d => d.Word))
                            {
                                Console.WriteLine(item);
                            }
                            Console.WriteLine(s);
                            Console.ReadKey();
                            break;
                        }
                    case 8:
                        {
                            Console.Write(" Enter name of the dictionary: ");
                            string filename = Console.ReadLine();
                            DictionaryDeserialise(path, filename, ref dictionary);
                            Console.Clear();
                            Console.WriteLine(s);
                            Console.WriteLine($"\t{dictionary.DictionaryType}");
                            Console.WriteLine(s);
                            foreach (var item in dictionary.Words.OrderByDescending(d => d.Word))
                            {
                                Console.WriteLine(item);
                            }
                            Console.WriteLine(s);
                            Console.ReadKey();
                            break;
                        }
                    case 9:
                        {
                            StreamReader reader = new StreamReader(path1 + "History.txt");
                            string str = reader.ReadToEnd();
                            reader.Close();
                            Console.ForegroundColor = ConsoleColor.Blue;                          
                            Console.WriteLine(" Checked words history:");
                            Console.WriteLine(str);
                            Console.ResetColor();
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(" To Delete History - Press<D>");
                            Console.WriteLine(" To Continue - Press<C>");
                            Console.WriteLine(s);                        
                            Console.Write(" Enter your choice: ");
                            Console.ResetColor();
                            char sym = Convert.ToChar(Console.ReadLine());
                            if (sym == 'D') {
                                File.Delete(path1 + "History.txt");
                            }
                            else if (sym == 'C')
                                break;
                            Console.ReadKey();
                            break;
                        }                       
                }
            } while (choice != 0);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(" Please, don't forget your printed file");
            Console.ResetColor();
            File.Delete(path1 + "Result.txt");
            Console.ReadKey();
            Console.WriteLine(" Thank you for checking!");          
        }

        public static void DictionaryDeserialise(string path, string filename, ref MyDictionary dictionary)
        {         
            XmlSerializer formatter = new XmlSerializer(typeof(MyDictionary));        
            using (FileStream fs = new FileStream(path + filename + ".xml", FileMode.Open))
            {
                dictionary = (MyDictionary)formatter.Deserialize(fs);                        
            }
        }

        public static void DictionarySerialise(string path, string filename, ref MyDictionary dictionary)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(MyDictionary));
            using (FileStream fs = new FileStream(path + filename + ".xml", FileMode.Truncate))            
            {
                formatter.Serialize(fs, dictionary);
            }
        }
        private static void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
   
}


