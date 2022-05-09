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
    public partial class CheckIn : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;
        int idRoom, idCust;

        public CheckIn()
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
            command = new SqlCommand("select reservationRoom.*, reservation.customerID from reservation join reservationRoom on reservationRoom.ReservationID = reservation.Id where reservation.bookingCode = @book", connection);
            command.Parameters.AddWithValue("@book", textBox1.Text);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            if (!reader.HasRows)
            {
                connection.Close();
                MessageBox.Show("Booking Code Could not Find!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                idCust = reader.GetInt32(8);
                connection.Close();
                dataGridView1.DataSource = Command.getdata(command);
                searchCust();
            }
        }

        void searchCust()
        {
            command = new SqlCommand("select * from customer where id = " + idCust, connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            textBox2.Text = reader.GetString(5);
            textBox3.Text =reader.GetString(1);
            textBox4.Text =reader.GetString(3);
            textBox5.Text =reader.GetInt32(6).ToString();
            textBox6.Text =reader.GetString(2);

            if (reader.GetString(4) == "L")
            {
                radioButton1.Checked = true;
            }
            else
            {
                radioButton2.Checked = true;
            }

            connection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength > 0)
            {
                loadgrid();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            idRoom = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(idRoom != 0)
            {
                command = new SqlCommand("update reservationRoom set checkInDateTime = getdate() where id = "+idRoom, connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Room Successfully Checked In", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                loadgrid();
            }
        }
    }
}
