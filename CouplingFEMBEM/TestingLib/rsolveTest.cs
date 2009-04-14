using Microsoft.VisualStudio.TestTools.UnitTesting;
using SbB.Diploma;

namespace TestingLib
{
    
    
    /// <summary>
    ///This is a test class for rsolveTest and is intended
    ///to contain all rsolveTest Unit Tests
    ///</summary>
    [TestClass()]
    public class rsolveTest
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
        ///A test for rmatrixsolve
        ///</summary>
        [TestMethod()]
        public void rmatrixsolveTest()
        {
            Matrix a = new Matrix(new double[3][]
                                      {
                                          new double[]{1,1,1},
                                          new double[]{0,1,0},
                                          new double[]{0,0,1}
        }); // TODO: Initialize to an appropriate value
            Vector b = new Vector(new double[]{3,1,1}); // TODO: Initialize to an appropriate value
            Vector x = new Vector(new double[]{1,1,1}); // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = LUSolve.Solve(a, b,  x);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
