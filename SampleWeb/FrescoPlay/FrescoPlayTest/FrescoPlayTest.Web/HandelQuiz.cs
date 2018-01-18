using FrescoPlayTest.Web.Common;
using FrescoPlayTest.Web.Models;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FrescoPlayTest.Web
{
    public class HandelQuiz
    {
        private readonly static ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public async Task<AnswerRelatedDetails> GetQuestionAndFirstAnswer(EnrollmentDetails enrollmentDetails)
        {
            AnswerRelatedDetails answerRelatedDetails = new AnswerRelatedDetails();
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
                answerRelatedDetails.ListQuestion = new List<Question>();
                for (int i = 0; i < quizQuestion.Assessment.Sections.Count(); i++)
                {
                    var test = quizQuestion.Assessment.Sections[i];
                    answerRelatedDetails.ListQuestion.AddRange(test.Questions);
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
                answerRelatedDetails.QuizQuestion = quizQuestion;
                answerRelatedDetails.QuizAnswer = quizAns;
                answerRelatedDetails.EnrollmentDetails = enrollmentDetails;
                answerRelatedDetails.CorrectAnswerString = correctAnswer;
                answerRelatedDetails.AnswerString = initquestions;
                answerRelatedDetails.AnswerCountString = questionlength;
                answerRelatedDetails.AllZeroString = allzero;
                answerRelatedDetails.TrialCount = 0;
            }
            HandleDb handleDb = new HandleDb();
            handleDb.SaveQuestionAnswerDetails(answerRelatedDetails.QuizQuestion);
            return answerRelatedDetails;
        }


        public AnswerAssesment PostQuizAnswer(AnswerRelatedDetails ansDetails)
        {
            AnswerAssesment answerAssesment = new AnswerAssesment() { Assessment = new AssessmentScore() };

            try
            {
                string webAddr = "https://play-api.fresco.me/api/v1/assessments/post_result.json";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);

                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("X-Api-Key",ansDetails.EnrollmentDetails.ApiKey );
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(ansDetails.QuizAnswer);
                    var modifiedJson = "data=" + json;
                    streamWriter.Write(modifiedJson);
                    streamWriter.Flush();
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    answerAssesment = JsonConvert.DeserializeObject<AnswerAssesment>(responseText);
                }

            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("Exception occured with details as InnerExeception : {0}, StackTrace: {1}, Message: {2}", ex.InnerException, ex.StackTrace, ex.Message);

            }

            return answerAssesment;
        }


        public AnswerRelatedDetails CreateQuizAnswer(AnswerRelatedDetails ansDetails)
        {
            QuizAnswer quizAns = new QuizAnswer();
            quizAns.ContentId = ansDetails.EnrollmentDetails.ContentId;
            quizAns.EnrollmentId = ansDetails.EnrollmentDetails.EnrolmentId;
            char[] answerStringCharArray = ansDetails.AnswerString.ToCharArray();
            quizAns.Sections = new AnswerSection[ansDetails.QuizQuestion.Assessment.Sections.Count() ];
            for(int i = 0; i<ansDetails.QuizQuestion.Assessment.Sections.Count();i++)
            {
                var test = ansDetails.QuizQuestion.Assessment.Sections[i];
                quizAns.Sections[i] = new AnswerSection();
                quizAns.Sections[i].SectionId = test.Id;
                quizAns.Sections[i].Questions = new AnswerQuestion[test.Questions.Count()];
                for(int j = 0; j < test.Questions.Count(); j++)
                {
                    quizAns.Sections[i].Questions[j] = new AnswerQuestion();
                    quizAns.Sections[i].Questions[j].QuestionId = test.Questions[j].Id;
                    int check = Convert.ToInt32(answerStringCharArray[j].ToString());
                    quizAns.Sections[i].Questions[j].AnswerIds = new int[] { test.Questions[j].Answers[check].Id };
                }
            }
            ansDetails.QuizAnswer = quizAns;
            return ansDetails;
        }



        public AnswerRelatedDetails GetNextAnswerCombination(AnswerRelatedDetails ansDetails,AnswerAssesment presentAssesment,AnswerAssesment prevAssesment,string previousAnswerString)
        {
            string currentAnsString = string.Empty;
            var totalLength = ansDetails.AnswerCountString.Length;
            if(ansDetails.TrialCount != 0)
            {
                ansDetails.IsChangesCorrect = presentAssesment.Assessment.TotalCorrectAnswers > prevAssesment.Assessment.TotalCorrectAnswers;
                ansDetails.IsChangesWrong = presentAssesment.Assessment.TotalCorrectAnswers < prevAssesment.Assessment.TotalCorrectAnswers;
            }
            var correctAnsStringCharArray = ansDetails.CorrectAnswerString.ToCharArray();
            var currentAnswerStringCharArray = ansDetails.AnswerString.ToCharArray();
            var prevAnsStringCharArray = previousAnswerString.ToCharArray();
            var answerCountStringCharArray = ansDetails.AnswerCountString.ToCharArray();
            currentAnsString = ansDetails.AnswerString;
            if (ansDetails.IsChangesCorrect || ansDetails.IsChangesWrong)
            {
                for (int y = 0; y < ansDetails.CorrectAnswerString.Length; y++)
                {
                    if(currentAnswerStringCharArray[y] != prevAnsStringCharArray[y])
                    {
                        correctAnsStringCharArray[y] = ansDetails.IsChangesCorrect ? currentAnswerStringCharArray[y] : prevAnsStringCharArray[y];
                        HandleDb handleDb = new HandleDb();
                        handleDb.UpdateAnswerDetails(correctAnsStringCharArray[y], ansDetails.ListQuestion[y]);
                    }
                }
                ansDetails.CorrectAnswerString = new string(correctAnsStringCharArray);
            }

            for (int d = totalLength ; d != 0; d--)
            {
                if (correctAnsStringCharArray[d - 1] == '9')
                {
                    if (currentAnswerStringCharArray[d - 1] < answerCountStringCharArray[d - 1])
                        currentAnswerStringCharArray[d - 1] = (char)(currentAnswerStringCharArray[d - 1] + 1);
                    
                    break;
                }
                else
                {
                    currentAnswerStringCharArray[d - 1] = correctAnsStringCharArray[d - 1];
                }
            }
            ansDetails.AnswerString = new string(currentAnswerStringCharArray);
            ansDetails.PreviousAnswerString = currentAnsString;
            return ansDetails;
        }

        //private AnswerRelatedDetails CheckAnswerValidity(AnswerRelatedDetails ansDetails,string modifiedAnswerString, int correctAnswerPosition = 100, bool isCorrectAnswerCheckedOnce = false)
        //{
        //    var totalLength = ansDetails.AnswerCountString.Length;
        //    bool isAnswerWrong = false;
        //    string modifiedAnswer = modifiedAnswerString;
        //    char[] ansCountCharArray = ansDetails.AnswerCountString.ToCharArray();
        //    char[] modifiedAnswerCharArray = modifiedAnswerString.ToCharArray();
        //    char[] answerCharArray = ansDetails.AnswerString.ToCharArray();
        //    char[] correctAnswerCharArray = ansDetails.CorrectAnswerString.ToCharArray();
        //    if (ansDetails.AnswerCountString != modifiedAnswerString)
        //    {
        //        for (int m = totalLength; m != 0; m--)
        //        {
        //            if(correctAnswerCharArray[m - 1] != '9')
        //            {
        //                if (isCorrectAnswerCheckedOnce && correctAnswerPosition <= m-1)
        //                {
                            
        //                }
        //                else
        //                {
        //                    modifiedAnswerCharArray[m - 2] = (char)(Convert.ToInt32(modifiedAnswerCharArray[m - 2]) + 1);
        //                    isAnswerWrong = true;
                            
        //                }
        //                for (int z = m - 1; z < totalLength; z++)
        //                {
        //                    if (correctAnswerCharArray[z] != '9')
        //                    {
        //                        modifiedAnswerCharArray[z] = correctAnswerCharArray[z];
        //                    }
        //                    else
        //                    {
        //                        modifiedAnswerCharArray[z] = '0';
        //                    }
        //                }
        //                modifiedAnswer = new string(modifiedAnswerCharArray);
        //                correctAnswerPosition = m - 1;
        //                isCorrectAnswerCheckedOnce = true;
        //                if (isAnswerWrong)
        //                {
        //                    break;
        //                }
        //            }
        //            if (modifiedAnswerCharArray[m - 1] > ansCountCharArray[m - 1] )
        //            {
        //                if (m > 1)
        //                {

        //                    modifiedAnswerCharArray[m - 2] = (char)(Convert.ToInt32(modifiedAnswerCharArray[m - 2]) + 1);
        //                    isAnswerWrong = true;
                            
        //                    for (int z = m - 1; z < totalLength; z++)
        //                    {
        //                        if (correctAnswerCharArray[z] != '9')
        //                        {
        //                            modifiedAnswerCharArray[z] = correctAnswerCharArray[z];
        //                        }
        //                        else
        //                        { 
        //                            modifiedAnswerCharArray[z] = '0';
        //                        }
        //                    }
        //                    modifiedAnswer = new string(modifiedAnswerCharArray);


        //                }
        //                else
        //                {
        //                    isAnswerWrong = false;
        //                    modifiedAnswerCharArray[m - 1] = (char)(Convert.ToInt32(modifiedAnswerCharArray[m - 1]) + 1);
        //                    modifiedAnswer = new string(modifiedAnswerCharArray);
        //                }
        //                break;
        //            }
        //        }
        //        if (isAnswerWrong)
        //        {
        //            ansDetails = CheckAnswerValidity(ansDetails,modifiedAnswer ,correctAnswerPosition,isCorrectAnswerCheckedOnce);
        //        }
        //    }
        //    ansDetails.AnswerString = modifiedAnswer;
        //    return ansDetails;
        //}
    }
}
