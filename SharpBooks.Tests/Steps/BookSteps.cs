namespace SharpBooks.Tests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TechTalk.SpecFlow;

    [Binding]
    public class BookSteps
    {
        [Given(@"a book")]
        public void GivenABook()
        {
            var book = new Book();

            ScenarioContext.Current.Set(book);
        }

        [When(@"I add the security '(.*)' to the book")]
        public void WhenIAddTheSecurityToTheBook(string securityName)
        {
            var security = (Security)ScenarioContext.Current[securityName];

            var book = ScenarioContext.Current.Get<Book>();

            book.AddSecurity(security);
        }
        
        [When(@"I add the account '(.*)' to the book")]
        public void WhenIAddTheAccountToTheBook(string accountName)
        {
            var account = (Account)ScenarioContext.Current[accountName];

            var book = ScenarioContext.Current.Get<Book>();

            book.AddAccount(account);
        }
        
        [When(@"I add transaction '(.*)' to the book")]
        public void WhenIAddTransactionToTheBook(string transactionName)
        {
            var transaction = (Transaction)ScenarioContext.Current[transactionName];

            var book = ScenarioContext.Current.Get<Book>();

            book.AddTransaction(transaction);
        }

        [When(@"I remove transaction '(.*)' from the book")]
        public void WhenIRemoveTransactionFromTheBook(string transactionName)
        {
            var transaction = (Transaction)ScenarioContext.Current[transactionName];

            var book = ScenarioContext.Current.Get<Book>();

            book.RemoveTransaction(transaction);
        }
    }
}
