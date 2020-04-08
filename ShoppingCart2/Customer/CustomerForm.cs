﻿using ShoppingCart.BL.Managers;
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
    public partial class CustomerForm : Form
    {
        private ICustomerManager _manager;
        private IAddressManager _addressManager;
        private IAddressTypeManager _addressTypeManager;
        private IOrderManager _orderManager; 
        private IOrderItemManager _orderItemManager;
        private IEnumerable<Customer> _resultList;
        public static List<Address> addressList;
        public static IEnumerable<AddressType> addressTypeList;
        public static Customer customer;
        public CustomerForm()
        {
            _manager = new CustomerManager();
            _addressManager = new AddressManager();
            _addressTypeManager = new AddressTypeManager();
            _orderManager = new OrderManager();
            _orderItemManager = new OrderItemManager();
            addressList = new List<Address>();
            addressTypeList = new List<AddressType>();
            _resultList = new List<Customer>();
            customer = new Customer();

            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            EditCustomerForm editCustomerForm = new EditCustomerForm();
            editCustomerForm.MdiParent = this.MdiParent;
            editCustomerForm.Show();
        }

        private void CustomerForm_Load(object sender, EventArgs e)
        {
            LoadCustomers();
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            if (ListViewCustomers.SelectedItems.Count > 0)
            {
                OrderForm orderForm = new OrderForm();
                orderForm.MdiParent = this.MdiParent;
                orderForm.Show();
            }
            
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (ListViewCustomers.SelectedItems.Count > 0)
            {
                EditCustomerForm editCustomerForm = new EditCustomerForm();
                editCustomerForm.MdiParent = this.MdiParent;
                editCustomerForm.Show();
            }
        }

        private void ListViewCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListViewCustomers.SelectedItems.Count > 0)
            {
                customer = new Customer() 
                { 
                    Id = Convert.ToInt32(ListViewCustomers.SelectedItems[0].SubItems[0].Text), 
                    LastName = ListViewCustomers.SelectedItems[0].SubItems[1].Text, 
                    FirstName = ListViewCustomers.SelectedItems[0].SubItems[2].Text, 
                    Email = ListViewCustomers.SelectedItems[0].SubItems[3].Text, 
                    MobileNumber = ListViewCustomers.SelectedItems[0].SubItems[4].Text
                };

                addressTypeList = _addressTypeManager.GetAll().Where(x => x.CustomerId == customer.Id);

                foreach (var addressItem in addressTypeList)
                {
                    Address address = _addressManager.GetAll().FirstOrDefault(x => x.Id == addressItem.AddressId);
                    addressList.Add(address);
                }
                
                btnOrder.Enabled = true;
                btnUpdate.Enabled = true;
                btnAdd.Enabled = false;
            }
            else
            {
                btnOrder.Enabled = false;
                btnUpdate.Enabled = false;
                btnAdd.Enabled = true;
            }
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            if (ListViewCustomers.SelectedItems.Count > 0)
            {
                CustomerProfile customerProfile = new CustomerProfile();
                customerProfile.MdiParent = this.MdiParent;
                customerProfile.Show();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<int> ids = new List<int>();
            List<int> addressIds = new List<int>();
            List<int> orderIds = new List<int>();

            foreach (ListViewItem item in ListViewCustomers.SelectedItems)
            {
                ids.Add(Convert.ToInt32(item.SubItems[0].Text));
            }

            foreach (var id in ids)
            {
                if (_addressTypeManager.GetAll().Where(x => x.CustomerId == id).Count() > 0)
                {
                    var addressTypeList = _addressTypeManager.GetAll().Where(x => x.CustomerId == id);

                    foreach (var addressType in addressTypeList)
                    {
                        addressIds.Add(addressType.AddressId);
                    }

                    MessageBox.Show(_addressTypeManager.DeleteByCustomerId(ids.ToArray()) ? "Address types deleted successfully" : "Address types were not deleted");
                    MessageBox.Show(_addressManager.Delete(addressIds.ToArray()) ? "Addresses deleted successfully" : "Addresses were not deleted");
                }
                else
                {
                    MessageBox.Show("No existing address type for this customer.");
                }

                if (_orderManager.GetAll().Where(x => x.CustomerId == id).Count() > 0)
                {
                    var orderList = _orderManager.GetAll().Where(x => x.CustomerId == id);

                    foreach (var orders in orderList)
                    {
                        orderIds.Add(orders.Id);
                    }

                    MessageBox.Show(_orderItemManager.DeleteByOrderId(orderIds.ToArray()) ? "Order item deleted successfully" : "Order items were not deleted");
                    MessageBox.Show(_orderManager.Delete(orderIds.ToArray()) ? "Order deleted successfully" : "Orders were not deleted");

                }
                else
                {
                    MessageBox.Show("No existing orders for this customer");
                }

            }

            if (_manager.Delete(ids.ToArray()))
            {
                MessageBox.Show("Customer details deleted successfully.");
                ListViewCustomers.Items.Clear();
                LoadCustomers();
                btnAdd.Enabled = true;
            }
            else
            {
                MessageBox.Show("Customer details were not deleted.");
            }
        }

        private void LoadCustomers() 
        {
            IList<Customer> customerList = _manager.GetAll();
            IList<AddressType> addressTypeList = _addressTypeManager.GetAll();
            IList<Address> addressList = _addressManager.GetAll();
            Address customerAddress = new Address();
            string addressString = string.Empty;
            string addressTypeString = string.Empty;

            foreach (var customer in customerList)
            {
                try
                {
                    AddressType customerAddressType = addressTypeList.FirstOrDefault(x => x.CustomerId == customer.Id);

                    if (customerAddressType != null)
                    {
                        customerAddress = addressList.FirstOrDefault(x => x.Id == customerAddressType.AddressId);

                        addressString = string.Join(", ", new string[] 
                        { 
                            customerAddress.AddressLine, 
                            customerAddress.City, 
                            customerAddress.Country, 
                            customerAddress.ZipCode 
                        });

                        addressTypeString = customerAddressType.AddressTypeName;

                    }
                    else
                    {
                        addressString = "No customer address";
                        addressTypeString = "No address type";
                    }
                }
                catch (Exception ex)
                {
                    addressString = ex.Message;
                    addressTypeString = ex.Message;
                }

                ListViewCustomers.Items.Add(new ListViewItem(new string[]
                  {
                            customer.Id.ToString(),
                            customer.LastName,
                            customer.FirstName,
                            customer.Email,
                            customer.MobileNumber,
                            addressString,
                            addressTypeString
                  }));
            }
        }

        private void CustomerForm_Activated(object sender, EventArgs e)
        {
            ListViewCustomers.Items.Clear();

            if (_resultList.Count() > 0)
            {
                LoadSearchResults();
            }
            else
            {
                LoadCustomers();
            }

            if (ListViewCustomers.SelectedItems.Count > 0)
            {
                btnOrder.Enabled = true;
                btnAdd.Enabled = false;
                btnUpdate.Enabled = true;
            }
            else
            {
                btnOrder.Enabled = false;
                btnUpdate.Enabled = false;
                btnAdd.Enabled = true;
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    int id = 0;

                    if (int.TryParse(txtSearch.Text, out id))
                    {
                        _resultList = _manager.GetAll().Where(x => x.Id == id);

                    }
                    else
                    {
                        string searchByName = txtSearch.Text.ToLower();
                        _resultList = _manager.GetAll().Where(x => x.FirstName.Contains(searchByName) || x.LastName.Contains(searchByName));
                    }

                    if (_resultList.Count() == 0)
                    {
                        MessageBox.Show("Item doesn't exist");

                    }
                    else
                    {
                        LoadSearchResults();
                    }

                }
                else
                {
                    MessageBox.Show("Please input your search query.");
                    txtSearch.Clear();
                    txtSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                txtSearch.Clear();
            }
        }

        private void CustomerForm_Click(object sender, EventArgs e)
        {
            ListViewCustomers.SelectedItems.Clear();
            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "Search by Id or Name";
            ListViewCustomers.Items.Clear();
            _resultList = new List<Customer>();
            LoadCustomers();
        }

        private void LoadSearchResults() 
        {
            ListViewCustomers.Items.Clear();
            string addressString = string.Empty;
            string addressTypeString = string.Empty;

            foreach (var result in _resultList)
            {
                var customerAddressType = _addressTypeManager.GetAll().FirstOrDefault(x => x.CustomerId == result.Id);

                if (customerAddressType != null)
                {
                    var customerAddress = _addressManager.GetAll().FirstOrDefault(x => x.Id == customerAddressType.AddressId);

                    if (customerAddress != null)
                    {
                        addressString = string.Join(", ", new string[]
                        {
                            customerAddress.AddressLine,
                            customerAddress.City,
                            customerAddress.Country,
                            customerAddress.ZipCode
                        });

                        addressTypeString = customerAddressType.AddressTypeName;
                    }
                    else
                    {
                        addressString = "No customer address";
                    }

                }
                else
                {
                    addressTypeString = "No customer address type";
                }

                ListViewCustomers.Items.Add(new ListViewItem(new string[]
                {
                    result.Id.ToString(),
                    result.LastName,
                    result.FirstName,
                    result.Email,
                    result.MobileNumber,
                    addressString,
                    addressTypeString
                }));
            }
        }

        private void txtSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
        }
    }
}
