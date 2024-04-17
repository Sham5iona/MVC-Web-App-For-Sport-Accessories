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
        public Guid FavoriteId
        {
            get { return _favourite_id;}
            private set { _favourite_id = value;}
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

        public ProductFavourite() //empty constructor for EFCore
        {
            
        }

    }
}
