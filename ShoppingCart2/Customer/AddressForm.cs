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
    public partial class AddressForm : Form
    {
        public static Address address;
        public static string addressType;
        private IAddressManager _addressManager;
        private IAddressTypeManager _addressTypeManager;
        public AddressForm()
        {
            address = new Address();
            _addressTypeManager = new AddressTypeManager();
            _addressManager = new AddressManager();
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            
            if (ValidateAllFields())
            {
                string streetLine = txtStreetLine.Text;
                string city = txtCity.Text;
                string country = txtCountry.Text;
                string zipcode = txtZipcode.Text;
                addressType = cboAddressType.Text;

                if (string.IsNullOrWhiteSpace(lblAddressId.Text))
                {
                    address = new Address() { AddressLine = streetLine, City = city, Country = country, ZipCode = zipcode };
                }
                else
                {
                    int id = Convert.ToInt32(lblAddressId.Text);
                    address = new Address() { Id = id, AddressLine = streetLine, City = city, Country = country, ZipCode = zipcode };
                }


                EditCustomerForm editCustomerForm = new EditCustomerForm();
                editCustomerForm.MdiParent = this.MdiParent;
                this.Close();
            }
            else
            {
                ClearTextBoxes();
            }
        }

        private void ClearTextBoxes()
        {
            foreach (TextBox textbox in this.Controls.OfType<TextBox>())
            {
                textbox.Clear();
            }

        }
        private bool ValidateAllFields()
        {
            foreach (TextBox textbox in this.Controls.OfType<TextBox>())
            {
                if (string.IsNullOrWhiteSpace(textbox.Text))
                {
                    textbox.Focus();
                    MessageBox.Show($"Please fill up {textbox.Name.Substring(3)}");
                    return false;
                }
            }

            return true;
        }

        private void AddressForm_Load(object sender, EventArgs e)
        {
        }

        private void AddressForm_Activated(object sender, EventArgs e)
        {
            if (CustomerForm.customer.Id > 0)
            {
                lblAddressId.Visible = true;
                label1.Visible = true;
            }

            if (EditCustomerForm.address.Id > 0)
            {
                lblAddressId.Visible = true;
                lblAddressId.Text = EditCustomerForm.address.Id.ToString();
                label1.Visible = true;
                label1.Text = "Address Id";
                txtStreetLine.Text = EditCustomerForm.address.AddressLine;
                txtCity.Text = EditCustomerForm.address.City;
                txtCountry.Text = EditCustomerForm.address.Country;
                txtZipcode.Text = EditCustomerForm.address.ZipCode;
                btnAdd.Text = "Edit Address";
            }
            else 
            {
                lblAddressId.Visible = false;
                label1.Visible = false;
            }

            if (addressType != null)
            {
                cboAddressType.SelectedItem = addressType;
            }
        }
    }
}
