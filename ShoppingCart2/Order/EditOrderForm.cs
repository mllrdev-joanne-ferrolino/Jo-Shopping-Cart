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
        public static OrderItem orderItem;
        public EditOrderForm()
        {
            orderItem = new OrderItem();
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int productId = Convert.ToInt32(lblId.Text);
                int quantity = Convert.ToInt32(txtQuantity.Text);
                float amount = (float)Convert.ToDouble(lblPrice.Text) * quantity;

                orderItem = new OrderItem() { ProductId = productId, Quantity = quantity, Amount = amount };

                if (orderItem != null)
                {
                    MessageBox.Show("Details inserted successfully.");
                    OrderForm orderForm = new OrderForm();
                    this.Close();
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
            if (ProductForm.product != null)
            {
                lblId.Text = ProductForm.product.Id.ToString();
                lblProductName.Text = ProductForm.product.Name;
                lblPrice.Text = ProductForm.product.Price.ToString();
                lblDescription.Text = ProductForm.product.Description;
            }
            
            if (OrderForm.orderItem.Quantity > 0)
            {
                btnUpdate.Visible = true;
                btnAdd.Visible = false;
                this.Text = "Edit Order";
                txtQuantity.Text = OrderForm.orderItem.Quantity.ToString();
            }
            
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                int productId = Convert.ToInt32(lblId.Text);
                int quantity = Convert.ToInt32(txtQuantity.Text);
                float amount = (float)Convert.ToDouble(lblPrice.Text) * quantity;

                orderItem = new OrderItem() { ProductId = productId, Quantity = quantity, Amount = amount };

                if (orderItem != null)
                {
                    MessageBox.Show("Details updated successfully.");
                    OrderForm orderForm = new OrderForm();
                    this.Close();
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
