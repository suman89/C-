using FrescoPlayTest.Web.Common;
using FrescoPlayTest.Web.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrescoPlayTest.Web.Controllers
{
    public class HomeController : Controller
    {
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

        public async System.Threading.Tasks.Task<ActionResult> GetQuestionDetail(EnrollmentDetails enrollmentDetails)
        {
            StaticDetails.DataSource = Server.MapPath("~/App_Data/Testdb.mdb");
            HandelQuiz handleQuiz = new HandelQuiz();
            AnswerRelatedDetails ansDetails = await handleQuiz.GetQuestionAndFirstAnswer(enrollmentDetails);
            ansDetails.PreviousAnswerString = ansDetails.AnswerString;
            AnswerAssesment prevAnswerAssesment = new AnswerAssesment();
            bool isSuccess = false;
            while (!isSuccess)
            {
                System.Threading.Thread.Sleep(10000);
                var answerAssesment = handleQuiz.PostQuizAnswer(ansDetails);
                _logger.InfoFormat("Answer assesment details. Correct Answer: {0},Pass Mark:{1},Time:{2},TrialCount:{3},Answer String:{4},Correct Answer String: {5}", answerAssesment.Assessment.TotalCorrectAnswers, answerAssesment.Assessment.PassingMarks, DateTime.Now, ansDetails.TrialCount + 1, ansDetails.AnswerString, ansDetails.CorrectAnswerString);
                if (answerAssesment.Assessment.Result != "FAIL")
                {
                    isSuccess = true;
                    break;
                }
                if (ansDetails.TrialCount > 100)
                    break;

                ansDetails = handleQuiz.GetNextAnswerCombination(ansDetails, answerAssesment, prevAnswerAssesment, ansDetails.PreviousAnswerString);

                prevAnswerAssesment = answerAssesment;
                ansDetails.TrialCount = ansDetails.TrialCount + 1;
                ansDetails = handleQuiz.CreateQuizAnswer(ansDetails);
            }
            return View(ansDetails);
        }
    }
}