using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrescoPlayTest.WindowsForms.Models
{

    public class QuizAnswer
    {
        [JsonProperty("enrollment_id")]
        public int EnrollmentId { get; set; }
        [JsonProperty("content_id")]
        public int ContentId { get; set; }
        [JsonProperty("sections")]
        public AnswerSection[] Sections { get; set; }
    }

    public class AnswerSection
    {
        [JsonProperty("section_id")]
        public int SectionId { get; set; }
        [JsonProperty("questions")]
        public AnswerQuestion[] Questions { get; set; }
    }

    public class AnswerQuestion
    {
        [JsonProperty("question_id")]
        public int QuestionId { get; set; }
        [JsonProperty("answer_ids")]
        public int[] AnswerIds { get; set; }
    }

}
