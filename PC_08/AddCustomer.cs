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
    public partial class AddCustomer : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;
        
        public AddCustomer()
        {
            InitializeComponent();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == 8);
        }

        bool val()
        {
            if (textBox1.TextLength < 1 || textBox2.TextLength < 1 || textBox3.TextLength < 1 || dateTimePicker1.Value == null || comboBox1.Text.Length < 1)
            {
                MessageBox.Show("All fields must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;

            }
            else if (textBox1.TextLength != 16)
            {

                MessageBox.Show("NIK must be 16 length characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if(dateTimePicker1.Value.Date > DateTime.Now.Date)
            {
                MessageBox.Show("Please insert a correct date", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            command = new SqlCommand("select * from customer where phoneNumber = @phone", connection);
            command.Parameters.AddWithValue("@phone", textBox5.Text);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                connection.Close();
                MessageBox.Show("Phone number is already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            connection.Close();

            command = new SqlCommand("select * from customer where nik = @nik", connection);
            command.Parameters.AddWithValue("@nik", textBox1.Text);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                connection.Close();
                MessageBox.Show("NIK is already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            connection.Close();

            return true;
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || char.IsSymbol(e.KeyChar) || e.KeyChar == 8);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || char.IsSymbol(e.KeyChar) || char.IsWhiteSpace(e.KeyChar) || e.KeyChar == 8);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (val())
            {
                string g = "";
                if (comboBox1.Text == "Male")
                {
                    g = "L";
                }
                else
                {
                    g = "P";
                }
                command = new SqlCommand("insert into customer values(@name, @nik, @email, @gender, @phone, @age, @dob)", connection);
                connection.Open();
                command.Parameters.AddWithValue("@name", textBox2.Text);
                command.Parameters.AddWithValue("@nik", textBox1.Text);
                command.Parameters.AddWithValue("@email", textBox3.Text);
                command.Parameters.AddWithValue("@gender", g);
                command.Parameters.AddWithValue("@phone", textBox5.Text);
                command.Parameters.AddWithValue("@age", Convert.ToInt32(DateTime.Now.ToString("yyyy")) - Convert.ToInt32(dateTimePicker1.Value.ToString("yyyy")));
                command.Parameters.AddWithValue("@dob", DateTime.Now.ToString("yyyy-MM-dd"));

                try
                {
                    command.ExecuteNonQuery();
                    MessageBox.Show("Successfully added", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    connection.Close();

                    Reservation reservation = new Reservation();
                    this.Hide();
                    reservation.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
