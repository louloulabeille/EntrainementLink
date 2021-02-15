using System;
using System.Linq;
using DbDefaultContext;
using Microsoft.EntityFrameworkCore;

namespace EntrainementLink
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //Requete1();
            //Requete2();
            Requete4();
        }

        static internal void Requete1 ()
        {
            using (DefaultContext dbContext = new DefaultContext())
            {
                var query = dbContext.Customers.Where(e=>e.CustomerId=="ANATR").Include(e=>e.Orders)
                    .ThenInclude(e=>e.OrderDetails)
                    .ThenInclude(e=>e.Product);
                foreach(var item in query)
                {
                    Console.WriteLine("Id customer :"+item.CustomerId+" Pays :"+item.Country);
                    foreach(var commande in item.Orders)
                    {
                        Console.WriteLine("Numéro de la commande :"+commande.OrderId);
                        foreach (var details in commande.OrderDetails)
                        {
                            Console.WriteLine("Nom du produit :" + details.Product.ProductName);
                        }
                    }
                }
            }
        }

        static internal void Requete2()
        {
            using (DefaultContext dbContext = new DefaultContext())
            {
                var query = dbContext.Customers.Where(e=>e.Orders.Any(e=>e.ShippedDate==null)).Distinct();
                foreach(var item in query)
                {
                    Console.WriteLine("Id customers :"+item.CustomerId+" Nom du client :"+item.CompanyName);
                }
            }
        }

        static internal void Requete4()
        {
            using (DefaultContext dbContext = new DefaultContext())
            {
                var query = dbContext.Products.Join(dbContext.Categories, e => e.CategoryId, y => y.CategoryId, (e, y) => new { e.CategoryId, y.CategoryName, e.UnitsInStock, e.UnitsOnOrder, e.UnitsReserved })
                    .GroupBy(e => e.CategoryId, e => e.UnitsInStock + e.UnitsOnOrder - e.UnitsReserved, (id, stock) => new { key = id, stock = stock });
                foreach (var item in query)
                {
                    Console.WriteLine("Id Catégorie :"+item.key+" stock = "+item.stock);
                }
            }
        }
    }
}
