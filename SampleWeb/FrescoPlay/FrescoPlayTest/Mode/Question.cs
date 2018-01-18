using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mode
{
    public class Question : DataAccessHandler.IEntity<Question>
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

        int _qId;
        [DisplayName("Question Id")]
        [Category("Column")]
        public int QuestionId
        {
            get { return _qId; }
            set { _qId = value; }
        }

        int _qType;
        [DisplayName("Question Type")]
        [Category("Column")]
        public int QuestionType
        {
            get { return _qType; }
            set { _qType = value; }
        }

        string _qDetails;
        [DisplayName("Question Details")]
        [Category("Column")]
        public string QuestionDetails
        {
            get { return _qDetails; }
            set { _qDetails = value; }
        }

        string _tName;
        [DisplayName("Topic Name")]
        [Category("Column")]
        public string TopicName
        {
            get { return _tName; }
            set { _tName = value; }
        }

        #endregion

        #region Initialize
        public Question() { }
        #endregion
    }
}
