using System.Net.WebSockets;

namespace ProjectOOP_OlehDrivko_w68340
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var basePath = @"";
        }

        public class FileManager
        {
            protected string FilePath;

            public FileManager(string filePath) 
            {
                FilePath = filePath;
            }


            public List<string> ReadFromFile()
            {
                if (File.Exists(this.FilePath))
                {
                    return File.ReadAllLines(FilePath).ToList();
                }
                return new List<string>();
            }

            public void WriteToFile(List<string> lines)
            {
                File.WriteAllLines(FilePath, lines);
            }
        }


        public class OrderQueueManager : FileManager
        {
            public OrderQueueManager(string filePath) : base(filePath)
            {
            }

            public void EnqueueOrder(Order order)
            {
                var orders = ReadFromFile();
                orders.Add($"Order,{order.OrderID},{order.Product.ProductID},{order.Client.ClientID}");
            }

        }


        private Product GetProduct(int productId)
        {
            var products = new ProductFileManager().ReadFromFile();
            var productDetails = products.FirstOrDefault(p => p.StartsWith($"Product,{productId},"));
            if (productDetails != null)
            {
                var productName = productDetails.Split(',')[2];
                return new Product { ProductID = productId, Name = productName };
            }
            return null;
        }

        public class ProductFileManager : FileManager
        {
            public ProductFileManager() : base("products.txt") { }
        }

        public class ClientFileManager : FileManager
        {
            public ClientFileManager() : base("clients.txt") { }
        }
        // Tworzenie klasy Product
        public class Product
        {
            public int ProductID { get; set; }
            public string  Name { get; set; }
        }
        // Tworzenie klasy Client
        public class Client
        {
            public int ClientID { get; set; }
            public string Name { get; set; }
        }
        // Tworzenie klasy Order
        public class Order
        {
            public int OrderID { get; set;}
            public Product Product { get; set;}

            public Client Client { get; set;}   

        }
    }
}