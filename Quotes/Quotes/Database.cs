using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Quotes
{
    /// <summary>
    /// Database class that holds two data-tables: data_table (used to hold individualt stocks data)
    /// and market_data_table (used to hold the individual market data)
    /// </summary>
    class Database
    {
        private DataTable data_table;
        private DataTable market_data_table;

        public DataTable Data_table
        {
            get { return data_table; }
            set { data_table = value; }
        }
        public DataTable Market_data_table
        {
            get { return market_data_table; }
            set { market_data_table = value; }
        }
        /// <summary>
        /// Timer that ticks every 5 seconds to pull the XML file
        /// </summary>
        private Timer updateTimer;

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="tableName">name of the table that holds individual stock data</param>
        /// <param name="colNames">Column names in both tables</param>
        public Database(string tableName, string[] colNames)
        {
            data_table = new DataTable(tableName);
            market_data_table = new DataTable("Market Table");

            foreach (string s in colNames)
            {
                data_table.Columns.Add(s);
            }

            foreach (string s in colNames)
            {
                market_data_table.Columns.Add(s);
            }

            updateTimer = new Timer();
            updateTimer.Interval = 5000; //Change the value here to increase/decrease the update time
            updateTimer.Tick += new EventHandler(updateTimer_Tick);
            updateTimer.Enabled = true;
        }

        void updateTimer_Tick(object sender, EventArgs e)
        {
            //Fetching all the stocks at once in XDocument file
            XDocument doc = Stock_quotes.FetchQuote(this.getAllSymbolsFromTable(data_table) + Main_view.market_symbol_string);
            //This will update the data_table            
            this.addValuesToTheTable(data_table, doc);
            //This will update the market_table
            this.addValuesToTheTable(market_data_table, doc);
        }

        /// <summary>
        /// Adds a stock symbol to the table or throws an ArgumentException
        /// </summary>
        /// <param name="symbol">symbol(s) to the added. Multiple entries are allowed that are separated by " " or ","</param>
        /// <param name="table"></param>
        public void addStockSymbolToTheTable(string symbol, DataTable table)
        {
            if (symbol != null && symbol.Length > 0)
            {
                XDocument xDoc = Stock_quotes.FetchQuote(symbol);
                List<Stock> list = Stock_quotes.getValidStocks(xDoc);
                foreach (Stock stock in list)
                {
                    table.Rows.Add(stock.Symbol, stock.Company ,stock.Date, stock.Time, stock.Y_close, stock.Trade, stock.Chg, stock.Perc_chg, stock.Volume, stock.High, stock.Low, stock.Chart_url, stock.Market_cap, stock.Exchange, stock.Currency);
                }
               
            }
            else
            {
                throw new ArgumentException("Added symbol is not accepted as a valid input");
            }
        }

        
        /// <summary>
        /// Gets all the symbols (in the symbol column) from the table
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public string getAllSymbolsFromTable(DataTable table)
        {
            StringBuilder result = new StringBuilder();
            foreach (DataRow row in table.Rows)
            {
                result.Append(row["Symbol"] + " ");
            }
            return result.ToString();
        }

        /// <summary>
        /// Updates the table data
        /// </summary>
        /// <param name="table"></param>
        /// <param name="doc"></param>
        public void addValuesToTheTable(DataTable table, XDocument doc)
        {
            foreach (DataRow row in table.Rows)
            {
                Stock stock = Stock_quotes.getThisStock(doc, (string)row["Symbol"], "symbol");
                row["Symbol"] = stock.Symbol;
                row["Company"] = stock.Company;
                row["Date"] = stock.Date;
                row["Time"] = stock.Time;
                row["Closed Yesterday"] = stock.Y_close;
                row["Trade"] = stock.Trade;
                row["Chg"] = stock.Chg;
                row["%Chg"] = stock.Perc_chg;
                row["Volume"] = stock.Volume;
                row["High"] = stock.High;
                row["Low"] = stock.Low;
                row["Chart"] = stock.Chart_url;
                row["Market Cap"] = stock.Market_cap;
                row["Exchange"] = stock.Exchange;
                row["Currency"] = stock.Currency;
            }
        }

        /// <summary>
        /// Retrives Chart URL from the table based on the stock symbol
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public string getChartURL(string symbol, DataTable table)
        {
            string result = string.Empty;
            if (table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    if (((string)row["Symbol"]).Equals(symbol))
                    {
                        result = (string)row["Chart"];
                        break;
                    }
                }
                return result;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// Saves the symbols that user has entered into the settings file 
        /// </summary>
        public void saveSymbols()
        {
            Properties.Settings.Default.symbols = new System.Collections.Specialized.StringCollection();
            foreach (DataRow row in data_table.Rows)
            {
                Properties.Settings.Default.symbols.Add((string)row["Symbol"]);
            }
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Loads symbols that user had entered previously from the settings file
        /// </summary>
        public void loadSavedSymbols()
        {
            var list = Properties.Settings.Default.symbols;
            
            if (list !=null && list.Count != 0)
            {
                StringBuilder symbols = new StringBuilder();
                foreach (string s in list)
                {
                    symbols.Append(s + " ");
                }
                try
                {
                    this.addStockSymbolToTheTable(symbols.ToString(), data_table);
                }
                catch (ArgumentException ar)
                {
                    MessageBox.Show(ar.Message);
                }
            }
            
        }




    }
}
