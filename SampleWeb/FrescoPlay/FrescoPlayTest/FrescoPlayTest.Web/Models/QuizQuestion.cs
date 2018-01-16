using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrescoPlayTest.Web.Models
{
    public class QuizQuestion
    {
        public Assessment Assessment { get; set; }
    }

    public class Assessment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonProperty("passing_marks")]
        public int PassingMarks { get; set; }
        public Section[] Sections { get; set; }
    }

    public class Section
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonProperty("assessment_id")]
        public int AssessmentId { get; set; }
        [JsonProperty("marks_per_questions")]
        public int MarksPerQuestions { get; set; }
        public Question[] Questions { get; set; }
    }

    public class Question
    {
        public int Id { get; set; }
        [JsonProperty("question")]
        public string QuestionDetail { get; set; }
        [JsonProperty("question_type")]
        public int QuestionType { get; set; }
        public int Marks { get; set; }
        public Answer[] Answers { get; set; }
    }

    public class Answer
    {
        public int Id { get; set; }
        [JsonProperty("answer")]
        public string AnswerDetail { get; set; }
    }
}

