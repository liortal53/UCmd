using NUnit.Framework;

namespace uCmd.Tests
{
    [TestFixture]
    public class PropertiesTests
    {
        [Test]
        public void SetStaticProperties_NonExistingProperty_NoException()
        {
            var args = new[] { "nonExisting", "42" };
            UCmd.SetStaticProperties("uCmd.Tests.PropertiesClass", args);
        }

        [Test]
        public void SetStaticProperties_Int_CorrectValue()
        {
            try
            {
                var args = new[] { "x", "42" };
                UCmd.SetStaticProperties("uCmd.Tests.PropertiesClass", args);

                Assert.AreEqual(42, PropertiesClass.x);
            }
            finally
            {
                PropertiesClass.x = 0;
            }
        }

        [Test]
        public void SetStaticProperties_PropertyWithPrivateSet_CorrectValue()
        {
            var args = new[] { "y", "99" };
            UCmd.SetStaticProperties("uCmd.Tests.PropertiesClass", args);

            Assert.AreEqual(99, PropertiesClass.y);
        }

        [Test]
        public void SetStaticProperties_EnumShortNotation_CorrectValue()
        {
            try
            {
                var args = new[] { "x", "42", "enumType", "TestOptionB" };
                UCmd.SetStaticProperties("uCmd.Tests.PropertiesClass", args);

                Assert.AreEqual(TestEnum.TestOptionB, PropertiesClass.enumType);
            }
            finally
            {
                PropertiesClass.x = 0;
            }
        }

        [Test]
        public void SetStaticProperties_EnumFullNotation_CorrectValue()
        {
            try
            {
                var args = new[] { "x", "42", "enumType", "TestEnum.TestOptionB" };
                UCmd.SetStaticProperties("uCmd.Tests.PropertiesClass", args);

                Assert.AreEqual(TestEnum.TestOptionB, PropertiesClass.enumType);
            }
            finally
            {
                PropertiesClass.x = 0;
            }
        }

        [Test]
        public void SetStaticProperties_NestedClass_CorrectValue()
        {
            var args = new[] { "v", "0.42", "abc", "TestEnum.TestOptionA" };

            UCmd.SetStaticProperties("uCmd.Tests.PropertiesClass", args);

            Assert.AreEqual(0.42f, PropertiesClass.Android.v);
        }
    }
}