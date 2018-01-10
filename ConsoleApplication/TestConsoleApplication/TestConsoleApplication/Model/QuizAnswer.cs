using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApplication.Model
{
    public class AnswerDetail
    {
        public QuizAnswer data { get; set; }
    }
    public class QuizAnswer
    {
        public int enrollment_id { get; set; }
        public int content_id { get; set; }
        public AnswerSection[] sections { get; set; }
    }

    public class AnswerSection
    {
        public int section_id { get; set; }
        public AnswerQuestion[] questions { get; set; }
    }

    public class AnswerQuestion
    {
        public int question_id { get; set; }
        public int[] answer_ids { get; set; }
    }

}
