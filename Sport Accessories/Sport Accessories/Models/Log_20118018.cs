using System.ComponentModel.DataAnnotations.Schema;

namespace Sport_Accessories.Models
{
    public class Log_20118018
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            private set { _id = value; }
        }

        private string _table_name;
        public string TableName
        {
            get { return _table_name; }
            private set { _table_name = value;}
        }

        private string _operation_type;
        public string OperationType
        {
            get { return _operation_type; }
            private set { _operation_type = value;}
        }

        private DateTime _created_at;
        public DateTime CreatedAt
        {
            get { return _created_at; }
            private set { _created_at = value; }
        }

        public Log_20118018() // emprty constructor for EFCore
        {
            
        }

        public Log_20118018(string table_name, string operation_type)
        {
            this.TableName = table_name;
            this.OperationType = operation_type;
        }

        //make a trigger in the database from the SSMS to insert every operation
        //on any table into this table. 

    }
}
