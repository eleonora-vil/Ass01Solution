using SalesBusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesRepositories
{
    public interface IProductRepository
    {
        public Product GetProductById(int productID);
        public List<Product> GetProducts();
        public void AddProduct(Product product);
        public void DeleteProduct(int productID);
        public void UpdateProduct(int productID, Product newProduct);
    }
}
