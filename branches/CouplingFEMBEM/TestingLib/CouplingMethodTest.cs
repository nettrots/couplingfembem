using SbB.Diploma;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace TestingLib
{
    
    
    /// <summary>
    ///This is a test class for CouplingMethodTest and is intended
    ///to contain all CouplingMethodTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CouplingMethodTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for CouplingMethod Constructor
        ///</summary>
        [TestMethod()]
        public void CouplingMethodConstructorTest()
        {
            string filename = string.Empty; // TODO: Initialize to an appropriate value
            CouplingMethod target = new CouplingMethod(@"c:\Documents and Settings\Admin\Мои документы\Visual Studio 2008\Projects\1\CouplingFEMBEM\CouplingFEMBEM\Cofig\example.yaml" );
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}
