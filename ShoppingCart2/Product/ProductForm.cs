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
using System.Transactions;
using System.Windows.Forms;

namespace ShoppingCart
{
    public partial class ProductForm : Form
    {
        private IProductManager _manager;
        private IOrderItemManager _orderItemManager;
        private Product _product;
        private Customer _customer;

        public Customer Customer
        {
            get { return _customer; }
            set { _customer = value; }
        }


        public ProductForm()
        {
            _manager = new ProductManager();
            _orderItemManager = new OrderItemManager();
          
            InitializeComponent();
        }

        private void Products_Load(object sender, EventArgs e)
        {
            LoadListViewItems();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                EditProductForm editProductForm = new EditProductForm();
                editProductForm.Text = "Add Product";

                if (editProductForm.ShowDialog() == DialogResult.OK)
                {
                    while (!editProductForm.IsSuccessful)
                    {
                        var result = editProductForm.ShowDialog();

                        if (result == DialogResult.Cancel)
                        {
                            editProductForm.Close();
                            break;
                        }
                    }

                    if (editProductForm.IsSuccessful)
                    {
                        ListViewProducts.Items.Clear();
                        LoadListViewItems();
                        btnAdd.Enabled = true;
                        btnDelete.Enabled = false;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            try
            {
                List<int> ids = new List<int>();

                foreach (ListViewItem item in ListViewProducts.SelectedItems)
                {
                    ids.Add(Convert.ToInt32(item.SubItems[0].Text));
                }

                using (var scope = new TransactionScope())
                {
                    foreach (var id in ids)
                    {
                        var productId = _orderItemManager.GetByProductId(id);

                        if (productId.Count > 0)
                        {
                            _orderItemManager.DeleteByProductId(productId.ToArray());
                        }
                    }


                    if (_manager.Delete(ids.ToArray()))
                    {
                        MessageBox.Show("Product details deleted successfully.");
                        ListViewProducts.Items.Clear();
                        LoadListViewItems();
                        btnAdd.Enabled = true;
                        btnDelete.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show("Details were not deleted.");
                    }



                    scope.Complete();
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
                    p.Stock.ToString(),
                    p.Status
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
                string status = ListViewProducts.SelectedItems[0].SubItems[5].Text;
                _product = new Product() { Id = id, Name = name, Price = price, Description = description, Stock = stock, Status = status };
                EditProductForm editProduct = new EditProductForm();
                editProduct.Product = _product;

                if (editProduct.ShowDialog() == DialogResult.OK)
                {
                    while (!editProduct.IsSuccessful)
                    {
                        var result = editProduct.ShowDialog();

                        if (result == DialogResult.Cancel)
                        {
                            editProduct.Close();
                            break;
                        }
                    }

                    if (editProduct.IsSuccessful)
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

                if (_customer != null)
                {
                    MainForm parent = (MainForm)this.MdiParent;
                    parent.viewCustomersToolStripMenuItem.Visible = true;
                    parent.ShopToolStripMenuItem.Enabled = true;
                    parent.Customer = _customer;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
