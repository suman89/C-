using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApplication.Model
{
    public class QuizQuestion
    {
        public Assessment assessment { get; set; }
    }

    public class Assessment
    {
        public int id { get; set; }
        public string name { get; set; }
        public int passing_marks { get; set; }
        public Section[] sections { get; set; }
    }

    public class Section
    {
        public int id { get; set; }
        public string name { get; set; }
        public int assessment_id { get; set; }
        public int marks_per_questions { get; set; }
        public Question[] questions { get; set; }
    }

    public class Question
    {
        public int id { get; set; }
        public string question { get; set; }
        public int question_type { get; set; }
        public int marks { get; set; }
        public Answer[] answers { get; set; }
    }

    public class Answer
    {
        public int id { get; set; }
        public string answer { get; set; }
    }
}

