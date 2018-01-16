using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrescoPlayTest.WindowsForms.Models
{
    public class AnswerRelatedDetails
    {

        public EnrollmentDetails EnrollmentDetails { get; set; }
        public QuizAnswer QuizAnswer { get; set; }
        public QuizQuestion QuizQuestion { get; set; }
        public bool IsChangesCorrect { get; set; }
        public bool IsChangesWrong { get; set; }
        public string AnswerCountString { get; set; }
        public string AnswerString { get; set; }
        public string PreviousAnswerString { get; set; }
        public string CorrectAnswerString { get; set; }
        public string AllZeroString { get; set; }
        public int TrialCount { get; set; }
        
    }
}
