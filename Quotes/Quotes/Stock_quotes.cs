using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Xml.Linq;
using System.Xml;

namespace Quotes
{
    static class Stock_quotes
    {
        /// <summary>
        /// Fetches the XML data in to the XDocument from the google finance apis
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static XDocument FetchQuote(string symbol)
        {
            symbol = symbol.Trim();
            symbol = symbol.Replace(" ", "&stock=");
            symbol = symbol.Replace(",", "&stock=");
            string url = "https://www.google.com/ig/api?stock=" + (symbol);
            return XDocument.Load(url);
        }

        /// <summary>
        /// Takes a XDocument, parses it and returns a list of stock objects that corresponds to valid
        /// stock symbols
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<Stock> getValidStocks(XDocument doc)
        {
            List<Stock> stocks = new List<Stock>();

            foreach (var root in doc.Root.Elements("finance"))
            {
                try
                {
                    if (root.Element("last") != null && root.Element("last").Attribute("data").Value != null && root.Element("last").Attribute("data").Value.Equals("0.00") == false)
                    {
                        stocks.Add(Stock_quotes.createNewStock(root));
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show(root.Element("symbol").Attribute("data").Value + " is not a valid stock symbol");
                    }
                }
                catch (Exception er)
                {
                    //Error message
                }

            }

            return stocks;
        }

        /// <summary>
        /// Retrieves a particular stock from the XDocument.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="symbol"></param>
        /// <param name="lookUpField"></param>
        /// <returns></returns>
        public static Stock getThisStock(XDocument doc, string symbol, string lookUpField)
        {
            Stock stock = null;

            foreach (var root in doc.Root.Elements("finance"))
            {
                if (root.Element(lookUpField).Attribute("data").Value.Equals(symbol))
                {
                    return Stock_quotes.createNewStock(root);
                }

            }

            return stock;
        }

        /// <summary>
        /// Creates a new Stock from XElement. 
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public static Stock createNewStock(XElement root)
        {
            Stock stock = new Stock();
            stock.Symbol = root.Element("symbol").Attribute("data").Value;
            DateTime eastern = Stock_quotes.UTCtoEastern(root.Element("current_date_utc").Attribute("data").Value, root.Element("current_time_utc").Attribute("data").Value);
            stock.Date = eastern.ToShortDateString();
            stock.Time = eastern.ToLongTimeString();
            stock.Trade = root.Element("last").Attribute("data").Value;
            stock.Chg = root.Element("change").Attribute("data").Value;
            stock.Perc_chg = root.Element("perc_change").Attribute("data").Value;
            stock.Volume = root.Element("volume").Attribute("data").Value;
            stock.High = root.Element("high").Attribute("data").Value;
            stock.Low = root.Element("low").Attribute("data").Value;
            stock.Chart_url = "https://www.google.com" + root.Element("chart_url").Attribute("data").Value;
            stock.Market_cap = root.Element("market_cap").Attribute("data").Value;
            stock.Exchange = root.Element("exchange").Attribute("data").Value;
            stock.Currency = root.Element("currency").Attribute("data").Value;
            stock.Company = root.Element("company").Attribute("data").Value;
            stock.Y_close = root.Element("y_close").Attribute("data").Value;

            return stock;
        }

        /// <summary>
        /// Converts date and time from UTC to Eastern standard
        /// </summary>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime UTCtoEastern(string date, string time)
        {
            int year = Convert.ToInt32(date.Substring(0, 4));
            int month = Convert.ToInt32(date.Substring(4, 2));
            int day = Convert.ToInt32(date.Substring(6, 2));

            int hours = Convert.ToInt32(time.Substring(0, 2));
            int mins = Convert.ToInt32(time.Substring(2, 2));
            int sec = Convert.ToInt32(time.Substring(4, 2));

            DateTime utcTime = new DateTime(year, month, day, hours, mins, sec, DateTimeKind.Utc);
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, easternZone);
        }


    }
}
