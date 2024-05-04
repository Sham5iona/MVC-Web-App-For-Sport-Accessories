using System.ComponentModel.DataAnnotations.Schema;

namespace Sport_Accessories.Models
{
    public class ProductFavourite
    {
        public Product Product { get; set; }
        private Guid _product_id;
        public Guid ProductId
        {
            get { return _product_id; }
            private set { _product_id = value; }
        }

        public Favourite Favourite { get; set; }
        private Guid _favourite_id;
        public Guid FavouriteId
        {
            get { return _favourite_id;}
            private set { _favourite_id = value;}
        }

        //insert a value when its changed or added
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        private DateTime _last_modified_20118018;
        public DateTime LastModified_20118018
        {
            get { return _last_modified_20118018; }
            private set { _last_modified_20118018 = value; }
        }

        public ProductFavourite() //empty constructor for EFCore
        {
            this.LastModified_20118018 = DateTime.Now;
        }
        public ProductFavourite(Guid product_id, Guid favourite_id)
        {
            this.ProductId = product_id;
            this.LastModified_20118018 = DateTime.Now;
            this.FavouriteId = favourite_id;
        }
    }
}
