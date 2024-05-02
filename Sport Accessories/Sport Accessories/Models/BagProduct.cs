using System.ComponentModel.DataAnnotations.Schema;

namespace Sport_Accessories.Models
{
    public class BagProduct
    {
        //Thats a mapping table between Bag and Product because they have a
        //many-to-many relationship
        public Bag Bag { get; set; }
        private Guid _bag_id;
        public Guid BagId
        {
            get { return _bag_id; }
            private set { _bag_id = value; }
        }

        public Product Product { get; set; }
        private Guid _product_id;
        public Guid ProductId
        {
            get { return _product_id; }
            private set { _product_id = value;}
        }

        //insert a value when its changed or added
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        private DateTime _last_modified_20118018;
        public DateTime LastModified_20118018
        {
            get { return _last_modified_20118018; }
            private set { _last_modified_20118018 = DateTime.Now; }

        }

        public BagProduct() //empty constructor for EFCore
        {
            this.LastModified_20118018 = DateTime.Now;

        }

    }
}
