using ShoppingCart;
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
    public partial class EditProductForm : Form
    {
        private IProductManager _manager;
        private Product _product;

        public Product Product
        {
            get { return _product; }
            set { _product = value; }
        }

        public EditProductForm()
        {
            _manager = new ProductManager();
            _product = new Product();
            InitializeComponent();
        }

        private bool _isValid;

        public bool IsValid
        {
            get { return _isValid; }
            set { _isValid = value; }
        }


        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateAllFields())
                {
                    string name = txtName.Text;
                    float price = txtPrice.Text.ToFloat();
                    string description = txtDescription.Text;
                    int stock = txtStock.Text.ToInt();

                    _product = new Product() { Name = name, Price = price, Description = description, Stock = stock };

                    MessageBox.Show(_manager.Insert(_product) > 0 ? "Details inserted successfully." : "Details were not inserted.");
                    _isValid = true;
                    ProductForm productForm = new ProductForm();
                    this.Close();
                }
                else
                {
                    _isValid = false;
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateAllFields())
                {
                    int id = lblId.Text.ToInt();
                    string name = txtName.Text;
                    float price = txtPrice.Text.ToFloat();
                    string description = txtDescription.Text;
                    int stock = txtStock.Text.ToInt();

                    _product = new Product() { Id = id, Name = name, Price = price, Description = description, Stock = stock };

                    DialogResult dialog = MessageBox.Show(_manager.Update(_product)? "Details updated successfully." : "Details were not updated.");

                    if (dialog == DialogResult.OK)
                    {
                        ProductForm productForm = new ProductForm();
                        this.Close();
                    }
                    
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
        }
        private void ClearTextBoxes()
        {
            txtName.Clear();
            txtPrice.Clear();
            txtDescription.Clear();
            txtStock.Clear();
            btnAdd.Visible = true;
            btnSave.Visible = false;
        }

        private bool ValidateAllFields() 
        {
            foreach (TextBox textbox in Controls.OfType<TextBox>())
            {
                if (string.IsNullOrWhiteSpace(textbox.Text))
                {
                    textbox.Focus();
                    errorProviderName.SetError(textbox, $"Please fill up this field {textbox.Name.Substring(3)}.");
                    return false;
                }
                else
                {
                    errorProviderName.SetError(textbox, string.Empty);
                }

                if (textbox == txtName)
                {
                    if (txtName.Text.Any(x => char.IsDigit(x)))
                    {
                        errorProviderName.SetError(txtName, "Name contains number. Please enter valid name.");
                        return false;
                    }
                    else
                    {
                        errorProviderName.SetError(txtName, string.Empty);
                    }
                }

                if (textbox == txtPrice)
                {
                    if (txtPrice.Text.Any(x => char.IsLetter(x)))
                    {
                        errorProviderName.SetError(txtPrice, "Price is invalid. Please enter valid price.");
                        return false;
                    }
                    else
                    {
                        errorProviderName.SetError(txtPrice, string.Empty);
                    }
                }

                if (textbox == txtStock)
                {
                    if (txtStock.Text.Any(x => char.IsLetter(x)) || txtStock.Text.Any(x => char.IsPunctuation(x)))
                    {
                        errorProviderName.SetError(txtStock, "Stock is invalid. Please enter valid stock.");
                        return false;
                    }
                    else
                    {
                        errorProviderName.SetError(txtStock, string.Empty);
                    }
                }

            }

            return true;
        }

        private void EditProductForm_Load(object sender, EventArgs e)
        {
            if (_product.Id != 0)
            {
                lblId.Text = _product.Id.ToString();
                txtName.Text = _product.Name;
                txtPrice.Text = _product.Price.ToString("0.00");
                txtDescription.Text = _product.Description;
                txtStock.Text = _product.Stock.ToString();
                btnAdd.Visible = false;
            }
            else
            {
                lblIdName.Visible = false;
                lblId.Visible = false;
                btnSave.Visible = false;
            }

        }

        private void ValidateTextBox(object sender, CancelEventArgs e)
        {
            TextBox textbox = (TextBox)sender;

            if (string.IsNullOrWhiteSpace(textbox.Text))
            {
                errorProviderName.SetError(textbox, $"Please fill up {textbox.Name.Substring(3)}.");
            }
            else
            {
                errorProviderName.SetError(textbox, string.Empty);
            }
        }
    }
}
