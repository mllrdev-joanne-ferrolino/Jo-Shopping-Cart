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

        private bool _isSuccessful;

        public bool IsSuccessful
        {
            get { return _isSuccessful; }
            set { _isSuccessful = value; }
        }

        private bool _isNew = true;
        private string _input;
        public EditOrderForm()
        {
            _orderItem = new OrderItem();
            InitializeComponent();
        }

        private bool Add() 
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
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Details were not inserted.");
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return true;
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
                _isNew = false;
                txtQuantity.Text = _input != null ? _input : _orderItem.Quantity.ToString();
               
            }

            btnOK.Text = _isNew ? "Add" : "Save";
            this.Text = _isNew ? "Add Order" : "Save Order";
        }

        private bool Save() 
        {
            try
            {
                _input = txtQuantity.Text;

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
                            this.Close();

                        }
                    }
                    else
                    {
                        MessageBox.Show("Details were not inserted.");
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return true;
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            _isSuccessful = _isNew ? Add() : Save();
        }
    }
}
