using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestConsoleApplication.Common;
using TestConsoleApplication.Model;

namespace TestConsoleApplication
{
    class Program
    {
        private readonly static ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            int testId = 2150;
            int enrolmentId = 477284;
            QuizAnswer quizAns = new QuizAnswer();
            quizAns.data = new AnswerDetail();
            quizAns.data.content_id = testId;
            quizAns.data.enrollment_id = enrolmentId;
            
            using (var client = HttpClientInitializer.GetClient())
            {
                var quizQuestion = client.GetAsync<QuizQuestion>("get_quiz_details.json?id=" + testId).Result.Content;
                _logger.InfoFormat("Section Count:{0}", quizQuestion.assessment.sections.Count());
                quizAns.data.sections = new AnswerSection[quizQuestion.assessment.sections.Count()];
                for (int i = 0; i <quizQuestion.assessment.sections.Count(); i++)
                {
                    var test = quizQuestion.assessment.sections[i];
                    quizAns.data.sections[i] = new AnswerSection();
                    quizAns.data.sections[i].section_id = test.id;
                    quizAns.data.sections[i].questions = new AnswerQuestion[test.questions.Count()];
                    string questionlength = string.Empty;
                    string initquestions = string.Empty;
                    string allzero = string.Empty;
                    for (int j = 0; j < test.questions.Count(); j++)
                    {
                        quizAns.data.sections[i].questions[j] = new AnswerQuestion();
                        quizAns.data.sections[i].questions[j].question_id = test.questions[j].id;
                        questionlength = questionlength + (test.questions[j].answers.Count()-1);
                        initquestions = initquestions + "0";
                    }
                    allzero = initquestions;
                    while(Convert.ToInt64(questionlength) != Convert.ToInt64(initquestions))
                    {
                        for(int x = 0; x < test.questions.Count(); x++ )
                        {
                            quizAns.data.sections[i].questions[x].answer_ids =new int[] { test.questions[x].answers[Convert.ToInt32(initquestions.Substring(x, 1))].id };
                            if (x == test.questions.Count() -1)
                            {
                                initquestions = DifferentCombination(questionlength, initquestions,allzero);
                            }
                        }
                        bool isSuccess = PostAnswerDetails(quizAns);
                        if(isSuccess)
                        {
                            break;
                        }
                    }
                }
            }

        }

        private static bool PostAnswerDetails(QuizAnswer quizAns)
        {
            bool isSuccess = false;
            try
            {
                using (var client1 = HttpClientInitializer.GetClient(false))
                {
                    var result = client1.PostAsJsonAsync<QuizAnswer,AnswerAssesment>("post_result.json", quizAns).Result.Content;
                    if (result.assessment.result != "FAIL")
                    {
                        isSuccess = true;
                    }
                    return isSuccess;
                }
            }
            catch(Exception ex)
            {
                _logger.ErrorFormat("Exception occured with details as InnerExeception : {0}, StackTrace: {1}, Message: {2}",ex.InnerException,ex.StackTrace,ex.Message);
                return isSuccess;
            }
        }

        private static string DifferentCombination(string questionlength, string initquestions,string allzero)
        {
            string modifiedinitquestion = (Convert.ToInt64(initquestions) + 1).ToString(allzero);
            string newanswers = CheckAnswerValidity(questionlength, modifiedinitquestion);
            return newanswers;
        }

        private static string CheckAnswerValidity(string questionlength, string initquestions)
        {
            var totallength = questionlength.Length;
            bool isAnswerWrong = false;
            string modifiedAnswer = initquestions;
            char[] questionlengthchararray = questionlength.ToCharArray();
            char[] initquestionschararray = initquestions.ToCharArray();
            for (int m = totallength; m != 0  ; m--)
            {
                if (initquestionschararray[m-1] > questionlengthchararray[m-1])
                {
                    if (m > 1)
                    {
                        initquestionschararray[m - 2] = (char)(Convert.ToInt32(initquestionschararray[m - 2]) + 1);
                        isAnswerWrong = true;
                    }
                    else
                    {
                        isAnswerWrong = false;
                        initquestionschararray[m - 1] = (char)(Convert.ToInt32(initquestionschararray[m - 1]) + 1);
                        modifiedAnswer = new string(initquestionschararray);
                    }
                    break;
                }
            }
            if (isAnswerWrong)
            {
                modifiedAnswer = CheckAnswerValidity(questionlength, new string(initquestionschararray));
            }
            return modifiedAnswer;
        }
    }
}
