using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;

namespace uCmd.Tests
{
    [TestFixture]
    [Category("Method invocation tests")]
    class MethodInvocationTests
    {
        private IMethodCallReceiver methodReceiver;

        [SetUp]
        public void TestSetup()
        {
            methodReceiver = Substitute.For<IMethodCallReceiver>();

            // Inject fake object to assist with asserting in each test.
            TestClass.Init(methodReceiver);
            Testing.TestClass.Init(methodReceiver);
        }

        [Test]
        public void RunWithCommandLineArgs_MethodWithNoArgs_Executed()
        {
            UCmd cmd = new UCmd();
            cmd.RunWithCommandLineArgs(GetArgs("TestClass.MethodNoArgs"));

            methodReceiver.Received(1).Call();
        }

        [Test]
        public void RunWithCommandLineArgs_MethodWithNoArgsNamespace_Executed()
        {
            UCmd cmd = new UCmd();
            cmd.RunWithCommandLineArgs(GetArgs("Testing.TestClass.MethodNoArgs"));

            methodReceiver.Received(1).Call();
        }

        [Test]
        public void RunWithCommandLineArgs_MethodWithInt_Executed()
        {
            UCmd cmd = new UCmd();
            cmd.RunWithCommandLineArgs(GetArgs("TestClass.MethodWithInt", "42"));

            methodReceiver.Received(1).Call(42);
        }

        [Test]
        public void RunWithCommandLineArgs_MethodWithIntNamespace_Executed()
        {
            UCmd cmd = new UCmd();
            cmd.RunWithCommandLineArgs(GetArgs("Testing.TestClass.MethodWithInt", "42"));

            methodReceiver.Received(1).Call(42);
        }

        [Test]
        public void RunWithCommandLineArgs_MethodWithBool_Executed()
        {
            UCmd cmd = new UCmd();
            cmd.RunWithCommandLineArgs(GetArgs("TestClass.MethodWithBool", "true"));

            methodReceiver.Received(1).Call(true);
        }

        [Test]
        public void RunWithCommandLineArgs_MethodWithBoolNamespace_Executed()
        {
            UCmd cmd = new UCmd();
            cmd.RunWithCommandLineArgs(GetArgs("Testing.TestClass.MethodWithBool", "true"));

            methodReceiver.Received(1).Call(true);
        }

        [Test]
        public void RunWithCommandLineArgs_MethodWithLong_Executed()
        {
            UCmd cmd = new UCmd();
            cmd.RunWithCommandLineArgs(GetArgs("TestClass.MethodWithLong", "2919391995"));

            methodReceiver.Received(1).Call(2919391995L);
        }

        [Test]
        public void RunWithCommandLineArgs_MethodWithDouble_Executed()
        {
            UCmd cmd = new UCmd();
            cmd.RunWithCommandLineArgs(GetArgs("TestClass.MethodWithDouble", "2342342342"));

            methodReceiver.Received(1).Call(2342342342d);
        }

        [Test]
        public void RunWithCommandLineArgs_MethodWithDouble2_Executed()
        {
            UCmd cmd = new UCmd();
            cmd.RunWithCommandLineArgs(GetArgs("TestClass.MethodWithDouble", "2342342342.492"));

            methodReceiver.Received(1).Call(2342342342.492d);
        }

        [Test]
        public void RunWithCommandLineArgs_MethodWithChar_Executed()
        {
            UCmd cmd = new UCmd();
            cmd.RunWithCommandLineArgs(GetArgs("TestClass.MethodWithChar", "a"));

            methodReceiver.Received(1).Call('a');
        }

        [Test]
        public void RunWithCommandLineArgs_MethodWithFloat_Executed()
        {
            UCmd cmd = new UCmd();
            cmd.RunWithCommandLineArgs(GetArgs("TestClass.MethodWithFloat", "42.24"));

            methodReceiver.Received(1).Call(42.24f);
        }

        [Test]
        public void RunWithCommandLineArgs_MethodWithString_Executed()
        {
            UCmd cmd = new UCmd();
            cmd.RunWithCommandLineArgs(GetArgs("TestClass.MethodWithString", "test"));

            methodReceiver.Received(1).Call("test");
        }

        [Test]
        public void RunWithCommandLineArgs_MethodWithEnum_Executed()
        {
            UCmd cmd = new UCmd();
            cmd.RunWithCommandLineArgs(GetArgs("TestClass.MethodWithEnum", "TestOptionA"));

            methodReceiver.Received(1).Call(TestEnum.TestOptionA);
        }

        [Test]
        [Ignore("Not sure if this should really be supported or not")]
        public void RunWithCommandLineArgs_MethodWithEnumUseFullName_Executed()
        {
            UCmd cmd = new UCmd();
            cmd.RunWithCommandLineArgs(GetArgs("TestClass.MethodWithEnum", "TestEnum.TestOptionA"));
        }

        [Test]
        public void RunWithCommandLineArgs_MethodWithEnumLowerCase_Executed()
        {
            UCmd cmd = new UCmd();
            cmd.RunWithCommandLineArgs(GetArgs("TestClass.MethodWithEnum", "testoptiona"));

            methodReceiver.Received(1).Call(TestEnum.TestOptionA);
        }

        [Test]
        public void RunWithCommandLineArgs_MethodWithEnumWithInvalidValue_Throws()
        {
            UCmd cmd = new UCmd();

            Assert.Throws<ArgumentException>(() =>
            {
                cmd.RunWithCommandLineArgs(GetArgs("TestClass.MethodWithEnum", "InvalidOption"));
            });
        }

        /// <summary>
        /// Helper method for generating an argument array to be used for testing method invocations
        /// </summary>
        private static string[] GetArgs(string methodToRun, params string[] args)
        {
            return new[] { "-executeMethod", "UCmd.Run", methodToRun }.Concat(args).ToArray();
        }
    }
}