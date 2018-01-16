using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrescoPlayTest.Web.Models;
using log4net;
using FrescoPlayTest.Web.Common;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace FrescoPlayTest.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly static ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public async Task<ActionResult> GetQuestionDetail(EnrollmentDetails enrollmentDetails)
        {
            AnswerPageViewModel answerPageVM = new AnswerPageViewModel();
            answerPageVM.EnrollmentDetails = enrollmentDetails;
            try
            {
                QuizAnswer quizAns = new QuizAnswer();

                quizAns.ContentId = enrollmentDetails.ContentId;
                quizAns.EnrollmentId = enrollmentDetails.EnrolmentId;
                using (var client = HttpClientInitializer.GetClient(enrollmentDetails.ApiKey, false))
                {
                    var quizQuestion = (await client.GetAsync<QuizQuestion>("get_quiz_details.json?id=" + quizAns.ContentId)).Content;
                    _logger.InfoFormat("Section Count:{0}", quizQuestion.Assessment.Sections.Count());
                    quizAns.Sections = new AnswerSection[quizQuestion.Assessment.Sections.Count()];
                    string questionlength = string.Empty;
                    string initquestions = string.Empty;
                    string allzero = string.Empty;
                    string correctAnswer = string.Empty;

                    for (int i = 0; i < quizQuestion.Assessment.Sections.Count(); i++)
                    {
                        var test = quizQuestion.Assessment.Sections[i];
                        quizAns.Sections[i] = new AnswerSection();
                        quizAns.Sections[i].SectionId = test.Id;
                        quizAns.Sections[i].Questions = new AnswerQuestion[test.Questions.Count()];
                        for (int j = 0; j < test.Questions.Count(); j++)
                        {
                            quizAns.Sections[i].Questions[j] = new AnswerQuestion();
                            quizAns.Sections[i].Questions[j].QuestionId = test.Questions[j].Id;
                            questionlength = questionlength + (test.Questions[j].Answers.Count() - 1);
                            initquestions = initquestions + "0";
                            correctAnswer = correctAnswer + "9";
                            quizAns.Sections[i].Questions[j].AnswerIds = new int[] { test.Questions[j].Answers[0].Id };
                        }
                        allzero = initquestions;

                    }
                    answerPageVM.AllZeroString = allzero;
                    answerPageVM.AnswerCountString = questionlength;
                    answerPageVM.AnswerString = initquestions;
                    answerPageVM.CorrectAnswerString = correctAnswer;
                    answerPageVM.QuizAnswerJsonString = JsonConvert.SerializeObject(quizAns);
                    answerPageVM.QuizQuestionJsonString = JsonConvert.SerializeObject(quizQuestion);
                    answerPageVM.TrialCount = 0;
                }
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("Error occured to get question details: {0}", ex.Message);
            }
            return View(answerPageVM);
        }

        public ActionResult GenerateAnswer(AnswerPageViewModel answerPageVM)
        {
            QuizQuestion quizQuestion = JsonConvert.DeserializeObject<QuizQuestion>(answerPageVM.QuizQuestionJsonString);
            answerPageVM.TrialCount = answerPageVM.TrialCount + 1;
            return View("GetQuestionDetail", answerPageVM);
        }
    }
}