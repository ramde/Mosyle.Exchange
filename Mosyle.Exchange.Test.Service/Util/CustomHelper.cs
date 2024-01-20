using System.Globalization;
using System.Xml.Linq;
using Mosyle.Exchange.Test.Service.Model;

namespace Mosyle.Exchange.Test.Service.Util
{
    internal static class CustomHelper
    {
        internal static string CReaderXmlElement(this XElement currentElement, string atributename)
        {
            try
            {
                return currentElement.Attribute(atributename).Value;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        internal static double CReaderXmlElementToDouble(this XElement currentElement, string atributename)
        {
            try
            {
                //fomart to US-EN numbers
                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberGroupSeparator = ".";
                
                return Convert.ToDouble(currentElement.Attribute(atributename).Value, provider);
            }
            catch (Exception)
            {
                return 0.0;
            }
        }

        internal static double CFindCurrency(this List<CurrencyEcb> currencies, string currencyCode)
        {
            CurrencyEcb? currency = currencies.Find(c => c.Currency == currencyCode);

            if (currency == null)
                throw new NullReferenceException("Currency " + currencyCode + " not found!");

            return currency.Rate;
        }

        internal static double CFindCurrency(this IEnumerable<CurrencyEcb> currencies, string currencyCode)
        {
            CurrencyEcb? currency = currencies.ToList().Find(c => c.Currency == currencyCode);

            if (currency == null)
                throw new NullReferenceException("Currency " + currencyCode + " not found!");

            return currency.Rate;
        }
    }
}
