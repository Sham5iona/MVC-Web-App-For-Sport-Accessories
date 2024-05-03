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

        //add default value in case when the superadmin is created and this property can't
        //be assigned because the setter is private
        private string _file_name = "defaultImage.jpg";
        public string FileName
        {
            get { return _file_name; }
            private set { _file_name = value; }
        }

        //insert a value when its changed or added
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        private DateTime _last_modified_20118018;
        public DateTime LastModified_20118018
        {
            get { return _last_modified_20118018; }
            private set { _last_modified_20118018 = value; }
        }

        public User(string username, string password,
                    string file_name_url, string email, int access_failed_count)
        {
            this.Products = new List<Product>();
            this.PasswordHash = password;
            this.FileName = file_name_url;
            this.UserName = username;
            this.Email = email;
            this.AccessFailedCount = access_failed_count;
        }
        public User() //empty constructor for EFCore
        {
            this.LastModified_20118018 = DateTime.Now;
        }
        internal void UpdateFile(string filename)
        {
            this.FileName = filename;
        }
    }
}