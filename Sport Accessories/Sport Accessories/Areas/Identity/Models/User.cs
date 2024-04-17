using Microsoft.AspNetCore.Identity;
using Sport_Accessories.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sport_Accessories.Areas.Identity.Models
{
    public class User : IdentityUser
    {
        public ICollection<Product> Products { get; set; }
        public Bag Bag { get; set; }
        public Favourite Favourite { get; set; }

        //generate a value when the row is inserted or updated
        //this column won't be touched in any way
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        private DateTime _last_modified_20118018;
        public DateTime LastModified_20118018
        {
            get { return _last_modified_20118018; }
            private set { _last_modified_20118018 = value; }
        }

        public User()
        {
            this.Products = new List<Product>();
        }
    }
}
