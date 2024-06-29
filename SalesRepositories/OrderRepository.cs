using SalesBusinessObjects;
using SalesDAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesRepositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDAO orderDAO = null;

        public OrderRepository()
        {
            if (orderDAO == null)
            {
                orderDAO = new OrderDAO();
            }
        }
        public void AddOrder(Order order) => OrderDAO.Instance.AddOrder(order);


        public void DeleteOrder(int orderID) => OrderDAO.Instance.DeleteOrder(orderID);


        public Order GetOrderById(int orderID) => OrderDAO.Instance.GetOrderById(orderID);

        public List<Order> GetOrders() => OrderDAO.Instance.GetOrders();


        public void UpdateOrder(int orderID, Order newOrder) => OrderDAO.Instance.UpdateOrder(orderID, newOrder);

    }
}
