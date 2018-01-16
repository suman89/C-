using FrescoPlayTest.WindowsForms.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrescoPlayTest.WindowsForms
{
    public partial class EnrolmentForm : Form
    {
        public EnrolmentForm()
        {
            InitializeComponent();
        }

        private void EnrolmentForm_Load(object sender, EventArgs e)
        {
            this.TrialCount.Hide();
            this.TrialCountValue.Text = "0";
            this.TrialCountValue.Hide();
        }

        private async void EnrolmentButton_Click(object sender, EventArgs e)
        {
            this.TrialCount.Show();
            this.TrialCountValue.Show();
            HandelQuiz handleQuiz = new HandelQuiz();
            AnswerRelatedDetails ansDetails = await handleQuiz.GetQuestionAndFirstAnswer(new EnrollmentDetails() { ApiKey = this.ApiKeyValue.Text, ContentId = Convert.ToInt32(this.ContentIdValue.Text.Trim()), EnrolmentId = Convert.ToInt32(this.EnrolmentIdValue.Text.Trim()) });
            ansDetails.PreviousAnswerString = ansDetails.AnswerString;
            AnswerAssesment prevAnswerAssesment = new AnswerAssesment();
            bool isSuccess = false;
            while (!isSuccess)
            {
                var answerAssesment = handleQuiz.PostQuizAnswer(ansDetails);
                if (answerAssesment.Assessment.Result != "FAIL")
                {
                    isSuccess = true;
                    break;
                }

                ansDetails = handleQuiz.GetNextAnswerCombination(ansDetails, answerAssesment, prevAnswerAssesment, ansDetails.PreviousAnswerString);

                prevAnswerAssesment = answerAssesment;
                ansDetails.TrialCount = ansDetails.TrialCount + 1;
                this.TrialCountValue.Text = ansDetails.TrialCount.ToString();
                ansDetails = handleQuiz.CreateQuizAnswer(ansDetails);
            }
            if (isSuccess)
            {
                MessageBox.Show("Quiz completed after " + (ansDetails.TrialCount + 1) + "trial");
            }

        }
    }
}
