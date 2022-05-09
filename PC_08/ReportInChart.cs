using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PC_08
{
    public partial class ReportInChart : Form
    {
        string[] month = { "Jan", "Feb", "March", "April", "May", "June", "July", "Aug", "Sep", "Oct", "Nov", "Dec" };
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;

        public ReportInChart()
        {
            InitializeComponent();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy";
            dateTimePicker1.ShowUpDown = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainFrontOffice main = new MainFrontOffice();
            this.Hide();
            main.ShowDialog();
        }

        void loadChart()
        {
            foreach(var series in chart1.Series)
            {
                series.Points.Clear();
            }
            chart1.ChartAreas[0].AxisX.Interval = 1;
            for (int i = 0; i < 12; i++)
            {
                int x = i + 1;
                int d = 0;
                for (int j = 0; j < 12; j++)
                {
                    command = new SqlCommand("select count(id) as num from reservation where month(datetime) = " + x + " and year(datetime) = " + dateTimePicker1.Text, connection);
                    connection.Open();
                    reader = command.ExecuteReader();
                    reader.Read();
                    if (reader.HasRows)
                    {
                        d = reader.GetInt32(0);

                    }
                    else
                    {
                        d = 0;
                    }
                    connection.Close();
                }

                chart1.Series["Guess"].Points.Add(d);
                chart1.Series["Guess"].Points[i].Label = d.ToString();
                chart1.Series["Guess"].Points[i].AxisLabel = month[i];
                chart1.Series["Guess"].Points[i].LegendText = month[i];
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            loadChart();
        }
    }
}
