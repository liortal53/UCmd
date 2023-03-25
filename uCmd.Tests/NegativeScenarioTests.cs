using NUnit.Framework;
using System;

namespace uCmd.Tests
{
    [TestFixture]
    [Category("Negative scenarios")]
    class NegativeScenarioTests
    {
        [Test]
        public void RunWithCommandLineArgs_NonExistingMethodName_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                UCmd cmd = new UCmd();
                cmd.RunWithCommandLineArgs(new[] { "a", "b", "c", "-executeMethod", "UCmd.Run", "abcd.xyz" });
            });
        }

        // ambiguous matching methods

        [Test]
        public void RunWithCommandLineArgs_MoreThanOneMatchForMethodNameAndArgs_TT()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                UCmd cmd = new UCmd();
                cmd.RunWithCommandLineArgs(new[] { "a", "b", "c", "-executeMethod", "UCmd.Run", "AmbiguousMethodsTestClass.WithNumber", "42" });
            });
        }
    }
}
