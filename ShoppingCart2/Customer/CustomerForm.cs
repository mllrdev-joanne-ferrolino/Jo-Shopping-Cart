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
using System.Runtime.InteropServices;
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
        private List<Customer> _customerResult;
        private List<Address> _addressResult;
        private List<AddressType> _typeResult;
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
            _customerResult = new List<Customer>();
            _addressResult = new List<Address>();
            _typeResult = new List<AddressType>();
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
            cboType.SelectedItem = string.Empty;
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
               
                if (_customerResult.Count > 0 || _addressResult.Count > 0 || _typeResult.Count > 0)
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

        private bool IsCustomerInfoFilled() 
        {
            foreach (TextBox textBox in grpCustomer.Controls.OfType<TextBox>())
            {
                if (!string.IsNullOrWhiteSpace(textBox.Text))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsAddressInfoFilled()
        {
            foreach (TextBox textBox in grpAddress.Controls.OfType<TextBox>())
            {
                if (!string.IsNullOrWhiteSpace(textBox.Text))
                {
                    return true;
                }
            }
            return false;
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                _customerResult = new List<Customer>();
                _addressResult = new List<Address>();
                _typeResult = new List<AddressType>();
                bool isItemExisting = true;

                if (IsCustomerInfoFilled())
                {
                    Customer searchItem = new Customer()
                    {
                        Id = txtSearchId.Text.ToInt(),
                        FirstName = txtSearchFName.Text,
                        LastName = txtSearchLName.Text,
                        Email = txtSearchEmail.Text,
                        MobileNumber = txtMobileNo.Text
                    };

                    _customerResult = _customerManager.Search(searchItem).ToList();

                    if (_customerResult.Count == 0)
                    {
                        MessageBox.Show("No items match your search query.");
                        isItemExisting = false;
                    }
                }

                if (IsAddressInfoFilled())
                {
                    Address searchAddress = new Address()
                    {
                        AddressLine = txtSearchStreet.Text,
                        City = txtSearchCity.Text,
                        Country = txtSearchCountry.Text,
                        ZipCode = txtSearchZipCode.Text
                    };

                    _addressResult = _addressManager.Search(searchAddress).ToList();

                    if (_addressResult.Count == 0)
                    {
                        MessageBox.Show("No items match your search query.");
                        isItemExisting = false;
                    }
                  
                }

                var type = cboType.SelectedItem.ToString();

                if (!string.IsNullOrWhiteSpace(cboType.SelectedItem.ToString()))
                {
                    _typeResult = _addressTypeManager.GetByName(type).ToList();

                    if (_typeResult.Count == 0)
                    {
                        MessageBox.Show("No items match your search query.");
                        isItemExisting = false;
                    }
                 
                }

                if (isItemExisting)
                {
                    LoadSearchResults();
                }
                else
                {
                    ListViewCustomers.Items.Clear();
                    LoadCustomers();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CustomerForm_Click(object sender, EventArgs e)
        {
            ListViewCustomers.SelectedItems.Clear();
            btnAdd.Enabled = true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            foreach (TextBox textBox in grpCustomer.Controls.OfType<TextBox>())
            {
                textBox.Text = string.Empty;
            }

            foreach (TextBox textBox in grpAddress.Controls.OfType<TextBox>())
            {
                textBox.Text = string.Empty;
            }

            cboType.SelectedItem = string.Empty;
            ListViewCustomers.Items.Clear();
            _customerResult = new List<Customer>();
            _addressResult = new List<Address>();
            _typeResult = new List<AddressType>();
            LoadCustomers();
        }

        private void LoadSearchResults() 
        {
            try
            {
                ListViewCustomers.Items.Clear();

                if (_customerResult.Count == 0)
                {
                    _customerResult = _customerManager.GetAll().ToList();
                }

                if (_addressResult.Count == 0)
                {
                    _addressResult = _addressManager.GetAll().ToList();
                }

                if (_typeResult.Count == 0)
                {
                    _typeResult = _addressTypeManager.GetAll().ToList();
                }

                var searchList = from c in _customerResult
                                 join t in _typeResult on c.Id equals t.CustomerId
                                 join a in _addressResult on t.AddressId equals a.Id
                                 select new
                                 {
                                     Id = c.Id,
                                     LastName = c.LastName,
                                     FirstName = c.FirstName,
                                     Email = c.Email,
                                     MobileNumber = c.MobileNumber,
                                     AddressString = string.Join(", ", new string[] { a.AddressLine, a.City, a.Country, a.ZipCode }),
                                     TypeName = t.Name
                                 };

                if (searchList.Count() > 0)
                {
                    foreach (var item in searchList)
                    {
                        ListViewCustomers.Items.Add(new ListViewItem(new string[]
                       {
                       item.Id.ToString(),
                       item.LastName,
                       item.FirstName,
                       item.Email,
                       item.MobileNumber,
                       item.AddressString,
                       item.TypeName
                       }));
                    }
                }
                else
                {
                    MessageBox.Show("No items matched your search query.");
                    ListViewCustomers.Items.Clear();
                    LoadCustomers();
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
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
