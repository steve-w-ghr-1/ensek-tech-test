// TODO: check if alt to managing state
// the below allows features to run in parallel. However, based on how the API feature files have been composed
// to reflect the API and using static test data we have shared data states. This has has a high chance of race conditions. 

//using NUnit.Framework;
//[assembly: Parallelizable(ParallelScope.Fixtures)]

namespace EnsekTestAutomation;

/*Typically we need to identify the environment the tests are being executed against. This
 would lead to configuration being used for the environment being tested e,g. connection db settings, 
 baseUrl taxonomy uses reference to the environment. For the system under test (SUT) this was not
 required, and therefore not implemented.
 */
public class Configuration
{
    public static readonly Dictionary<string, string> BaseUrls = new()
    {
        { "ensekTestApiApp", "https://qacandidatetest.ensek.io/ENSEK/" }
    };
}