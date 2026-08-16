using Microsoft.Extensions.Logging;
using Tell;

namespace Playground;

[TestClass]
public class SmokeTests
{
    [TestMethod]
    public void WorkDirAndFirstArg()
    {
        var argsString = "../../../../../superpower/play lol --args=xx";
        var args = argsString.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var tell = new TellCommand();
        var ruleParams = tell.GetRunRuleParams(args);
        Console.WriteLine(ruleParams);
    }

    [TestMethod]
    public void WorkDirAndTarget()
    {
        var argsString = "../../../ test --args=xx";
        var args = argsString.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var tell = new TellCommand();
        var ruleParams = tell.GetRunRuleParams(args);
        Console.WriteLine(ruleParams);
    }
}