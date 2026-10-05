using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP224_Kadrgaliev
{
    public class TextStat
    {
        private string text;
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                if (value == null || value.Length < 100)
                {
                    throw new ArgumentException("Текст должен содержать минимум 100 символов.");
                }
                text = value;
            }
        }
        public int WordCount { get; set; }
        public int SentenceCount { get; set; }
        public List<string> ShortestWords { get; set; }
        public List<string> LongestWords { get; set; }
        public int VowelCount { get; set; }
        public int ConsonantCount { get; set; }
        public Dictionary<char, int> LetterQuantity { get; set; }
        public TextStat()
        {
            LetterQuantity = new Dictionary<char, int>();
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            List<TextStat> history = new List<TextStat>();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Программа анализа текста");
                Console.WriteLine("1 - Ввести новый текст");
                Console.WriteLine("2 - Показать статистику");
                Console.WriteLine("3 - Выход");
                Console.Write("Выберите действие: ");
                string choice = Console.ReadLine();
                if (choice == "1")
                {
                    Console.Clear();
                    AnalyzeNewText(history);
                    Console.WriteLine();
                    Console.WriteLine("Нажмите любую клавишу...");
                    Console.ReadKey();
                }
                else if (choice == "2")
                {
                    Console.Clear();
                    ShowHistory(history);
                    Console.WriteLine();
                    Console.WriteLine("Нажмите любую клавишу...");
                    Console.ReadKey();
                }
                else if (choice == "3")
                {
                    Console.Clear();
                    Console.WriteLine("Выход из программы");
                    return;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Такой команды нет");
                    Console.WriteLine();
                    Console.WriteLine("Нажмите любую клавишу...");
                    Console.ReadKey();
                }
            }
        }
        static List<string> GetWords(string text)
        {
            List<string> words = new List<string>();
            string currentWord = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (char.IsLetter(text[i]))
                {
                    currentWord += text[i];
                }
                else
                {
                    if (currentWord.Length > 0)
                    {
                        if (IsWord(currentWord))
                        {
                            words.Add(currentWord);
                        }
                        currentWord = "";
                    }
                }
            }
            if (currentWord.Length > 0)
            {
                if (IsWord(currentWord))
                {
                    words.Add(currentWord);
                }
            }
            return words;
        }
        static bool IsWord(string word)
        {
            if (word.Length > 1)
            {
                return true;
            }
            string oneLetterWords = "аивксуояai";
            string lowerWord = word.ToLower();
            return oneLetterWords.Contains(lowerWord);
        }
        static int CountSentences(string text)
        {
            int count = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '.' || text[i] == '!' || text[i] == '?')
                {
                    int next = i + 1;

                    while (next < text.Length && char.IsWhiteSpace(text[next]))
                    {
                        next++;
                    }
                    if (next < text.Length && char.IsUpper(text[next]))
                    {
                        count++;
                    }
                    else if (next == text.Length)
                    {
                        count++;
                    }
                }
            }
            string trimmedText = text.TrimEnd();
            if (trimmedText.Length > 0)
            {
                char last = trimmedText[trimmedText.Length - 1];
                if (last != '.' && last != '!' && last != '?')
                {
                    count++;
                }
            }
            return count;
        }
        static int CountWords(string text)
        {
            List<string> wordsList = GetWords(text);
            return wordsList.Count;
        }
        static void FindShortestAndLongestWords(List<string> words, out List<string> shortestWords, out List<string> longestWords)
        {
            shortestWords = new List<string>();
            longestWords = new List<string>();
            if (words.Count == 0)
            {
                return;
            }
            int shortestLength = words[0].Length;
            int longestLength = words[0].Length;
            for (int i = 1; i < words.Count; i++)
            {
                if (words[i].Length < shortestLength)
                {
                    shortestLength = words[i].Length;
                }
                if (words[i].Length > longestLength)
                {
                    longestLength = words[i].Length;
                }
            }
            for (int i = 0; i < words.Count; i++)
            {
                string word = words[i].ToLower();
                if (words[i].Length == shortestLength && !shortestWords.Contains(word))
                {
                    shortestWords.Add(word);
                }
                if (words[i].Length == longestLength && !longestWords.Contains(word))
                {
                    longestWords.Add(word);
                }
            }
        }
        static void CountLetters(string text, out int vowels, out int consonants)
        {
            vowels = 0;
            consonants = 0;
            string vowelLetters = "аеёиоуыэюя" + "aeiou";
            string consonantLetters = "бвгджзйклмнпрстфхцчшщ" + "bcdfghjklmnpqrstvwxyz";
            for (int i = 0; i < text.Length; i++)
            {
                char letter = char.ToLower(text[i]);
                if (vowelLetters.Contains(letter))
                {
                    vowels++;
                }
                else if (consonantLetters.Contains(letter))
                {
                    consonants++;
                }
            }
        }
        static void CountLetterFrequency(string text, TextStat textStat)
        {
            string allowedLetters = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя" + "abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < text.Length; i++)
            {
                char letter = char.ToLower(text[i]);

                if (allowedLetters.Contains(letter))
                {
                    if (textStat.LetterQuantity.ContainsKey(letter))
                    {
                        textStat.LetterQuantity[letter]++;
                    }
                    else
                    {
                        textStat.LetterQuantity.Add(letter, 1);
                    }
                }
            }
        }
        static void AnalyzeNewText(List<TextStat> history)
        {
            TextStat textStat = new TextStat();
            Console.WriteLine("Введите текст (не менее 100 символов):");
            string inputText = Console.ReadLine();
            try
            {
                textStat.Text = inputText;
                textStat.SentenceCount = CountSentences(textStat.Text);
                textStat.WordCount = CountWords(textStat.Text);
                CountLetters(textStat.Text, out int vowels, out int consonants);
                textStat.VowelCount = vowels;
                textStat.ConsonantCount = consonants;
                FindShortestAndLongestWords(GetWords(textStat.Text), out List<string> shortestWords, out List<string> longestWords);
                textStat.ShortestWords = shortestWords;
                textStat.LongestWords = longestWords;
                CountLetterFrequency(textStat.Text, textStat);
                history.Add(textStat);
                Console.Clear();
                Console.WriteLine("Текст успешно обработан.");
                Console.WriteLine();
            }
            catch (ArgumentException ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.WriteLine();
            }
        }
        static void ShowStatistics(TextStat textStat)
        {
            Console.WriteLine("----- СТАТИСТИКА -----");
            Console.WriteLine($"Количество предложений: {textStat.SentenceCount}");
            Console.WriteLine($"Количество слов: {textStat.WordCount}");
            Console.WriteLine($"Количество гласных: {textStat.VowelCount}");
            Console.WriteLine($"Количество согласных: {textStat.ConsonantCount}");
            Console.WriteLine($"Кратчайшие слова: " + $"{string.Join(", ", textStat.ShortestWords)}");
            Console.WriteLine($"Длиннейшие слова: " + $"{string.Join(", ", textStat.LongestWords)}");
            Console.WriteLine("Количество каждой буквы:");
            foreach (KeyValuePair<char, int> letter in textStat.LetterQuantity)
            {
                Console.WriteLine($"Буква '{letter.Key}': {letter.Value}");
            }
            Console.WriteLine();
        }
        static void ShowHistory(List<TextStat> history)
        {
            if (history.Count == 0)
            {
                Console.WriteLine("История пока пустая.");
                return;
            }
            Console.WriteLine("----- ИСТОРИЯ -----");
            for (int i = 0; i < history.Count; i++)
            {
                string shortText = history[i].Text;
                if (shortText.Length > 50)
                {
                    shortText = shortText.Substring(0, 50) + "...";
                }
                Console.WriteLine($"{i + 1} - {shortText}");
            }
            Console.Write("\nВыберите номер текста: ");
            int number;
            if (!int.TryParse(Console.ReadLine(), out number))
            {
                Console.WriteLine("Некорректный номер.");
                return;
            }
            if (number < 1 || number > history.Count)
            {
                Console.WriteLine("Такого текста нет.");
                return;
            }
            Console.Clear();
            TextStat selectedText = history[number - 1];
            Console.WriteLine($"Текст №{number}");
            Console.WriteLine();
            ShowStatistics(selectedText);
        }
    }
}