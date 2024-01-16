using Microsoft.EntityFrameworkCore;
using System.Data.SQLite;
using System.Data.SqlTypes;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;

namespace ShopApp
{
    public class Catalog
    {
        public List<Item> Items { get; set; } = new List<Item>();

        /*private static string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\dbSuperDataMeasurments.db";*/

        private static string path = "mydata.db";

        public static bool CreateDB()
        {
            try
            {
                //string dbPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\dbSuperDaata";
                using (var connection = new SQLiteConnection("Data Source=" + path))
                {
                    connection.Open();
                    SQLiteCommand command = new SQLiteCommand();
                    command.Connection = connection;
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS products(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, name TEXT NOT NULL, description TEXT, price TEXT NOT NULL, img_url TEXT NOT NULL);";
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE IF NOT EXISTS cart(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, item_id INTEGER NOT NULL, name TEXT NOT NULL, description TEXT, price TEXT NOT NULL, img_url TEXT NOT NULL);";
                    command.ExecuteNonQuery();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static List<Item> GetItems()
        {
            List<Item> items = new List<Item>();
            var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseSqlite("Data Source=store.db").Options;
            using (var dbContext = new AppDBContext(options))
            {
                items = dbContext.Items.ToList();
            }

            return items;
        }

        public static Item GetItem(int id)
        {
            Item item = new Item();
            var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseSqlite("Data Source=store.db").Options;
            using (var dbContext = new AppDBContext(options))
            {
                item = dbContext.Items.FirstOrDefault(item => item.Id == id);
            }

            return item;
        }

        public static List<Order> GetOrders()
        {
            List<Order> orders = new List<Order>();
            var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseSqlite("Data Source=store.db").Options;
            using (var dbContext = new AppDBContext(options))
            {
                orders = dbContext.Orders.ToList();
            }

            return orders;
        }

        public static void DeleteAllItems()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseSqlite("Data Source=store.db").Options;
            using (var dbContext = new AppDBContext(options))
            {
                for (int i=0; i<1000; i++)
                {
                    var item = dbContext.Items.FirstOrDefault(itm => itm.Id == i);
                    if(item != null)
                    {
                        dbContext.Items.Remove(item);
                    }
                }
                for (int i = 0; i < 1000; i++)
                {
                    var item = dbContext.Cart.FirstOrDefault(itm => itm.Id == i);
                    if (item != null)
                    {
                        dbContext.Cart.Remove(item);
                    }
                }
                dbContext.SaveChanges();
            }
        }

        public static List<Item> GetItemsFromCart()
        {
            List<Item> items = new List<Item>();
            List<CartItem> cartItems = new List<CartItem>();
            var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseSqlite("Data Source=store.db").Options;
            using (var dbContext = new AppDBContext(options))
            {
                cartItems = dbContext.Cart.ToList();
                foreach(CartItem ci in cartItems)
                {
                    var i = new Item();
                    i.Id = ci.Id;
                    i.Name = ci.Name;
                    i.Description = ci.Description;
                    i.Price = ci.Price;
                    i.ImgUrl = ci.ImgUrl;
                    items.Add(i);
                }
            }

            return items;
        }

        public static void RemoveItemFromCart(int id)
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseSqlite("Data Source=store.db").Options;
            using (var dbContext = new AppDBContext(options))
            {
                var item = dbContext.Cart.FirstOrDefault(t => t.Id == id);
                if (item != null)
                {
                    dbContext.Cart.Remove(item);
                    dbContext.SaveChanges();
                }
            }
        }

        public static void RemoveItem(int id)
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseSqlite("Data Source=store.db").Options;
            using (var dbContext = new AppDBContext(options))
            {
                var item = dbContext.Items.FirstOrDefault(t => t.Id == id);
                if (item != null)
                {
                    dbContext.Items.Remove(item);
                    RemoveItemFromCart(id);
                    dbContext.SaveChanges();
                }
            }
        }
        /*public static Item GetItem(int id)
        {
            Item item = new Item();
            item.Id = -1;
            var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseSqlite("Data Source=store.db").Options;
            using (var dbContext = new AppDBContext(options))
            {
                item = dbContext.Items.FirstOrDefault(i => i.Id == id);
            }

            return item;
        }*/

        public static void AddItem(string name, string description, double price, string img_url)
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseSqlite("Data Source=store.db").Options;
            using (var dbContext = new AppDBContext(options))
            {
                var newItem = new Item();
                //newItem.Id = dbContext.Items.Count() + 1;
                newItem.Name = name;
                newItem.Description = description;
                newItem.Price = price;
                newItem.ImgUrl = img_url;
                dbContext.Items.Add(newItem);
                dbContext.SaveChanges();
            }
        }

        public static void AddItemToCart(int item_id, string name, string description, double price, string img_url)
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseSqlite("Data Source=store.db").Options;
            using (var dbContext = new AppDBContext(options))
            {
                var newItem = new CartItem();
                newItem.Id = item_id;
                newItem.Name = name;
                newItem.Description = description;
                newItem.Price = price;
                newItem.ImgUrl = img_url;
                dbContext.Cart.Add(newItem);
                dbContext.SaveChanges();
            }
        }

        void ChangeItem(int id, string newName, string newDescription, double newPrice, string newImgUrl)
        {
            Item item = new Item();
            var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseSqlite("Data Source=store.db").Options;
            using (var dbContext = new AppDBContext(options))
            {
                item = dbContext.Items.FirstOrDefault(i => i.Id == id);
                if(item != null)
                {
                    item.Name = newName;
                    item.Description = newDescription;
                    item.Price = newPrice;
                    item.ImgUrl = newImgUrl;
                }
                dbContext.SaveChanges();
            }
        }
    }




}
