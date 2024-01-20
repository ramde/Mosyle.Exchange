using Mosyle.Exchange.Test.Service.Model;
using System.Xml.Linq;
using System.Runtime.Caching;
using Mosyle.Exchange.Test.Service.Util;

namespace Mosyle.Exchange.Test.Service
{
    public class CurrencyExchange
    {
        private const string UriXml = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml";
        private const string CacheKey = "Currencies";
        public List<CurrencyEcb> currencies { get { return this.GetCorrencies().ToList(); } }

        public CurrencyExchange() { }

        private IEnumerable<CurrencyEcb> GetCorrencies()
        {
            ObjectCache cache = MemoryCache.Default;

            try
            {
                if (cache.Contains(CacheKey))
                    return (IEnumerable<CurrencyEcb>)cache.Get(CacheKey);

                XElement reader = XElement.Load(UriXml);

                if (!reader.HasElements)
                {
                    //Get offline Correncies
                    reader = XElement.Load("/data/eurofxrefDaily.xml");
                }

                List<CurrencyEcb> currenciesCache = new List<CurrencyEcb>();

                foreach (XElement cubeElement in reader.Elements().LastOrDefault().Elements().LastOrDefault().Elements())
                {
                    string currencyCode = cubeElement.CReaderXmlElement("currency");

                    if (currencyCode != string.Empty)
                    {
                        CurrencyEcb currency = new CurrencyEcb()
                        {
                            Currency = currencyCode,
                            Rate = cubeElement.CReaderXmlElementToDouble("rate")
                        };

                        currenciesCache.Add(currency);
                    }
                }

                CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
                cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddHours(1.0);
                cache.Add(CacheKey, currenciesCache, cacheItemPolicy);

                return currenciesCache;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public double GetExchangeOnDemand(ExchangeDTO exchange)
        {
            try
            {
                return currencies.CFindCurrency(exchange.CurrencyCodeO) / currencies.CFindCurrency(exchange.CurrencyCodeD) * exchange.ValueD;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
