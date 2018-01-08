﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApplication.Common
{
    public class HttpResult
    {
        public HttpResult()
        {
            this.Errors = new Collection<string>();
        }

        protected HttpResult(IList<string> errors)
        {
            this.Errors = new Collection<string>(errors);
        }

        public HttpStatusCode StatusCode { get; set; }
        public ErrorModel HttpError { get; set; }
        public string RawJson { get; set; }
        public Collection<string> Errors { get; private set; }
        public bool Succeeded { get { return StatusCode != default(HttpStatusCode) && IsSuccessStatusCode(StatusCode); } }

        bool IsSuccessStatusCode(HttpStatusCode statusCode)
        {
            return statusCode >= HttpStatusCode.OK && statusCode <= (HttpStatusCode)299;
        }

        public static HttpResult Failure(string error)
        {
            return new HttpResult(new string[] { error });
        }

        public static HttpResult Failure(IList<string> errors)
        {
            return new HttpResult(errors);
        }
    }
}
