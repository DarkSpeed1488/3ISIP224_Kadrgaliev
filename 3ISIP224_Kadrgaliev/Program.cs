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
        
        public string ShortestWord { get; set; }
        public string LongestWord { get; set; }

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
            
        }
    }
}
