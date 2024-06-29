using SalesBusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesRepositories
{
    public interface IOrderDetailRepository
    {
        public OrderDetail GetOrderDetailById(int orderID);
        public void UpdateOrderDetail(int orderID, OrderDetail orderDetail);
        public List<OrderDetail> GetOrderDetails();
        public void CreateOrderDetail(OrderDetail orderDetail);
        public void DeleteOrderDetails(int orderID);
        Task<OrderDetail> GetOrderDetailByOrderIdAndProductIdAsync(int orderId, int productId);

    }
}
