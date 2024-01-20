namespace Mosyle.Exchange.Test.UnitTest
{
    using Mosyle.Exchange.Test.Service;
    using Mosyle.Exchange.Test.Service.Model;

    public class ExchangeTest
    {
        CurrencyExchange currencyExchange;

        public ExchangeTest()
        {
            currencyExchange = new CurrencyExchange();
        }

        private double GetExchange(ExchangeDTO exchange)
        {
            return currencyExchange.
                currencies.Find(c => c.Currency == exchange.CurrencyCodeO).Rate / currencyExchange.
                currencies.Find(c => c.Currency == exchange.CurrencyCodeD).Rate * exchange.ValueD;
        }
        [Fact]
        public void TestExchangeMath()
        {
            ExchangeDTO param = new ExchangeDTO() { CurrencyCodeD = "GBP", ValueD = 2.0, CurrencyCodeO = "BRL" };
            Assert.Equal(GetExchange(param), currencyExchange.GetExchangeOnDemand(param));
        }

        [Fact]
        public void TestBThrowsOnDemandExchange()
        {
            ExchangeDTO param = new ExchangeDTO()
            {
                CurrencyCodeD = "RMD", //Invalid Currency
                ValueD = 2.0,
                CurrencyCodeO = "BRL"
            };

            Assert.Throws<NullReferenceException>(() => currencyExchange.GetExchangeOnDemand(param));
        }

        //XUnit Musyle's Challenger

        [Fact]
        public void TestOnDemandExchange()
        {
            ExchangeDTO param = new ExchangeDTO() { CurrencyCodeD = "GBP", ValueD = 2.0, CurrencyCodeO = "BRL" };
            Assert.True(currencyExchange.GetExchangeOnDemand(param) > 0);
        }

        [Fact]
        public void TestAllExchange()
        {
            foreach (var currency in currencyExchange.currencies)
            {
                ExchangeDTO param = new ExchangeDTO()
                {
                    ValueD = 2.0,
                    CurrencyCodeO = currency.Currency
                };

                foreach (var currencyD in currencyExchange.currencies.Where(c => c.Currency != param.CurrencyCodeO))
                {
                    param.CurrencyCodeD = currencyD.Currency;
                    Assert.True(currencyExchange.GetExchangeOnDemand(param) > 0);
                }
            }
        }
    }

}