using Microsoft.EntityFrameworkCore;
using SalesBusinessObjects;

namespace SalesDAOs
{
    public class OrderDetailDAO
    {
        private readonly FStoreContext db;
        private static OrderDetailDAO instance;

        public OrderDetailDAO()
        {
            db = new FStoreContext();
        }

        public static OrderDetailDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OrderDetailDAO();
                }
                return instance;
            }
        }
        public List<OrderDetail> GetOrderDetails()
        {
            return db.OrderDetails.ToList();
        }
        public OrderDetail GetOrderDetailById(int orderID)
        {
            return db.OrderDetails.FirstOrDefault(x => x.OrderId == orderID);
        }

        public void UpdateOrderDetail(int orderID, OrderDetail orderDetail)
        {
            var existingOrderDetail = GetOrderDetailById(orderID);
            if (existingOrderDetail != null)
            {

                existingOrderDetail.UnitPrice = orderDetail.UnitPrice;
                existingOrderDetail.Quantity = orderDetail.Quantity;
                existingOrderDetail.Discount = orderDetail.Discount;

                db.OrderDetails.Attach(existingOrderDetail);
                db.Entry(existingOrderDetail).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        public void CreateOrderDetail(OrderDetail orderDetail)
        {
            OrderDetail newOrderDetail = GetOrderDetailById(orderDetail.ProductId);
            if (newOrderDetail == null)
            {
                db.OrderDetails.Add(orderDetail);
                db.SaveChanges();
            }
        }
        public void DeleteOrderDetails(int orderID)
        {
            OrderDetail orderDetail = GetOrderDetailById(orderID);
            if (orderDetail != null)
            {
                db.OrderDetails.Remove(orderDetail);
                db.SaveChanges();
            }
        }
        public async Task<OrderDetail> GetOrderDetailByOrderIdAndProductIdAsync(int orderId, int productId)
        {
            return await db.OrderDetails
               .FirstOrDefaultAsync(od => od.OrderId == orderId && od.ProductId == productId);
        }
    }
}
