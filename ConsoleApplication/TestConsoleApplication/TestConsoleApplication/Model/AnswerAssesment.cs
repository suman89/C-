using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApplication.Model
{

    public class AnswerAssesment
    {
        public AssessmentScore assessment { get; set; }
    }

    public class AssessmentScore
    {
        public int user_score { get; set; }
        public int passing_marks { get; set; }
        public string result { get; set; }
        public float percentile { get; set; }
        public int total_correct_answers { get; set; }
        public int quiz_marks { get; set; }
        public int total_questions_attempted { get; set; }
        public object[] incorrect_questions { get; set; }
        public string miles { get; set; }
        public int miles_earned { get; set; }
    }
}
