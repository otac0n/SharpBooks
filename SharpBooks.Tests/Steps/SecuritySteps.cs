using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace SharpBooks.Tests.Steps
{
    [Binding]
    public class SecuritySteps
    {
        [Given(@"a currency '(.*)'")]
        public void GivenACurrency(string name)
        {
            var security = new Security(
                securityId: Guid.NewGuid(),
                securityType: SecurityType.Currency,
                name: "Test Currency '" + name + "'",
                symbol: "XTS",
                signFormat: "{0}",
                fractionTraded: 100);

            ScenarioContext.Current.Add(name, security);
        }
    }
}
