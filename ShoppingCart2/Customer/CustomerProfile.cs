using ShoppingCart.BL.Managers;
using ShoppingCart.BL.Managers.Interfaces;
using ShoppingCart.BL.Entities;
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
using ShoppingCart2.Models;
using System.Runtime.InteropServices;

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

        private CustomerDTO FetchData() 
        {
            try
            {
                CustomerDTO customerDTO = new CustomerDTO();
                customerDTO.Details = _customerManager.GetById(_customer.Id);
                customerDTO.Orders = _orderManager.GetByCustomerId(_customer.Id).Select(x => new OrderDTO()
                {
                    Id = x.Id,
                    DeliveryDate = x.DeliveryDate,
                    Status = x.Status,
                    TotalAmount = x.TotalAmount
                }).ToList();

                var newAddress = from t in _addressTypeManager.GetByCustomerId(_customer.Id)
                                 join a in _addressManager.GetAll()
                                 on t.AddressId equals a.Id
                                 select new AddressDTO() { Details = a, AddressCode = (AddressCode)t.AddressCode };

                customerDTO.Addresses = newAddress.ToList();
                return customerDTO;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            
        }
        private void LoadData() 
        {
            try
            {
                var customerDTO = FetchData();

                lblCustomerId.Text = customerDTO.Details.Id.ToString();
                lblName.Text = $"{customerDTO.Details.FirstName.Trim()} {customerDTO.Details.LastName.Trim()}";
                lblEmail.Text = customerDTO.Details.Email;
                lblMobileNumber.Text = customerDTO.Details.MobileNumber;
                _customer = customerDTO.Details;

                foreach (var address in customerDTO.Addresses)
                {
                    if (address.AddressCode == AddressCode.Shipping)
                    {
                        tabControlAddress.SelectedTab = tabPage1;
                        lblShippingAddressId.Text = address.Details.Id.ToString();
                        lblStreetLineName.Text = address.Details.AddressLine;
                        lblCityName.Text = address.Details.City;
                        lblCountryName.Text = address.Details.Country;
                        lblZipCodeName.Text = address.Details.ZipCode;

                        var newAddType = new AddressType() 
                        { 
                            AddressId = address.Details.Id, 
                            CustomerId = customerDTO.Details.Id, 
                            AddressCode = (int)address.AddressCode
                        };
                        _addressTypeList.Add(newAddType);
                    }
                    else if (address.AddressCode == AddressCode.Mailing)
                    {
                        tabControlAddress.SelectedTab = tabPage2;
                        lblMailingAddressId.Text = address.Details.Id.ToString();
                        label12.Text = address.Details.AddressLine;
                        label13.Text = address.Details.City;
                        label14.Text = address.Details.Country;
                        label15.Text = address.Details.ZipCode;

                        var newAddType = new AddressType() 
                        { 
                            AddressId = address.Details.Id, 
                            CustomerId = customerDTO.Details.Id, 
                            AddressCode = (int)address.AddressCode
                        };
                        _addressTypeList.Add(newAddType);
                    }
                    else if (address.AddressCode == AddressCode.Billing)
                    {
                        tabControlAddress.SelectedTab = tabPage3;
                        lblBillingAddressId.Text = address.Details.Id.ToString();
                        label20.Text = address.Details.AddressLine;
                        label21.Text = address.Details.City;
                        label22.Text = address.Details.Country;
                        label23.Text = address.Details.ZipCode;

                        var newAddType = new AddressType() 
                        { 
                            AddressId = address.Details.Id, 
                            CustomerId = customerDTO.Details.Id, 
                            AddressCode = (int)address.AddressCode
                        };
                        _addressTypeList.Add(newAddType);
                    }

                    _addressList.Add(address.Details);
                }

                ListViewOrderItems.Items.Clear();
                ListViewOrderItems.Items.AddRange(customerDTO.Orders.Select(x => new ListViewItem(new string[]
                {
                        x.Id.ToString(),
                        customerDTO.Details.Id.ToString(),
                        x.TotalAmount.ToString("0.00"),
                        x.DeliveryDate.ToString(),
                        x.Status
                })).ToArray());

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }
        private void btnEditDetails_Click(object sender, EventArgs e)
        {
            try
            {
                EditCustomerForm editCustomerForm = new EditCustomerForm();
                editCustomerForm.Customer = _customer;
                editCustomerForm.AddressList = _addressList;
                editCustomerForm.AddressTypeList = _addressTypeList;

                if (editCustomerForm.ShowDialog() == DialogResult.OK)
                {
                    while (!editCustomerForm.IsSuccessful)
                    {
                        var result = editCustomerForm.ShowDialog();

                        if (result == DialogResult.Cancel)
                        {
                            editCustomerForm.Close();
                            break;
                        }
                    }

                    LoadData();
                }
                else
                {
                    editCustomerForm.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
