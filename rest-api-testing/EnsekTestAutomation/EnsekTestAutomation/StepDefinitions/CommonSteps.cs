using EnsekTestAutomation.Utils;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Reqnroll;
using RestSharp;

namespace EnsekTestAutomation.StepDefinitions;

[Binding]
public class CommonSteps
{
    private readonly RestHelper _restHelper;
    private readonly ScenarioContext _scenarioContext;
    private readonly SecureTokenSteps _secureTokenSteps;
    
    public CommonSteps(RestHelper restHelper, ScenarioContext scenarioContext, SecureTokenSteps secureTokenSteps)
    {
        _restHelper = restHelper;
        _scenarioContext = scenarioContext;
        _secureTokenSteps = secureTokenSteps;
    }

    [Then("the response status code is '(.*)'")]
    public void ThenTheResponseStatusCodeIs(int statusCode)
    {
        var response = _scenarioContext.Get<RestResponse>("ApiResponse");
        var responseStatusCode = Convert.ToInt32(response.StatusCode);
        responseStatusCode.Should().Be(statusCode);
    }

    [Given("I have reset the test data")]
    public async Task GivenIHaveResetTheTestData()
    {
        var baseUrl = Configuration.BaseUrls["ensekTestApiApp"];
        var resource = "reset";
        _restHelper.SetHttpClient(baseUrl);

        //get token if not available, which gets set in _scenarioContext
        if (!_scenarioContext.TryGetValue("token", out var value))
            await _secureTokenSteps.GetSecureToken();
        
        var token = ScenarioContextHelper.GetFromScenarioContext<string>(_scenarioContext, "token");
        
        var headers = new[] { "Key", "Value" };
        var table = new DataTable(headers);
        table.AddRow(new Dictionary<string, string>
        {
            { "Key", "Authorization" },
            { "Value", $"Bearer {token}" }
        });
        
        var response = await _restHelper.SendPostRequest(resource, null, table);
        _scenarioContext["ApiResponse"] = response;
        ThenTheResponseStatusCodeIs(200);
    }
    
    [Then("the response contains")]
    public void ResponseBodyContains(DataTable expectedDataTable)
    {
        var response = JToken.Parse(_scenarioContext.Get<RestResponse>("ApiResponse").Content ?? throw new InvalidOperationException());
        foreach (var row in expectedDataTable.Rows)
        {
            var expectedValue = row[1];
            var apiResponseKey = row[0];
            var apiActualResponseBodyKeyValue = response.SelectToken(apiResponseKey)?.ToString();
            switch (expectedValue)
            {
                case "[today]":
                    if (!DateTime.TryParse(apiActualResponseBodyKeyValue, out var parsedTime)) throw new InvalidOperationException($">>>> Error with DateTime for order {row[0]}");
                    parsedTime.ToUniversalTime().Date.Should().Be(DateTime.UtcNow.Date);
                    //TODO: check time also with 1 minute
                    break;
                default:
                    expectedValue.Should().Be(apiActualResponseBodyKeyValue);
                    break;
            }
        }
    }
    
    [Then("the response contains data as in file '(.*)'")]
    public void VerifyResponseBody(string fileName)
    {
        var actualApiResponseBody = JToken.Parse(_scenarioContext.Get<RestResponse>("ApiResponse").Content ?? 
                                                 throw new InvalidOperationException());
        var expectedApiResponseBody = FileReadHelper.ReadFile(fileName);
        JsonHelper.CompareJson(expectedApiResponseBody, actualApiResponseBody,false);
    }
}