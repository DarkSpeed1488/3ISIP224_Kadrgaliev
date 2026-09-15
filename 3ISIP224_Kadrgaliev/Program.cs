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


    internal class Program
    {
        static void Main(string[] args)
        {
            List<Product> products = new List<Product>();

            products.Add(new Product("Хлеб", 87, 33, Category.Еда));
            products.Add(new Product("Молоко", 117, 41, Category.Еда));
            products.Add(new Product("Телефон", 35000, 5, Category.Электроника));
            products.Add(new Product("Наушники", 5000, 13, Category.Электроника));
            products.Add(new Product("Футболка", 1500, 12, Category.Одежда));
            ShowAllProducts(products);
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
    }
}