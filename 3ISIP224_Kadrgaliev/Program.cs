using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP224_Kadrgaliev
{
    internal enum Genre
    {
        Novel = 1,
        Detective = 2,
        Fantasy = 3
    }
    internal class Book
    {
        private static int nextId = 1;

        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Author { get; private set; }
        public Genre Genre { get; private set; }
        public int Year { get; private set; }
        public decimal Price { get; private set; }

        public Book(string title, string author, Genre genre, int year, decimal price)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Название не может быть пустым.");
            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Автор не может быть пустым.");
            if (!Enum.IsDefined(typeof(Genre), genre))
                throw new ArgumentException("Выберите существующий жанр.");
            if (year < 1 || year > DateTime.Now.Year)
                throw new ArgumentOutOfRangeException("year");
            if (price < 0 || price > 1000000000m)
                throw new ArgumentOutOfRangeException("price");
            Id = nextId++;
            Title = title.Trim();
            Author = author.Trim();
            Genre = genre;
            Year = year;
            Price = price;
        }
    }
    internal class Program
    {
        private static readonly List<Book> books = new List<Book>();

        private static void AddTestBooks()
        {
            books.Add(new Book("Мастер и Маргарита", "Михаил Булгаков", Genre.Novel, 1967, 750m));
            books.Add(new Book("Собачье сердце", "Михаил Булгаков", Genre.Novel, 1925, 420m));
            books.Add(new Book("Убийство в Восточном экспрессе", "Агата Кристи", Genre.Detective, 1934, 550m));
            books.Add(new Book("Дюна", "Фрэнк Герберт", Genre.Fantasy, 1965, 1200m));
            books.Add(new Book("Пикник на обочине", "Аркадий и Борис Стругацкие", Genre.Fantasy, 1972, 680m));
        }
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            AddTestBooks();
            Console.WriteLine("Учёт книг в библиотеке");
            PrintBooks(books);
        }
        private static void PrintBooks(IEnumerable<Book> source)
        {
            List<Book> result = source.ToList();
            if (result.Count == 0)
            {
                Console.WriteLine("Книги не найдены.");
                return;
            }
            foreach (Book book in result)
            {
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("ID: " + book.Id);
                Console.WriteLine("Название: " + book.Title);
                Console.WriteLine("Автор: " + book.Author);
                Console.WriteLine("Жанр: " + GetGenreName(book.Genre));
                Console.WriteLine("Год издания: " + book.Year);
                Console.WriteLine("Цена: " + book.Price.ToString("F2") + " руб.");
            }
            Console.WriteLine("Всего книг: " + result.Count);
        }
        private static string GetGenreName(Genre genre)
        {
            switch (genre)
            {
                case Genre.Novel: return "Роман";
                case Genre.Detective: return "Детектив";
                case Genre.Fantasy: return "Фантастика";
                default: return "Неизвестный жанр";
            }
        }

    }
}
