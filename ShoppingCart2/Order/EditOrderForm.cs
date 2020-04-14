using ShoppingCart;
using ShoppingCart.BL.Managers;
using ShoppingCart.BL.Managers.Interfaces;
using ShoppingCart.BL.Models;
using ShoppingCart.Utilities;
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

        private bool _isValid;

        public bool IsValid
        {
            get { return _isValid; }
            set { _isValid = value; }
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
                if (ValidateQuantity())
                {
                    int productId = lblId.Text.ToInt();
                    int quantity = txtQuantity.Text.ToInt();
                    float amount = lblPrice.Text.ToFloat() * quantity;

                    _orderItem = new OrderItem() { ProductId = productId, Quantity = quantity, Amount = amount };

                    if (_orderItem != null)
                    {
                        if (MessageBox.Show("Item added to cart.") == DialogResult.OK)
                        {
                            OrderForm orderForm = new OrderForm();
                            orderForm.OrderItem = _orderItem;
                            _isValid = true;
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Details were not inserted.");
                    }
                }
                else
                {
                    _isValid = false;
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
                if (ValidateQuantity())
                {
                    int productId = lblId.Text.ToInt();
                    int quantity = txtQuantity.Text.ToInt();
                    float amount = lblPrice.Text.ToFloat() * quantity;

                    _orderItem = new OrderItem() { ProductId = productId, Quantity = quantity, Amount = amount };

                    if (_orderItem != null)
                    {
                        if (MessageBox.Show("Order updated successfully.") == DialogResult.OK)
                        {
                            OrderForm orderForm = new OrderForm();
                            orderForm.OrderItem = _orderItem;
                            _isValid = true;
                            this.Close();

                        }
                    }
                    else
                    {
                        MessageBox.Show("Details were not inserted.");
                    }
                }
                else
                {
                    _isValid = false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private bool ValidateQuantity() 
        {
            if (string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                errorProvider.SetError(txtQuantity, "Please enter quantity");
                return false;
            }
            else if (txtQuantity.Text.Any(x => char.IsLetter(x)) || txtQuantity.Text.Any(x => !char.IsNumber(x)))
            {
                errorProvider.SetError(txtQuantity, "Please enter valid number for quantity");
                return false;
            }
            else
            {
                errorProvider.SetError(txtQuantity, string.Empty);
            }

            return true;
        }
    }
}
