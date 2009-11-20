using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpCash;

namespace SharpCash.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class RecurrenceTests
    {
        public RecurrenceTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        private int CountOccurences(RecurrenceBase gen)
        {
            gen.Reset();
            int i = 0;
            while (true)
            {
                if (gen.GetNextOccurence() != null)
                {
                    i++;
                }
                else
                {
                    break;
                }
            }

            return i;
        }

        [TestMethod]
        public void MonthDayRecurrence_EndDateIsEqualToStartDate_OnlyOneOccurenceHappens()
        {
            DateTime startDate = new DateTime(2000, 1, 1);
            DateTime endDate = startDate;
            var monthly = new MonthDayRecurrence(startDate, endDate, 1, null);

            int occ = CountOccurences(monthly);

            Assert.AreEqual(1, occ);
        }

        [TestMethod]
        public void MonthDayRecurrence_EndDateIsNMonthsAway_NPlusOneOccurenceHappens()
        {
            DateTime startDate = new DateTime(2000, 1, 1);

            for (int i = 1; i <= 100; i++)
            {
                DateTime endDate = startDate.AddMonths(i);
                var monthly = new MonthDayRecurrence(startDate, endDate, 1, null);

                int occ = CountOccurences(monthly);

                Assert.AreEqual(i + 1, occ);
            }
        }

        [TestMethod]
        public void MonthDayRecurrence_EndDateIsBeforeStartDate_NoOccurencesHappen()
        {
            DateTime startDate = new DateTime(2000, 1, 1);
            DateTime endDate = startDate.AddDays(-1);
            var monthly = new MonthDayRecurrence(startDate, endDate, 1, null);

            int occ = CountOccurences(monthly);

            Assert.AreEqual(0, occ);
        }

        [TestMethod]
        public void MonthDayRecurrence_StartDateIsJanuary31_SecondOccurenceIsInFebruary()
        {
            DateTime startDate = new DateTime(2000, 1, 31);
            DateTime endDate = startDate.AddMonths(100);
            var monthly = new MonthDayRecurrence(startDate, endDate, 1, null);

            var first = monthly.GetNextOccurence();
            var second = monthly.GetNextOccurence();

            Assert.IsTrue(second.HasValue);
            Assert.AreEqual(2, second.Value.Month); // February
        }

        [TestMethod]
        public void EndOfMonthRecurrence_StartDateIsFirstOfMonth_FirstOccurenceLastDayInStartDatesMonth()
        {
            DateTime startDate = new DateTime(2000, 1, 1);
            DateTime endDate = startDate.AddMonths(100);
            var endofmonthly = new EndOfMonthRecurrence(startDate, endDate, 1, null);

            var first = endofmonthly.GetNextOccurence().Value;

            Assert.AreEqual(startDate.Year, first.Year);
            Assert.AreEqual(startDate.Month, first.Month);
            Assert.AreEqual(DateTime.DaysInMonth(startDate.Year, startDate.Month), first.Day);
        }
    }
}
