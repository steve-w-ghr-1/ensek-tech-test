using Newtonsoft.Json.Linq;

namespace EnsekTestAutomation.Utils;

public static class FileReadHelper
{
    public static JToken ReadFile(string fileName)
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, "Data", fileName);

        try
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File '{fileName}' not found at path {filePath}");
            }
            var jsonString = File.ReadAllText(filePath);
            Console.WriteLine($">>>>> File read successfully: {fileName}");
            var json = JToken.Parse(jsonString);
            
            return json;
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }    
}