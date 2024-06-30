using SalesBusinessObjects;
using SalesRepositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for WindowOrders.xaml
    /// </summary>
    public partial class WindowOrders : Window
    {
        private readonly IOrderRepository orderRepository = null;
        private readonly IOrderDetailRepository orderDetailRepository = null;
        private readonly IMemberRepository memberRepository = null;

        public ObservableCollection<OrderDetail> OrderDetail { get; set; } = new ObservableCollection<OrderDetail>();

        public WindowOrders()
        {
            InitializeComponent();
            if (orderRepository == null)
            {
                orderRepository = new OrderRepository();
            }
            if (orderDetailRepository == null)
            {
                orderDetailRepository = new OrderDetailRepository();
            }
            if (memberRepository == null)
            {
                memberRepository = new MemberRepository();
            }
        }
        private bool IsValidInput()
        {
            // Example validation: check if required fields are not empty
            if (string.IsNullOrEmpty(txtMemberId.Text) ||
                string.IsNullOrEmpty(txtFreight.Text))
            {
                return false;
            }

            // Add more validations as needed

            return true;
        }

        private void NewOrderButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate input fields
            if (!IsValidInput())
            {
                MessageBox.Show("Please check the input fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                int memberId = int.Parse(txtMemberId.Text);

                // Check if the member exists
                if (memberRepository.GetMember(memberId) == null)
                {
                    MessageBox.Show("Member does not exist.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Gather data from UI controls
                DateTime orderDate = (DateTime)OrderDatePicker.SelectedDate.Value;
                DateTime? requireDate = RequireDatePicker.SelectedDate.HasValue ? (DateTime?)RequireDatePicker.SelectedDate : null;
                DateTime? shippedDate = ShippedDatePicker.SelectedDate.HasValue ? (DateTime?)ShippedDatePicker.SelectedDate : null;
                decimal freight = decimal.Parse(txtFreight.Text);

                // Create a new Order object with the gathered data
                Order newOrder = new Order
                {
                    MemberId = memberId,
                    OrderDate = orderDate,
                    RequireDate = requireDate,
                    ShippedDate = shippedDate,
                    Freight = freight
                };

                // Add the new order to the database
                orderRepository.AddOrder(newOrder);
                MessageBox.Show("Create successfully!");
                this.Window_Loaded(sender, e);
                // Refresh the UI to show the new order
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create order: {ex.InnerException?.Message ?? ex.Message}");
            }
        }



        private void SaveOrderButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate input fields
            if (!IsValidInput())
            {
                MessageBox.Show("Please check the input fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Proceed with updating the order
            try
            {
                Order selectedOrder = (Order)OrdersListBox.SelectedItem;
                if (selectedOrder != null)
                {
                    selectedOrder.MemberId = int.Parse(txtMemberId.Text);
                    selectedOrder.OrderDate = OrderDatePicker.SelectedDate ?? selectedOrder.OrderDate;
                    selectedOrder.RequireDate = RequireDatePicker.SelectedDate ?? selectedOrder.RequireDate;
                    selectedOrder.ShippedDate = ShippedDatePicker.SelectedDate ?? selectedOrder.ShippedDate;
                    selectedOrder.Freight = decimal.Parse(txtFreight.Text);

                    orderRepository.UpdateOrder(selectedOrder.OrderId, selectedOrder);
                    MessageBox.Show("Save successful!");
                    // Refresh the UI
                    this.Window_Loaded(sender, e);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update order: {ex.Message}");
            }
        }



        private void OrdersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrdersListBox.SelectedItems.Count > 0)
            {
                Order selectedOrder = (Order)OrdersListBox.SelectedItem;
                txtOrderID.Text = selectedOrder.OrderId.ToString();
                txtMemberId.Text = selectedOrder.MemberId.ToString();
                OrderDatePicker.SelectedDate = selectedOrder.OrderDate;
                RequireDatePicker.SelectedDate = selectedOrder.RequireDate;
                ShippedDatePicker.SelectedDate = selectedOrder.ShippedDate;
                txtFreight.Text = selectedOrder.Freight.ToString();

                // Clear existing OrderDetail items
                OrderDetail.Clear();
                // Add new OrderDetail items corresponding to the selected order
                foreach (var detail in selectedOrder.OrderDetails)
                {
                    OrderDetail.Add(detail);
                }

                // Synchronize selection in OrderDetailsDataGrid
                var index = OrderDetailsDataGrid.Items.IndexOf(selectedOrder.OrderDetails.FirstOrDefault());
                if (index >= 0)
                {
                    OrderDetailsDataGrid.SelectedIndex = index;
                }
            }
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OrdersListBox.ItemsSource = orderRepository.GetOrders();
            OrdersListBox.Items.Refresh();

            OrderDetailsDataGrid.ItemsSource = orderDetailRepository.GetOrderDetails();
            OrderDetailsDataGrid.Items.Refresh();
        }
        private void btnDeleteOrder(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtOrderID.Text.Length > 0)
                {
                    int orderID = Int32.Parse(txtOrderID.Text);
                    orderRepository.DeleteOrder(orderID);
                    MessageBox.Show("Deleted successfully!");
                    this.Window_Loaded(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting member: {ex.Message}");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            mainWindow.Show();
            Close();
        }
        private void btnClearOrder(object sender, RoutedEventArgs e)
        {
            txtOrderID.Text = "";
            txtMemberId.Clear();
            txtFreight.Clear();
            OrderDatePicker.SelectedDate = null;
            ShippedDatePicker.SelectedDate = null;
            RequireDatePicker.SelectedDate = null;
        }

        private void OrderDetailsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrderDetailsDataGrid.SelectedItems.Count > 0)
            {
                // Enable the btnUpdateDetail button
                btnUpdateDetail.IsEnabled = true;
            }
            else
            {
                // Disable the btnUpdateDetail button if no item is selected
                btnUpdateDetail.IsEnabled = false;
            }
        }



        private void CreateOrderDetailFromSelectedOrder()
        {
            if (OrderDetailsDataGrid.SelectedItem != null)
            {
                OrderDetail selectedOrderDetail = (OrderDetail)OrderDetailsDataGrid.SelectedItem;
                WindowOrderDetails windowOrderDetails = new WindowOrderDetails(this);
                windowOrderDetails.LoadOrderDetail(selectedOrderDetail);
                windowOrderDetails.Show();
            }
        }
        private void btnCreateDetail_Click(object sender, RoutedEventArgs e)
        {
           
            WindowOrderDetails windowOrderDetails = new WindowOrderDetails(this);
            windowOrderDetails.Show();
        }
        private void btnUpdateDetail_Click(object sender, RoutedEventArgs e)
        {
            CreateOrderDetailFromSelectedOrder();
        }

        private void btnDeleteDetail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (OrderDetailsDataGrid.SelectedItem != null)
                {
                    OrderDetail selectedOrderDetail = (OrderDetail)OrderDetailsDataGrid.SelectedItem;
                    int orderlID = selectedOrderDetail.OrderId;
                    orderDetailRepository.DeleteOrderDetails(orderlID);
                    MessageBox.Show("Deleted successfully!");
                    this.Window_Loaded(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting Order Detail: {ex.Message}");
            }
        }

    }
}
