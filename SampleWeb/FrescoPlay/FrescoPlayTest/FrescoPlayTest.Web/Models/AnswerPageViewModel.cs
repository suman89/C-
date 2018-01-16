using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrescoPlayTest.Web.Models
{
    public class AnswerPageViewModel
    {
        public EnrollmentDetails EnrollmentDetails { get; set; }
        public string QuizAnswerJsonString { get; set; }
        public string QuizQuestionJsonString { get; set; }
        public bool IsChangesCorrect { get; set; }
        public bool IsChangesWrong { get; set; }
        public string AnswerCountString { get; set; }
        public string AnswerString { get; set; }
        public string CorrectAnswerString { get; set; }
        public string AllZeroString { get; set; }
        public int TrialCount { get; set; }
    }
}