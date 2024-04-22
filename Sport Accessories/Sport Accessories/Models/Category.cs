using System.ComponentModel.DataAnnotations.Schema;

namespace Sport_Accessories.Models
{
    public class Category
    {
        private Guid _category_id;
        public Guid CategoryId
        {
            get { return _category_id;}
            private set { _category_id = value; }
        }

        private string _category_name;
        public string CategoryName
        {
            get { return _category_name;}
            private set { _category_name = value;}
        }

        private bool _is_active;
        public bool IsActive
        {
            get { return _is_active; }
            private set { _is_active = value; }
        }

        public ICollection<Product> Products { get; set; }

        //insert a value when its changed or added
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        private DateTime _last_modified_20118018;
        public DateTime LastModified_20118018
        {
            get { return _last_modified_20118018; }
            private set { _last_modified_20118018 = value; }
        }
        public Category() //empty constructor for EFCore
        {
            
        }

        public Category(string category_name, bool is_active)
        {
            this.CategoryName = category_name;
            this.IsActive = is_active;
            this.Products = new List<Product>();
            this.LastModified_20118018 = DateTime.Now;
        }
    }
}
