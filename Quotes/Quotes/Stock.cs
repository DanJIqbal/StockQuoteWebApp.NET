using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quotes
{
    /// <summary>
    /// Simple Stock object class used to transfer data from XML file to the Data-table
    /// </summary>
    class Stock
    {
        private string symbol;

        public string Symbol
        {
            get { return symbol; }
            set { symbol = value; }
        }
        private string date;

        public string Date
        {
            get { return date; }
            set { date = value; }
        }
        private string time;

        public string Time
        {
            get { return time; }
            set { time = value; }
        }
        private string trade;

        public string Trade
        {
            get { return trade; }
            set { trade = value; }
        }
        private string chg;

        public string Chg
        {
            get { return chg; }
            set { chg = value; }
        }
        private string perc_chg;

        public string Perc_chg
        {
            get { return perc_chg; }
            set { perc_chg = value; }
        }
        private string volume;

        public string Volume
        {
            get { return volume; }
            set { volume = value; }
        }
        private string high;

        public string High
        {
            get { return high; }
            set { high = value; }
        }
        private string low;

        public string Low
        {
            get { return low; }
            set { low = value; }
        }

        private string chart_url;

        public string Chart_url
        {
            get { return chart_url; }
            set { chart_url = value; }
        }

        private string market_cap;

        public string Market_cap
        {
            get { return market_cap; }
            set { market_cap = value; }
        }
        private string exchange;

        public string Exchange
        {
            get { return exchange; }
            set { exchange = value; }
        }
        private string currency;

        public string Currency
        {
            get { return currency; }
            set { currency = value; }
        }

        private string company;

        public string Company
        {
            get { return company; }
            set { company = value; }
        }
        private string y_close;

        public string Y_close
        {
            get { return y_close; }
            set { y_close = value; }
        }


        public Stock()
        {
            symbol = string.Empty;
            date = string.Empty;
            time = string.Empty;
            trade = string.Empty;
            chg = string.Empty;
            perc_chg = string.Empty;
            volume = string.Empty;
            high = string.Empty;
            low = string.Empty;
            chart_url = string.Empty;
            market_cap = string.Empty;
            exchange = string.Empty;
            currency = string.Empty;
            company = string.Empty;
            y_close = string.Empty;
        }
    }
}
