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
        private OrderForm _orderForm = new OrderForm();
        private ProductForm _productForm = new ProductForm();
        private CustomerProfile _customerProfile = new CustomerProfile();
        private CustomerForm _customerForm = new CustomerForm();
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

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void ShopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viewCustomersToolStripMenuItem.Enabled = true;

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

            if (this.ActiveMdiChild != null && this.ActiveMdiChild != _productForm)
            {
                this.ActiveMdiChild.Close();
            }

            _productForm.MdiParent = this;
            _productForm.Show();
        }

        private void viewCustomersToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
            
            _customerForm.MdiParent = this;
            _customerForm.Show();
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
          
        }
    }
}
