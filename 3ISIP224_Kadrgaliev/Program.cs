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
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Название не может быть пустым.");
            if (string.IsNullOrWhiteSpace(author)) throw new ArgumentException("Автор не может быть пустым.");
            if (!Enum.IsDefined(typeof(Genre), genre)) throw new ArgumentException("Выберите существующий жанр.");
            if (year < 1 || year > DateTime.Now.Year) throw new ArgumentOutOfRangeException("year");
            if (price < 0 || price > 1000000000m) throw new ArgumentOutOfRangeException("price");
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
        private static bool isRunning = true;
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
            RunMenu();
            if (!isRunning) Console.WriteLine("Ввод завершён. Программа закрыта.");
        }
        private static void RunMenu()
        {
            while (isRunning)
            {
                Console.WriteLine("");
                Console.WriteLine("========== БИБЛИОТЕКА ==========");
                Console.WriteLine("1. Вывести все книги");
                Console.WriteLine("2. Добавить книгу");
                Console.WriteLine("3. Удалить книгу по ID");
                Console.WriteLine("4. Найти по названию");
                Console.WriteLine("5. Найти по автору");
                Console.WriteLine("6. Найти по жанру");
                Console.WriteLine("7. Отсортировать по названию");
                Console.WriteLine("8. Отсортировать по году");
                Console.WriteLine("9. Самые дорогие и дешёвые книги");
                Console.WriteLine("10. Количество книг по авторам");
                Console.WriteLine("11. Вставить блок книг");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите команду: ");
                string input = Console.ReadLine();
                if (input == null)
                {
                    isRunning = false;
                    return;
                }
                int command;
                if (!int.TryParse(input, out command) || command < 0 || command > 11)
                {
                    Console.WriteLine("Введите целое число от 0 до 11.");
                    continue;
                }
                Console.WriteLine();
                switch (command)
                {
                    case 0: Console.WriteLine("До свидания!"); return;
                    case 1: PrintBooks(books); break;
                    case 2: AddBook(); break;
                    case 3: DeleteBook(); break;
                    case 4: SearchByTitle(); break;
                    case 5: SearchByAuthor(); break;
                    case 6: SearchByGenre(); break;
                    case 7: SortByTitle(); break;
                    case 8: SortByYear(); break;
                    case 9: ShowPriceExtremes(); break;
                    case 10: GroupByAuthors(); break;
                    case 11: ImportBooks(); break;
                }
            }
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
            Console.WriteLine();
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
        private static void AddBook()
        {
            string title;
            while (true)
            {
                Console.Write("Название: ");
                title = Console.ReadLine();
                if (title == null)
                {
                    isRunning = false;
                    return;
                }
                if (!string.IsNullOrWhiteSpace(title))
                {
                    title = title.Trim();
                    break;
                }
                Console.WriteLine("Значение не может быть пустым.");
            }
            string author;
            while (true)
            {
                Console.Write("Автор: ");
                author = Console.ReadLine();
                if (author == null)
                {
                    isRunning = false;
                    return;
                }
                if (!string.IsNullOrWhiteSpace(author))
                {
                    author = author.Trim();
                    break;
                }
                Console.WriteLine("Значение не может быть пустым.");
            }
            foreach (Genre availableGenre in Enum.GetValues(typeof(Genre))) Console.WriteLine((int)availableGenre + " — " + GetGenreName(availableGenre));
            int genreNumber;
            while (true)
            {
                Console.Write("Номер жанра: ");
                string input = Console.ReadLine();
                if (input == null)
                {
                    isRunning = false;
                    return;
                }
                if (int.TryParse(input, out genreNumber) && genreNumber >= 1 && genreNumber <= 3) break;
                Console.WriteLine("Введите целое число от " + 1 + " до " + 3 + ".");
            }
            Genre genre = (Genre)genreNumber;
            int year;
            while (true)
            {
                Console.Write("Год издания: ");
                string input = Console.ReadLine();
                if (input == null)
                {
                    isRunning = false;
                    return;
                }
                if (int.TryParse(input, out year) && year >= 1 && year <= DateTime.Now.Year) break;
                Console.WriteLine("Введите целое число от " + 1 + " до " + DateTime.Now.Year + ".");
            }
            decimal price;
            while (true)
            {
                Console.Write("Цена (руб.): ");
                string input = Console.ReadLine();
                if (input == null)
                {
                    isRunning = false;
                    return;
                }
                if (decimal.TryParse(input, out price)
                    && price >= 0
                    && price <= 1000000000m
                    && decimal.Round(price, 2) == price)
                {
                    break;
                }
                Console.WriteLine("Введите цену от 0 до 1000000000, не более двух знаков после запятой.");
            }
            Book book = new Book(title, author, genre, year, price);
            books.Add(book);
            Console.WriteLine("Книга добавлена. ID: " + book.Id);
        }
        private static void DeleteBook()
        {
            int id;
            while (true)
            {
                Console.Write("ID книги для удаления: ");
                string input = Console.ReadLine();
                if (input == null)
                {
                    isRunning = false;
                    return;
                }
                if (int.TryParse(input, out id) && id >= 1 && id <= int.MaxValue)
                    break;
                Console.WriteLine("Введите целое число от " + 1 + " до " + int.MaxValue + ".");
            }
            Book book = books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                Console.WriteLine("Книга с таким ID не найдена.");
                return;
            }
            books.Remove(book);
            Console.WriteLine("Книга удалена: " + book.Title);
        }
        private static void SearchByTitle()
        {
            string title;
            while (true)
            {
                Console.Write("Название или его часть: ");
                title = Console.ReadLine();
                if (title == null)
                {
                    isRunning = false;
                    return;
                }
                if (!string.IsNullOrWhiteSpace(title))
                {
                    title = title.Trim();
                    break;
                }
                Console.WriteLine("Значение не может быть пустым.");
            }
            PrintBooks(books.Where(b => b.Title.IndexOf(title, StringComparison.OrdinalIgnoreCase) >= 0));
        }
        private static void SearchByAuthor()
        {
            string author;
            while (true)
            {
                Console.Write("Имя автора или его часть: ");
                author = Console.ReadLine();
                if (author == null)
                {
                    isRunning = false;
                    return;
                }
                if (!string.IsNullOrWhiteSpace(author))
                {
                    author = author.Trim();
                    break;
                }
                Console.WriteLine("Значение не может быть пустым.");
            }
            PrintBooks(books.Where(b => b.Author.IndexOf(author, StringComparison.OrdinalIgnoreCase) >= 0));
        }
        private static void SearchByGenre()
        {
            foreach (Genre availableGenre in Enum.GetValues(typeof(Genre))) Console.WriteLine((int)availableGenre + " — " + GetGenreName(availableGenre));
            int genreNumber;
            while (true)
            {
                Console.Write("Номер жанра: ");
                string input = Console.ReadLine();
                if (input == null)
                {
                    isRunning = false;
                    return;
                }
                if (int.TryParse(input, out genreNumber) && genreNumber >= 1 && genreNumber <= 3) break;
                Console.WriteLine("Введите целое число от " + 1 + " до " + 3 + ".");
            }
            Genre genre = (Genre)genreNumber;
            PrintBooks(books.Where(b => b.Genre == genre));
        }
        private static void SortByTitle()
        {
            List<Book> sorted = books.OrderBy(b => b.Title, StringComparer.CurrentCultureIgnoreCase).ThenBy(b => b.Id).ToList();
            books.Clear();
            books.AddRange(sorted);
            Console.WriteLine("Книги отсортированы по названию:");
            PrintBooks(books);
        }
        private static void SortByYear()
        {
            List<Book> sorted = books.OrderBy(b => b.Year).ThenBy(b => b.Id).ToList();
            books.Clear();
            books.AddRange(sorted);
            Console.WriteLine("Книги отсортированы по году (от старых к новым):");
            PrintBooks(books);
        }
        private static void ShowPriceExtremes()
        {
            if (books.Count == 0)
            {
                Console.WriteLine("Список книг пуст.");
                return;
            }
            decimal minPrice = books.Min(b => b.Price);
            decimal maxPrice = books.Max(b => b.Price);
            Console.WriteLine("Самые дорогие книги:");
            PrintBooks(books.Where(b => b.Price == maxPrice));
            Console.WriteLine("Самые дешёвые книги:");
            PrintBooks(books.Where(b => b.Price == minPrice));
        }
        private static void GroupByAuthors()
        {
            if (books.Count == 0)
            {
                Console.WriteLine("Список книг пуст.");
                return;
            }
            var groups = books.GroupBy(b => b.Author, StringComparer.CurrentCultureIgnoreCase).OrderBy(g => g.Key, StringComparer.CurrentCultureIgnoreCase);
            Console.WriteLine("Количество книг каждого автора:");
            foreach (var group in groups) Console.WriteLine(group.Key + ": " + group.Count());
        }
        private static void ImportBooks()
        {
            Console.WriteLine("Вставьте строки в формате: Название;Автор;Жанр;Год;Цена");
            Console.WriteLine("Жанры: Роман, Детектив, Фантастика (регистр не важен).");
            Console.WriteLine("Цена вводится так же, как при обычном добавлении книги.");
            Console.WriteLine("Завершите ввод пустой строкой.");
            int added = 0;
            int rejected = 0;
            int lineNumber = 0;
            while (true)
            {
                string line = Console.ReadLine();
                if (line == null)
                {
                    isRunning = false;
                    break;
                }
                if (string.IsNullOrWhiteSpace(line)) break;
                lineNumber++;
                string[] fields = line.Split(';');
                if (fields.Length != 5)
                {
                    rejected++;
                    Console.WriteLine("Строка " + lineNumber + ": нужно ровно 5 полей, разделённых точкой с запятой.");
                    continue;
                }
                string title = fields[0].Trim();
                string author = fields[1].Trim();
                if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author))
                {
                    rejected++;
                    Console.WriteLine("Строка " + lineNumber + ": название и автор не могут быть пустыми.");
                    continue;
                }
                Genre genre;
                switch (fields[2].Trim().ToLowerInvariant())
                {
                    case "роман": genre = Genre.Novel; break;
                    case "детектив": genre = Genre.Detective; break;
                    case "фантастика": genre = Genre.Fantasy; break;
                    default:
                        rejected++;
                        Console.WriteLine("Строка " + lineNumber + ": жанр должен быть Роман, Детектив или Фантастика.");
                        continue;
                }
                int year;
                if (!int.TryParse(fields[3].Trim(), out year) || year < 1 || year > DateTime.Now.Year)
                {
                    rejected++;
                    Console.WriteLine("Строка " + lineNumber + ": год должен быть целым числом от 1 до " + DateTime.Now.Year + ".");
                    continue;
                }
                decimal price;
                if (!decimal.TryParse(fields[4].Trim(), out price) || price < 0 || price > 1000000000m || decimal.Round(price, 2) != price)
                {
                    rejected++;
                    Console.WriteLine("Строка " + lineNumber + ": цена должна быть от 0 до 1000000000 и иметь не более двух знаков после запятой.");
                    continue;
                }
                Book book = new Book(title, author, genre, year, price);
                books.Add(book);
                added++;
                Console.WriteLine("Строка " + lineNumber + ": добавлена книга с ID " + book.Id);
            }
            Console.WriteLine("Импорт завершён. Добавлено: " + added + "; ошибок: " + rejected + ".");
        }
    }
}