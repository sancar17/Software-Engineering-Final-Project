using System;
using System.Net.Http.Headers;
using ContextSwitcher;

namespace BackendClient.Api
{
    public partial class BackendClient
    {
        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request,
            System.Text.StringBuilder urlBuilder)
        {
            if (!string.IsNullOrEmpty(TokenStorage.Instance.Token))
            {
                //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer " + TokenStorage.Instance.Token);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",TokenStorage.Instance.Token);
                Console.WriteLine(request.Headers);
            }
        }
    }
}