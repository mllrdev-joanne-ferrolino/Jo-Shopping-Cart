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
    public partial class CustomerProfile : Form
    {
        private ICustomerManager _customerManager;
        private IAddressManager _addressManager;
        private IAddressTypeManager _addressTypeManager;
        private IOrderItemManager _orderItemManager;
        private IOrderManager _orderManager;
        public static Address addressItem;
        public static Customer customer;
        public static Order order;
        public CustomerProfile()
        {
            _customerManager = new CustomerManager();
            _addressManager = new AddressManager();
            _addressTypeManager = new AddressTypeManager();
            _orderItemManager = new OrderItemManager();
            _orderManager = new OrderManager();
            customer = new Customer();
            order = new Order();
            InitializeComponent();
        }

        private void CustomerProfile_Load(object sender, EventArgs e)
        {
        }
        private void CustomerProfile_Activated(object sender, EventArgs e)
        {
            try
            {
                if (CustomerForm.customer == null)
                {
                    MessageBox.Show("No existing customer yet. Select customer.");
                }
                else
                {
                    if (CustomerForm.customer.Id > 0)
                    {
                        Customer customer = _customerManager.GetAll().FirstOrDefault(x => x.Id == CustomerForm.customer.Id);
                        lblCustomerId.Text = CustomerForm.customer.Id.ToString();
                        lblFirstName.Text = customer.FirstName;
                        lblLastName.Text = customer.LastName;
                        lblEmail.Text = customer.Email;
                        lblMobileNumber.Text = customer.MobileNumber;

                        var typeList = _addressTypeManager.GetAll().Where(x => x.CustomerId == CustomerForm.customer.Id);

                        if (typeList.Count() > 0)
                        {
                            foreach (var addressType in typeList)
                            {
                                var addressList = _addressManager.GetAll().Where(x => x.Id == addressType.AddressId);

                                if (addressList.Count() > 0)
                                {
                                    foreach (var address in addressList)
                                    {
                                        if (addressType.AddressTypeName == "Shipping Address")
                                        {
                                            tabControlAddress.SelectedTab = tabPage1;
                                            lblShippingAddressId.Text = address.Id.ToString();
                                            lblStreetLineName.Text = address.AddressLine;
                                            lblCityName.Text = address.City;
                                            lblCountryName.Text = address.Country;
                                            lblZipCodeName.Text = address.ZipCode;
                                        }
                                        else if (addressType.AddressTypeName == "Mailing Address")
                                        {
                                            tabControlAddress.SelectedTab = tabPage2;
                                            lblMailingAddressId.Text = address.Id.ToString();
                                            label12.Text = address.AddressLine;
                                            label13.Text = address.City;
                                            label14.Text = address.Country;
                                            label15.Text = address.ZipCode;

                                        }
                                        else if (addressType.AddressTypeName == "Billing Address")
                                        {
                                            tabControlAddress.SelectedTab = tabPage3;
                                            lblBillingAddressId.Text = address.Id.ToString();
                                            label20.Text = address.AddressLine;
                                            label21.Text = address.City;
                                            label22.Text = address.Country;
                                            label23.Text = address.ZipCode;
                                        }

                                    }

                                }
                                else
                                {
                                    MessageBox.Show("No address for this customer");
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("No address type for this customer.");
                        }
                       
                        var orderInfo = _orderManager.GetAll().Where(x => x.CustomerId == CustomerForm.customer.Id);

                        if (orderInfo.Count() > 0)
                        {
                            ListViewOrderItems.Items.Clear();
                            ListViewOrderItems.Items.AddRange(orderInfo.Select(x => new ListViewItem(new string[]
                        {
                        x.Id.ToString(),
                        x.CustomerId.ToString(),
                        x.TotalAmount.ToString("0.00"),
                        x.DeliveryDate.ToShortDateString(),
                        x.Status
                        })).ToArray());

                        }
                        else
                        {
                            MessageBox.Show("There are no orders for this customer.");
                            btnAddOrder.Enabled = true;
                        }

                    }
                    else
                    {
                        EditCustomerForm editCustomerForm = new EditCustomerForm();
                        editCustomerForm.MdiParent = this.MdiParent;
                        editCustomerForm.Show();
                    }
                }
               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void btnEditDetails_Click(object sender, EventArgs e)
        {
            EditCustomerForm editCustomerForm = new EditCustomerForm();
            editCustomerForm.MdiParent = this.MdiParent;
            editCustomerForm.Show();
        }

        private void ListViewOrderItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListViewOrderItems.SelectedItems.Count > 0)
            {
                btnViewOrder.Enabled = true;

                int id = Convert.ToInt32(ListViewOrderItems.SelectedItems[0].SubItems[0].Text);
                float totalAmount = (float)Convert.ToDouble(ListViewOrderItems.SelectedItems[0].SubItems[2].Text);
                var deliveryDate = Convert.ToDateTime(ListViewOrderItems.SelectedItems[0].SubItems[3].Text);
                string status = ListViewOrderItems.SelectedItems[0].SubItems[4].Text;
                
                order = new Order() { Id = id, CustomerId = CustomerForm.customer.Id, DeliveryDate = deliveryDate, Status = status, TotalAmount = totalAmount};
            }
        }

        private void btnAddOrder_Click(object sender, EventArgs e)
        {
            OrderForm orderForm = new OrderForm();
            orderForm.MdiParent = this.MdiParent;
            orderForm.Show();
            this.Close();
        }

        private void btnViewOrder_Click(object sender, EventArgs e)
        {
            CheckoutForm checkoutForm = new CheckoutForm();
            checkoutForm.MdiParent = this.MdiParent;
            checkoutForm.Show();
        }
    }
}
