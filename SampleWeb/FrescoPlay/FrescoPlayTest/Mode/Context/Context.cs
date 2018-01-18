using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mode
{
    public class Context
    {
        #region Class field and properties
        
        #region Singleton
        private static Context _instance = null;
        /// <summary>
        /// Singleton context
        /// </summary>
        public static Context Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Context();
                return _instance;
            }
        }
        #endregion

        #region Model
        private Personnel _personnel = null;
        public Personnel Personnel
        {
            get
            {
                _personnel = new Personnel();
                _personnel.Load();
                return _personnel;
            }
        }

        private Question _question;
        public Question Question
        {
            get
            {
                _question = new Question();
                _question.Load();
                return _question;
            }
        }

        private Answer _answer;
        public Answer Answer
        {
            get
            {
                _answer = new Answer();
                _answer.Load();
                return _answer;
            }
        }
        #endregion

        #endregion

        #region Initialize
        private Context() { }
        #endregion
        
    }
}
