﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SalesWPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            switch (button.Name)
            {
                case "btn_Product":
                    WindowProducts windowProducts = new();
                    windowProducts.Show();
                    Close();
                    break;
                case "btn_Member":
                    WindowMembers windowMembers = new();
                    windowMembers.Show();
                    Close();
                    break;
                case "btn_Order":
                    WindowOrders windowOrders = new();
                    windowOrders.Show();
                    Close();
                    break;
            }
        }
    }
}
