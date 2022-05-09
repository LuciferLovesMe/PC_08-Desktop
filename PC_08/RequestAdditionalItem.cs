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
    public partial class RequestAdditionalItem : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;
        int room;

        public RequestAdditionalItem()
        {
            InitializeComponent();
            loaditems();
            loadrooms();

            dataGridView4.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView4.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainFrontOffice main = new MainFrontOffice();
            this.Hide();
            main.ShowDialog();
        }

        void loadrooms()
        {
            command = new SqlCommand("select * from room where status = 'unavail'", connection);
            comboBox1.DisplayMember = "roomNumber";
            comboBox1.ValueMember = "id";
            comboBox1.DataSource = Command.getdata(command);
        }

        void loaditems()
        {
            command = new SqlCommand("select * from item", connection);
            comboBox2.DisplayMember = "name";
            comboBox2.ValueMember = "id";
            comboBox2.DataSource = Command.getdata(command);
            loadprice();
        }

        int loadprice()
        {
            command = new SqlCommand("select requestPrice from item where id = " + comboBox2.SelectedValue, connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            int p = reader.GetInt32(0);
            connection.Close();
            textBox3.Text = p.ToString();
            return p;
        }

        int getId()
        {
            command = new SqlCommand("select top(1) id from reservationRoom where roomId = " + comboBox1.SelectedValue + " order by id desc", connection);
            connection.Open ();
            reader = command.ExecuteReader();
            reader.Read();
            int id = reader.GetInt32(0);
            connection.Close();
            return id;
        }

        double getTotal()
        {
            double it = 0;

            for (int i = 0; i < dataGridView4.RowCount; i++)
            {
                it += Convert.ToDouble(dataGridView4.Rows[i].Cells[4].Value);
            }

            return it;
        }

        void clear()
        {
            dataGridView4.Rows.Clear();

            lbltotal.Text = getTotal().ToString();
            lbltotal.Text = String.Format("{0:n}", Convert.ToDouble(lbltotal.Text));
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            textBox4.Text = (numericUpDown1.Value * loadprice()).ToString();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadprice();
        }

        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView4.CurrentRow.Selected = true;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text.Length > 0 || numericUpDown1.Value > 0)
            {
                int row = dataGridView4.Rows.Add();
                dataGridView4.Rows[row].Cells[0].Value = comboBox2.SelectedValue;
                dataGridView4.Rows[row].Cells[1].Value = comboBox2.Text;
                dataGridView4.Rows[row].Cells[2].Value = numericUpDown1.Value;
                dataGridView4.Rows[row].Cells[3].Value = textBox3.Text;
                dataGridView4.Rows[row].Cells[4].Value = textBox4.Text;

                lbltotal.Text = getTotal().ToString();
                lbltotal.Text = String.Format("{0:n}", Convert.ToDouble(lbltotal.Text));
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView4.CurrentRow.Selected)
            {
                dataGridView4.Rows.Remove(dataGridView4.SelectedRows[0]);

                lbltotal.Text = getTotal().ToString();
                lbltotal.Text = String.Format("{0:n}", Convert.ToDouble(lbltotal.Text));
            }
            else
            {
                MessageBox.Show("Please Select a Item!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView4.RowCount < 1)
            {
                MessageBox.Show("Please Select a Item!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                for (int i = 0; i < dataGridView4.RowCount; i++)
                {
                    command = new SqlCommand("insert into reservationRequestItem values(@id, @itemID, @qty, @tot)", connection);
                    command.Parameters.AddWithValue("@id", getId());
                    command.Parameters.AddWithValue("@itemID", dataGridView4.Rows[i].Cells[0].Value);
                    command.Parameters.AddWithValue("@qty", Convert.ToInt32(dataGridView4.Rows[i].Cells[2].Value));
                    command.Parameters.AddWithValue("@tot", Convert.ToInt32(dataGridView4.Rows[i].Cells[4].Value));
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }

                MessageBox.Show("Successfully inserted!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clear();
            }
        }
    }
}
