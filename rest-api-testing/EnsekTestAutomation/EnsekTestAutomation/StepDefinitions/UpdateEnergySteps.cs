using EnsekTestAutomation.Utils;
using Newtonsoft.Json.Linq;
using Reqnroll;

namespace EnsekTestAutomation.StepDefinitions;

[Binding]
public class UpdateEnergySteps
{
    private readonly RestHelper _restHelper;
    private readonly ScenarioContext _scenarioContext;

    public UpdateEnergySteps(RestHelper restHelper, ScenarioContext scenarioContext)
    {
        _restHelper = restHelper;
        _scenarioContext = scenarioContext;
    }

    [When("I request an update to an order with details:")]
    public async Task WhenIRequestAnUpdateToAnOrderWithDetails(DataTable requestBodyDetails)
    {
        var fileName = "orderUpdateDefaultRequestBody.json";
        var requestPayload = JToken.Parse(FileReadHelper.ReadFile(fileName).ToString());
        var updatedRequestBody = JsonHelper.UpdateJson(requestPayload, requestBodyDetails);
        
        // extract the orderId to set into the endpoint resource
        var orderId = updatedRequestBody["id"].ToString();
        
        var baseUrl = Configuration.BaseUrls["ensekTestApiApp"];
        var resource = $"orders/{orderId}";
        _restHelper.SetHttpClient(baseUrl);

        var response = await _restHelper.SendPutRequest(resource, updatedRequestBody.ToString(), null);
        _scenarioContext["ApiResponse"] = response;
    }
}