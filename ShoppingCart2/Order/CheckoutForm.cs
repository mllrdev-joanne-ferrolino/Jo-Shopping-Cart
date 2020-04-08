using ShoppingCart.BL.Managers;
using ShoppingCart.BL.Managers.Interfaces;
using ShoppingCart.BL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShoppingCart2
{
    public partial class CheckoutForm : Form
    {
        private IOrderItemManager _orderItemManager;
        private IOrderManager _orderManager;
        private IProductManager _productManager;
        public CheckoutForm()
        {
            _orderItemManager = new OrderItemManager();
            _productManager = new ProductManager();
            _orderManager = new OrderManager();
            InitializeComponent();
        }

        private void CheckoutForm_Load(object sender, EventArgs e)
        {
            
        }

        private void CheckoutForm_Activated(object sender, EventArgs e)
        {
            try
            {
                int orderId = 0;
                IEnumerable<Order> orderList = new List<Order>();

                if (CustomerProfile.order.Id > 0)
                {
                    orderId = CustomerProfile.order.Id;
                    orderList = _orderManager.GetAll().Where(x => x.Id == orderId);
                }
                else
                {
                    orderId = _orderItemManager.GetAll().LastOrDefault().OrderId;
                    orderList = _orderManager.GetAll().Where(x => x.Id == orderId);
                }

                IEnumerable<OrderItem> orderItemList = _orderItemManager.GetAll().Where(x => x.OrderId == orderId);
                IList<Product> productList = _productManager.GetAll();

                if (orderItemList != null && orderList != null)
                {
                    ListViewOrderItems.Items.AddRange(orderItemList.Select(x => new ListViewItem(new string[] 
                    { 
                        x.ProductId.ToString(), 
                        productList.FirstOrDefault(y => y.Id == x.ProductId).Name, 
                        x.Quantity.ToString(), 
                        x.Amount.ToString("0.00") 
                    })).ToArray());

                    if (CustomerProfile.order.Id > 0)
                    {
                        lblOrderId.Text = CustomerProfile.order.Id.ToString();
                        lblDeliveryDate.Text = orderList.FirstOrDefault().DeliveryDate.ToString();
                        lblStatus.Text = orderList.FirstOrDefault().Status;
                        lblTotalAmount.Text = orderList.FirstOrDefault().TotalAmount.ToString("0.00");

                    }
                    else
                    {
                        lblOrderId.Text = orderList.LastOrDefault().Id.ToString();
                        lblDeliveryDate.Text = orderList.LastOrDefault().DeliveryDate.ToString();
                        lblStatus.Text = orderList.LastOrDefault().Status;
                        lblTotalAmount.Text = orderList.LastOrDefault().TotalAmount.ToString();
                    }
                   
                }
                else
                {
                    MessageBox.Show("No values in database.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CheckoutForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CustomerProfile.order = new Order();
        }
    }
}
