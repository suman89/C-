using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrescoPlayTest.Web.Common
{
    public class ErrorModel
    {
        public string[] Errors { get; set; }
        public string Id { get; set; }
        public string RequestId { get; set; }
        public string Message { get; set; }

    }
}
