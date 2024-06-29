using Microsoft.EntityFrameworkCore;
using SalesBusinessObjects;


namespace SalesDAOs
{
    public class OrderDAO
    {
        private readonly FStoreContext db = null;
        private static OrderDAO instance = null;

        public OrderDAO()
        {
            db = new FStoreContext();
        }

        public static OrderDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OrderDAO();
                }
                return instance;
            }
        }
        public Order GetOrderById(int orderID)
        {
            return db.Orders.FirstOrDefault(x => x.OrderId == orderID);
        }

        public List<Order> GetOrders()
        {
            return db.Orders.ToList();
        }
        public void AddOrder(Order order)
        {
            Order newOrder = GetOrderById(order.OrderId);
            if (newOrder == null)
            {
                db.Orders.Add(order);
                db.SaveChanges();
            }
        }
        public void DeleteOrder(int orderID)
        {
            Order order = GetOrderById(orderID);
            if (order != null)
            {
                db.Orders.Remove(order);
                db.SaveChanges();
            }
        }
        public void UpdateOrder(int orderID, Order newOrder)
        {
            var existingOrder = GetOrderById(orderID);
            if (existingOrder != null)
            {
                existingOrder.MemberId = newOrder.MemberId;
                existingOrder.OrderDate = newOrder.OrderDate;
                existingOrder.RequireDate = newOrder.RequireDate;
                existingOrder.ShippedDate = newOrder.ShippedDate;
                existingOrder.Freight = newOrder.Freight;

                db.Orders.Attach(existingOrder);
                db.Entry(existingOrder).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}
