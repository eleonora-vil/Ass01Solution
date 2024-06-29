using SalesBusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesRepositories
{
    public interface IOrderRepository
    {
        public Order GetOrderById(int orderID);
        public List<Order> GetOrders();
        public void AddOrder(Order order);
        public void DeleteOrder(int orderID);
        public void UpdateOrder(int orderID, Order newOrder);

    }
}
