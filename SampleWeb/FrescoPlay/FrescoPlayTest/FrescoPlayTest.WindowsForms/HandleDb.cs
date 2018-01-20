using FrescoPlayTest.WindowsForms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrescoPlayTest.WindowsForms
{
    public class HandleDb
    {
        public HandleDb()
        {
            #region Initialize DataAccessAdapter
            string datasource = @"../../../Testdb.mdb";
            string password = "";
            DataAccessHandler.DataAccessAdapter.Initialize(datasource, password);
            #endregion
        }

        public void SaveQuestionAnswerDetails(QuizQuestion quizQuestion)
        {
            DataAccessHandler.Collections.Entities<Mode.Question> questions = Mode.Context.Instance.Question.Collection;
            DataAccessHandler.Collections.Entities<Mode.Answer> answers = Mode.Context.Instance.Answer.Collection;

            //var check = new Mode.Question();
            //check.QuestionId = 133;
            //check.QuestionDetails = "hfjfjfj";
            //check.QuestionType = 2;
            //Mode.Context.Instance.Question.Insert(check);


            foreach (var sec in quizQuestion.Assessment.Sections)
            {
                foreach(var ques in sec.Questions)
                {
                    var tempQues = new Mode.Question();
                    tempQues.QuestionId = ques.Id;
                    tempQues.QuestionDetails = ques.QuestionDetail;
                    tempQues.QuestionType = ques.QuestionType;
                    if (questions.Where(a => a.QuestionId == ques.Id).Count() == 0)
                    {
                        Mode.Context.Instance.Question.Insert(tempQues);
                    }
                    foreach(var ans in ques.Answers)
                    {
                        var tempAns = new Mode.Answer();
                        tempAns.AnswerId = ans.Id;
                        tempAns.AnswerDetails = ans.AnswerDetail;
                        tempAns.QuestId = ques.Id;
                        tempAns.IsCorrect = false;
                        if (answers.Where(a => a.AnswerId == ans.Id).Count() == 0)
                        {
                            Mode.Context.Instance.Answer.Insert(tempAns);
                        }
                    }
                }
            }
        }


        public void UpdateAnswerDetails(char v, Question question)
        {
            DataAccessHandler.Collections.Entities<Mode.Answer> answers = Mode.Context.Instance.Answer.Collection;

            int ansOption = Convert.ToInt32(new string(v, 1));
            var tempAns = new Mode.Answer();
            tempAns.AnswerId = question.Answers[ansOption].Id;
            tempAns.AnswerDetails = question.Answers[ansOption].AnswerDetail;
            tempAns.QuestId = question.Id;
            tempAns.IsCorrect = true;
            tempAns.ID = answers.First(a => a.AnswerId == tempAns.AnswerId).ID;
            Mode.Context.Instance.Answer.Update(tempAns);
        }
    }
}
