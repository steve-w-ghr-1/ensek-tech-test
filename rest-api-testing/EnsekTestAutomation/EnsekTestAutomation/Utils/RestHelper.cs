using RestSharp;
using DataTable = Reqnroll.DataTable;

namespace EnsekTestAutomation.Utils;
//TODO: pull out logging into method
//TODO - SetHttpClient - add any applicable options, like max timeout
//TODO - look at reducing methods for put/post including passing headers or not

public class RestHelper
{
    //get http client from restSharp
    private RestClient _client;

    public void SetHttpClient(string baseUrl)
    {
        var options = new RestClientOptions(baseUrl) {
            Timeout = TimeSpan.FromMilliseconds(3000)
        };
        _client = new RestClient(options);
    }

    public async Task<RestResponse> SendGetRequest(string resource)
    {
        var request = new RestRequest(resource, Method.Get);
        Console.WriteLine($">>>>> Request Resource: {request.Resource}");
        //note to self - can use generic overload <request> to ds into a class.
        var response = await _client.ExecuteAsync(request);
        Console.WriteLine($">>>>> Response {response.Content}");
        return response;
    }
    
    public async Task<RestResponse> SendPostRequest(string resource, string? requestPayload, DataTable? headers)
    {
        var request = new RestRequest(resource, Method.Post);
        
        if (headers != null)
        {
            foreach (var row in headers.Rows)
            {
                var key = row["Key"];
                var value = row["Value"]; 
                request.AddHeader(key, value);
            }
        }
        
        if (requestPayload != null)
        {
            request = request.AddJsonBody(requestPayload);
        }
        
        Console.WriteLine($">>>>> Request Resource: {request.Resource}");
        
        foreach (var header in request.Parameters.Where(p => p.Type == ParameterType.HttpHeader))
        {
            Console.WriteLine($"Header: {header.Name} = {header.Value}");
        }
        
        Console.WriteLine($">>>>> Request Body: {requestPayload}");
        
        var response = await _client.ExecuteAsync(request);
        Console.WriteLine($">>>>> Response {response.Content}");
        return response;
    }    
    
    public async Task<RestResponse> SendPutRequest(string resource, string? requestPayload, DataTable? headers)
    {
        var request = new RestRequest(resource, Method.Put);
        
        if (requestPayload != null)
        {
            request = request.AddJsonBody(requestPayload);
        }
        
        if (headers != null)
        {
            foreach (var row in headers.Rows)
            {
                var key = row["Key"];
                var value = row["Value"]; 
                request.AddHeader(key, value);
            }
        }

        Console.WriteLine($">>>>> Request Resource: {request.Resource}");
        
        foreach (var header in request.Parameters.Where(p => p.Type == ParameterType.HttpHeader))
        {
            Console.WriteLine($"Header: {header.Name} = {header.Value}");
        }
        
        Console.WriteLine($">>>>> Request Body: {requestPayload}");
        
        var response = await _client.ExecuteAsync(request);
        Console.WriteLine($">>>>> Response {response.Content}");
        return response;
    }  
}