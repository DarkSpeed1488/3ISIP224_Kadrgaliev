using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP224_Kadrgaliev
{
    enum Category
    {
        Еда,
        Электроника,
        Одежда
    }
    class Product
    {
        private static int nextID = 1;
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Category ProductCategory { get; set; }
        public bool InStock => Quantity > 0;
        public Product(string name, decimal price, int quantity, Category category)
        {
            ID = nextID++;
            Name = name;
            Price = price;
            Quantity = quantity;
            ProductCategory = category;
        }
    }
    class Sale
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public Sale(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
            TotalPrice = product.Price * quantity;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Product> products = new List<Product>();
            Stack<Sale> salesHistory = new Stack<Sale>();
            products.Add(new Product("Хлеб", 87, 33, Category.Еда));
            products.Add(new Product("Молоко", 117, 41, Category.Еда));
            products.Add(new Product("Телефон", 35000, 13, Category.Электроника));
            products.Add(new Product("Наушники", 5000, 13, Category.Электроника));
            products.Add(new Product("Футболка", 1500, 12, Category.Одежда));
            while (true)
            {
                Console.WriteLine("===== УЧЁТ ТОВАРОВ =====");
                Console.WriteLine("1. Показать все товары");
                Console.WriteLine("2. Добавить товар");
                Console.WriteLine("3. Удалить товар");
                Console.WriteLine("4. Заказать поставку");
                Console.WriteLine("5. Продать товар");
                Console.WriteLine("6. Поиск товара");
                Console.WriteLine("7. История продаж");
                Console.WriteLine("8. Отменить последнюю продажу");
                Console.WriteLine("9. Отчёт о продажах");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ShowAllProducts(products);
                        break;
                    case "2":
                        AddProduct(products);
                        break;
                    case "3":
                        RemoveProduct(products);
                        break;
                    case "4":
                        OrderProduct(products);
                        break;
                    case "5":
                        SellProduct(products, salesHistory);
                        break;
                    case "6":
                        SearchProduct(products);
                        break;
                    case "7":
                        ShowSalesHistory(salesHistory);
                        break;
                    case "8":
                        CancelLastSale(salesHistory);
                        break;
                    case "9":
                        SalesReport(salesHistory);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверная команда.");
                        break;
                }
            }
        }
        static void ShowInfo(Product product)
        {
            Console.WriteLine($"Код: {product.ID}");
            Console.WriteLine($"Название: {product.Name}");
            Console.WriteLine($"Цена: {product.Price} руб.");
            Console.WriteLine($"Количество: {product.Quantity}");
            Console.WriteLine($"На складе: {(product.InStock ? "Да" : "Нет")}");
            Console.WriteLine($"Категория: {product.ProductCategory}");
            Console.WriteLine();
        }
        static void ShowAllProducts(List<Product> products)
        {
            if (products.Count == 0)
            {
                Console.WriteLine("Список товаров пуст.");
                return;
            }
            foreach (Product product in products)
            {
                ShowInfo(product);
            }
        }
        static Category ChooseCategory()
        {
            while (true)
            {
                Console.WriteLine("Категории:");
                Console.WriteLine("1. Еда");
                Console.WriteLine("2. Электроника");
                Console.WriteLine("3. Одежда");
                Console.Write("Выберите категорию: ");
                if (int.TryParse(Console.ReadLine(), out int number))
                {
                    if (number >= 1 && number <= 3)
                    {
                        return (Category)(number - 1);
                    }
                }
                Console.WriteLine("Неверная категория.");
            }
        }
        static void AddProduct(List<Product> products)
        {
            Console.Write("Введите название товара: ");
            string name = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Название не может быть пустым.");
                Console.Write("Введите название товара: ");
                name = Console.ReadLine();
            }
            decimal price;
            while (true)
            {
                Console.Write("Введите цену: ");
                if (decimal.TryParse(Console.ReadLine(), out price) && price >= 0)
                {
                    break;
                }
                Console.WriteLine("Введите корректную цену.");
            }
            int quantity;
            while (true)
            {
                Console.Write("Введите количество: ");
                if (int.TryParse(Console.ReadLine(), out quantity) && quantity >= 0)
                {
                    break;
                }
                Console.WriteLine("Количество не может быть отрицательным.");
            }
            Category category = ChooseCategory();
            Product product = new Product(name, price, quantity, category);
            products.Add(product);
            Console.WriteLine($"Товар добавлен. Его код: {product.ID}");
        }
        static void SearchProduct(List<Product> products)
        {
            Console.Write("Введите код товара: ");
            if (!int.TryParse(Console.ReadLine(), out int ID))
            {
                Console.WriteLine("Некорректный код.");
                return;
            }
            Console.Write("Введите название товара: ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Название не может быть пустым.");
                return;
            }
            Category category = ChooseCategory();
            bool found = false;
            foreach (Product product in products)
            {
                if (product.ID == ID &&
                    product.Name.ToLower() == name.ToLower() &&
                    product.ProductCategory == category)
                {
                    ShowInfo(product);
                    found = true;
                }
            }
            if (!found)
            {
                Console.WriteLine("Товар не найден.");
            }
        }
        static Product FindByCode(List<Product> products, int ID)
        {
            foreach (Product product in products)
            {
                if (product.ID == ID)
                {
                    return product;
                }
            }
            return null;
        }
        static void RemoveProduct(List<Product> products)
        {
            Console.Write("Введите код товара для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int ID))
            {
                Console.WriteLine("Некорректный код.");
                return;
            }
            Product product = FindByCode(products, ID);
            if (product == null)
            {
                Console.WriteLine("Товар с таким кодом не найден.");
                return;
            }
            products.Remove(product);
            Console.WriteLine($"Товар {product.Name} удалён.");
        }
        static void OrderProduct(List<Product> products)
        {
            Console.Write("Введите код товара для поставки: ");
            if (!int.TryParse(Console.ReadLine(), out int ID))
            {
                Console.WriteLine("Некорректный код.");
                return;
            }
            Product product = FindByCode(products, ID);
            if (product == null)
            {
                Console.WriteLine("Товар с таким кодом не найден.");
                return;
            }
            int quantity;
            while (true)
            {
                Console.Write("Введите количество товара для поставки: ");
                if (int.TryParse(Console.ReadLine(), out quantity) && quantity > 0)
                {
                    break;
                }
                Console.WriteLine("Количество должно быть больше нуля.");
            }
            product.Quantity += quantity;
            Console.WriteLine("Поставка выполнена.");
            Console.WriteLine($"Теперь товара {product.Name}: {product.Quantity}");
        }
        static void SellProduct(List<Product> products, Stack<Sale> salesHistory)
        {
            Console.Write("Введите код товара для продажи: ");
            if (!int.TryParse(Console.ReadLine(), out int ID))
            {
                Console.WriteLine("Некорректный код.");
                return;
            }
            Product product = FindByCode(products, ID);
            if (product == null)
            {
                Console.WriteLine("Товар с таким кодом не найден.");
                return;
            }
            if (!product.InStock)
            {
                Console.WriteLine("Товара нет на складе.");
                return;
            }
            int quantity;
            while (true)
            {
                Console.Write("Введите количество товара для продажи: ");
                if (int.TryParse(Console.ReadLine(), out quantity) && quantity > 0)
                {
                    break;
                }
                Console.WriteLine("Введите корректное количество.");
            }
            if (quantity > product.Quantity)
            {
                Console.WriteLine("Недостаточно товара на складе.");
                Console.WriteLine($"Доступно: {product.Quantity}");
                return;
            }
            product.Quantity -= quantity;
            salesHistory.Push(new Sale(product, quantity));
            Console.WriteLine("Товар продан.");
            Console.WriteLine($"Осталось товара {product.Name}: {product.Quantity}");
        }
        static void ShowSalesHistory(Stack<Sale> salesHistory)
        {
            if (salesHistory.Count == 0)
            {
                Console.WriteLine("История продаж пуста.");
                return;
            }
            Console.WriteLine("===== ИСТОРИЯ ПРОДАЖ =====");
            foreach (Sale sale in salesHistory)
            {
                Console.WriteLine($"Товар: {sale.Product.Name}");
                Console.WriteLine($"Количество: {sale.Quantity}");
                Console.WriteLine($"Сумма: {sale.TotalPrice} руб.");
                Console.WriteLine();
            }
        }
        static void CancelLastSale(Stack<Sale> salesHistory)
        {
            if (salesHistory.Count == 0)
            {
                Console.WriteLine("Нет продаж для отмены.");
                return;
            }
            Sale lastSale = salesHistory.Pop();
            lastSale.Product.Quantity += lastSale.Quantity;
            Console.WriteLine($"Последняя продажа товара {lastSale.Product.Name} отменена.");
            Console.WriteLine($"На склад возвращено: {lastSale.Quantity} шт.");
        }
        static void SalesReport(Stack<Sale> salesHistory)
        {
            if (salesHistory.Count == 0)
            {
                Console.WriteLine("Продаж ещё не было.");
                return;
            }
            decimal totalSum = 0;
            Console.WriteLine("===== ОТЧЁТ О ПРОДАЖАХ =====");
            foreach (Sale sale in salesHistory)
            {
                Console.WriteLine($"Товар: {sale.Product.Name}");
                Console.WriteLine($"Продано: {sale.Quantity} шт.");
                Console.WriteLine($"Сумма продажи: {sale.TotalPrice} руб.");
                Console.WriteLine();

                totalSum += sale.TotalPrice;
            }

            Console.WriteLine($"Общая сумма продаж: {totalSum} руб.");
        }
    }
}