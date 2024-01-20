namespace ProjectOOP_OlehDrivko_w68340
{
    internal class Program
    {
        static void Main(string[] args)
        {

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