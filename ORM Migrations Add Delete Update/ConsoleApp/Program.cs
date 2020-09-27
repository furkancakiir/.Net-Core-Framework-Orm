using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ConsoleApp
{     
    public class ShopContext: DbContext
    {        
        // Bir migrations eklemek istediğimizde mutlaka public Dbset<> lerini oluşturup daha sonra
        // dotnet ef migrations add addTableName diyerek ekleme işlemini gerçekleştirip.
        // daha sonra dotnet ef database update diyerek ilgili tabloyu database aktarabiliriz.

        // örnegin olusturdugumuz tablelara column eklemek istiyoruz burada verileri kaybetmemek adına bir güncelleme islemi gerceklestirmemiz gerekmektedir
        // dotnet ef migrations add addColumnProductCategoryId gibi isimlendirerek migrations ne ise yaradıgınıda anlayabilmemize olanak sağlayacaktır.
        // daha sonra tekrardan dotnet ef database update diyerek veri tabanımızı güncelleyebiliriz.
        

        // migrationsları ekledigimizde  bir önceki adıma geri gelebilmek için. dotnet ef database update (ilgili migrationsun adı) yazarak silme islemi gerceklesir.
        // burada dikkat edilmesi gereken husus migrations klosründeki _ den sonraki yazıların alınarak çalıştırılması.

        // eklenilen migrationslarımızı silmek içinde dotnet ef migrations remove olarak terminale yazarak silebiliriz.
        // dotnet ef database update 0 yazarsak tüm tablolarla birlikte verileri siler.

        // dotnet ef database drop diyerekte veritabanımızdaki database silme gerceklestirir. açık bir tablo varsa connection varsa yani hata verebilir
        // dotnet ef database drop -- force yazarak connection olsa bile database silinir.



        
        public DbSet<Product> Products {get;set;}
        public DbSet<Category> Categories { get; set; }

        public DbSet<Order> Orders {get;set;}

        public DbSet <Deneme> Denemes {get;set;}

        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder
            .UseLoggerFactory(MyLoggerFactory)
            .UseMySql(@"server=127.0.0.1;port=3306;database=Shopdb;user=root;password=145322;");
            // .UseSqlite("Data Source=shop.db");
            // .UseSqlServer(@"Server=.\SQLEXPRESS;Initial Catalog=ShopDb;Integrated Security=SSPI;"); 
            
        }
    }

    public class Product
    { 
        public int Id { get; set; }
       
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }


        public int CategoryId{get;set;}
       

    }

     public class Order
        {
            public int Id { get; set; }
            public int ProductId { get; set; }
            public DateTime DateAdded{get; set;}
        }       

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }


    }

    public class Deneme
    {

        public int Id {get;set;}
        public string deneme{get;set;}

        public string productName {get;set;}
    }






    class Program
    {
        static void Main(string[] args)
        {
         AddProducts();
        }

         static void DeleteProduct(int id )
        {
             using(var db = new ShopContext())
             {

                 var p = new Product(){Id=7};

                //  db.Products.Remove(p);
                 db.Entry(p).State=EntityState.Deleted;
                 db.SaveChanges();

                //  var p =db.Products.FirstOrDefault(i=>i.Id==id);

                //  if(p!=null){
                //      db.Products.Remove(p);
                //      db.SaveChanges();
                //      Console.WriteLine("Veri silindi");
                //  }
             }
         }



        static void UpdateProduct()
        {
             using(var db = new  ShopContext())
             {

                 var p = db.Products.Where(i=>i.Id==1).FirstOrDefault();

                 if(p!=null){
                     p.Price=2400;
                     db.Products.Update(p);
                     db.SaveChanges();
                 }





                //   var entity = new Product(){Id=1};
                //   db.Products.Attach(entity);
                //   entity.Price=3000;
                //   db.SaveChanges();



                //   var productElement = db.Products.Where(i=>i.Id==1).FirstOrDefault();
                //   if(productElement!=null)
                //   { 
                //      productElement.Price*=1.2m;
                //      db.SaveChanges();
                //      Console.WriteLine("Güncelleme Gerçekleşti");

                //   }
             }
        }

        static void AddProducts()
        {
            using(var db = new ShopContext()){ 

            var products = new List<Product>()
            {
                new Product{Name="Samsung S6",Price=2000},
                new Product{Name="Samsung S7",Price=3000},
                new Product{Name="Samsung S8",Price=4000},
                new Product{Name="Samsung S9",Price=5000},
                new Product{Name="Samsung S10",Price=6000}
             };

            db.Products.AddRange(products);
            
            db.SaveChanges();



            
            
            Console.WriteLine("Veriler eklendi");



            
            }
        }

        static void AddProduct()
        {

            using(var db = new ShopContext()){ 

            var p = new Product{Name="Samsung S12",Price=8000};
            db.Products.Add(p);
            db.SaveChanges();
                    
            Console.WriteLine("Veriler eklendi");
            
            }
        }

        static void GetAllProducts()
        {
            using (var context= new ShopContext())
            {
             var products = context.Products.Select(products=> new {products.Name,products.Price}).ToList();



             foreach (var p in products)
             {
                 Console.WriteLine($"name:{p.Name},price{p.Price}");
             }

                

            }
            
        } 

        static void GetProductById(int id)
        {

            using (var context= new ShopContext())
            {
             var result = context.Products.Where(p=>p.Id==id).Select(p=>new{p.Name,p.Price}).FirstOrDefault();
            
                 Console.WriteLine($"name:{result.Name},price{result.Price}");
             

                

            }
        }    
    
        static void GetProductName(string name){
             using (var context= new ShopContext())
            {
             var products = context.Products.Where(p=>p.Name.ToLower().Contains(name)).Select(p=>new{p.Name,p.Price}).ToList();
            
                foreach (var item in products )
                {
                      Console.WriteLine($"name:{item.Name},price{item.Price}");
             
                }
             

                

            }
        }
    

    
    }
}
