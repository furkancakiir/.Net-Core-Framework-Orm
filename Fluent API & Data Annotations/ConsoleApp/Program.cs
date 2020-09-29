using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ConsoleApp
{     
    public class ShopContext: DbContext
    {        
    

        public DbSet<Product> Products {get;set;}
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users {get;set;}
        public DbSet<Address> Addresses {get;set;}
        
        public DbSet<Customer> Customers{get;set;}

      

        
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder
            .UseLoggerFactory(MyLoggerFactory)
            .UseMySql(@"server=127.0.0.1;port=3306;database=Shopdb;user=root;password=145322;");
            // .UseSqlite("Data Source=shop.db");
            // .UseSqlServer(@"Server=.\SQLEXPRESS;Initial Catalog=ShopDb;Integrated Security=SSPI;"); 
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>().HasKey(t=> new {t.ProductId,t.CategoryId});

            modelBuilder.Entity<ProductCategory>().HasOne(pc=>pc.Product).WithMany(p=>p.ProductCategories).HasForeignKey(pc=>pc.ProductId);

            modelBuilder.Entity<ProductCategory>().HasOne(c=>c.Category).WithMany(c=>c.ProductCategories).HasForeignKey(c=>c.CategoryId);

            modelBuilder.Entity<Product>().ToTable("Urunler");

            modelBuilder.Entity<Customer>().Property(p=>p.IdentityNumber).HasMaxLength(11).IsRequired(); // Fluent Api ile ilgili kolona gecis yapıp Customer tablosundaki IdentityNumber kolonu zorunlu hale getirildi.

            modelBuilder.Entity<User>().HasIndex(u=>u.Username).IsUnique();



        }
    }


   public class User
   {
       public User()
       {
           this.Addresses=new List<Address>();
       }
        public int Id { get; set; }   
        [Required]
        [MaxLength(15),MinLength(8)]
        //[MinLength(8)]
       public string Username { get; set; }

       [Column(TypeName="varchar(20)")]
       public string Email{get;set;}

       public Customer Customer { get; set; }

       public List<Address> Addresses { get; set; }

   }

   public class Address
   {
      public int Id { get; set; }     
      public string Fullname { get; set; }
      public string Title { get; set; }
      public string Email { get; set; }
      public string Body{get;set;}
      public User user {get;set;}
     public int UserId{get;set;}

   }

   public class Customer
   { 
       [Column("Customer_id")] // Customer clasındaki Id tablomuza Customer_id olarak degistirilir.
       public int Id { get; set; }
       [Required]
       public string IdentityNumber { get; set; }
       [Required]
       public string Firstname { get; set; }
       [Required]
       public string LastName { get; set; }

       [NotMapped] // fullname kolunu veri tabanımıza aktarılmaz.
       public string FullName { get; set; }

       public User user { get; set; }

       public int UserId { get; set; }


   }

   public class Supplier
   {
       public int Id { get; set; }
       public string Name { get; set; }
       public string TaxNumber { get; set; }
   }



    public class Product
    { 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // None olursa otomatik bir değer gelmiyecektir ID kısmına. Identity primary key gibi 1 kere ekler degistirilemez.
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Aktarılan tarih bir daha değiştirilemiyecektir..
        public DateTime InsertDate { get; set; } =DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)] // Aktarılan tarih Computed olduğunda son güncelleme tarihinde degistirilebilme imkanı sunucaktır.
        public DateTime LastUpdateDate { get; set; } =DateTime.Now;

        public List<ProductCategory> ProductCategories {get;set;}


    }

    // [NotMapped] // databasede tablo olarak oluşturmayacaktır.
    [Table("Ürün Kategorileri")]
    public class ProductCategory{
        public int ProductId{get;set;}
        public Product Product {get;set;}
        public int CategoryId{get;set;}

        public Category Category{get;set;}
    }
     

    public class Category
    {
        
        public int CategoryId { get; set; }
        public string Name { get; set; }
        
        public List<ProductCategory> ProductCategories {get;set;}
    }
     

    class Program
    {
        static void Main(string[] args)
        {
         // InsertUser();
         //InsertAdress();
            



         using(var db = new ShopContext())
         {
             
            //  var customer = new Customer(){
            //           IdentityNumber="12321412",
            //           Firstname="Furkan",
            //           LastName="Çakır",
            //           user=db.Users.FirstOrDefault(i=>i.Id==3)


            //  };
            //  db.Customers.AddRange(customer);
            //  db.SaveChanges();

            // var user = new User(){
            //     Username="Deneme",
            //     Email="ASDASDAS@gmail.com",
            //     Customer=new Customer() {
            //         Firstname="Deneme",
            //         LastName="Deneme",
            //         IdentityNumber="12345"
            //     }

            // };
            // db.Users.AddRange(user);
            // db.SaveChanges();

            // var products = new List<Product>()
            // {
            //     new Product(){Name="Samsung S5",Price=2000},
            //     new Product(){Name="Samsung S6",Price=3000},
            //     new Product(){Name="Samsung S7",Price=4000},
            //     new Product(){Name="Samsung S8",Price=5000},
            //     new Product(){Name="Samsung S9",Price=6000},
            // };

            // db.Products.AddRange(products);

            // var categories = new List<Category>()
            // {
            // new Category(){Name="Telefon"},  
            // new Category(){Name="Elektronik"},  
            // new Category(){Name="Bilgisayar"},  
            // };

            //  db.Categories.AddRange(categories);

             // db.SaveChanges();
            
            // int[] ids = new int[2]{1,2};
            // var p = db.Products.Find(1);

            // p.ProductCategories=ids.Select(cid=> new ProductCategory(){

            //     CategoryId=cid,
            //     ProductId=p.Id


            // }).ToList();

            // db.SaveChanges();

            // var p = new Product()
            // {
            //     Name="Samsung S6",
            //     Price=5666
            // };

            var p = db.Products.FirstOrDefault();  // tek bir verim vardı ve güncelleme islemi yaptırdım . InsertDate ve LastUpdateDate görüntülemek için faydalı olucaktır.
            p.Name="Samsung S10";
            p.Price=5555;
            db.SaveChanges();
           
         }
        }

        static void InsertUser()
        {
            var user = new List<User>()
            {
                new User(){Username="furkancakir",Email="info@furkancakir.dev"},
                new User(){Username="ahmetcakir",Email="info@ahmetcakir.dev"},
                new User(){Username="mehmetcakir",Email="info@mehmetcakir.dev"},
                new User(){Username="aysecakir",Email="info@aysecakir.dev"},
                
            };

            using(var db = new ShopContext()){
                db.Users.AddRange(user);
                db.SaveChanges();
            }



        }
        static void InsertAdress()
        {
            var adress = new List<Address>()
            {

              new Address(){Fullname="Furkan Çakır",Title="Ev Adresi",Body="İstanbul",UserId=1},
              new Address(){Fullname="Furkan Çakır",Title="İş Adresi",Body="İstanbul",UserId=1},
              new Address(){Fullname="Ahmet Çakır",Title="iş Adresi",Body="İstanbul",UserId=2},
              new Address(){Fullname="Ahmet Çakır",Title="iş Adresi",Body="İstanbul",UserId=2},
              new Address(){Fullname="Mehmet Çakır",Title="iş Adresi",Body="İstanbul",UserId=3},
              new Address(){Fullname="Ayse Çakır",Title="iş Adresi",Body="İstanbul",UserId=4},
              
            };
            using(var db = new ShopContext())
            {
              db.Addresses.AddRange(adress);
              db.SaveChanges();
            }
        }

    }
}
