using ShoppingCart;
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
    public partial class EditOrderForm : Form
    {
        private Product _product;

        public Product Product
        {
            get { return _product; }
            set { _product = value; }
        }

        private OrderItem _orderItem;

        public OrderItem OrderItem
        {
            get { return _orderItem; }
            set { _orderItem = value; }
        }

        public EditOrderForm()
        {
            _orderItem = new OrderItem();
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int productId = Convert.ToInt32(lblId.Text);
                int quantity = Convert.ToInt32(txtQuantity.Text);
                float amount = (float)Convert.ToDouble(lblPrice.Text) * quantity;

                _orderItem = new OrderItem() { ProductId = productId, Quantity = quantity, Amount = amount };

                if (_orderItem != null)
                {
                    if (MessageBox.Show("Item added to cart.") == DialogResult.OK)
                    {
                        OrderForm orderForm = new OrderForm();
                        orderForm.OrderItem = _orderItem;
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Details were not inserted.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void EditOrderForm_Load(object sender, EventArgs e)
        {
            if (_product != null)
            {
                lblId.Text = _product.Id.ToString();
                lblProductName.Text = _product.Name;
                lblPrice.Text = _product.Price.ToString("0.00");
                lblDescription.Text = _product.Description;
            }

            if (_orderItem.Quantity > 0)
            {
                btnUpdate.Visible = true;
                btnAdd.Visible = false;
                this.Text = "Edit Order";
                txtQuantity.Text = _orderItem.Quantity.ToString();
            }
            
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                int productId = Convert.ToInt32(lblId.Text);
                int quantity = Convert.ToInt32(txtQuantity.Text);
                float amount = (float)Convert.ToDouble(lblPrice.Text) * quantity;

                _orderItem = new OrderItem() { ProductId = productId, Quantity = quantity, Amount = amount };

                if (_orderItem != null)
                {
                    if (MessageBox.Show("Order updated successfully.") == DialogResult.OK)
                    {
                        OrderForm orderForm = new OrderForm();
                        orderForm.OrderItem = _orderItem;
                        this.Close();

                    }
                }
                else
                {
                    MessageBox.Show("Details were not inserted.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
