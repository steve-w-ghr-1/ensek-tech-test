using EnsekTestAutomation.Utils;
using Reqnroll;

namespace EnsekTestAutomation.StepDefinitions;

[Binding]
public class GetEnergyProductsSteps
{
    private readonly RestHelper _restHelper;
    private readonly ScenarioContext _scenarioContext;

    public GetEnergyProductsSteps(RestHelper restHelper, ScenarioContext scenarioContext)
    {
        _restHelper = restHelper;
        _scenarioContext = scenarioContext;
    }

    [When("I request to view all energy products")]
    public async Task WhenIRequestToViewAllEnergyProducts()
    {
        var baseUrl = Configuration.BaseUrls["ensekTestApiApp"];
        var resource = $"energy";
        _restHelper.SetHttpClient(baseUrl);

        var response = await _restHelper.SendGetRequest(resource);
        _scenarioContext["ApiResponse"] = response;
    }
}