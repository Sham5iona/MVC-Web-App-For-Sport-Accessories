using Microsoft.AspNetCore.Identity;
using Sport_Accessories.Areas.Identity.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sport_Accessories.Models
{
    public class Product
    {
        private Guid _product_id;
        public Guid ProductId
        {
            get { return _product_id; }
            private set { _product_id = value; }
        }

        private string _product_name;
        public string ProductName
        {
            get { return _product_name; }
            private set { _product_name = value;}
        }

        public Category Category { get; set; }
        private Guid _category_id;
        public Guid CategoryId
        {
            get { return _category_id; }
            set { _category_id = value;}
        }

        private string _product_description;
        public string ProductDescription
        {
            get { return _product_description; }
            private set { _product_description = value;}
        }

        //insert a value when its changed or added
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        private DateTime _updated_at;
        public DateTime UpdatedAt
        {
            get { return _updated_at; }
            private set { _updated_at = DateTime.Now; }
        }

        private int _viewers;
        public int Viewers
        {
            get { return _viewers; }
            private set { _viewers = value; }
        }

        public User User { get; set; }

        //the user id is string because the id in the IdentityUser class is a string

        private string _user_id;
        public string UserId
        {
            get { return _user_id; }
            set { _user_id = value;}
        }

        private char _currency;
        public char Currency
        {
            get { return _currency; }
            private set { _currency = value;}
        }

        private bool _is_promo;
        public bool IsPromo
        {
            get { return _is_promo; }
            private set { _is_promo = value; }
        }

        public Photo Photo {  get; set; }
        private Guid _photo_id;
        public Guid PhotoId
        {
            get { return _photo_id; }
            set { _photo_id = value;}
        }

        public ICollection<BagProduct> BagProducts { get; set; }

        public ICollection<ProductFavourite> ProductFavourites { get; set; }

        private decimal _price;
        public decimal Price
        {
            get { return _price; }
            private set { _price = value;}
        }

        private decimal? _new_price;
        public decimal? NewPrice
        {
            get { return _new_price; }
            set { _new_price = value;}
        }

        //insert a value when its changed or added
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        private DateTime _last_modified_20118018;
        public DateTime LastModified_20118018
        {
            get { return _last_modified_20118018; }
            private set { _last_modified_20118018 = DateTime.Now; }
        }

        public Product() //empty constructor
        {
            LastModified_20118018 = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public Product(string product_name, string description, int viewers,
                       string user_id, char currency, bool is_promo, decimal price,
                       decimal? new_price, Guid category_id)
        {
            this.ProductName = product_name;
            this.ProductDescription = description;
            this.Viewers = viewers;
            this.UserId = user_id;
            this.Currency = currency;
            this.IsPromo = is_promo;
            this.Price = price;
            this.NewPrice = new_price;
            this.CategoryId = category_id;
            this.BagProducts = new List<BagProduct>();
            this.ProductFavourites = new List<ProductFavourite>();
            this.LastModified_20118018 = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }

    }
}
