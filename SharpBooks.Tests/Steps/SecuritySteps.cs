namespace SharpBooks.Tests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TechTalk.SpecFlow;

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
                symbol: "Currency - " + name,
                format: new CurrencyFormat(currencySymbol: name, positiveFormat: PositiveFormat.SuffixSpaced, negativeFormat: NegativeFormat.Prefix),
                fractionTraded: 100);

            ScenarioContext.Current.Add(name, security);
        }
    }
}
