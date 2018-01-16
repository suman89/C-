﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApplication.Common
{
    public static class HttpClientInitializer
    {
        public static HttpClient GetClient(bool isAuthRequired = true)
        {
            var client = new HttpClient();
            client.Init(isAuthRequired);

            return client;
        }

        private static void Init(this HttpClient client, bool isAuthRequired = true)
        {
            client.BaseAddress = new Uri("https://play-api.fresco.me/api/v1/assessments/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-Api-Key", "JvdtZCtlFBSt4qzARID3B-WO2D5_DqW76xhAUPVI0J8");
            ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
        }
    }
}
