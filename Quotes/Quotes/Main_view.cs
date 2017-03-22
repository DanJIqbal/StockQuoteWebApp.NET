using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Quotes
{
    /// <summary>
    /// Main form that is used to view stock data 
    /// </summary>
    public partial class Main_view : Form
    {
        private Database database;
        private PictureBox box;
        private ToolTip toolTip;
        public static string market_symbol_string = ".DJI .INX .IXIC"; //add more symbols here to include more markets in the market table
        public Main_view()
        {
            InitializeComponent();
            database = new Database("Stock Table", new string[] {"Symbol","Company", "Date", "Time","Closed Yesterday", "Trade", "Chg", "%Chg" , "Volume", "High", "Low", "Chart", "Market Cap", "Exchange", "Currency" });
            database.addStockSymbolToTheTable(market_symbol_string, database.Market_data_table);

            dataGridView1.DataSource = database.Data_table;
            dataGridView2.DataSource = database.Market_data_table;

            database.loadSavedSymbols();
            
            dataGridView1.Columns["Chart"].Visible = false;
            dataGridView2.Columns["Chart"].Visible = false;

            box = new PictureBox("https://www.google.com/finance/chart?q=NASDAQ:GOOG&tlf=12");
            toolTip = new ToolTip();
        }

        /// <summary>
        /// Whenever user enters a stock symbol in the text box and hit Add button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addSymbolButton_Click(object sender, EventArgs e)
        {
            try
            {
                database.addStockSymbolToTheTable(symbolTextBox.Text, database.Data_table);
            }
            catch (ArgumentException ar)
            {
                MessageBox.Show(ar.Message);
            }
            symbolTextBox.SelectAll();
        }

        /// <summary>
        /// To make sure change in trade values show red and green colors appropriately 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != DBNull.Value)
            {
                //+ or - sign
                if(((string)e.Value).StartsWith("+"))
                {
                    e.CellStyle.ForeColor = Color.Green;
                }
                else if (((string)e.Value).StartsWith("-"))
                {
                    e.CellStyle.ForeColor = Color.Red;
                }

                if (((string)e.Value).EndsWith("%") == true && ((string)e.Value).StartsWith("-") == false)
                {
                    e.CellStyle.ForeColor = Color.Green;
                }

            }
        }

        /// <summary>
        /// TO show chart of a stock whenever mouse enters a %Chg cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.ColumnIndex == 7 && e.RowIndex >=0)
            {
                string symbol = (string)dataGridView1.Rows[e.RowIndex].Cells["Symbol"].Value;
                string url = database.getChartURL(symbol, database.Data_table);
                if (url != string.Empty)
                {
                    box.changeChartURLofPictureBox(url);
                    box.Location = Cursor.Position;
                    box.Show();
                }
            }
        }

        /// <summary>
        /// To hide the chart.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 7)
            {
                box.Location = Cursor.Position;
                box.Hide();
            }
        }

        private void Main_view_FormClosing(object sender, FormClosingEventArgs e)
        {
            database.saveSymbols();
        }

        private void dataGridView2_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 7 && e.RowIndex >= 0)
            {
                string symbol = (string)dataGridView2.Rows[e.RowIndex].Cells["Symbol"].Value;
                string url = database.getChartURL(symbol, database.Market_data_table);
                if (url != string.Empty)
                {
                    box.changeChartURLofPictureBox(url);
                    box.Location = Cursor.Position;
                    box.Show();
                }
            }
        }

        private void dataGridView2_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 7)
            {
                box.Location = Cursor.Position;
                box.Hide();
            }
        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != DBNull.Value)
            {
                //+ or - sign
                if (((string)e.Value).StartsWith("+"))
                {
                    e.CellStyle.ForeColor = Color.Green;
                }
                else if (((string)e.Value).StartsWith("-"))
                {
                    e.CellStyle.ForeColor = Color.Red;
                }

                if (((string)e.Value).EndsWith("%") == true && ((string)e.Value).StartsWith("-") == false)
                {
                    e.CellStyle.ForeColor = Color.Green;
                }

            }
        }

        private void dataGridView1_MouseHover(object sender, EventArgs e)
        {
            int VisibleTime = 1000;  //in milliseconds
            toolTip.Show("Stocks", dataGridView1, 0, 0, VisibleTime);
        }

        private void dataGridView2_MouseHover(object sender, EventArgs e)
        {
            int VisibleTime = 1000;  //in milliseconds
            toolTip.Show("Markets", dataGridView2, 0, 0, VisibleTime);
        }


    }
}
