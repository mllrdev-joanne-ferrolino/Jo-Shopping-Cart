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
                IEnumerable<Order> orderList = new List<Order>();

                if (_order.Id > 0)
                {
                    orderId = _order.Id;
                    orderList = _orderManager.GetAll().Where(x => x.Id == orderId);
                }
                else
                {
                    orderId = _orderItemManager.GetAll().LastOrDefault().OrderId;
                    
                    orderList = _orderManager.GetAll().Where(x => x.Id == orderId);
                }

                IEnumerable<OrderItem> orderItemList = _orderItemManager.GetAll().Where(x => x.OrderId == orderId);

                if (orderItemList != null && orderList != null)
                {
                    ListViewOrderItems.Items.AddRange(orderItemList.Select(x => new ListViewItem(new string[] 
                    { 
                        x.ProductId.ToString(),
                        _productManager.GetById(x.ProductId).Name,
                        x.Quantity.ToString(), 
                        x.Amount.ToString("0.00") 
                    })).ToArray());

                    if (_order.Id > 0)
                    {
                        lblOrderId.Text = _order.Id.ToString();
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
            _order = new Order();
        }
    }
}
