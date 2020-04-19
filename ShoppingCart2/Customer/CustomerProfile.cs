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
    public partial class CustomerProfile : Form
    {
        private ICustomerManager _customerManager;
        private IAddressManager _addressManager;
        private IAddressTypeManager _addressTypeManager;
        private IOrderManager _orderManager;
        private List<Address> _addressList;
        private List<AddressType> _addressTypeList;
        private Customer _customer;
        public Customer Customer
        {
            get { return _customer; }
            set { _customer = value; }
        }

        private Order _order;
        public CustomerProfile()
        {
            _customerManager = new CustomerManager();
            _addressManager = new AddressManager();
            _addressTypeManager = new AddressTypeManager();
            _orderManager = new OrderManager();
            _customer = new Customer();
            _order = new Order();
            _addressTypeList = new List<AddressType>();
            _addressList = new List<Address>();
            InitializeComponent();
        }

        private void CustomerProfile_Activated(object sender, EventArgs e)
        {
            try
            {
                if (_customer == null)
                {
                    MessageBox.Show("No existing customer yet. Select customer.");
                    
                }
                else
                {
                    if (_customer.Id > 0)
                    {
                        MainForm parent = (MainForm)this.MdiParent;
                        parent.viewCustomersToolStripMenuItem.Visible = true;
                        parent.ShopToolStripMenuItem.Enabled = true;
                        parent.Customer = _customer;
                        LoadData();

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

        private void LoadData() 
        {
            _customer = _customerManager.GetById(_customer.Id);
            lblCustomerId.Text = _customer.Id.ToString();
            lblName.Text = $"{_customer.FirstName} {_customer.LastName}";
            lblEmail.Text = _customer.Email;
            lblMobileNumber.Text = _customer.MobileNumber;

            var typeList = _addressTypeManager.GetAll().Where(x => x.CustomerId == _customer.Id);

            if (typeList.Count() > 0)
            {
                foreach (var addressType in typeList)
                {
                    var addressList = _addressManager.GetAll().Where(x => x.Id == addressType.AddressId);

                    if (addressList.Count() > 0)
                    {
                        foreach (var address in addressList)
                        {
                            if (addressType.Name == "Shipping Address")
                            {
                                tabControlAddress.SelectedTab = tabPage1;
                                lblShippingAddressId.Text = address.Id.ToString();
                                lblStreetLineName.Text = address.AddressLine;
                                lblCityName.Text = address.City;
                                lblCountryName.Text = address.Country;
                                lblZipCodeName.Text = address.ZipCode;
                                _addressTypeList.Add(addressType);
                            }
                            else if (addressType.Name == "Mailing Address")
                            {
                                tabControlAddress.SelectedTab = tabPage2;
                                lblMailingAddressId.Text = address.Id.ToString();
                                label12.Text = address.AddressLine;
                                label13.Text = address.City;
                                label14.Text = address.Country;
                                label15.Text = address.ZipCode;
                                _addressTypeList.Add(addressType);

                            }
                            else if (addressType.Name == "Billing Address")
                            {
                                tabControlAddress.SelectedTab = tabPage3;
                                lblBillingAddressId.Text = address.Id.ToString();
                                label20.Text = address.AddressLine;
                                label21.Text = address.City;
                                label22.Text = address.Country;
                                label23.Text = address.ZipCode;
                                _addressTypeList.Add(addressType);
                            }

                            _addressList.Add(address);

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

            var orderInfo = _orderManager.GetAll().Where(x => x.CustomerId == _customer.Id);

            if (orderInfo.Count() > 0)
            {
                ListViewOrderItems.Items.Clear();
                ListViewOrderItems.Items.AddRange(orderInfo.Select(x => new ListViewItem(new string[]
            {
                        x.Id.ToString(),
                        x.CustomerId.ToString(),
                        x.TotalAmount.ToString("0.00"),
                        x.DeliveryDate.ToString(),
                        x.Status
            })).ToArray());

            }
            else
            {
                MessageBox.Show("There are no orders for this customer.");
                btnAddOrder.Enabled = true;
            }
        }
        private void btnEditDetails_Click(object sender, EventArgs e)
        {
            EditCustomerForm editCustomerForm = new EditCustomerForm();
            editCustomerForm.Customer = _customer;
            editCustomerForm.AddressList = _addressList;
            editCustomerForm.AddressTypeList = _addressTypeList;

            if (editCustomerForm.ShowDialog() == DialogResult.OK)
            {
                while (!editCustomerForm.IsSuccessful)
                {
                    editCustomerForm.ShowDialog();
                }

                if (editCustomerForm.IsSuccessful)
                {
                    LoadData();
                }
               
            }
            
        }

        private void ListViewOrderItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ListViewOrderItems.SelectedItems.Count > 0)
                {
                    int id = ListViewOrderItems.SelectedItems[0].SubItems[0].Text.ToInt();
                    float totalAmount = ListViewOrderItems.SelectedItems[0].SubItems[2].Text.ToFloat();
                    DateTime deliveryDate = ListViewOrderItems.SelectedItems[0].SubItems[3].Text.ToDateTime();
                    string status = ListViewOrderItems.SelectedItems[0].SubItems[4].Text;

                    _order = new Order() { Id = id, CustomerId = _customer.Id, DeliveryDate = deliveryDate, Status = status, TotalAmount = totalAmount };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void btnAddOrder_Click(object sender, EventArgs e)
        {
            OrderForm orderForm = new OrderForm();
            orderForm.Customer = _customer;
            orderForm.MdiParent = this.MdiParent;
            orderForm.Show();
            this.Close();
        }

        private void ListViewOrderItems_DoubleClick(object sender, EventArgs e)
        {
            CheckoutForm checkoutForm = new CheckoutForm();
            checkoutForm.Order = _order;
            checkoutForm.MdiParent = this.MdiParent;
            checkoutForm.Show();
        }
    }
}
