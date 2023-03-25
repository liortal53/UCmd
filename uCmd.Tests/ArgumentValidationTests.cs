using NUnit.Framework;
using System;

namespace uCmd.Tests
{
    [TestFixture]
    [Category("Command line argument validation tests")]
    class ArgumentValidationTests
    {
        // Test argument validation

        [Test]
        public void RunWithCommandLineArgs_Null_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                UCmd cmd = new UCmd();
                cmd.RunWithCommandLineArgs(null);
            });
        }

        [Test]
        public void RunWithCommandLineArgs_EmptyArray_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                UCmd cmd = new UCmd();
                cmd.RunWithCommandLineArgs(new string[0]);
            });
        }

        [Test]
        public void RunWithCommandLineArgs_MissingExecuteMethodArgument_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                UCmd cmd = new UCmd();
                cmd.RunWithCommandLineArgs(new[] { "UCmd.Run", "TestClass.TestMethod" });
            });
        }

        [Test]
        public void RunWithCommandLineArgs_MissingUCmdMethodArgument_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                UCmd cmd = new UCmd();
                cmd.RunWithCommandLineArgs(new[] { "-executeMethod", "TestClass.TestMethod" });
            });
        }

        [Test]
        public void RunWithCommandLineArgs_MissingExecuteMethodArgumentWithPrecedingArgs_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                UCmd cmd = new UCmd();
                cmd.RunWithCommandLineArgs(new[] { "a", "b", "c", "UCmd.Run", "TestClass.TestMethod" });
            });
        }

        [Test]
        public void RunWithCommandLineArgs_MissingUCmdMethodArgumentWithPrecedingArgs_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                UCmd cmd = new UCmd();
                cmd.RunWithCommandLineArgs(new[] { "a", "b", "c", "-executeMethod", "TestClass.TestMethod" });
            });
        }
    }
}
