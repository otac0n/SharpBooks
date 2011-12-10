namespace SharpBooks.Tests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TechTalk.SpecFlow;

    [Binding]
    public class AccountSteps
    {
        [Given(@"an account '(.*)' with security '(.*)'")]
        public void GivenAnAccountWithSecurity(string accountName, string securityName)
        {
            var security = (Security)ScenarioContext.Current[securityName];

            var account = new Account(
                accountId: Guid.NewGuid(),
                security: security,
                parentAccount: null,
                name: accountName,
                smallestFraction: security.FractionTraded);

            ScenarioContext.Current.Add(accountName, account);
        }

        [Given(@"an account '(.*)' with no security")]
        public void GivenAnAccountWithNoSecurity(string accountName)
        {
            var account = new Account(
                accountId: Guid.NewGuid(),
                security: null,
                parentAccount: null,
                name: accountName,
                smallestFraction: null);

            ScenarioContext.Current.Add(accountName, account);
        }
    }
}
