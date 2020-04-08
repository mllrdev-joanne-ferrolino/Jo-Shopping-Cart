﻿using ShoppingCart;
using ShoppingCart.BL.Managers;
using ShoppingCart.BL.Managers.Interfaces;
using ShoppingCart.BL.Models;
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

namespace ShoppingCart2
{
   
    public partial class OrderForm : Form
    {
        private OrderItem _orderItem;

        public OrderItem OrderItem
        {
            get { return _orderItem; }
            set { _orderItem = value; }
        }

        private Customer _customer;

        public Customer Customer
        {
            get { return _customer; }
            set { _customer = value; }
        }


        private Product _product;
        private List<OrderItem> _orderItemList;
        private IOrderManager _orderManager;
        private IProductManager _productManager;
        private IOrderItemManager _orderItemManager;
       
        public OrderForm()
        {
            _product = new Product();
            _orderManager = new OrderManager();
            _productManager = new ProductManager();
            _orderItemManager = new OrderItemManager();
            _orderItem = new OrderItem();
            _orderItemList = new List<OrderItem>();
            InitializeComponent();
        }

        private void OrderForm_Load(object sender, EventArgs e)
        {
            LoadProductItems();

            if (_customer == null)
            {
                MessageBox.Show("No customer selected.");
            }
            else if (_customer.Id > 0)
            {
                lblCustomerId.Text = _customer.Id.ToString();
                lblCustomerName.Text = $"{_customer.FirstName} {_customer.LastName}";
            }
            else
            {
                MessageBox.Show("No existing customer yet. Select customer.");
                CustomerForm customerForm = new CustomerForm();
                customerForm.MdiParent = this.MdiParent;
                customerForm.Show();
            }

            if (ListViewProducts.SelectedItems.Count == 0)
            {
                btnAdd.Enabled = false;
            }
            
        }

        private void LoadProductItems() 
        {
            ListViewProducts.Items.AddRange(_productManager.GetAll().Select(p => new ListViewItem(new string[] 
            { 
                p.Id.ToString(), 
                p.Name, 
                p.Price.ToString("0.00"), 
                p.Description, 
                p.Stock.ToString() 
            })).ToArray());

        }

        private void LoadOrderItems() 
        {
            ListViewOrders.Items.AddRange(_orderItemList.Select(o => new ListViewItem(new string[] 
            { 
                o.ProductId.ToString(), 
                _product.Name, 
                o.Quantity.ToString(), 
                _product.Price.ToString("0.00"), 
                o.Amount.ToString("0.00") 
            })).ToArray());
        }

        private void ListViewProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListViewProducts.SelectedItems.Count > 0)
            {
                ListViewItem listViewItem = ListViewProducts.SelectedItems[0];
                int id = Convert.ToInt32(listViewItem.SubItems[0].Text);
                string productName = listViewItem.SubItems[1].Text;
                float price = (float)Convert.ToDouble(listViewItem.SubItems[2].Text);
                string description = listViewItem.SubItems[3].Text;

                _product = new Product() { Id = id, Name = productName, Price = price, Description = description };
                btnAdd.Enabled = true;
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ListViewProducts.SelectedItems.Count > 0)
            {
                EditOrderForm editOrderForm = new EditOrderForm();
                editOrderForm.Product = _product;

                if (editOrderForm.ShowDialog() == DialogResult.OK)
                {
                    _orderItem = editOrderForm.OrderItem;
                    _orderItemList.Add(_orderItem);
                    ListViewOrders.Items.Clear();
                    LoadOrderItems();
                    float totalAmount = _orderItemList.Sum(x => x.Amount);
                    lblTotalAmount.Text = totalAmount.ToString("0.00");

                }
                

            }
           
        }

        private void OrderForm_Activated(object sender, EventArgs e)
        {
            if (_orderItem == null) 
            {
                MessageBox.Show("No order is selected.");
            }
            else if (_orderItem.ProductId > 0)
            {
                if (_orderItemList.Where(x => x.ProductId == _orderItem.ProductId) == null)
                {
                    _orderItemList.Add(_orderItem);
                }
                else
                {
                    OrderItem item = _orderItemList.FirstOrDefault(x => x.ProductId == _orderItem.ProductId);
                    item.Quantity = _orderItem.Quantity;
                }

                ListViewOrders.Items.Clear();
                LoadOrderItems();
                float totalAmount = _orderItemList.Sum(x => x.Amount);
                lblTotalAmount.Text = totalAmount.ToString("0.00");
            }
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            try
            {

                Order order = new Order() 
                { 
                    CustomerId = Convert.ToInt32(lblCustomerId.Text), 
                    TotalAmount = (float)Convert.ToDouble(lblTotalAmount.Text), 
                    DeliveryDate = DateTime.Now, 
                    Status = "for shipping"
                };

                if (_orderManager.Insert(order))
                {
                    int orderId = _orderManager.GetAll().LastOrDefault().Id;

                    foreach (var item in _orderItemList)
                    {
                        item.OrderId = orderId;
                        MessageBox.Show(_orderItemManager.Insert(item)? "Order placed successfully." : "Order items were not inserted.");
                        
                    }

                    _orderItem = new OrderItem();
                    _product = new Product();
                    ListViewOrders.Items.Clear();
                    CheckoutForm checkoutForm = new CheckoutForm();
                    checkoutForm.MdiParent = this.MdiParent;
                    checkoutForm.Show();

                }
                else
                {
                    MessageBox.Show("Order failed.");
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
                        id = Convert.ToInt32(txtSearch.Text);
                        Product result = _productManager.GetById(id);

                        if (result == null)
                        {
                            MessageBox.Show("Item doesn't exist");
                            txtSearch.Clear();
                        }
                        else
                        {
                            ListViewItem resultItem = new ListViewItem(new string[] { result.Id.ToString(), result.Name, result.Price.ToString("0.00"), result.Description, result.Stock.ToString() });
                            ListViewProducts.Items.Clear();
                            ListViewProducts.Items.Add(resultItem);
                        }
                    }
                    else
                    {
                        string searchByName = txtSearch.Text.ToLower();
                        IList<Product> resultList = new List<Product>();
                        resultList = _productManager.GetByName(searchByName);

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

        private void btnViewAll_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            ListViewProducts.Items.Clear();
            LoadProductItems();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _orderItemList.Clear();
            ListViewOrders.Items.Clear();
            lblTotalAmount.Text = string.Empty;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int productId = Convert.ToInt32(ListViewOrders.SelectedItems[0].SubItems[0].Text);

                foreach (var item in _orderItemList)
                {
                    if (item.ProductId == productId)
                    {
                        _orderItemList.Remove(item);
                        MessageBox.Show("Item deleted.");
                        break;
                    }
                }

                ListViewOrders.Items.Clear();
                LoadOrderItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            

        }

        private void ListViewOrders_DoubleClick(object sender, EventArgs e)
        {

            try
            {
                if (ListViewOrders.SelectedItems.Count > 0)
                {
                    ListViewItem ListViewOrderItem = ListViewOrders.SelectedItems[0];
                    int id = Convert.ToInt32(ListViewOrderItem.SubItems[0].Text);
                    string name = ListViewOrderItem.SubItems[1].Text;
                    int quantity = Convert.ToInt32(ListViewOrderItem.SubItems[2].Text);
                    float price = (float)Convert.ToDouble(ListViewOrderItem.SubItems[3].Text);
                    string description = _productManager.GetById(id).Description;
                    float amount = price * quantity;

                    _product = new Product() { Id = id, Name = name, Price = price, Description = description };
                    _orderItem = new OrderItem() { ProductId = id, Quantity = quantity, Amount = amount };

                    EditOrderForm editOrder = new EditOrderForm();
                    editOrder.Product = _product;
                    editOrder.OrderItem = _orderItem;

                    if (editOrder.ShowDialog() == DialogResult.OK)
                    {
                        _orderItem = editOrder.OrderItem;

                        if (_orderItem.ProductId > 0)
                        {
                            OrderItem item = _orderItemList.FirstOrDefault(x => x.ProductId == _orderItem.ProductId);
                            item.Quantity = _orderItem.Quantity;
                            item.Amount = _orderItem.Amount;
                           
                            ListViewOrders.Items.Clear();
                            LoadOrderItems();
                            float totalAmount = _orderItemList.Sum(x => x.Amount);
                            lblTotalAmount.Text = totalAmount.ToString("0.00");
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
        }
    }
}
