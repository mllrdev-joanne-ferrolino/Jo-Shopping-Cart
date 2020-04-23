﻿using ShoppingCart.BL.Managers;
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
using System.Transactions;
using System.Windows.Forms;

namespace ShoppingCart2
{
    public partial class CustomerForm : Form
    {
        private ICustomerManager _customerManager;
        private IAddressManager _addressManager;
        private IAddressTypeManager _addressTypeManager;
        private IOrderManager _orderManager; 
        private IOrderItemManager _orderItemManager;
        private List<Customer> _resultList;
        private List<Address> _addressList;
        private IEnumerable<AddressType> _addressTypeList;
        private Customer _customer;
        public CustomerForm()
        {
            _customerManager = new CustomerManager();
            _addressManager = new AddressManager();
            _addressTypeManager = new AddressTypeManager();
            _orderManager = new OrderManager();
            _orderItemManager = new OrderItemManager();
            _addressList = new List<Address>();
            _addressTypeList = new List<AddressType>();
            _resultList = new List<Customer>();
            _customer = new Customer();

            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                EditCustomerForm editCustomerForm = new EditCustomerForm();

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

                    ListViewCustomers.Items.Clear();
                    LoadCustomers();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void CustomerForm_Load(object sender, EventArgs e)
        {
            LoadCustomers();
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (ListViewCustomers.SelectedItems.Count > 0)
                {
                    OrderForm orderForm = new OrderForm();
                    orderForm.Customer = _customer;
                    orderForm.MdiParent = this.MdiParent;
                    orderForm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void ListViewCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ListViewCustomers.SelectedItems.Count > 0)
                {
                    _customer = new Customer()
                    {
                        Id = ListViewCustomers.SelectedItems[0].SubItems[0].Text.ToInt(),
                        LastName = ListViewCustomers.SelectedItems[0].SubItems[1].Text,
                        FirstName = ListViewCustomers.SelectedItems[0].SubItems[2].Text,
                        Email = ListViewCustomers.SelectedItems[0].SubItems[3].Text,
                        MobileNumber = ListViewCustomers.SelectedItems[0].SubItems[4].Text
                    };
                    
                    _addressTypeList = _addressTypeManager.GetByCustomerId(_customer.Id);

                    foreach (var addressItem in _addressTypeList)
                    {
                        Address address = _addressManager.GetById(addressItem.AddressId);
                        _addressList.Add(address);
                    }

                    btnOrder.Enabled = true;
                    btnAdd.Enabled = false;
                }
                else
                {
                    btnOrder.Enabled = false;
                    btnAdd.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> ids = new List<int>();
                List<int> addressIds = new List<int>();
                List<int> orderIds = new List<int>();

                foreach (ListViewItem item in ListViewCustomers.SelectedItems)
                {
                    ids.Add(item.SubItems[0].Text.ToInt());
                }

                using (var scope = new TransactionScope())
                {
                    foreach (var id in ids)
                    {
                        if (_addressTypeManager.GetByCustomerId(id).Count() > 0)
                        {
                            var addressTypeList = _addressTypeManager.GetByCustomerId(id);

                            foreach (var addressType in addressTypeList)
                            {
                                addressIds.Add(addressType.AddressId);
                            }

                            MessageBox.Show(_addressTypeManager.Delete(ids.ToArray()) ? "Address types deleted successfully" : "Address types were not deleted");
                            MessageBox.Show(_addressManager.Delete(addressIds.ToArray()) ? "Addresses deleted successfully" : "Addresses were not deleted");
                        }
                        else
                        {
                            MessageBox.Show("No existing address type for this customer.");
                        }

                        if (_orderManager.GetByCustomerId(id).Count() > 0)
                        {
                            var orderList = _orderManager.GetByCustomerId(id);

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

                    if (_customerManager.Delete(ids.ToArray()))
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

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
           
        }

        private void LoadCustomers() 
        {
            try
            {
                IList<Customer> customerList = _customerManager.GetAll();
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

                            addressTypeString = customerAddressType.Name;

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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void CustomerForm_Activated(object sender, EventArgs e)
        {
            try
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
                }
                else
                {
                    btnOrder.Enabled = false;
                    btnAdd.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                        _resultList.Clear();
                        Customer result = _customerManager.GetById(id);

                        if (result != null)
                        {
                            _resultList.Add(result);
                        }
                        
                    }
                    else
                    {
                        string searchByName = txtSearch.Text.ToLower();
                        _resultList = _customerManager.GetSearchResult(searchByName);
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
            try
            {
                ListViewCustomers.Items.Clear();
                string addressString = string.Empty;
                string addressTypeString = string.Empty;

                foreach (var result in _resultList)
                {
                    var customerAddressType = _addressTypeManager.GetAddressType(result.Id);

                    if (customerAddressType != null)
                    {
                        var customerAddress = _addressManager.GetById(customerAddressType.AddressId);

                        if (customerAddress != null)
                        {
                            addressString = string.Join(", ", new string[]
                            {
                            customerAddress.AddressLine,
                            customerAddress.City,
                            customerAddress.Country,
                            customerAddress.ZipCode
                            });

                            addressTypeString = customerAddressType.Name;
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void txtSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
        }

        private void ListViewCustomers_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (ListViewCustomers.SelectedItems.Count > 0)
                {
                    CustomerProfile customerProfile = new CustomerProfile();
                    customerProfile.Customer = _customer;
                    customerProfile.MdiParent = this.MdiParent;
                    customerProfile.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }
    }
}
