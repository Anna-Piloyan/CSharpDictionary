using DictionaryLibrary;
using System;
using System.Text.RegularExpressions;

public static class MyStringExtension
{
    public static bool IsValidLatinEnter(this string word)
    {
        string pattern = @"^[a-zA-Z]{1,20}$";
        Regex regex = new Regex(pattern);
        return regex.IsMatch(word);
    }
    public static bool IsValidCyrillicEnter(this string word)
    {
        string pattern = @"^[а-яА-Я]{1,20}$";
        Regex regex = new Regex(pattern);
        return regex.IsMatch(word);
    }
    

}