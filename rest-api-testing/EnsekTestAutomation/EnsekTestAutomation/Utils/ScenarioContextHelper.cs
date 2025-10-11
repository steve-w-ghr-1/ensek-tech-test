using Reqnroll;

namespace EnsekTestAutomation.Utils;

public static class ScenarioContextHelper
{
    
    public static T GetFromScenarioContext<T>(ScenarioContext context, string key)
    {
        if (!context.TryGetValue(key, out var value))
            throw new InvalidOperationException($"Key '{key}' is missing from the scenario context.");

        return (T)value;
    }
    
}