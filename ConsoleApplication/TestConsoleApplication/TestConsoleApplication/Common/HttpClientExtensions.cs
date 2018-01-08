using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApplication.Common
{
    public static class HttpClientExtensions
    {
        private readonly static ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static async Task<HttpResult<T>> GetAsync<T>(this HttpClient client, string address )
        {
            try
            {
                using (HttpResponseMessage response = await client.GetAsync(address))
                {
                    HttpResult<T> result = new HttpResult<T>() { StatusCode = response.StatusCode };
                    if (response.Content != null)
                    {
                        result.RawJson = await response.Content.ReadAsStringAsync();
                        _logger.Info("Response :" + result.RawJson);
                        if (!response.IsSuccessStatusCode)
                        {
                            ErrorModel errorResponse;
                            bool isKnownModelType = true;
                            try
                            {
                                errorResponse = JsonConvert.DeserializeObject<ErrorModel>(result.RawJson);
                            }
                            catch
                            {
                                _logger.ErrorFormat("Error response is not of type ErrorModel, while invoking API: {0}", response.RequestMessage.RequestUri);
                                isKnownModelType = false;
                                errorResponse = new ErrorModel
                                {
                                    Id = "234",
                                    RequestId = Guid.NewGuid().ToString(),
                                };
                            }

                            if (!isKnownModelType)
                            {
                                errorResponse.Message = await response.Content.ReadAsStringAsync();
                            }

                            result.HttpError = errorResponse;
                        }
                        else
                        {
                            result.Content = JsonConvert.DeserializeObject<T>(result.RawJson); ;

                        }
                    }
                    return result;
                }
            }
            catch (HttpRequestException ex)
            {
                return HttpResult<T>.Failure(ex.Message);
            }
        }

        public static async Task<HttpResult<TResponse>> PostAsJsonAsync<TRequest, TResponse>(this HttpClient client, string address, TRequest content)
        {
            try
            {
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, address))
                {
                    request.Content = new ObjectContent<TRequest>(content, Helper.GetJsonFormatter());
                    using (HttpResponseMessage response = await client.SendAsync(request))
                    {
                        HttpResult<TResponse> result = new HttpResult<TResponse>() { StatusCode = response.StatusCode };
                        if (response.Content != null)
                        {
                            result.RawJson =await response.Content.ReadAsStringAsync();
                            _logger.Info("Response :" + result.RawJson);
                            if (!response.IsSuccessStatusCode)
                            {
                                ErrorModel errorResponse;
                                bool isKnownModelType = true;
                                try
                                {
                                    errorResponse = await response.Content.ReadAsAsync<ErrorModel>();
                                }
                                catch
                                {
                                    _logger.ErrorFormat("Error response is not of type ErrorModel, while invoking API: {0} using HTTP {1}", response.RequestMessage.RequestUri, response.RequestMessage.Method);
                                    isKnownModelType = false;
                                    errorResponse = new ErrorModel
                                    {
                                        Id = "234",
                                        RequestId = Guid.NewGuid().ToString(),
                                    };
                                }

                                if (!isKnownModelType)
                                {
                                    errorResponse.Message = await response.Content.ReadAsStringAsync();
                                }

                                result.HttpError = errorResponse;
                            }
                            else
                            {
                                result.Content = await response.Content.ReadAsAsync<TResponse>();
                            }

                        }
                        return result;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return HttpResult<TResponse>.Failure(ex.Message);
            }
        }


        static string GetErrorMessageToDisplay(ErrorModel error)
        {
            return error.Message + " Request Id: " + error.RequestId;
        }
    }
}
