using ShoppingCart.BL.Managers;
using ShoppingCart.BL.Managers.Interfaces;
using ShoppingCart.BL.Entities;
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
        private Customer _customer;

        public Customer Customer
        {
            get { return _customer; }
            set { _customer = value; }
        }

        private Order _order;

        public Order Order
        {
            get { return _order; }
            set { _order = value; }
        }

        public CheckoutForm()
        {
            _orderItemManager = new OrderItemManager();
            _productManager = new ProductManager();
            _orderManager = new OrderManager();
            InitializeComponent();
        }

        private void CheckoutForm_Activated(object sender, EventArgs e)
        {
            try
            {
                int orderId = 0;

                if (_order.Id > 0)
                {
                    orderId = _order.Id;
                    lblOrderId.Text = _order.Id.ToString();
                    lblDeliveryDate.Text = _order.DeliveryDate.ToString();
                    lblStatus.Text = _order.Status;
                    lblTotalAmount.Text = _order.TotalAmount.ToString("0.00");
                }
                else
                {
                    MessageBox.Show("No order selected");
                }
                
                var orderItemList = _orderItemManager.GetByOrderId(orderId);

                if (orderItemList != null)
                {
                    ListViewOrderItems.Items.AddRange(orderItemList.Select(x => new ListViewItem(new string[] 
                    { 
                        x.ProductId.ToString(),
                        _productManager.GetById(x.ProductId).Name,
                        x.Quantity.ToString(), 
                        x.Amount.ToString("0.00") 
                    })).ToArray());

                   
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
            _order = new Order();
        }
    }
}
