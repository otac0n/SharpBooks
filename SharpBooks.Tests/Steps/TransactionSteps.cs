namespace SharpBooks.Tests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class TransactionSteps
    {
        [Then(@"transaction '(.*)' is valid")]
        [Then(@"transaction '(.*)' should be valid")]
        public void ThenTransactionIsValid(string transactionName)
        {
            var transaction = (Transaction)ScenarioContext.Current[transactionName];

            Assert.That(transaction.IsValid, Is.True);
        }

        [Then(@"transaction '(.*)' is invalid")]
        [Then(@"transaction '(.*)' should be invalid")]
        public void ThenTransactionIsInvalid(string transactionName)
        {
            var transaction = (Transaction)ScenarioContext.Current[transactionName];

            Assert.That(transaction.IsValid, Is.False);
        }

        [Given(@"an empty transaction '(.*)' with the security '(.*)'")]
        public void GivenAnEmptyTransactionWithTheSecurity(string transactionName, string securityName)
        {
            var security = (Security)ScenarioContext.Current[securityName];

            var transaction = new Transaction(
                transactionId: Guid.NewGuid(),
                baseSecurity: security);

            ScenarioContext.Current.Add(transactionName, transaction);
        }

        [Given(@"a transaction '(.*)' with the following splits")]
        public void GivenATransactionWithTheFollowingSplits(string transactionName, Table table)
        {
            this.GivenATransactionWithBaseSecurityAndTheFollowingSplits(transactionName, null, table);
        }

        [Given(@"a transaction '(.*)' with base security '(.*)' and the following splits")]
        public void GivenATransactionWithBaseSecurityAndTheFollowingSplits(string transactionName, string baseSecurity, Table table)
        {
            Assume.That(table.Header, Has.Member("Account"));
            Assume.That(table.Header, Has.Member("Security"));
            Assume.That(table.Header, Has.Member("Amount"));

            var accounts = (from a in table.Rows
                            group a by a["Account"] into g
                            let acctName = g.Key
                            let account = (Account)ScenarioContext.Current[acctName]
                            select new
                            {
                                acctName,
                                account
                            }).ToDictionary(a => a.acctName, a => a.account);

            var securities = (from a in table.Rows
                              group a by a["Security"] into g
                              let secName = g.Key
                              let security = (Security)ScenarioContext.Current[secName]
                              select new
                              {
                                  secName,
                                  security
                              }).ToDictionary(a => a.secName, a => a.security);

            Security transactionSecurity;

            if (string.IsNullOrEmpty(baseSecurity))
            {
                var firstAccount = accounts[table.Rows[0]["Account"]];
                transactionSecurity = firstAccount.Security;
            }
            else
            {
                transactionSecurity = (Security)ScenarioContext.Current[baseSecurity];
            }

            var transaction = new Transaction(
                transactionId: Guid.NewGuid(),
                baseSecurity: transactionSecurity);

            var securityCount = (from a in accounts
                                 group a.Value by a.Value.Security into g
                                 select g.Key).Count();

            if (securityCount > 1)
            {
                Assume.That(table.Header, Has.Member("Transaction Amount"));
            }

            foreach (var row in table.Rows)
            {
                var account = accounts[row["Account"]];
                var security = securities[row["Security"]];
                var amount = long.Parse(row["Amount"]);
                var transactionAmount = securityCount > 1 ? long.Parse(row["Transaction Amount"]) : amount;

                var split = transaction.AddSplit();
                split.SetAccount(account);
                split.SetSecurity(security);
                split.SetAmount(amount);
                split.SetTransactionAmount(transactionAmount);
            }

            ScenarioContext.Current.Add(transactionName, transaction);
        }

        [When(@"the following splits are added to transaction '(.*)'")]
        public void WhenTheFollowingSplitsAreAddedToTransaction(string transactionName, Table table)
        {
            var transaction = (Transaction)ScenarioContext.Current[transactionName];

            var accounts = (from a in table.Rows
                            group a by a["Account"] into g
                            let acctName = g.Key
                            let account = (Account)ScenarioContext.Current[acctName]
                            select new
                            {
                                acctName,
                                account
                            }).ToDictionary(a => a.acctName, a => a.account);

            var securityCount = (from a in accounts
                                 group a.Value by a.Value.Security into g
                                 select g.Key).Count();

            if (securityCount > 1)
            {
                Assume.That(table.Header, Has.Member("Transaction Amount"));
            }

            foreach (var row in table.Rows)
            {
                var account = accounts[row["Account"]];
                var amount = long.Parse(row["Amount"]);
                var transactionAmount = securityCount > 1 ? long.Parse(row["Transaction Amount"]) : amount;

                var split = transaction.AddSplit();
                split.SetAccount(account);
                split.SetSecurity(account.Security);
                split.SetAmount(amount);
                split.SetTransactionAmount(transactionAmount);
            }
        }
    }
}
