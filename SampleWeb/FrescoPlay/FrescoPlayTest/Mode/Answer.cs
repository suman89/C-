using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mode
{
    public class Answer : DataAccessHandler.IEntity<Answer>
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

        int _aId;
        [DisplayName("Answer Id")]
        [Category("Column")]
        public int AnswerId
        {
            get { return _aId; }
            set { _aId = value; }
        }

        string _aDetails;
        [DisplayName("Answer Details")]
        [Category("Column")]
        public string AnswerDetails
        {
            get { return _aDetails; }
            set { _aDetails = value; }
        }

        bool _isCorrect;
        [DisplayName("Is Correct Answer")]
        [Category("Column")]
        public bool IsCorrect
        {
            get { return _isCorrect; }
            set { _isCorrect = value; }
        }

        int _qId;
        [DisplayName("Question Id")]
        [Category("Column")]
        public int QuestionId
        {
            get { return _qId; }
            set { _qId = value; }
        }
        #endregion

        #region Initialize
        public Answer() { }
        #endregion
    }
}
