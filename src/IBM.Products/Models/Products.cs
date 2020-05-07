using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;

namespace IBM.Products.Models
{
    public class Products
    {
        public List<Product> Items { get; private set; }

        public Products()
        {
            LoadProducts(null);
        }

        public Products(string name)
        {
            LoadProducts($"where lower(name) like '%{name.ToLower()}%'");
        }

        private void LoadProducts(string where)
        {
            Items = new List<Product>();
            var conn = Helpers.NewConnection();
            var cmd = new SqliteCommand($"select id from product {where}", conn);
            conn.Open();

            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var id = Guid.Parse(rdr["id"].ToString());
                Items.Add(new Product(id));
            }
            conn.Close();
        }
    }

    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }
        
        [JsonIgnore]
        public bool IsNew { get; }

        public Product()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }

        public Product(Guid id)
        {
            IsNew = true;
            var conn = Helpers.NewConnection();
            var cmd = new SqliteCommand($"select * from product where id = '{id}'", conn);
            conn.Open();

            var rdr = cmd.ExecuteReader();
            if (!rdr.Read())
                return;

            IsNew = false;
            Id = Guid.Parse(rdr["id"].ToString());
            Name = rdr["name"].ToString();
            Description = (DBNull.Value == rdr["description"]) ? null : rdr["description"].ToString();
            Price = decimal.Parse(rdr["price"].ToString());
            DeliveryPrice = decimal.Parse(rdr["delivery_price"].ToString());
            conn.Close();
        }

        public void Save()
        {
            var conn = Helpers.NewConnection();
            var cmd = IsNew ? 
                new SqliteCommand($"insert into product (id, name, description, price, delivery_price) values ('{Id}', '{Name}', '{Description}', {Price}, {DeliveryPrice})", conn) : 
                new SqliteCommand($"update product set name = '{Name}', description = '{Description}', price = {Price}, delivery_price = {DeliveryPrice} where id = '{Id}'", conn);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void Delete()
        {
            foreach (var option in new ProductOptions(Id).Items)
                option.Delete();

            var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = new SqliteCommand($"delete from product where id = '{Id}'", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }

    public class ProductOptions
    {
        public List<ProductOption> Items { get; private set; }

        public ProductOptions()
        {
            LoadProductOptions(null);
        }

        public ProductOptions(Guid productId)
        {
            LoadProductOptions($"where product_id = '{productId}'");
        }

        private void LoadProductOptions(string where)
        {
            Items = new List<ProductOption>();
            var conn = Helpers.NewConnection();
            var cmd = new SqliteCommand($"select id from product_option {where}", conn);
            conn.Open();

            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var id = Guid.Parse(rdr["id"].ToString());
                Items.Add(new ProductOption(id));
            }
            conn.Close();
        }
    }

    public class ProductOption
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public bool IsNew { get; }

        public ProductOption()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }

        public ProductOption(Guid id)
        {
            IsNew = true;
            var conn = Helpers.NewConnection();
            var cmd = new SqliteCommand($"select * from product_option where id = '{id}'", conn);
            conn.Open();

            var rdr = cmd.ExecuteReader();
            if (!rdr.Read())
                return;

            IsNew = false;
            Id = Guid.Parse(rdr["id"].ToString());
            ProductId = Guid.Parse(rdr["product_id"].ToString());
            Name = rdr["name"].ToString();
            Description = (DBNull.Value == rdr["description"]) ? null : rdr["description"].ToString();
            
            conn.Close();
        }

        public void Save()
        {
            var conn = Helpers.NewConnection();
            var cmd = IsNew ?
                new SqliteCommand($"insert into product_option (id, product_id, name, description) values ('{Id}', '{ProductId}', '{Name}', '{Description}')", conn) :
                new SqliteCommand($"update product_option set name = '{Name}', description = '{Description}' where id = '{Id}'", conn);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void Delete()
        {
            var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = new SqliteCommand($"delete from product_option where id = '{Id}'", conn);
            cmd.ExecuteReader();
            conn.Close();
        }
    }
}