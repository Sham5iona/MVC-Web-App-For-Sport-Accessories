using Microsoft.AspNetCore.Identity;
using Sport_Accessories.Areas.Identity.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sport_Accessories.Models
{
    public class Bag
    {
        private Guid _bag_id;
        public Guid BagId
        {
            get { return _bag_id; }
            private set { _bag_id = value; }
        }

        public ICollection<BagProduct> BagProducts { get; set; }

        public User User { get; set; }

        //the user id is string because the id in the IdentityUser class is a string
        private string _user_id;
        public string UserId
        {
            get { return _user_id; }
            private set { _user_id = value; }
        }

        //generate a value when the row is inserted or updated
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        private DateTime _updated_at;
        public DateTime UpdatedAt
        {
            get { return _updated_at; }
            private set { _updated_at = value; }
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

        public Bag() //empty constructor for EFCore
        {
            
        }

        public Bag(string user_id)
        {
            this.BagProducts = new List<BagProduct>();
            this.UserId = user_id;
        }

    }
}
