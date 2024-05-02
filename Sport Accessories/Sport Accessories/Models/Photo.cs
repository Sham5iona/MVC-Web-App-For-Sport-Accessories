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
            set { _file_name = value; }
        }

        public Product Product { get; set; }

        //insert a value when its changed or added
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        private DateTime _last_modified_20118018;
        public DateTime LastModified_20118018
        {
            get { return _last_modified_20118018; }
            private set { _last_modified_20118018 = DateTime.Now; }
        }

        public Photo() //empty constructor for EFCore
        {
            this.LastModified_20118018 = DateTime.Now;

        }

        public Photo(string file_name, Guid product_id)
        {
            this._file_name = file_name;
            this.LastModified_20118018 = DateTime.Now;
        }

        internal void UpdateFile(string file_name)
        {
            this.FileName = file_name;
        }
    }
}
