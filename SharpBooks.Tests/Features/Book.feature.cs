// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.4.0.0
//      Runtime Version:4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
namespace SharpBooks.Tests.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.4.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Book")]
    public partial class BookFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Book.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Book", "Books hold collections of interrelated transactions, securities and accounts.", GenerationTargetLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Transactions are locked when added to a book")]
        public virtual void TransactionsAreLockedWhenAddedToABook()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Transactions are locked when added to a book", ((string[])(null)));
#line 4
this.ScenarioSetup(scenarioInfo);
#line 5
    testRunner.Given("a book");
#line 6
      testRunner.And("a currency \'C\'");
#line 7
      testRunner.And("an account \'A\' with security \'C\'");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Account",
                        "Amount"});
            table1.AddRow(new string[] {
                        "A",
                        "100"});
            table1.AddRow(new string[] {
                        "A",
                        "-100"});
#line 8
      testRunner.And("a transaction \'T\' with the following splits", ((string)(null)), table1);
#line 12
     testRunner.When("I add the security \'C\' to the book");
#line 13
      testRunner.And("I add the account \'A\' to the book");
#line 14
      testRunner.And("I add transaction \'T\' to the book");
#line 15
     testRunner.Then("transaction \'T\' should be locked");
#line hidden
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Transactions are unlocked when removed from a book")]
        public virtual void TransactionsAreUnlockedWhenRemovedFromABook()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Transactions are unlocked when removed from a book", ((string[])(null)));
#line 17
this.ScenarioSetup(scenarioInfo);
#line 18
    testRunner.Given("a book");
#line 19
      testRunner.And("a currency \'C\'");
#line 20
      testRunner.And("an account \'A\' with security \'C\'");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Account",
                        "Amount"});
            table2.AddRow(new string[] {
                        "A",
                        "100"});
            table2.AddRow(new string[] {
                        "A",
                        "-100"});
#line 21
      testRunner.And("a transaction \'T\' with the following splits", ((string)(null)), table2);
#line 25
     testRunner.When("I add the security \'C\' to the book");
#line 26
      testRunner.And("I add the account \'A\' to the book");
#line 27
      testRunner.And("I add transaction \'T\' to the book");
#line 28
      testRunner.And("I remove transaction \'T\' from the book");
#line 29
     testRunner.Then("transaction \'T\' should be unlocked");
#line hidden
            testRunner.CollectScenarioErrors();
        }
    }
}
#endregion