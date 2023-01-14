using System.Text;
using System.Text.Json;
using Mango.Web.Models;
using Mango.Web.Services.IServices;

namespace Mango.Web.Services;

public class BaseService : IBaseService
{
    
    public ResponseDto responseModel { get; set; }

    public IHttpClientFactory HttpClientFactory { get; set; }

    public BaseService(IHttpClientFactory httpClientFactory)
    {
        HttpClientFactory = httpClientFactory;
        responseModel = new ResponseDto();
    }
    
    public async Task<T> SendAsync<T>(ApiRequest apiRequest)
    {
        try
        {
            var client = HttpClientFactory.CreateClient("MangoAPI");
            HttpRequestMessage message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(apiRequest.Url);
            client.DefaultRequestHeaders.Clear();
            if (apiRequest.Data != null)
            {
                message.Content = new StringContent(JsonSerializer.Serialize(apiRequest.Data), Encoding.UTF8, "application/json");
            }

            HttpResponseMessage apiResponse = null;
            switch (apiRequest.ApiType)
            {
                case SD.ApiType.GET:
                    message.Method = HttpMethod.Get;
                    break;
                case SD.ApiType.POST:
                    message.Method = HttpMethod.Post;
                    break;
                case SD.ApiType.PUT:
                    message.Method = HttpMethod.Put;
                    break;
                case SD.ApiType.DELETE:
                    message.Method = HttpMethod.Delete;
                    break;
                default:
                    message.Method = HttpMethod.Get;
                    break;
            }
            apiResponse = await client.SendAsync(message);

            var apiContent = await apiResponse.Content.ReadAsStringAsync();
            var responseDto = JsonSerializer.Deserialize<T>(apiContent, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
            return responseDto;


        }
        catch (Exception e)
        {
            var dto = new ResponseDto()
            {
                DisplayMessage = "Error",
                ErrorMessages = new List<string>() { e.Message },
                IsSuccess = false
            };

            var res = JsonSerializer.Serialize(dto);
            var apiResponseDto = JsonSerializer.Deserialize<T>(res);
            return apiResponseDto;
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(true);
    }
}