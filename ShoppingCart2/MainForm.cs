using ShoppingCart;
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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void ShopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrderForm orderForm = new OrderForm();

            if (this.ActiveMdiChild != null && this.ActiveMdiChild != orderForm)
            {
                this.ActiveMdiChild.Close();
            }

            orderForm.MdiParent = this;
            orderForm.Show();
        }

        private void ViewProductsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductForm productForm = new ProductForm();

            if (this.ActiveMdiChild != null && this.ActiveMdiChild != productForm)
            {
                this.ActiveMdiChild.Close();
            }

            productForm.MdiParent = this;
            productForm.Show();
        }

        private void viewCustomersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EditCustomerForm.customer == null)
            {
                if (MessageBox.Show("No existing customer yet. Select customer.") == DialogResult.OK)
                {
                    CustomerForm customerForm = new CustomerForm();
                    if (this.ActiveMdiChild != null && this.ActiveMdiChild != customerForm)
                    {
                        this.ActiveMdiChild.Close();
                    }

                    customerForm.MdiParent = this;
                    customerForm.Show();
                }
                
            }
            else
            {
                CustomerProfile customerProfile = new CustomerProfile();

                if (this.ActiveMdiChild != null && this.ActiveMdiChild != customerProfile)
                {
                    this.ActiveMdiChild.Close();
                }

                customerProfile.MdiParent = this;
                customerProfile.Show();
            }

        }

        private void viewCustomerListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomerForm customerForm = new CustomerForm();
            customerForm.MdiParent = this;
            customerForm.Show();
        }
    }
}
