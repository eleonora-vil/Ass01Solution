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
    /// Interaction logic for WindowProducts.xaml
    /// </summary>
    public partial class WindowProducts : Window
    {
        private readonly IProductRepository productRepository = null;

        public WindowProducts()
        {
            InitializeComponent();
            if (productRepository == null)
            {
                productRepository = new ProductRepository();
            }
        }

        private void dgProducts_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgProducts.SelectedItem != null)
            {
                Product selectedProduct = dgProducts.SelectedItem as Product;

                // Hiển thị thông tin sản phẩm đã chọn lên các TextBox tương ứng
                txtProductId.Text = selectedProduct.ProductId.ToString();
                txtCategoryId.Text = selectedProduct.CategoryId.ToString();
                txtProductName.Text = selectedProduct.ProductName;
                txtUnitPrice.Text = selectedProduct.UnitPrice.ToString();
                txtUnitsInStock.Text = selectedProduct.UnitsInStock.ToString();
                txtWeight.Text = selectedProduct.Weight;

                // Bật nút Update và Delete khi có sản phẩm được chọn
                btnUpdate.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnAdd.IsEnabled = false;
            }
            else
            {
                // Nếu không có sản phẩm nào được chọn, xóa dữ liệu trong các TextBox và vô hiệu hóa nút Update và Delete
                ClearFields();
           
            }
        }

        private void ClearFields()
        {
            // Hàm để xóa dữ liệu trong các TextBox
            txtProductId.Text = "Product ID will auto generate";
            txtCategoryId.Text = "";
            txtProductName.Text = "";
            txtUnitPrice.Text = "";
            txtUnitsInStock.Text = "";
            txtWeight.Text = "";
            btnUpdate.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnAdd.IsEnabled = true;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Kiểm tra các trường bắt buộc phải điền
                if (string.IsNullOrWhiteSpace(txtCategoryId.Text) ||
                    string.IsNullOrWhiteSpace(txtProductName.Text) ||
                    string.IsNullOrEmpty(txtUnitPrice.Text) ||
                    string.IsNullOrEmpty(txtUnitsInStock.Text) ||
                    string.IsNullOrEmpty(txtWeight.Text))
                {
                    MessageBox.Show("All fields must be filled");
                    return;
                }

                // Tạo mới sản phẩm từ các giá trị nhập vào
                Product product = new Product
                {
                    CategoryId = int.Parse(txtCategoryId.Text),
                    ProductName = txtProductName.Text,
                    UnitPrice = decimal.Parse(txtUnitPrice.Text),
                    UnitsInStock = int.Parse(txtUnitsInStock.Text),
                    Weight = txtWeight.Text,
                };

                // Thêm sản phẩm vào Repository
                productRepository.AddProduct(product);
                MessageBox.Show("Added successfully!");

                // Load lại danh sách sản phẩm và làm sạch các trường nhập liệu
                this.Window_Loaded(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add product: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Kiểm tra nếu không có sản phẩm nào được chọn
                if (dgProducts.SelectedItem == null)
                {
                    MessageBox.Show("Please select a product to update.");
                    return;
                }

                // Lấy sản phẩm được chọn từ DataGrid
                Product selectedProduct = dgProducts.SelectedItem as Product;

                // Cập nhật thông tin sản phẩm từ các TextBox
                selectedProduct.CategoryId = int.Parse(txtCategoryId.Text);
                selectedProduct.ProductName = txtProductName.Text;
                selectedProduct.UnitPrice = decimal.Parse(txtUnitPrice.Text);
                selectedProduct.UnitsInStock = int.Parse(txtUnitsInStock.Text);
                selectedProduct.Weight = txtWeight.Text;

                // Cập nhật vào Repository
                productRepository.UpdateProduct(selectedProduct.ProductId,selectedProduct);
                MessageBox.Show("Updated successfully!");

                // Load lại danh sách sản phẩm và làm sạch các trường nhập liệu
                this.Window_Loaded(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update product: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Kiểm tra nếu không có sản phẩm nào được chọn
                if (dgProducts.SelectedItem == null)
                {
                    MessageBox.Show("Please select a product to delete.");
                    return;
                }

                // Xác nhận xóa sản phẩm
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this product?",
                    "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    // Lấy sản phẩm được chọn từ DataGrid
                    Product selectedProduct = dgProducts.SelectedItem as Product;

                    // Xóa sản phẩm từ Repository
                    productRepository.DeleteProduct(selectedProduct.ProductId);
                    MessageBox.Show("Deleted successfully!");

                    // Load lại danh sách sản phẩm và làm sạch các trường nhập liệu
                    this.Window_Loaded(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete product: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            // Làm sạch các trường nhập liệu
            ClearFields();
       
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // Đóng cửa sổ hiện tại
            this.Close();
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dgProducts.ItemsSource = productRepository.GetProducts();
            dgProducts.Items.Refresh();
        }
    }
}