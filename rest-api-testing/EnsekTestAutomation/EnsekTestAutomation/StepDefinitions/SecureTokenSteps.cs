using System.Diagnostics;
using EnsekTestAutomation.Utils;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Reqnroll;
using RestSharp;

namespace EnsekTestAutomation.StepDefinitions;

[Binding]
public class SecureTokenSteps
{
    private readonly RestHelper _restHelper;
    private readonly ScenarioContext _scenarioContext;

    public SecureTokenSteps(RestHelper restHelper, ScenarioContext scenarioContext)
    {
        _restHelper = restHelper;
        _scenarioContext = scenarioContext;
    }

    [Given("I have a valid API secure token")]
    public async Task GetSecureToken()
    {
        await GetSecureToken("secureTokenDefaultRequestBody.json");
    }
    
    public async Task GetSecureToken(string fileName)
    {
        var baseUrl = Configuration.BaseUrls["ensekTestApiApp"];
        var resource = "login";
        _restHelper.SetHttpClient(baseUrl);
        
        var requestPayload = JToken.Parse(FileReadHelper.ReadFile(fileName).ToString());
        var response = await _restHelper.SendPostRequest(resource, requestPayload.ToString(), null) ??
                       throw new ArgumentNullException("The response was null");
        _scenarioContext["ApiResponse"] = response;
        
        //TODO reference to a method / step to check result
        var responseStatusCode = Convert.ToInt32(response.StatusCode);
        responseStatusCode.Should().Be(200);

        Debug.Assert(response.Content != null, "response.Content != null");
        var token = JsonHelper.GetValue(JToken.Parse(response.Content), "access_token");
        _scenarioContext["token"] = token;
    }
}