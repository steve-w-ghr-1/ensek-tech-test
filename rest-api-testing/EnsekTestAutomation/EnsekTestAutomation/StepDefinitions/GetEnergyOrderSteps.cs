using EnsekTestAutomation.Utils;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Reqnroll;
using RestSharp;

namespace EnsekTestAutomation.StepDefinitions;

[Binding]
public class GetEnergyOrderSteps
{
    private readonly RestHelper _restHelper;
    private readonly ScenarioContext _scenarioContext;

    public GetEnergyOrderSteps(RestHelper restHelper, ScenarioContext scenarioContext)
    {
        _restHelper = restHelper;
        _scenarioContext = scenarioContext;
    }
    
    [Then("I see my energy order in the current order list")]
    public async Task ThenISeeMyEnergyOrderInTheCurrentOrderList()
    {
        await GetAllOrderDetails();
        var response = _scenarioContext.Get<RestResponse>("ApiResponse");
        var responseBody = JToken.Parse(response.Content);

        var orderId = ScenarioContextHelper.GetFromScenarioContext<string>(_scenarioContext, "orderId");
        var isOnlyOneMatch = JsonHelper.HasExactlyOneMatchById(responseBody, orderId);
        isOnlyOneMatch.Should().BeTrue(">>>> The order was present but appeared twice");
    }
    
    [When("I request to view all orders")]
    public async Task GetAllOrderDetails()
    {
        var baseUrl = Configuration.BaseUrls["ensekTestApiApp"];
        var resource = $"orders";
        _restHelper.SetHttpClient(baseUrl);

        var response = await _restHelper.SendGetRequest(resource);
        _scenarioContext["ApiResponse"] = response;
    }
    
    [Then("I see '(.*)' orders placed before today")]
    public async Task ThenISeeOrdersPlacedBeforeToday(int totalOrders)
    {
        await GetAllOrderDetails();
        var response = ScenarioContextHelper.GetFromScenarioContext<RestResponse>(_scenarioContext, "ApiResponse");
        var responseBody = JToken.Parse(response.Content);
        var totalOrdersBeforeToday = CountOrdersBeforeToday(responseBody);
        totalOrdersBeforeToday.Should().Be(totalOrders);
    }
    
    public static int CountOrdersBeforeToday(JToken orders)
    {
        var count = 0;

        foreach (var order in orders)
        {
            var timeString = order["time"]?.ToString();
            if (!DateTime.TryParse(timeString, out var parsedTime)) throw new InvalidOperationException($">>>> Error with DateTime for order {order}");
            if (parsedTime.ToUniversalTime() < DateTime.Today)
                count++;
        }

        return count;
    }
}