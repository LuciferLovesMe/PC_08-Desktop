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
    public partial class ReportForm : Form
    {
        SqlCommand command;
        SqlConnection connection = new SqlConnection(Utils.conn);
        
        Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
        public ReportForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainFrontOffice main = new MainFrontOffice();
            this.Hide();
            main.ShowDialog();
        }

        void loadgrid()
        {
            command = new SqlCommand("select Reservation.*, ReservationRoom.CheckInDateTime, ReservationRoom.CheckOutDateTime, Room.RoomNumber from Reservation join ReservationRoom on ReservationRoom.ReservationID = Reservation.ID join Room on room.ID = ReservationRoom.RoomID", connection);
            dataGridView1.DataSource = Command.getdata(command);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            loadgrid();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            app.Workbooks.Add(Type.Missing);
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                app.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
            }

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    app.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value;
                }
            }

            app.Visible = true;
        }
    }
}
