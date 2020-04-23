using ShoppingCart;
using ShoppingCart.BL.Managers;
using ShoppingCart.BL.Managers.Interfaces;
using ShoppingCart.BL.Models;
using ShoppingCart.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
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
        private string[] _input;
        private string[] _shippingAddress;
        private string[] _mailingAddress;
        private string[] _billingAddress;

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

        private bool _isNew = true;

        private bool _isSuccessful;

        public bool IsSuccessful
        {
            get { return _isSuccessful; }
            set { _isSuccessful = value; }
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

        private bool Insert() 
        {
            try
            {
                if (ValidateAllFields())
                {
                    List<int> addressIds = new List<int>();

                    var IsExisting = _customerManager.ItemExist(txtFirstName.Text, txtLastName.Text);

                    if (IsExisting)
                    {
                        MessageBox.Show("Customer exists.");
                        return false;
                    }
                    else
                    {
                        GetData();

                        try
                        {
                            using (var scope = new TransactionScope())
                            {
                                if (_addressList.Count > 0)
                                {
                                    int customerId = _customerManager.Insert(_customer);

                                    if (customerId > 0)
                                    {
                                        foreach (var address in _addressList)
                                        {
                                            int addressId = _addressManager.Insert(address);

                                            if (addressId > 0)
                                            {
                                                address.Id = addressId;
                                            }
                                            else
                                            {
                                                MessageBox.Show("Address details were not inserted.");
                                                return false;
                                            }
                                        }

                                        try
                                        {
                                            for (int i = 0; i < _addressList.Count; i++)
                                            {
                                                _addressTypeList[i].AddressId = _addressList[i].Id;
                                                _addressTypeList[i].CustomerId = customerId;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.Message);
                                        }

                                        foreach (var addressType in _addressTypeList)
                                        {
                                            if (_typeManager.Insert(addressType))
                                            {
                                                MessageBox.Show("Address details inserted successfully.");
                                            }
                                            else
                                            {
                                                MessageBox.Show("Address type was not inserted.");
                                                return false;
                                            }

                                        }

                                        if (MessageBox.Show("Customer details inserted successfully.") == DialogResult.OK)
                                        {
                                            _addressList.Clear();
                                            _addressTypeList.Clear();
                                            _address = new Address();
                                        }

                                        CustomerProfile customerProfile = new CustomerProfile();
                                        customerProfile.MdiParent = this.MdiParent;
                                        this.Close();

                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Customer details were not inserted.");
                                    ClearTextBoxes();
                                    return false;
                                }

                                scope.Complete();
                            }

                        }
                        catch (TransactionAbortedException ex)
                        {
                            MessageBox.Show(ex.Message);
                            return false;
                        }
                        
                    }

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ClearTextBoxes();
            }

            return true;
        }

        private bool ValidateAllFields()
        {
            foreach (TextBox textbox in groupBoxBasicInfo.Controls.OfType<TextBox>().OrderBy(x => x.TabIndex))
            {
                if (string.IsNullOrWhiteSpace(textbox.Text))
                {
                    textbox.Focus();
                    errorProvider.SetError(textbox, $"Please fill up {textbox.Name.Substring(3)}.");
                    return false;
                }
                else
                {
                    errorProvider.SetError(textbox, string.Empty);
                }

                if (textbox == txtEmail)
                {
                    string[] split = txtEmail.Text.Split('@');
                    bool valid = split.Length > 1 && split.LastOrDefault().Contains('.');

                    if (!valid)
                    {
                        errorProvider.SetError(txtEmail, "Please enter valid email.");
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
                        errorProvider.SetError(txtFirstName, "Name contains number. Please enter valid name.");
                        return false;
                    }
                    if (txtLastName.Text.Any(y => char.IsDigit(y)))
                    {
                        errorProvider.SetError(txtLastName, "Name contains number. Please enter valid name.");
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
                    if (_input != null)
                    {
                        var textboxes = groupBoxBasicInfo.Controls.OfType<TextBox>().OrderBy(x => x.TabIndex);

                        for (int i = 0; i < _input.Length; i++)
                        {
                            textboxes.ElementAt(i).Text = _input[i];
                        }

                    }
                    else
                    {
                        txtFirstName.Text = _customer.FirstName;
                        txtLastName.Text = _customer.LastName;
                        txtEmail.Text = _customer.Email;
                        txtMobileNumber.Text = _customer.MobileNumber;
                    }

                    lblCustomerIdName.Visible = true;
                    lblCustomerId.Visible = true;
                    lblCustomerId.Text = _customer.Id.ToString();
                    _isNew = false;
                }

                btnOK.Text = _isNew ? "Submit" : "Edit";
                this.Text = _isNew ? "Add Customer" : "Edit Customer";

                if (_addressList.Count > 0 && _addressTypeList.Count > 0)
                {
                    if (_shippingAddress != null)
                    {
                        var addressTextBoxes = tabPage1.Controls.OfType<TextBox>().OrderBy(x => x.TabIndex);

                        for (int i = 0; i < _shippingAddress.Length; i++)
                        {
                            addressTextBoxes.ElementAt(i).Text = _shippingAddress[i];
                        }

                    }
                    if (_mailingAddress != null)
                    {
                        var addressTextBoxes = tabPage2.Controls.OfType<TextBox>().OrderBy(x => x.TabIndex);

                        for (int i = 0; i < _mailingAddress.Length; i++)
                        {
                            addressTextBoxes.ElementAt(i).Text = _mailingAddress[i];
                        }

                    }
                    if (_billingAddress != null)
                    {
                        var addressTextBoxes = tabPage3.Controls.OfType<TextBox>().OrderBy(x => x.TabIndex);

                        for (int i = 0; i < _billingAddress.Length; i++)
                        {
                            addressTextBoxes.ElementAt(i).Text = _billingAddress[i];
                        }

                    }
                    else
                    {
                        foreach (var addressTypeItem in _addressTypeList)
                        {
                            var address = _addressList.FirstOrDefault(x => x.Id == addressTypeItem.AddressId);

                            if (addressTypeItem.Name == "Shipping Address")
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
                            else if (addressTypeItem.Name == "Mailing Address")
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
                            else if (addressTypeItem.Name == "Billing Address")
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
               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }
        
        private bool Edit() 
        {
            try
            {
                _input = new string[] { txtFirstName.Text, txtLastName.Text, txtEmail.Text, txtMobileNumber.Text};

                _shippingAddress = new string[] { txtShippingStreetLine.Text, txtShippingCity.Text, txtShippingCountry.Text, txtShippingZipcode.Text};
                _mailingAddress = new string[] { txtMailingStreet.Text, txtMailingCity.Text, txtMailingCountry.Text, txtMailingZipcode.Text};
                _billingAddress = new string[] { txtBillingStreet.Text, txtBillingCity.Text, txtBillingCountry.Text, txtBillingZipcode.Text };

                if (ValidateAllFields())
                {
                    List<int> existingId = new List<int>();

                    GetData();

                    using (var scope = new TransactionScope())
                    {
                        if (_customerManager.Update(_customer))
                        {

                            foreach (var item in _addressList)
                            {
                                if (item.Id == 0)
                                {
                                    int id = _addressManager.Insert(item);

                                    if (id > 0)
                                    {
                                        MessageBox.Show("Address details added successfully.");
                                    }
                                    else
                                    {
                                        MessageBox.Show("Address details were not added.");
                                        return false;
                                    }

                                    item.Id = id;
                                }
                                else
                                {
                                    if (_addressManager.Update(item))
                                    {
                                        MessageBox.Show("Address details updated successfully.");
                                    }
                                    else
                                    {
                                        MessageBox.Show("Address details were not updated.");
                                        return false;
                                    }
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
                                    if (_typeManager.Insert(addressType))
                                    {
                                        MessageBox.Show("Address type inserted successfully.");
                                    }
                                    else
                                    {
                                        MessageBox.Show("Address type was not inserted.");
                                        return false;
                                    }
                                }

                            }

                            if (MessageBox.Show("Customer details updated successfully.") == DialogResult.OK)
                            {
                                _addressList.Clear();
                                _addressTypeList.Clear();
                                _address = new Address();

                                CustomerProfile customerProfile = new CustomerProfile();
                                customerProfile.MdiParent = this.MdiParent;
                                this.Close();
                            }

                        }
                        else
                        {
                            MessageBox.Show("Customer details were not updated.");
                            ClearTextBoxes();
                            return false;
                        }

                        scope.Complete();
                    }
                  

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ClearTextBoxes();
                return false;
                
            }

            return true;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            foreach (TextBox textbox in groupBoxBasicInfo.Controls.OfType<TextBox>()) 
            {
                textbox.Clear();
            }
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
                    Id = lblCustomerId.Text.ToInt(),
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
                            Id = lblShippingAddressId.Text.ToInt(),
                            AddressLine = txtShippingStreetLine.Text,
                            City = txtShippingCity.Text,
                            Country = txtShippingCountry.Text,
                            ZipCode = txtShippingZipcode.Text
                        };

                        AddressType addressType = new AddressType() { AddressId = shippingAddress.Id, Name = "Shipping Address" };
                        _addressList.Add(shippingAddress);
                        _addressTypeList.Add(addressType);

                    }
                    else if (_addressList.Count > 0)
                    {
                        if (_addressList.FirstOrDefault(x => x.Id == lblShippingAddressId.Text.ToInt()) != null)
                        {
                            var address = _addressList.FirstOrDefault(x => x.Id == lblShippingAddressId.Text.ToInt());
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
                            Id = lblMailingAddressId.Text.ToInt(),
                            AddressLine = txtMailingStreet.Text,
                            City = txtMailingCity.Text,
                            Country = txtMailingCountry.Text,
                            ZipCode = txtMailingZipcode.Text
                        };

                        AddressType addressType = new AddressType() { AddressId = mailingAddress.Id, Name = "Mailing Address" };
                        _addressList.Add(mailingAddress);
                        _addressTypeList.Add(addressType);
                    }
                    else if (_addressList.Count > 0)
                    {
                        if (_addressList.FirstOrDefault(x => x.Id == lblMailingAddressId.Text.ToInt()) != null)
                        {
                            var address = _addressList.FirstOrDefault(x => x.Id == lblMailingAddressId.Text.ToInt());
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
                            Id = lblBillingAddressId.Text.ToInt(),
                            AddressLine = txtBillingStreet.Text,
                            City = txtBillingCity.Text,
                            Country = txtBillingCountry.Text,
                            ZipCode = txtBillingZipcode.Text
                        };

                        AddressType addressType = new AddressType() { AddressId = billingAddress.Id, Name = "Mailing Address" };

                        _addressList.Add(billingAddress);
                        _addressTypeList.Add(addressType);
                    }
                    else if (_addressList.Count > 0)
                    {
                        if (_addressList.FirstOrDefault(x => x.Id == lblBillingAddressId.Text.ToInt()) != null)
                        {
                            var address = _addressList.FirstOrDefault(x => x.Id == lblBillingAddressId.Text.ToInt());
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            _isSuccessful = _isNew ? Insert() : Edit();
        }

    }
}
