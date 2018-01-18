using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Mode
{
    public class Personnel:DataAccessHandler.IEntity<Personnel>
    {
        #region Table Fields
        int _id;
        [DisplayName("Identity")]
        [Category("Column")]
        [DataObjectFieldAttribute(true, true, false)]//Primary key attribute 
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        
        string _fName;
        [DisplayName("First Name")]
        [Category("Column")]
        public string FName
        {
            get { return _fName; }
            set { _fName = value; }
        }

        string _lName;
        [DisplayName("Last Name")]
        [Category("Column")]
        public string LName
        {
            get { return _lName; }
            set { _lName = value; }
        }
        #endregion

        #region Initialize
        public Personnel() { }
        #endregion
    }
}
