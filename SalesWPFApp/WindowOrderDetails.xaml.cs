using SalesBusinessObjects;
using SalesRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SalesWPFApp
{
    /// <summary>
    /// Interaction logic for WindowOrderDetails.xaml
    /// </summary>
    public partial class WindowOrderDetails : Window
    {
        private readonly IOrderDetailRepository orderDetailRepository = null;
        private WindowOrders parentWindow;


        public WindowOrderDetails(WindowOrders parentWindow)
        {
            InitializeComponent();
            if (orderDetailRepository == null)
            {
                orderDetailRepository = new OrderDetailRepository();
            }
            this.parentWindow = parentWindow;
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int orderID = int.Parse(txtOrderId.Text);
                int productID = int.Parse(txtProductId.Text);
                int quantity = int.Parse(txtQuantity.Text); // Ensure proper parsing
                decimal unitPrice = decimal.Parse(txtUnitPrice.Text); // Example fields
                double discount = double.Parse(txtDiscount.Text);

                // Check if the OrderDetail already exists
                var existingOrderDetail = await orderDetailRepository.GetOrderDetailByOrderIdAndProductIdAsync(orderID, productID);

                if (existingOrderDetail != null)
                {
                    // Retrieve the existing entity from the database
                    OrderDetail orderDetailToUpdate = existingOrderDetail;
                    orderDetailToUpdate.Quantity = quantity;
                    orderDetailToUpdate.UnitPrice = unitPrice;
                    orderDetailToUpdate.Discount = discount;

                    // Update the existing entity
                    orderDetailRepository.UpdateOrderDetail(orderID, orderDetailToUpdate);
                    MessageBox.Show("Update successful!");
                }
                else
                {
                    // Create a new entity if it does not exist
                    OrderDetail orderDetail = new OrderDetail
                    {
                        OrderId = orderID,
                        ProductId = productID,
                        UnitPrice = unitPrice,
                        Quantity = quantity,
                        Discount = discount
                    };

                    orderDetailRepository.CreateOrderDetail(orderDetail);
                    MessageBox.Show("Create successful!");
                    Close();
                }

                if (parentWindow != null)
                {
                    parentWindow.Window_Loaded(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Something went wrong: {ex.Message}");
            }
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
           
            Close();
        }
        public void LoadOrderDetail(OrderDetail orderDetail)
        {
            txtOrderId.Text = orderDetail.OrderId.ToString();
            txtProductId.Text = orderDetail.ProductId.ToString();
            txtQuantity.Text = orderDetail.Quantity.ToString();
            txtUnitPrice.Text = orderDetail.UnitPrice.ToString();
            txtDiscount.Text = orderDetail.Discount.ToString();
        }
    }
}
