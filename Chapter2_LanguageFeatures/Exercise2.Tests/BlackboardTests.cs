using Guts.Client.Core;
using Guts.Client.Core.TestTools;
namespace Exercise2.Tests;

[ExerciseTestFixture("dotNet2", "H02", "Exercise02", @"Exercise2\BlackBoard.cs;Exercise2\IBlackBoard.cs")]
public class BlackboardTests
{

    [MonitoredTest("BlackBoard - Should implement IBlackBoard")]
    public void _01_ShouldImplementIStudentAdministration()
    {
        Assert.That(typeof(BlackBoard).IsAssignableTo(typeof(IBlackBoard)));
    }

    [MonitoredTest("IBlackBoard interface should not have been changed")]
    public void _02_IBlackBoardInterfaceShouldNotHaveBeenChanged()
    {
        string hash = Solution.Current.GetFileHash(@"Exercise2\IBlackBoard.cs");
        Assert.That(hash, Is.EqualTo("34-C2-C7-DC-A7-08-36-66-60-05-A5-3C-B6-7E-AD-5E"));
    }
}