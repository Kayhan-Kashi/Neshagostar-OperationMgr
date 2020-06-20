using Neshagostar.DAL.DataModel;
using Neshagostar.WebUI.Areas.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Neshagostar.WebUI.Areas.Storage.Controllers.ProductsRelated
{
    public class ProductsController : Controller
    {

        NeshagostarContext context = new NeshagostarContext();


        // GET: Storage/Products
        public ActionResult Index()
        {
            var products = context.Products.ToList();
            List<ProductStorageDTO> productDTOs = new List<ProductStorageDTO>();
            foreach(var product in products)
            {
                if(GetRemainingProduct(product.Id) > 0)
                {
                    var allocatedAmount = context.AllocatedProducts.Include("SendingAllocateds").Include("ImportedProduct")
                        .Where(p => p.ImportedProduct.ProductId == product.Id)
                        .ToList()
                        .Sum(p => p.Amount);
                    productDTOs.Add(new ProductStorageDTO
                    {
                        ProductId = product.Id,
                        ProductName = product.Title,
                        ProductCode = product.Code,
                        ExistingAmount = GetRemainingProduct(product.Id),
                        UnAllocatedAmount = GetUnAllocatedProduct(product.Id)

                    });
                }
  
            }
            return View(productDTOs.OrderBy(p => p.ProductCode).ToList());
        }

        private double GetUnAllocatedProduct(Guid productId)
        {
            var importedProducts = context.ImportedProducts.Include("AllocatedProducts.SendingAllocateds").Where(p => p.ProductId.Equals(productId)).ToList();
            double totalImported = 0;
            double totalAlloc = 0;
            double totalSent = 0;
            double totalUnsent = 0;
            double totalAvail = 0;
            double totalUnAlloc = 0;

            if (importedProducts != null && importedProducts.Count > 0)
            {
                double sumImported = 0;
                double sumAlloc = 0;
                double sumSent = 0;
                double sumUnSent = 0;
                double sumUnAlloc = 0;

                double sumAvail = 0;

                foreach (var prod in importedProducts)
                {
                    sumImported += prod.AmountImported;
                    sumAlloc += prod.AmountAllocated;
                    sumSent += prod.AmountSent;
                    sumUnSent += prod.AmountUnSent;
                    sumUnAlloc += prod.AmountUnAllocated;

                    //sumAvail += prod.AmountUnSent - (prod.AmountAllocated - prod.AmountSent);
                    totalUnAlloc = sumUnAlloc;
                }

                totalImported = sumImported;
                totalAlloc = sumAlloc;
                totalSent = sumSent;
                totalUnsent = sumUnSent;
                totalAvail = sumAvail;
            }

            //return totalUnsent - (totalAlloc - totalSent);
            //return totalAvail;
            return totalUnAlloc;
        }

        private double GetRemainingProduct(Guid productId)
        {
            var importedProducts = context.ImportedProducts.Include("AllocatedProducts.SendingAllocateds").Where(p => p.ProductId.Equals(productId)).ToList();
            double totalImported = 0;
            if (importedProducts != null && importedProducts.Count > 0)
            {
                double sum = 0;
                foreach(var prod in importedProducts)
                {
                    sum += prod.AmountUnSent;
                }
                totalImported = sum;
            }
            //var totalImportedAmount = context.ImportedProducts.Where(p => p.ProductId.Equals(productId)).Sum(p => p.AmountImported);
            var sendings = context.Sendings.Where(p => p.OrderItem.ProductId.Equals(productId)).ToList();
            double totalsent = 0;
            if (sendings != null && sendings.Count > 0)
            {
                double sum = 0;
                foreach (var sentItem in sendings)
                {
                    sum += sentItem.AmountSent;
                }
                totalsent = sum;
            }
            return totalImported;
        }

        
    }
}