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
            this.FilePath = filePath;
            }


            public List<string> ReadFromFile()
            {
                if (File.Exists(this.FilePath))
                {
                    return File.ReadAllLines(FilePath).ToList();
                }
                return new List<string>();
            }
        }
        public class Product
        {
            public int ProductID { get; set; }
            public string  Name { get; set; }
        }

        public class Client
        {
            public int ClientID { get; set; }
            public string Name { get; set; }
        }

        public class Order
        {
            public int OrderID { get; set;}
            public Product Product { get; set;}

            public Client Client { get; set;}   

        }
    }
}