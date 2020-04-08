﻿using ShoppingCart;
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
    public partial class EditCustomerForm : Form
    {
        private ICustomerManager _customerManager;
        private IAddressManager _addressManager;
        private IAddressTypeManager _typeManager;
        private List<AddressType> _addressTypeList;
        private Address _address;
        private AddressType _addressType;

        public List<AddressType> AddressTypeList
        {
            get { return _addressTypeList; }
            set { _addressTypeList = value; }
        }

        private List<Address> _addressList;
        
        public List<Address> AddressList
        {
            get { return _addressList; }
            set { _addressList = value; }
        }

        private Customer _customer;

        public Customer Customer
        {
            get { return _customer; }
            set { _customer = value; }
        }

 
        public EditCustomerForm()
        {
            _customerManager = new CustomerManager();
            _addressManager = new AddressManager();
            _typeManager = new AddressTypeManager();
            _addressList = new List<Address>();
            _addressTypeList = new List<AddressType>();
            _customer = new Customer();
            _address = new Address();
            _addressType = new AddressType();
            InitializeComponent();
        }

        private bool Filters(Customer customer) 
        {
            return customer.FirstName.ToLower() == txtFirstName.Text.ToLower() && customer.LastName.ToLower() == txtLastName.Text.ToLower();
        }

        private void GetData() 
        {
            try
            {
                if (string.IsNullOrWhiteSpace(lblCustomerId.Text))
                {
                    lblCustomerId.Text = "0";
                }

                _customer = new Customer()
                {
                    Id = Convert.ToInt32(lblCustomerId.Text),
                    LastName = txtLastName.Text,
                    FirstName = txtFirstName.Text,
                    Email = txtEmail.Text,
                    MobileNumber = txtMobileNumber.Text
                };

                if (!string.IsNullOrWhiteSpace(txtShippingStreetLine.Text))
                {
                    if (string.IsNullOrWhiteSpace(lblShippingAddressId.Text))
                    {
                        lblShippingAddressId.Text = "0";
                        Address shippingAddress = new Address()
                        {
                            Id = Convert.ToInt32(lblShippingAddressId.Text),
                            AddressLine = txtShippingStreetLine.Text,
                            City = txtShippingCity.Text,
                            Country = txtShippingCountry.Text,
                            ZipCode = txtShippingZipcode.Text
                        };

                        AddressType addressType = new AddressType() { AddressId = shippingAddress.Id, AddressTypeName = "Shipping Address" };
                        _addressList.Add(shippingAddress);
                        _addressTypeList.Add(addressType);

                    }
                    else if (_addressList.Count > 0)
                    {
                        if (_addressList.FirstOrDefault(x => x.Id == Convert.ToInt32(lblShippingAddressId.Text)) != null)
                        {
                            var address = _addressList.FirstOrDefault(x => x.Id == Convert.ToInt32(lblShippingAddressId.Text));
                            address.AddressLine = txtShippingStreetLine.Text;
                            address.City = txtShippingCity.Text;
                            address.Country = txtShippingCountry.Text;
                            address.ZipCode = txtShippingZipcode.Text;
                        }
                    }

                }

                if (!string.IsNullOrWhiteSpace(txtMailingStreet.Text))
                {
                    if (string.IsNullOrWhiteSpace(lblMailingAddressId.Text))
                    {
                        lblMailingAddressId.Text = "0";

                        Address mailingAddress = new Address()
                        {
                            Id = Convert.ToInt32(lblMailingAddressId.Text),
                            AddressLine = txtMailingStreet.Text,
                            City = txtMailingCity.Text,
                            Country = txtMailingCountry.Text,
                            ZipCode = txtMailingZipcode.Text
                        };

                        AddressType addressType = new AddressType() { AddressId = mailingAddress.Id, AddressTypeName = "Mailing Address" };
                        _addressList.Add(mailingAddress);
                        _addressTypeList.Add(addressType);
                    }
                    else if (_addressList.Count > 0)
                    {
                        if (_addressList.FirstOrDefault(x => x.Id == Convert.ToInt32(lblMailingAddressId.Text)) != null)
                        {
                            var address = _addressList.FirstOrDefault(x => x.Id == Convert.ToInt32(lblMailingAddressId.Text));
                            address.AddressLine = txtMailingStreet.Text;
                            address.City = txtMailingCity.Text;
                            address.Country = txtMailingCountry.Text;
                            address.ZipCode = txtMailingZipcode.Text;
                        }
                    }
                   
                }

                if (!string.IsNullOrWhiteSpace(txtBillingStreet.Text))
                {
                    if (string.IsNullOrWhiteSpace(lblMailingAddressId.Text))
                    {
                        lblMailingAddressId.Text = "0";
                        Address billingAddress = new Address()
                        {
                            Id = Convert.ToInt32(lblBillingAddressId.Text),
                            AddressLine = txtBillingStreet.Text,
                            City = txtBillingCity.Text,
                            Country = txtBillingCountry.Text,
                            ZipCode = txtBillingZipcode.Text
                        };

                        AddressType addressType = new AddressType() { AddressId = billingAddress.Id, AddressTypeName = "Mailing Address" };

                        _addressList.Add(billingAddress);
                        _addressTypeList.Add(addressType);
                    }
                    else if (_addressList.Count > 0)
                    {
                        if (_addressList.FirstOrDefault(x => x.Id == Convert.ToInt32(lblBillingAddressId.Text)) != null)
                        {
                            var address = _addressList.FirstOrDefault(x => x.Id == Convert.ToInt32(lblBillingAddressId.Text));
                            address.AddressLine = txtBillingStreet.Text;
                            address.City = txtBillingCity.Text;
                            address.Country = txtBillingCountry.Text;
                            address.ZipCode = txtBillingZipcode.Text;
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateAllFields())
                {
                    List<int> addressIds = new List<int>();

                    var existingCustomer = _customerManager.GetAll().Where(x => Filters(x));

                    if (existingCustomer.Count() > 0)
                    {
                        MessageBox.Show("Customer exists.");
                    }
                    else
                    {
                        GetData();

                        if (_customerManager.Insert(_customer))
                        {
                            foreach (var address in _addressList)
                            {
                                if (_addressManager.Insert(address))
                                {
                                    address.Id = _addressManager.GetAll().Select(x => x.Id).LastOrDefault();

                                }
                                else
                                {
                                    MessageBox.Show("Address details were not inserted.");
                                }
                            }

                            try
                            {
                                for (int i = 0; i < _addressList.Count; i++)
                                {
                                    _addressTypeList[i].AddressId = _addressList[i].Id;
                                    _addressTypeList[i].CustomerId = _customerManager.GetAll().Select(x => x.Id).LastOrDefault();
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }

                            foreach (var addressType in _addressTypeList)
                            {
                                MessageBox.Show(_typeManager.Insert(addressType) ? "Address details inserted successfully." : "Address type was not inserted.");
                            }

                            MessageBox.Show("Customer details inserted successfully.");
                            _addressList.Clear();
                            _addressTypeList.Clear();
                            _address = new Address();

                            CustomerProfile customerProfile = new CustomerProfile();
                            customerProfile.MdiParent = this.MdiParent;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Customer details were not inserted.");
                            ClearTextBoxes();
                        }


                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ClearTextBoxes();
            }
        }

        private bool ValidateAllFields()
        {
            foreach (TextBox textbox in groupBoxBasicInfo.Controls.OfType<TextBox>().OrderBy(x => x.TabIndex))
            {
                if (string.IsNullOrWhiteSpace(textbox.Text))
                {
                    textbox.Focus();
                    errorProvider.SetError(textbox, "Please fill up this field.");
                    MessageBox.Show($"Please fill up {textbox.Name.Substring(3)}");
                    return false;
                }
                else
                {
                    errorProvider.SetError(textbox, string.Empty);
                }

                if (textbox == txtEmail)
                {
                    if (!txtEmail.Text.Contains("@") || !txtEmail.Text.Contains("."))
                    {
                        errorProvider.SetError(txtEmail, "Invalid email.");
                        MessageBox.Show("Please enter valid email.");
                        return false;
                    }
                    else
                    {
                        errorProvider.SetError(txtEmail, string.Empty);
                    }
                }
                
                if (textbox == txtFirstName || textbox == txtLastName)
                {
                    if (txtFirstName.Text.Any(x => char.IsDigit(x)))
                    {
                        errorProvider.SetError(txtFirstName, "Name contains number.");
                        MessageBox.Show("Please enter valid name.");
                        return false;
                    }
                    if (txtLastName.Text.Any(y => char.IsDigit(y)))
                    {
                        errorProvider.SetError(txtLastName, "Name contains number.");
                        MessageBox.Show("Please enter valid name.");
                        return false;
                    }
                    else
                    {
                        errorProvider.SetError(txtFirstName, string.Empty);
                        errorProvider.SetError(txtLastName, string.Empty);
                    }
                }
            }

            return true;
        }

        private void ClearTextBoxes()
        {
            foreach (TextBox textbox in this.Controls.OfType<TextBox>())
            {
                textbox.Clear();
            }
          
        }

        private void EditCustomerForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (_customer != null && _customer.Id > 0)
                {
                    lblCustomerIdName.Visible = true;
                    lblCustomerId.Visible = true;
                    lblCustomerId.Text = _customer.Id.ToString();
                    txtFirstName.Text = _customer.FirstName;
                    txtLastName.Text = _customer.LastName;
                    txtEmail.Text = _customer.Email;
                    txtMobileNumber.Text = _customer.MobileNumber;
                }
                else
                {
                    btnSubmit.Visible = true;
                    btnEdit.Visible = false;

                }

                if (_addressList.Count > 0 && _addressTypeList.Count > 0)
                {
                    btnSubmit.Visible = false;
                    btnEdit.Visible = true;
                    this.Text = "Edit Customer";

                    foreach (var addressTypeItem in _addressTypeList)
                    {
                        var address = _addressList.FirstOrDefault(x => x.Id == addressTypeItem.AddressId);

                        if (addressTypeItem.AddressTypeName == "Shipping Address")
                        {
                            tabControlAddress.SelectedTab = tabPage1;
                            lblShippingAddressId.Text = address.Id.ToString();
                            lblShippingAddressId.Visible = true;
                            lblShippingIdName.Visible = true;
                            txtShippingStreetLine.Text = address.AddressLine;
                            txtShippingCity.Text = address.City;
                            txtShippingCountry.Text = address.Country;
                            txtShippingZipcode.Text = address.ZipCode;

                        }
                        else if (addressTypeItem.AddressTypeName == "Mailing Address")
                        {
                            tabControlAddress.SelectedTab = tabPage2;
                            lblMailingAddressId.Text = address.Id.ToString();
                            lblMailingAddressId.Visible = true;
                            lblMailingIdName.Visible = true;
                            txtMailingStreet.Text = address.AddressLine;
                            txtMailingCity.Text = address.City;
                            txtMailingCountry.Text = address.Country;
                            txtMailingZipcode.Text = address.ZipCode;


                        }
                        else if (addressTypeItem.AddressTypeName == "Billing Address")
                        {
                            tabControlAddress.SelectedTab = tabPage3;
                            lblBillingAddressId.Text = address.Id.ToString();
                            lblBillingAddressId.Visible = true;
                            lblBillingIdName.Visible = true;
                            txtBillingStreet.Text = address.AddressLine;
                            txtBillingCity.Text = address.City;
                            txtBillingCountry.Text = address.Country;
                            txtBillingZipcode.Text = address.ZipCode;

                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           

        }
        
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateAllFields())
                {
                    List<int> existingId = new List<int>();
                    List<Address> addressList = new List<Address>();
                    Address address = new Address();

                    GetData();

                    foreach (var item in _addressTypeList)
                    {
                        address = _addressList.FirstOrDefault(x => x.Id == item.AddressId);

                        if (item.AddressTypeName == "Shipping Address")
                        {
                            address.Id = Convert.ToInt32(lblShippingAddressId.Text);
                            address.AddressLine = txtShippingStreetLine.Text;
                            address.City = txtShippingCity.Text;
                            address.Country = txtShippingCountry.Text;
                            address.ZipCode = txtShippingZipcode.Text;
                        }
                        else if (item.AddressTypeName == "Mailing Address")
                        {
                            address.Id = Convert.ToInt32(lblMailingAddressId.Text);
                            address.AddressLine = txtMailingStreet.Text;
                            address.City = txtMailingCity.Text;
                            address.Country = txtMailingCountry.Text;
                            address.ZipCode = txtMailingZipcode.Text;
                        }
                        else if (item.AddressTypeName == "Billing Address")
                        {
                            address.Id = Convert.ToInt32(lblBillingAddressId.Text);
                            address.AddressLine = txtBillingStreet.Text;
                            address.City = txtBillingCity.Text;
                            address.Country = txtBillingCountry.Text;
                            address.ZipCode = txtBillingZipcode.Text;
                        }

                        addressList.Add(address);
                    }

                    if (_customerManager.Update(_customer))
                    {
                        MessageBox.Show("Customer details updated successfully.");

                        foreach (var item in addressList)
                        {
                            if (item.Id == 0)
                            {
                                MessageBox.Show(_addressManager.Insert(item) ? "Address details added successfully." : "Address details were not added.");

                                item.Id = _addressManager.GetAll().Select(x => x.Id).LastOrDefault();
                            }
                            else
                            {
                                MessageBox.Show(_addressManager.Update(item) ? "Address details updated successfully." : "Address details were not updated.");

                            }

                        }

                        try
                        {
                            for (int i = 0; i < _addressList.Count; i++)
                            {
                                if (_addressTypeList[i].AddressId == _addressList[i].Id)
                                {
                                    existingId.Add(_addressTypeList[i].AddressId);
                                }
                                else
                                {
                                    _addressTypeList[i].AddressId = _addressList[i].Id;
                                }

                                _addressTypeList[i].CustomerId = _customer.Id;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                        foreach (var addressType in _addressTypeList)
                        {
                            if (existingId.Where(x => x == addressType.AddressId).Count() == 0)
                            {
                                MessageBox.Show(_typeManager.Insert(addressType) ? "Address type inserted successfully." : "Address type was not inserted.");
                            }

                        }

                        _addressList.Clear();
                        _addressTypeList.Clear();
                        _address = new Address();

                        CustomerProfile customerProfile = new CustomerProfile();
                        customerProfile.MdiParent = this.MdiParent;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Customer details were not updated.");
                        ClearTextBoxes();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ClearTextBoxes();
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            foreach (TextBox textbox in groupBoxBasicInfo.Controls.OfType<TextBox>()) 
            {
                textbox.Clear();
            }
        }
    }
}
