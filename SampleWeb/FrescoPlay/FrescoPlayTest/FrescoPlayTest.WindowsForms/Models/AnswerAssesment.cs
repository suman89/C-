using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrescoPlayTest.WindowsForms.Models
{

    public class AnswerAssesment
    {
        public AssessmentScore Assessment { get; set; }
    }

    public class AssessmentScore
    {
        [JsonProperty("user_score")]
        public int UserScore { get; set; }
        [JsonProperty("passing_marks")]
        public int PassingMarks { get; set; }
        public string Result { get; set; }
        public float Percentile { get; set; }
        [JsonProperty("total_correct_answers")]
        public int TotalCorrectAnswers { get; set; }
        [JsonProperty("quiz_marks")]
        public int QuizMarks { get; set; }
        [JsonProperty("total_questions_attempted")]
        public int TotalQuestionsAttempted { get; set; }
        [JsonProperty("incorrect_questions")]
        public object[] IncorrectQuestions { get; set; }
        public string miles { get; set; }
        [JsonProperty("miles_earned")]
        public int MilesEarned { get; set; }
    }
}
