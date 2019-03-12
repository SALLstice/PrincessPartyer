using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace PrincessPartyer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SQLiteConnection m_dbConnection;

            m_dbConnection = new SQLiteConnection("Data Source=C:/Users/Matt/Documents/sqllite/WUAS.db;Version=3;");
            m_dbConnection.Open();

            SQLiteCommand command = new SQLiteCommand("select Package from packages", m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                cmbPackages.Items.Add(reader["Package"]);

            command = new SQLiteCommand("select Extras from extras", m_dbConnection);
            reader = command.ExecuteReader();
            while (reader.Read())
                clbExtras.Items.Add(reader["Extras"]);

            command = new SQLiteCommand("select Princess from princesses", m_dbConnection);
            reader = command.ExecuteReader();
            while (reader.Read())
                cmbPrincess.Items.Add(reader["Princess"]);

            command = new SQLiteCommand("select PaymentMethod from paymentmethods", m_dbConnection);
            reader = command.ExecuteReader();
            while (reader.Read())
                cmbPaymentMethod.Items.Add(reader["PaymentMethod"]);

            command = new SQLiteCommand("select DiscoveredFrom from discovered", m_dbConnection);
            reader = command.ExecuteReader();
            while (reader.Read())
                cmbReference.Items.Add(reader["DiscoveredFrom"]);

            m_dbConnection.Close();

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        { }


        private void button1_Click(object sender, EventArgs e)
        {
            SQLiteConnection m_dbConnection;

            m_dbConnection = new SQLiteConnection("Data Source=C:/Users/Matt/Documents/sqllite/WUAS.db;Version=3;");
            m_dbConnection.Open();

            string sqlcols, sqlvals, sql;
            SQLiteCommand command;
            SQLiteDataReader reader;
            string lastEventID;

            sqlcols = "insert into events (BookDate, EventDate,Princess,Package,Extras,PartyAddress) ";
            sqlvals = "values ('";
            sqlvals += calBookDate.Text + "', '";
            sqlvals += calPartyDate.Text + "', '";
            sqlvals += (cmbPrincess.SelectedIndex+1) + "', '";
            sqlvals += (cmbPackages.SelectedIndex+1) + "', '";
            sqlvals += "0" + "', '";
            sqlvals += txtPartyAddress.Text + "')";
            sql = sqlcols + sqlvals;
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            
            SQLiteCommand cmd = new SQLiteCommand("SELECT last_insert_rowid() from events", m_dbConnection);
            reader = cmd.ExecuteReader();
            lastEventID = reader["last_insert_rowid()"].ToString();

            sqlcols = "insert into customer (ParentName, ChildName, ChildAge, Address, Reference, EventID) ";
            sqlvals = "values ('";
            sqlvals += txtParentName.Text + "', '";
            sqlvals += txtChildName.Text + "', '";
            sqlvals += txtChildAge.Text + "', '";
            sqlvals += txtHomeAddress.Text + "', '";
            sqlvals += cmbReference.Text + "', '";
            sqlvals += lastEventID + "')";
            sql = sqlcols + sqlvals;

            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery(); m_dbConnection = new SQLiteConnection("Data Source=C:/Users/Matt/Documents/sqllite/WUAS.db;Version=3;");

            m_dbConnection.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            label13.Text = cmbPrincess.SelectedIndex.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=C:/Users/Matt/Documents/sqllite/WUAS.db;Version=3;");
            m_dbConnection.Open();

            SQLiteCommand command;
            SQLiteDataReader reader;
            foreach (var series in BarGraph.Series)
            {
                series.Points.Clear();
            }

            command = new SQLiteCommand("SELECT strftime('%m',EventDate) || '/' || strftime('%Y', EventDate) as Date, count(*) as count FROM events AS e GROUP BY Date ORDER BY Date ASC", m_dbConnection);
            reader = command.ExecuteReader();
            while (reader.Read())
                BarGraph.Series["Dates"].Points.AddXY(reader["Date"], reader["Count"]);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SQLiteConnection m_dbConnection;

            m_dbConnection = new SQLiteConnection("Data Source=C:/Users/Matt/Documents/sqllite/WUAS.db;Version=3;");
            m_dbConnection.Open();
            foreach (var series in BarGraph.Series)
            {
                series.Points.Clear();
            }

            SQLiteCommand command;
            SQLiteDataReader reader;
            
            command = new SQLiteCommand("SELECT strftime('%m',BookDate) || '/' || strftime('%Y', BookDate) as Date, count(*) as count FROM events AS e GROUP BY Date ORDER BY Date ASC", m_dbConnection);
            reader = command.ExecuteReader();
            while (reader.Read())
                BarGraph.Series["Dates"].Points.AddXY(reader["Date"], reader["Count"]);
        }
    }
}
