using MauiCommon.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MauiCommon.Service
{
    public delegate Task<T> HttpSuccessHandlerDelegate<T>(HttpResponseMessage response);
    public delegate Task<T> HttpStreamHandlerDelegate<T>(Stream stream);

    public interface IHttpClientService
    {
        Task<T> Get<T>(Uri uri, ILogService log, HttpSuccessHandlerDelegate<T> successHandler, [CallerMemberName] string caller = null);
        Task<T> Post<T>(Uri uri, HttpContent content, ILogService log, HttpSuccessHandlerDelegate<T> successHandler, [CallerMemberName] string caller = null);
        Task<T> GetStream<T>(Uri uri, ILogService log, HttpStreamHandlerDelegate<T> streamHandler, [CallerMemberName] string caller = null);

    }

    public class HttpClientService : IHttpClientService
    {
        private static readonly HttpClient Client = new HttpClient();

        public async Task<T> Get<T>(Uri uri, ILogService log, HttpSuccessHandlerDelegate<T> successHandler, [CallerMemberName] string caller = null)
        {
            string requestUri = uri.AbsoluteUri;

            try
            {
                using (var response = await Client.GetAsync(uri, new CancellationToken()))
                {
                    requestUri = response.RequestMessage.RequestUri.AbsoluteUri;

                    if (response.IsSuccessStatusCode)
                    {
                        return await successHandler(response);
                    }
                    else
                    {
                        if (log != null)
                        {
                            log.Warning($"{caller}: service call not successful ({requestUri}), HTTP status code: {response.StatusCode}, response: {response}");

                        }
                    }
                }
            }
            catch (Exception ex)
           {
                if (log != null)
                {
                    log.Error($"{caller}: service call not successful ({requestUri}): {ex}");
                }
            }

            return default(T);
        }

        public async Task<T> Post<T>(Uri uri, HttpContent content, ILogService log, HttpSuccessHandlerDelegate<T> successHandler, [CallerMemberName] string caller = null)
        {
            string requestUri = uri.AbsoluteUri;

            try
            {
                using (var response = await Client.PostAsync(uri, content, new CancellationToken()))
                {
                    requestUri = response.RequestMessage.RequestUri.AbsoluteUri;

                    if (response.IsSuccessStatusCode)
                    {
                        return await successHandler(response);
                    }
                    else
                    {
                        if (log != null)
                        {
                            log.Warning($"{caller}: service call not successful ({requestUri}), HTTP status code: {response.StatusCode}, response: {response}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (log != null)
                {
                    log.Error($"{caller}: service call not successful ({requestUri}): {ex}");
                }
            }

            return default(T);
        }

        public async Task<T> GetStream<T>(Uri uri, ILogService log, HttpStreamHandlerDelegate<T> streamHandler, [CallerMemberName] string caller = null)
        {
            string requestUri = uri.AbsoluteUri;

            try
            {
                using (var stream = await Client.GetStreamAsync(uri))
                {
                    return await streamHandler(stream);
                }
            }
            catch (Exception ex)
            {
                if (log != null)
                {
                    log.Error($"{caller}: service call not successful ({requestUri}): {ex}");
                }
            }

            return default(T);
        }

    }
}
