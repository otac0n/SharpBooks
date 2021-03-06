// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.5.0.0
//      Runtime Version:4.0.30319.239
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
namespace SharpBooks.Tests.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.5.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Transaction")]
    public partial class TransactionFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Transaction.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Transaction", "Transactions should be consistent", GenerationTargetLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("Transaction is invalid when it has no splits")]
        public virtual void TransactionIsInvalidWhenItHasNoSplits()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Transaction is invalid when it has no splits", ((string[])(null)));
#line 4
this.ScenarioSetup(scenarioInfo);
#line 5
    testRunner.Given("a currency \'C\'");
#line 6
      testRunner.And("an empty transaction \'T\' with the security \'C\'");
#line 7
    testRunner.Then("transaction \'T\' is invalid");
#line hidden
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Transaction is valid when it has a single zero split")]
        public virtual void TransactionIsValidWhenItHasASingleZeroSplit()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Transaction is valid when it has a single zero split", ((string[])(null)));
#line 9
this.ScenarioSetup(scenarioInfo);
#line 10
    testRunner.Given("a currency \'C\'");
#line 11
   testRunner.And("an account \'A\' with security \'C\'");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Account",
                        "Security",
                        "Amount"});
            table1.AddRow(new string[] {
                        "A",
                        "C",
                        "0"});
#line 12
      testRunner.And("a transaction \'T\' with the following splits", ((string)(null)), table1);
#line 15
    testRunner.Then("transaction \'T\' is valid");
#line hidden
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Transaction is invalid when it has a single nonzero split")]
        public virtual void TransactionIsInvalidWhenItHasASingleNonzeroSplit()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Transaction is invalid when it has a single nonzero split", ((string[])(null)));
#line 17
this.ScenarioSetup(scenarioInfo);
#line 18
    testRunner.Given("a currency \'C\'");
#line 19
   testRunner.And("an account \'A\' with security \'C\'");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Account",
                        "Security",
                        "Amount"});
            table2.AddRow(new string[] {
                        "A",
                        "C",
                        "1"});
#line 20
      testRunner.And("a transaction \'T\' with the following splits", ((string)(null)), table2);
#line 23
    testRunner.Then("transaction \'T\' is invalid");
#line hidden
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Transaction is invalid when it has unbalanced splits")]
        public virtual void TransactionIsInvalidWhenItHasUnbalancedSplits()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Transaction is invalid when it has unbalanced splits", ((string[])(null)));
#line 25
this.ScenarioSetup(scenarioInfo);
#line 26
    testRunner.Given("a currency \'C\'");
#line 27
   testRunner.And("an account \'A\' with security \'C\'");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Account",
                        "Security",
                        "Amount"});
            table3.AddRow(new string[] {
                        "A",
                        "C",
                        "1"});
            table3.AddRow(new string[] {
                        "A",
                        "C",
                        "-2"});
#line 28
      testRunner.And("a transaction \'T\' with the following splits", ((string)(null)), table3);
#line 32
    testRunner.Then("transaction \'T\' is invalid");
#line hidden
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Transaction is invalid when its base currency does not match any splits")]
        public virtual void TransactionIsInvalidWhenItsBaseCurrencyDoesNotMatchAnySplits()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Transaction is invalid when its base currency does not match any splits", ((string[])(null)));
#line 34
this.ScenarioSetup(scenarioInfo);
#line 35
    testRunner.Given("a currency \'C1\'");
#line 36
   testRunner.And("an account \'A\' with security \'C1\'");
#line 37
      testRunner.And("a currency \'C2\'");
#line 38
      testRunner.And("an empty transaction \'T\' with the security \'C2\'");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Account",
                        "Security",
                        "Amount"});
            table4.AddRow(new string[] {
                        "A",
                        "C",
                        "0"});
#line 39
     testRunner.When("the following splits are added to transaction \'T\'", ((string)(null)), table4);
#line 42
    testRunner.Then("transaction \'T\' is invalid");
#line hidden
            testRunner.CollectScenarioErrors();
        }
    }
}
#endregion
