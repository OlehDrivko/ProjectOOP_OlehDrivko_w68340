using projectOOP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace projectOOP
{
    public class FileManager
    {
        protected string FilePath;

        public FileManager(string filePath)
        {
            FilePath = filePath;
        }

        public List<string> ReadFromFile()
        {
            if (File.Exists(FilePath))
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

    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
    }

    public class Client
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public Product Product { get; set; }
        public Client Client { get; set; }
    }

    public class OrderQueueManager : FileManager
    {
        public OrderQueueManager(string basePath) : base(basePath) { }

        public void EnqueueOrder(Order order)
        {
            var orders = ReadFromFile();
            orders.Add($"Order,{order.OrderId},{order.Product.ProductId},{order.Client.ClientId}");
            WriteToFile(orders);
        }

        public Order DequeueOrder()
        {
            var orders = ReadFromFile();
            if (orders.Any())
            {
                var firstOrder = orders.First();
                orders.RemoveAt(0);
                WriteToFile(orders);

                var orderDetails = firstOrder.Split(',');
                var orderId = int.Parse(orderDetails[1]);
                var productId = int.Parse(orderDetails[2]);
                var clientId = int.Parse(orderDetails[3]);

                return new Order
                {
                    OrderId = orderId,
                    Product = GetProduct(productId),
                    Client = GetClient(clientId)
                };
            }
            else
            {
                return null;
            }
        }

        public List<Order> GetOrders()
        {
            var orders = ReadFromFile();
            var orderList = new List<Order>();

            foreach (var order in orders)
            {
                var orderDetails = order.Split(',');

                if (orderDetails.Length == 4 &&
                    orderDetails[0] == "Order" &&
                    int.TryParse(orderDetails[1], out int orderId) &&
                    int.TryParse(orderDetails[2], out int productId) &&
                    int.TryParse(orderDetails[3], out int clientId))
                {
                    var product = GetProduct(productId);
                    var client = GetClient(clientId);

                    if (product != null && client != null)
                    {
                        var newOrder = new Order
                        {
                            OrderId = orderId,
                            Product = product,
                            Client = client
                        };

                        orderList.Add(newOrder);
                    }
                    else
                    {
                        Console.WriteLine("Error reading order. Cannot find product or customer.");
                    }
                }
                else
                {
                    Console.WriteLine("Error reading order. Incorrect data format.");
                }
            }

            return orderList;
        }

        private Product GetProduct(int productId)
        {
            var products = new ProductFileManager().ReadFromFile();
            var productDetails = products.FirstOrDefault(p => p.StartsWith($"Product,{productId},"));
            if (productDetails != null)
            {
                var productName = productDetails.Split(',')[2];
                return new Product { ProductId = productId, Name = productName };
            }
            return null;
        }

        private Client GetClient(int clientId)
        {
            var clients = new ClientFileManager().ReadFromFile();
            var clientDetails = clients.FirstOrDefault(c => c.StartsWith($"Client,{clientId},"));
            if (clientDetails != null)
            {
                var clientName = clientDetails.Split(',')[2];
                return new Client { ClientId = clientId, Name = clientName };
            }
            return null;
        }
    }

    public class ProductFileManager : FileManager
    {
        public ProductFileManager() : base("products.txt") { }
    }

    public class ClientFileManager : FileManager
    {
        public ClientFileManager() : base("clients.txt") { }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var basePath = @"C:\Users\olegd\OneDrive\Робочий стіл\projectOOP\projectOOP_3.0_Oleg\projectOOP";
            var orderQueueManager = new OrderQueueManager(Path.Combine(basePath, "orders.txt"));

            var product = new Product { ProductId = 1, Name = "ProductA" };
            new ProductFileManager().WriteToFile(new List<string> { $"Product,{product.ProductId},{product.Name}" });

            var client = new Client { ClientId = 1, Name = "ClientA" };
            new ClientFileManager().WriteToFile(new List<string> { $"Client,{client.ClientId},{client.Name}" });

            var order = new Order { OrderId = 1, Product = product, Client = client };
            orderQueueManager.EnqueueOrder(order);

            Console.WriteLine("Current Orders:");
            var currentOrders = orderQueueManager.GetOrders();
            foreach (var currentOrder in currentOrders)
            {
                Console.WriteLine($"Order ID: {currentOrder.OrderId}, Product: {currentOrder.Product.Name}, Client: {currentOrder.Client.Name}");
            }

            Console.WriteLine("\nProcessing Next Order:");
            var nextOrder = orderQueueManager.DequeueOrder();
            if (nextOrder != null)
            {
                Console.WriteLine($"Processed Order ID: {nextOrder.OrderId}, Product: {nextOrder.Product.Name}, Client: {nextOrder.Client.Name}");
            }
        }
    }
}


