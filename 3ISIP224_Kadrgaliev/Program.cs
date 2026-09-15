using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP224_Kadrgaliev
{
    class Product
    {
        private static int nextID = 1;
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Category ProductCategory { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
        }
    }
}