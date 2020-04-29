using ShoppingCart;
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
    public partial class MainForm : Form
    {
        private Customer _customer;
        public Customer Customer
        {
            get { return _customer; }
            set { _customer = value; }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void ShopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viewCustomersToolStripMenuItem.Visible = true;

            OrderForm _orderForm = new OrderForm();

            if (this.ActiveMdiChild != null && this.ActiveMdiChild != _orderForm)
            {
                this.ActiveMdiChild.Close();
            }

            _orderForm.MdiParent = this;
            _orderForm.Customer = _customer;
            _orderForm.Show();
        }

        private void ViewProductsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viewCustomersToolStripMenuItem.Enabled = true;

            ProductForm _productForm = new ProductForm();

            if (this.ActiveMdiChild != null && this.ActiveMdiChild != _productForm)
            {
                this.ActiveMdiChild.Close();
            }

            _productForm.MdiParent = this;
            _productForm.Show();
        }

        private void viewCustomersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomerProfile _customerProfile = new CustomerProfile();

            if (this.ActiveMdiChild != null && this.ActiveMdiChild != _customerProfile)
            {
                this.ActiveMdiChild.Close();
            }

            
            _customerProfile.MdiParent = this;
            _customerProfile.Customer = _customer;
            _customerProfile.Show();
        }

        private void viewCustomerListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomerForm _customerForm = new CustomerForm();

            if (this.ActiveMdiChild != null && this.ActiveMdiChild != _customerForm)
            {
                this.ActiveMdiChild.Close();
            }

            _customerForm.MdiParent = this;
            _customerForm.Show();
        }

    }
}
