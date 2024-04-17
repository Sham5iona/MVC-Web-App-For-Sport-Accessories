using System.ComponentModel.DataAnnotations.Schema;

namespace Sport_Accessories.Models
{
    public class Photo
    {
        private Guid _photo_id;
        public Guid PhotoId
        {
            get { return _photo_id; }
            private set { _photo_id = value; }
        }

        private string _file_name;
        public string FileName
        {
            get { return _file_name; }
            private set { _file_name = value; }
        }

        public Product Product { get; set; }
        private Guid _product_id;
        public Guid ProductId
        {
            get { return _product_id; }
            private set { _product_id = value; }
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

        public Photo() //empty constructor for EFCore
        {
            
        }

        public Photo(string file_name, Guid product_id)
        {
            this._file_name = file_name;
            this.ProductId = product_id;
        }
    }
}
