using System.CommandLine;
using System;

namespace TestProject
{
    public class Tests
    {
        Option<bool> _headlessOption;
        Option<string> _nameOption;

        class TestCLParser : CommandLineParser.CLParser
        {
            public TestCLParser(string[] cmdArgs) : base(cmdArgs) { }
        }

        [SetUp]
        public void Setup()
        {
            _headlessOption = new Option<bool>("--headless") { Description = "Run the application in headless mode without GUI." };
            _nameOption = new Option<string>("--name") { Description = "Your name" };
        }

        /// <summary>
        /// Test for no command line arguments.
        /// </summary>
        [Test]
        public void Test1()
        {
            TestCLParser cLParser = new TestCLParser(new string[] { "TestApp.exe" });
            cLParser.Add(_headlessOption);
            cLParser.Add(_nameOption);
            Assert.IsFalse(cLParser.HasErrors());
        }

        /// <summary>
        /// Test for invalid command line arguments.
        /// </summary>
        [Test]
        public void Test2()
        {
            TestCLParser cLParser = new TestCLParser(new string[] { "TestApp.exe", "asdjklasd" });
            cLParser.Add(_headlessOption);
            cLParser.Add(_nameOption);
            Assert.IsTrue(cLParser.HasErrors());
        }

        /// <summary>
        /// Test for invalid command line arguments.
        /// </summary>
        [Test]
        public void Test3()
        {
            TestCLParser cLParser = new TestCLParser(new string[] { "TestApp.exe", "--djklasd" });
            cLParser.Add(_headlessOption);
            cLParser.Add(_nameOption);
            Assert.IsTrue(cLParser.HasErrors());
        }

        /// <summary>
        /// Test for invalid command line arguments.
        /// </summary>
        [Test]
        public void Test4()
        {
            TestCLParser cLParser = new TestCLParser(new string[] { "TestApp.exe", "--nameAndy" });
            cLParser.Add(_headlessOption);
            cLParser.Add(_nameOption);
            Assert.IsTrue(cLParser.HasErrors());
        }

        /// <summary>
        /// Test for missing command arguments.
        /// </summary>
        [Test]
        public void Test5()
        {
            TestCLParser cLParser = new TestCLParser(new string[] { "TestApp.exe", "--name" });
            cLParser.Add(_headlessOption);
            cLParser.Add(_nameOption);
            Assert.IsTrue(cLParser.HasErrors());
        }

        /// <summary>
        /// Test for string missing line arguments.
        /// </summary>
        [Test]
        public void Test6()
        {
            TestCLParser cLParser = new TestCLParser(new string[] { "TestApp.exe", "--name", "Andy" });
            cLParser.Add(_headlessOption);
            cLParser.Add(_nameOption);
            Assert.IsFalse(cLParser.HasErrors());
            Assert.AreEqual("Andy", cLParser.GetValueForOption<string>("--name"));
        }

        /// <summary>
        /// Test for bool missing line arguments.
        /// </summary>
        [Test]
        public void Test7()
        {
            TestCLParser cLParser = new TestCLParser(new string[] { "TestApp.exe", "--name", "Andy" });
            cLParser.Add(_headlessOption);
            cLParser.Add(_nameOption);
            Assert.IsFalse(cLParser.HasErrors());
            Assert.AreEqual(false, cLParser.GetValueForOption<bool>("--headless"));
        }

        /// <summary>
        /// Test for bool missing line arguments.
        /// </summary>
        [Test]
        public void Test8()
        {
            TestCLParser cLParser = new TestCLParser(new string[] { "TestApp.exe", "--name", "Andy", "--headless" });
            cLParser.Add(_headlessOption);
            cLParser.Add(_nameOption);
            Assert.IsFalse(cLParser.HasErrors());
            Assert.AreEqual(true, cLParser.GetValueForOption<bool>("--headless"));
        }
    }
}