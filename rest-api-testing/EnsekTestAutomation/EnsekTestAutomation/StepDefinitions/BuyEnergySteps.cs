using System.Text.RegularExpressions;
using EnsekTestAutomation.Utils;
using Newtonsoft.Json.Linq;
using Reqnroll;
using RestSharp;

namespace EnsekTestAutomation.StepDefinitions;

[Binding]
public class BuyEnergySteps
{
    private readonly RestHelper _restHelper;
    private readonly ScenarioContext _scenarioContext;
    
    public BuyEnergySteps(RestHelper restHelper, ScenarioContext scenarioContext)
    {
        _restHelper = restHelper;
        _scenarioContext = scenarioContext;
    }
    
    public static readonly Dictionary<string, int> EnergyId = new()
    {
        { "gas", 1 },
        { "nuclear", 2 },
        { "electric", 3 },
        { "oil", 4 },
        { "fake", 99 }
    };

    [When("I order '(.*)' units of '(electric|gas|nuclear|oil|fake)' energy")]
    public async Task WhenIOrderUnitsOfEnergy(int unitQty, string energyType)
    {
        var baseUrl = Configuration.BaseUrls["ensekTestApiApp"];
        var resource = $"buy/{EnergyId[energyType]}/{unitQty}";
        _restHelper.SetHttpClient(baseUrl);

        var response = await _restHelper.SendPutRequest(resource, null, null);
        _scenarioContext["ApiResponse"] = response;
    }

    /* Note for reviewer: Before implementing this automation test I would have queried the API design wrt response.
     The API response makes it difficult for the consumer to parse the message to get the orderId. It would be preferable
     to return a better structured response i.e. order number in its own field.
    */
    [Then("the energy purchase response contains my order number")]
    public void ThenTheEnergyPurchaseResponseContainsMyOrderNumber()
    {
        var response = _scenarioContext.Get<RestResponse>("ApiResponse");
        var orderId = ExtractOrderNumber(response);
        _scenarioContext["orderId"] = orderId;
    }

    public string ExtractOrderNumber(RestResponse apiPurchaseResponse)
    {
        ArgumentNullException.ThrowIfNull(apiPurchaseResponse, nameof(apiPurchaseResponse)); 
        var apiResponseContent = apiPurchaseResponse.Content ?? throw new ArgumentNullException(nameof(apiPurchaseResponse));
        var responseMessage = JsonHelper.GetValue(JToken.Parse(apiResponseContent), "message")
            ?? throw new ArgumentNullException($"No response message found: {apiPurchaseResponse}");
        var guidMatch = Regex.Match(responseMessage, @"\b[a-fA-F0-9]{8}\b(-[a-fA-F0-9]{4}){3}-[a-fA-F0-9]{12}\b");

        if (!guidMatch.Success)
        {
            throw new InvalidOperationException(">>>> No Order ID of GUID format found in the message.");
        }

        Guid orderId = Guid.Parse(guidMatch.Value);
        Console.WriteLine($">>>> Extracted Order ID: {orderId}");
        return orderId.ToString();
    }
}