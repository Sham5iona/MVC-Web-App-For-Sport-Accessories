using Microsoft.AspNetCore.Identity;
using Sport_Accessories.Areas.Identity.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sport_Accessories.Models
{
    public class Favourite
    {
        private Guid _favourite_id;
        public Guid FavouriteId
        {
            get { return _favourite_id; }
            private set { _favourite_id = value; }
        }

        public User User { get; set; }

        //the user id is string because the id in the IdentityUser class is a string

        private string _user_id;
        public string UserId
        {
            get { return _user_id; }
            private set { _user_id = value;}
        }

        public ICollection<ProductFavourite> ProductFavourites { get; set; }

        //insert a value when its changed or added
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        private DateTime _updated_at;
        public DateTime UpdatedAt
        {
            get { return _updated_at; }
            private set { _updated_at = DateTime.Now; }
        }

        //insert a value when its changed or added
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        private DateTime _last_modified_20118018;
        public DateTime LastModified_20118018
        {
            get { return _last_modified_20118018; }
            private set { _last_modified_20118018 = DateTime.Now; }
        }

        public Favourite() //empty constructor for EFCore
        {
            
        }

        public Favourite(string user_id)
        {
            this.UserId = user_id;
            this.ProductFavourites = new List<ProductFavourite>();
        }
    }
}
