using ShoppingCart;
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
        private List<Address> _addressList;
       private List<AddressType> _addressTypeList;
        public static Address address;
        public static Customer customer;
 
        public EditCustomerForm()
        {
            _customerManager = new CustomerManager();
            _addressManager = new AddressManager();
            _typeManager = new AddressTypeManager();
            _addressList = new List<Address>();
            _addressTypeList = new List<AddressType>();
            address = new Address();
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateAllFields())
                {
                    List<int> addressIds = new List<int>();

                    var existingCustomer = _customerManager.GetAll().Where(x => x.FirstName.ToLower() == txtFirstName.Text.ToLower() && x.LastName.ToLower() == txtLastName.Text.ToLower());

                    if (existingCustomer.Count() > 0)
                    {
                        MessageBox.Show("Customer exists.");
                    }
                    else
                    {
                        customer = new Customer()
                        {
                            LastName = txtLastName.Text,
                            FirstName = txtFirstName.Text,
                            Email = txtEmail.Text,
                            MobileNumber = txtMobileNumber.Text
                        };

                        if (_addressList.Count > 0 && _addressTypeList.Count > 0)
                        {
                            if (_customerManager.Insert(customer))
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
                                AddressForm.address = new Address();

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
                        else
                        {
                            MessageBox.Show("Please add Address info");
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


        private void EditCustomerForm_Activated(object sender, EventArgs e)
        {
            if (customer != null)
            {
                lblCustomerId.Text = customer.Id.ToString();
                txtFirstName.Text = customer.FirstName;
                txtLastName.Text = customer.LastName;
                txtEmail.Text = customer.Email;
                txtMobileNumber.Text = customer.MobileNumber;
            }

            if (AddressForm.address != null && !string.IsNullOrWhiteSpace(AddressForm.addressType) && AddressForm.address.AddressLine != null)
            {
                if (AddressForm.addressType == "Shipping Address")
                {
                    tabControlAddress.SelectedTab = tabPage1;
                    lblShippingAddressId.Text = AddressForm.address.Id.ToString();
                    lblShippingAddressId.Visible = true;
                    lblShippingAddressStreetLine.Text = AddressForm.address.AddressLine;
                    lblShippingAddressCity.Text = AddressForm.address.City;
                    lblShippingAddressCountry.Text = AddressForm.address.Country;
                    lblShippingAddressZipCode.Text = AddressForm.address.ZipCode;
                    btnAddAddress.Text = "Edit Address";
                    AddressType addressType = new AddressType() { AddressId = AddressForm.address.Id, AddressTypeName = AddressForm.addressType };
                    _addressTypeList.Add(addressType);
                }
                if (AddressForm.addressType == "Mailing Address")
                {
                    tabControlAddress.SelectedTab = tabPage2;
                    lblMailingAddressId.Text = AddressForm.address.Id.ToString();
                    lblMailingAddressId.Visible = true;
                    lblMailingAddressStreetLine.Text = AddressForm.address.AddressLine;
                    lblMailingAddressCity.Text = AddressForm.address.City;
                    lblMailingAddressCountry.Text = AddressForm.address.Country;
                    lblMailingAddressZipCode.Text = AddressForm.address.ZipCode;
                    btnAddAddress.Text = "Edit Address";
                    AddressType addressType = new AddressType() { AddressId = AddressForm.address.Id, AddressTypeName = AddressForm.addressType };
                    _addressTypeList.Add(addressType);
                }
                if (AddressForm.addressType == "Billing Address")
                {
                    tabControlAddress.SelectedTab = tabPage3;
                    lblBillingAddressId.Text = AddressForm.address.Id.ToString();
                    lblBillingAddressId.Visible = true;
                    lblBillingAddressStreetLine.Text = AddressForm.address.AddressLine;
                    lblBillingAddressCity.Text = AddressForm.address.City;
                    lblBillingAddressCountry.Text = AddressForm.address.Country;
                    lblBillingAddressZipCode.Text = AddressForm.address.ZipCode;
                    btnAddAddress.Text = "Edit Address";
                    AddressType addressType = new AddressType() { AddressId = AddressForm.address.Id, AddressTypeName = AddressForm.addressType };
                    _addressTypeList.Add(addressType);
                }

                _addressList.Add(AddressForm.address);

            }

        }

        private void EditCustomerForm_Load(object sender, EventArgs e)
        {
            if (CustomerForm.customer != null && CustomerForm.customer.Id > 0)
            {
                lblCustomerId.Text = CustomerForm.customer.Id.ToString();
                txtFirstName.Text = CustomerForm.customer.FirstName;
                txtLastName.Text = CustomerForm.customer.LastName;
                txtEmail.Text = CustomerForm.customer.Email;
                txtMobileNumber.Text = CustomerForm.customer.MobileNumber;
                this.Text = "Edit Customer Details";
                btnSubmit.Enabled = false;
                btnEdit.Enabled = true;
            }

            if (CustomerForm.addressTypeList.Count() > 0)
            {
                foreach (var addressTypeItem in CustomerForm.addressTypeList)
                {
                    Address address = CustomerForm.addressList.FirstOrDefault(x => x.Id == addressTypeItem.AddressId);

                    if (addressTypeItem.AddressTypeName == "Shipping Address")
                    {
                        tabControlAddress.SelectedTab = tabPage1;
                        lblShippingAddressId.Text = address.Id.ToString();
                        lblShippingAddressId.Visible = true;
                        lblShippingAddressStreetLine.Text = address.AddressLine;
                        lblShippingAddressCity.Text = address.City;
                        lblShippingAddressCountry.Text = address.Country;
                        lblShippingAddressZipCode.Text = address.ZipCode;
                        btnAddAddress.Text = "Edit Address";
                    }

                    if (addressTypeItem.AddressTypeName == "Mailing Address")
                    {
                        tabControlAddress.SelectedTab = tabPage2;
                        lblMailingAddressId.Text = address.Id.ToString();
                        lblMailingAddressId.Visible = true;
                        lblMailingAddressStreetLine.Text = address.AddressLine;
                        lblMailingAddressCity.Text = address.City;
                        lblMailingAddressCountry.Text = address.Country;
                        lblMailingAddressZipCode.Text = address.ZipCode;
                        btnAddAddress.Text = "Edit Address";
                    }

                    if (addressTypeItem.AddressTypeName == "Billing Address")
                    {
                        tabControlAddress.SelectedTab = tabPage3;
                        lblBillingAddressId.Text = address.Id.ToString();
                        lblBillingAddressId.Visible = true;
                        lblBillingAddressStreetLine.Text = address.AddressLine;
                        lblBillingAddressCity.Text = address.City;
                        lblBillingAddressCountry.Text = address.Country;
                        lblBillingAddressZipCode.Text = address.ZipCode;
                        btnAddAddress.Text = "Edit Address";
                    }

                }

            }

        }

        private void btnAddAddress_Click(object sender, EventArgs e)
        {
            customer = new Customer()
            {
                LastName = txtLastName.Text,
                FirstName = txtFirstName.Text,
                Email = txtEmail.Text,
                MobileNumber = txtMobileNumber.Text
            };

            if (string.IsNullOrWhiteSpace(lblCustomerId.Text))
            {
                customer.Id = 0;
            }
            else
            {
                customer.Id = Convert.ToInt32(lblCustomerId.Text);
            }

          

            string activeAddressType = tabControlAddress.SelectedTab.Text;
            AddressForm.addressType = activeAddressType;
           
            if (btnAddAddress.Text == "Edit Address" || address.Id > 0)
            {
                if (activeAddressType != null)
                {
                    if (activeAddressType == "Shipping Address")
                    {
                        address = new Address()
                        {
                            Id = Convert.ToInt32(lblShippingAddressId.Text),
                            AddressLine = lblShippingAddressStreetLine.Text,
                            City = lblShippingAddressCity.Text,
                            Country = lblShippingAddressCountry.Text,
                            ZipCode = lblShippingAddressZipCode.Text
                        };
                    }
                    if (activeAddressType == "Mailing Address")
                    {
                        address = new Address()
                        {
                            Id = Convert.ToInt32(lblMailingAddressId.Text),
                            AddressLine = lblMailingAddressStreetLine.Text,
                            City = lblMailingAddressCity.Text,
                            Country = lblMailingAddressCountry.Text,
                            ZipCode = lblMailingAddressZipCode.Text
                        };
                    }
                    if (activeAddressType == "Billing Address")
                    {
                        address = new Address()
                        {
                            Id = Convert.ToInt32(lblBillingAddressId.Text),
                            AddressLine = lblBillingAddressStreetLine.Text,
                            City = lblBillingAddressCity.Text,
                            Country = lblBillingAddressCountry.Text,
                            ZipCode = lblBillingAddressZipCode.Text
                        };
                    }
                }

            }

            AddressForm.addressType = activeAddressType;
            AddressForm addressForm = new AddressForm();
            addressForm.MdiParent = this.MdiParent;
            addressForm.Show();

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateAllFields())
                {
                    List<int> existingId = new List<int>();

                    customer = new Customer() 
                    { 
                        Id = Convert.ToInt32(lblCustomerId.Text), 
                        LastName = txtLastName.Text, 
                        FirstName = txtFirstName.Text, 
                        Email = txtEmail.Text, 
                        MobileNumber = txtMobileNumber.Text
                    };

                    if (_customerManager.Update(customer))
                    {
                        MessageBox.Show("Customer details updated successfully.");

                        if (_addressList.Count > 0 && _addressTypeList.Count > 0)
                        {
                            foreach (var address in _addressList)
                            {
                                if (address.Id == 0)
                                {
                                    MessageBox.Show(_addressManager.Insert(address) ? "Address details added successfully." : "Address details were not added.");
                                    address.Id = _addressManager.GetAll().Select(x => x.Id).LastOrDefault();
                                }
                                else
                                {
                                    MessageBox.Show(_addressManager.Update(address) ? "Address details updated successfully." : "Address details were not updated.");

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

                                    _addressTypeList[i].CustomerId = customer.Id;
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
                        }

                        _addressList.Clear();
                        _addressTypeList.Clear();
                        AddressForm.address = new Address();

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

        private void tabControlAddress_Click(object sender, EventArgs e)
        {
            if (tabControlAddress.SelectedTab.Text == "Shipping Address")
            {
                btnAddAddress.Text = lblShippingAddressId.Visible == true ? "Edit Address" : "Add Address";

            }
            else if (tabControlAddress.SelectedTab.Text == "Mailing Address")
            {
                btnAddAddress.Text = lblMailingAddressId.Visible == true ? "Edit Address" : "Add Address";
            }
            else if (tabControlAddress.SelectedTab.Text == "Billing Address")
            {
                btnAddAddress.Text = lblBillingAddressId.Visible == true ? "Edit Address" : "Add Address";
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
