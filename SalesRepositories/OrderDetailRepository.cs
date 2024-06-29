using SalesBusinessObjects;
using SalesDAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesRepositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly OrderDetailDAO orderDetailDAO = null;

        public OrderDetailRepository()
        {
            if (orderDetailDAO == null)
            {
                orderDetailDAO = new OrderDetailDAO();
            }
        }

        public void CreateOrderDetail(OrderDetail orderDetail)
        => OrderDetailDAO.Instance.CreateOrderDetail(orderDetail);

        public void DeleteOrderDetails(int orderID)
        => OrderDetailDAO.Instance.DeleteOrderDetails(orderID);

        public OrderDetail GetOrderDetailById(int orderID) => OrderDetailDAO.Instance.GetOrderDetailById(orderID);

        public Task<OrderDetail> GetOrderDetailByOrderIdAndProductIdAsync(int orderId, int productId)
        => OrderDetailDAO.Instance.GetOrderDetailByOrderIdAndProductIdAsync((int)orderId, productId);

        public List<OrderDetail> GetOrderDetails()
        => OrderDetailDAO.Instance.GetOrderDetails();

        public void UpdateOrderDetail(int orderID, OrderDetail orderDetail) => OrderDetailDAO.Instance.UpdateOrderDetail(orderID, orderDetail);

    }
}
