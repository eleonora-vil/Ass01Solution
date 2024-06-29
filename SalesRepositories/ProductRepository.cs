using SalesBusinessObjects;
using SalesDAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesRepositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDAO productDAO = null;

        public ProductRepository()
        {
            if (productDAO == null)
            {
                productDAO = new ProductDAO();
            }
        }
        public void AddProduct(Product product) => ProductDAO.Instance.AddProduct(product);
        public void DeleteProduct(int productID) => ProductDAO.Instance.DeleteProduct(productID);
        public Product GetProductById(int productID) => ProductDAO.Instance.GetProductById(productID);
        public List<Product> GetProducts() => ProductDAO.Instance.GetProducts();
        public void UpdateProduct(int productID, Product newProduct) => ProductDAO.Instance.UpdateProduct(productID, newProduct);


    }
}
