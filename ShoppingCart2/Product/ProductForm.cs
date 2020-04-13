using ShoppingCart.BL.Managers;
using ShoppingCart.BL.Managers.Interfaces;
using ShoppingCart.BL.Models;
using ShoppingCart.Utilities;
using ShoppingCart2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShoppingCart
{
    public partial class ProductForm : Form
    {
        private IProductManager _manager;
        private Product _product;
   
        public ProductForm()
        {
            _manager = new ProductManager();
          
            InitializeComponent();
        }

        private void Products_Load(object sender, EventArgs e)
        {
            LoadListViewItems();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            EditProductForm editProductForm = new EditProductForm();
            editProductForm.Text = "Add Product";

            if (editProductForm.ShowDialog() == DialogResult.OK)
            {
                while (editProductForm.IsValid == false)
                {
                    editProductForm.ShowDialog();
                }

                if (editProductForm.IsValid == true)
                {
                    ListViewProducts.Items.Clear();
                    LoadListViewItems();
                    btnAdd.Enabled = true;
                    btnDelete.Enabled = false;
                }
               
            }
        }
        private void btnView_Click(object sender, EventArgs e)
        {
            ListViewProducts.Items.Clear();
            txtSearch.Text = string.Empty;
            LoadListViewItems();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<int> ids = new List<int>();

            foreach (ListViewItem item in ListViewProducts.SelectedItems)
            {
                ids.Add(Convert.ToInt32(item.SubItems[0].Text));
            }

            if (_manager.Delete(ids.ToArray()))
            {
                MessageBox.Show("Details deleted successfully.");
                ListViewProducts.Items.Clear();
                LoadListViewItems();
                btnAdd.Enabled = true;
                btnDelete.Enabled = false;
            }
            else
            {
                MessageBox.Show("Details were not deleted.");
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
                        Product result = _manager.GetById(id);

                        if (result == null)
                        {
                            MessageBox.Show("Item doesn't exist");
                            txtSearch.Clear();
                        }
                        else
                        {
                            ListViewItem resultItem = new ListViewItem(new string[] 
                            { 
                                result.Id.ToString(), 
                                result.Name, 
                                result.Price.ToString("0.00"), 
                                result.Description, 
                                result.Stock.ToString() 
                            });

                            ListViewProducts.Items.Clear();
                            ListViewProducts.Items.Add(resultItem);
                        }
                    }
                    else
                    {
                        string searchByName = txtSearch.Text.ToLower();
                        IList<Product> resultList = new List<Product>();
                        resultList = _manager.GetByName(searchByName);

                        if (resultList.Count == 0)
                        {
                            MessageBox.Show("Item doesn't exist");
                            txtSearch.Clear();
                        }
                        else
                        {
                            ListViewProducts.Items.Clear();
                            ListViewProducts.Items.AddRange(resultList.Select(x => new ListViewItem(new string[] 
                            { 
                                x.Id.ToString(), 
                                x.Name, 
                                x.Price.ToString("0.00"), 
                                x.Description, 
                                x.Stock.ToString() 
                            })).ToArray());
                        }
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

        private void ListViewProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListViewProducts.SelectedItems.Count > 0)
            {
                btnAdd.Enabled = false;
                btnDelete.Enabled = true;
            }
          
        }

        private void LoadListViewItems() 
        {
            ListViewProducts.Items.AddRange(_manager.GetAll().Select(
                p => new ListViewItem(new string[]{
                    p.Id.ToString(), 
                    p.Name, 
                    p.Price.ToString("0.00"), 
                    p.Description, 
                    p.Stock.ToString()
                })).ToArray());
        }

        private void ProductForm_Click(object sender, EventArgs e)
        {
            if (ListViewProducts.SelectedItems.Count > 0)
            {
                ListViewProducts.SelectedItems.Clear();
                btnAdd.Enabled = true;
                btnDelete.Enabled = false;
            }
        }

        private void txtSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
        }

        private void ListViewProducts_DoubleClick(object sender, EventArgs e)
        {
            if (ListViewProducts.SelectedItems.Count > 0)
            {
                int id = ListViewProducts.SelectedItems[0].SubItems[0].Text.ToInt();
                string name = ListViewProducts.SelectedItems[0].SubItems[1].Text;
                float price = ListViewProducts.SelectedItems[0].SubItems[2].Text.ToFloat();
                string description = ListViewProducts.SelectedItems[0].SubItems[3].Text;
                int stock = ListViewProducts.SelectedItems[0].SubItems[4].Text.ToInt();
                _product = new Product() { Id = id, Name = name, Price = price, Description = description, Stock = stock };
                EditProductForm editProduct = new EditProductForm();
                editProduct.Product = _product;

                if (editProduct.ShowDialog() == DialogResult.OK)
                {
                    ListViewProducts.Items.Clear();
                    LoadListViewItems();

                    if (ListViewProducts.SelectedItems.Count == 0)
                    {
                        btnAdd.Enabled = true;
                        btnDelete.Enabled = false;
                    }
                }

            }
        }

        private void ProductForm_Activated(object sender, EventArgs e)
        {
            try
            {
                ListViewProducts.Items.Clear();
                LoadListViewItems();

                if (ListViewProducts.SelectedItems.Count == 0)
                {
                    btnAdd.Enabled = true;
                    btnDelete.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
