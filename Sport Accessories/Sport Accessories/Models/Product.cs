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
            private set { _category_id = value;}
        }

        private string _product_description;
        public string ProductDescription
        {
            get { return _product_description; }
            private set { _product_description = value;}
        }

        //generate a value when the row is inserted or updated
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        private DateTime _updated_at;
        public DateTime UpdatedAt
        {
            get { return _updated_at; }
            private set { _updated_at = value;}
        }

        private int _count;
        public int Count
        {
            get { return _count; }
            private set { _count = value; }
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
            private set { _user_id = value;}
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

        public ICollection<Photo> Photos { get; set; }

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
            private set { _new_price = value;}
        }

        //generate a value when the row is inserted or updated
        //this column won't be touched in any way
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        private DateTime _last_modified_20118018;
        public DateTime LastModified_20118018
        {
            get { return _last_modified_20118018; }
            private set { _last_modified_20118018 = value; }
        }

        public Product() //empty constructor
        {
            
        }

        public Product(string product_name, string description, int count, int viewers,
                       string user_id, char currency, bool is_promo, decimal price,
                       decimal? new_price, Guid category_id)
        {
            this.ProductName = product_name;
            this.ProductDescription = description;
            this.Count = count;
            this.Viewers = viewers;
            this.UserId = user_id;
            this.Currency = currency;
            this.IsPromo = is_promo;
            this.Price = price;
            this.NewPrice = new_price;
            this.CategoryId = category_id;
            this.Photos = new List<Photo>();
            this.BagProducts = new List<BagProduct>();
            this.ProductFavourites = new List<ProductFavourite>();
        }

    }
}
