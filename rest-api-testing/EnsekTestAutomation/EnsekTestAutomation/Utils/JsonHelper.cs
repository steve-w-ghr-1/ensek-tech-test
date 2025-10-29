using Newtonsoft.Json.Linq;
using Reqnroll;
using Two.JsonDeepEqual;

namespace EnsekTestAutomation.Utils;

public static class JsonHelper
{
    public static void CompareJson(JToken expectedJson, JToken actualJson, bool isArrayOrderIgnored)
    {
        var options = new JsonDeepEqualDiffOptions
        {
            IgnoreArrayElementOrder = isArrayOrderIgnored
        };
        
        JsonDeepEqualAssert.Equal(
            expectedJson,
            actualJson,
            options
        );
    }

    public static JToken UpdateJson(JToken jsonPayload, DataTable dataTable)
    {
        ArgumentNullException.ThrowIfNull(jsonPayload, nameof(jsonPayload));
        ArgumentNullException.ThrowIfNull(dataTable, nameof(dataTable));

        foreach (var row in dataTable.Rows)
        {
            var rowKey = row[0];
            var rowValue = row[1];
            var valueToUpdate = jsonPayload?.SelectToken(rowKey);
            
            //need to honour existing data types
            if (valueToUpdate != null && valueToUpdate is JValue currentValue)
            {
                var convertedValue = ConvertToJsonType(rowValue, currentValue.Type);
                currentValue.Value = convertedValue;
            }
        }
        
        return jsonPayload!;
    }

    private static object? ConvertToJsonType(string input, JTokenType targetType)
    {
        try
        {
            return targetType switch
            {
                JTokenType.Integer => int.TryParse(input, out var i) ? i : input,
                JTokenType.Float => double.TryParse(input, out var d) ? d : input,
                JTokenType.Boolean => bool.TryParse(input, out var b) ? b : input,
                JTokenType.Date => DateTime.TryParse(input, out var dt) ? dt : input,
                JTokenType.Guid => Guid.TryParse(input, out var g) ? g : input,
                JTokenType.Null => null,
                _ => input
            };
        }
        catch
        {
            // TODO - maybe throw error
            return input;
        }
    }
    
    public static string? GetValue(JToken jsonPayload, string path)
    {
        if (jsonPayload == null || string.IsNullOrWhiteSpace(path))
            return null;

        try
        {
            return jsonPayload.SelectToken(path, errorWhenNoMatch: false)?.ToString();
        }
        catch
        {
            return null;
        }
    }
    
    public static bool HasExactlyOneMatchById(JToken jsonPayload, string targetId)
    {
        int matchCount = jsonPayload.Count(obj =>
        {
            var orderId = obj["id"];
            return orderId != null && 
                   string.Equals(orderId.ToString(), targetId, StringComparison.OrdinalIgnoreCase);
        });

        return matchCount == 1;
    }
}