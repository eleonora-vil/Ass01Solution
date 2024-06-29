using Microsoft.EntityFrameworkCore;
using SalesBusinessObjects;

namespace SalesDAOs
{
    public class ProductDAO
    {
        private readonly FStoreContext db = null;
        private static ProductDAO instance = null;

        public ProductDAO()
        {
            db = new FStoreContext();
        }

        public static ProductDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductDAO();
                }
                return instance;
            }
        }

        public Product GetProductById(int productID)
        {
            return db.Products.FirstOrDefault(x => x.ProductId == productID);
        }

        public List<Product> GetProducts()
        {
            return db.Products.ToList();
        }
        public void AddProduct(Product product)
        {
            Product newProduct = GetProductById(product.ProductId);
            if (newProduct == null)
            {
                db.Products.Add(product);
                db.SaveChanges();
            }
        }
        public void DeleteProduct(int productID)
        {
            Product product = GetProductById(productID);
            if (product != null)
            {
                db.Products.Remove(product);
                db.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException($"Product with ID {productID} not found.");
            }
        }

        public void UpdateProduct(int productID, Product newProduct)
        {
            var existingProduct = GetProductById(productID);
            if (existingProduct != null)
            {
                existingProduct.CategoryId = newProduct.CategoryId;
                existingProduct.ProductName = newProduct.ProductName;
                existingProduct.Weight = newProduct.Weight;
                existingProduct.UnitPrice = newProduct.UnitPrice;
                existingProduct.UnitsInStock = newProduct.UnitsInStock;

                db.Products.Attach(existingProduct);
                db.Entry(existingProduct).State = EntityState.Modified;
                db.SaveChanges();
            }
        }



    }
}
