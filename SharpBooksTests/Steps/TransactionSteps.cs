using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using NUnit.Framework;

namespace SharpBooks.Tests.Steps
{
    [Binding]
    public class TransactionSteps
    {
        [Then(@"transaction '(.*)' is locked")]
        [Then(@"transaction '(.*)' should be locked")]
        public void ThenTransactionShouldBeLocked(string transactionName)
        {
            var transaction = (Transaction)ScenarioContext.Current[transactionName];

            Assert.That(transaction.IsLocked, Is.True);
        }

        [Then(@"transaction '(.*)' is unlocked")]
        [Then(@"transaction '(.*)' should be unlocked")]
        public void ThenTransactionShouldBeUnlocked(string transactionName)
        {
            var transaction = (Transaction)ScenarioContext.Current[transactionName];

            Assert.That(transaction.IsLocked, Is.False);
        }

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
            Assume.That(table.Header, Has.Member("Account"));
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

            var firstAccount = accounts[table.Rows[0]["Account"]];

            var transaction = new Transaction(
                transactionId: Guid.NewGuid(),
                baseSecurity: firstAccount.Security);

            var securityCount = (from a in accounts
                                 group a.Value by a.Value.Security into g
                                 select g.Key).Count();

            if (securityCount > 1)
            {
                Assume.That(table.Header, Has.Member("Transaction Amount"));
            }

            using (var tLock = transaction.Lock())
            {
                foreach (var row in table.Rows)
                {
                    var account = accounts[row["Account"]];
                    var amount = long.Parse(row["Amount"]);
                    var transactionAmount = securityCount > 1 ? long.Parse(row["Transaction Amount"]) : amount;

                    var split = transaction.AddSplit(tLock);
                    split.SetAccount(account, tLock);
                    split.SetAmount(amount, tLock);
                    split.SetTransactionAmount(transactionAmount, tLock);
                }
            }

            ScenarioContext.Current.Add(transactionName, transaction);
        }
    }
}
