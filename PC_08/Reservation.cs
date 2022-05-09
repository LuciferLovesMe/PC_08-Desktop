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
    public partial class Reservation : Form
    {
        SqlCommand command;
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlDataReader reader;
        int idCust, idReser, idRoom, roomPrice, roomNumber, subTotal;
        string desc;

        public Reservation()
        {
            InitializeComponent();
            loadcustomer();
            loadtype();
            loaditems();
            radioButton1.Checked = true;

            lblcode.Text = getCode();

            dataGridView3.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView3.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView4.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView4.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        string getCode()
        {
            command = new SqlCommand("select count(id) as num from reservation", connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                int c = reader.GetInt32(0);
                connection.Close();
                return "B000"+c.ToString();
            }
            else
            {
                connection.Close();
                return "B00001";
            }

        }

        int loadprice()
        {
            command = new SqlCommand("select requestPrice from item where id = "+comboBox2.SelectedValue, connection);
            connection.Open();
            reader= command.ExecuteReader();
            reader.Read();
            int p = reader.GetInt32(0);
            connection.Close();
            textBox3.Text = p.ToString();
            return p;
        }

        void clear()
        {
            dataGridView2.DataSource = null;
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            dataGridView4.Rows.Clear();
            textBox2.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
        }

        void insert()
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainFrontOffice main = new MainFrontOffice();
            this.Hide();
            main.ShowDialog();
        }

        void loadcustomer()
        {
            command = new SqlCommand("select * from customer where name like '%' + @params + '%'", connection);
            command.Parameters.AddWithValue("@params", textBox1.Text);
            dataGridView1.DataSource = Command.getdata(command);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            loadcustomer();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            idCust = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == 8);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!dataGridView2.CurrentRow.Selected)
            {
                MessageBox.Show("Please Select a Room!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (textBox2.TextLength < 1)
            {
                MessageBox.Show("Please Insert the Staying Field!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else if (Convert.ToInt32(textBox2.Text) < 1)
            {
                MessageBox.Show("Please Insert the Staying Field!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int row = dataGridView3.Rows.Add();
                dataGridView3.Rows[row].Cells[0].Value = idRoom;
                dataGridView3.Rows[row].Cells[1].Value = roomNumber;
                dataGridView3.Rows[row].Cells[2].Value = roomPrice;
                dataGridView3.Rows[row].Cells[3].Value = desc;
                dataGridView3.Rows[row].Cells[4].Value = roomPrice * Convert.ToInt32(textBox2.Text);

                lbltotal.Text = gettotal().ToString();
                lbltotal.Text = String.Format("{0:n}", Convert.ToDouble(lbltotal.Text));
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView3.CurrentRow.Selected)
            {
                dataGridView3.Rows.Remove(dataGridView3.SelectedRows[0]);

                lbltotal.Text = gettotal().ToString();
                lbltotal.Text = String.Format("{0:n}", Convert.ToDouble(lbltotal.Text));
            }
            else
            {
                MessageBox.Show("Please Select a Room!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if(textBox2.TextLength > 0)
            {
                dateTimePicker2.Value = dateTimePicker1.Value.AddDays(Convert.ToInt32(textBox2.Text));

            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadprice();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            textBox4.Text = (numericUpDown1.Value * loadprice()).ToString();
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView3.CurrentRow.Selected = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(comboBox1.Text.Length > 0 || numericUpDown1.Value > 0)
            {
                int row = dataGridView4.Rows.Add();
                dataGridView4.Rows[row].Cells[0].Value = comboBox2.SelectedValue;
                dataGridView4.Rows[row].Cells[1].Value = comboBox2.Text;
                dataGridView4.Rows[row].Cells[2].Value = numericUpDown1.Value;
                dataGridView4.Rows[row].Cells[3].Value = textBox3.Text;
                dataGridView4.Rows[row].Cells[4].Value = textBox4.Text;

                lbltotal.Text = gettotal().ToString();
                lbltotal.Text = String.Format("{0:n}", Convert.ToDouble(lbltotal.Text));
            }
        }

        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView4.CurrentRow.Selected = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView4.CurrentRow.Selected)
            {
                dataGridView4.Rows.Remove(dataGridView4.SelectedRows[0]);

                lbltotal.Text = gettotal().ToString();
                lbltotal.Text = String.Format("{0:n}", Convert.ToDouble(lbltotal.Text));
            }
            else
            {
                MessageBox.Show("Please Select a Item!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (idCust == 0)
            {
                MessageBox.Show("Please select a customer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (dataGridView3.RowCount < 1)
            {
                MessageBox.Show("Please Insert a Room", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                command = new SqlCommand("insert into reservation values(getdate(), " + Model.id + ", " + idCust + ", '"+getCode()+"')", connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

                try
                {
                    SqlCommand command1 = new SqlCommand("select top(1) id from reservation order by id desc", connection);
                    connection.Open();
                    reader = command1.ExecuteReader();
                    reader.Read();
                    idReser = reader.GetInt32(0);
                    connection.Close();

                    for (int i = 0; i < dataGridView3.RowCount; i++)
                    {
                        command = new SqlCommand("insert into reservationRoom values (@id, @roomID, @start, @dur, @price, @ci, @co)", connection);
                        command.Parameters.AddWithValue("@id", idReser);
                        command.Parameters.AddWithValue("@roomID", dataGridView3.Rows[i].Cells[0].Value);
                        command.Parameters.AddWithValue("@start", DateTime.Now.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@dur", Convert.ToInt32(textBox2.Text));
                        command.Parameters.AddWithValue("@price", dataGridView3.Rows[i].Cells[4].Value);
                        command.Parameters.AddWithValue("@ci", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@co", dateTimePicker2.Value.ToString("yyyy-MM-dd"));
                        try
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                            connection.Close();

                            command = new SqlCommand("update room set status = 'unavail' where id = " + dataGridView3.Rows[i].Cells[0].Value, connection);
                            connection.Open();
                            command.ExecuteNonQuery();
                            connection.Close();
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }

                    if (dataGridView4.RowCount > 0)
                    {
                        for (int i = 0; i < dataGridView4.RowCount; i++)
                        {
                            command = new SqlCommand("insert into reservationRequestItem values(@id, @itemID, @qty, @tot)", connection);
                            command.Parameters.AddWithValue("@id", idReser);
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
                    else
                    {
                        MessageBox.Show("Successfully inserted!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                    }

                       
                }
                catch (Exception ex)
                {
                    MessageBox.Show("res" + ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            AddCustomer add = new AddCustomer();
            add.ShowDialog();
        }

        void loadtype()
        {
            command = new SqlCommand("select * from roomtype", connection);
            comboBox1.DisplayMember = "name";
            comboBox1.ValueMember = "id";
            comboBox1.DataSource = Command.getdata(command);
        }

        double gettotal()
        {
            double r = 0, it = 0;
            for (int i = 0; i < dataGridView3.RowCount; i++)
            {
                r += Convert.ToDouble(dataGridView3.Rows[i].Cells[4].Value);
            }

            for (int i = 0; i < dataGridView4.RowCount; i++)
            {
                it += Convert.ToDouble(dataGridView4.Rows[i].Cells[4].Value);
            }

            return r + it;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            command = new SqlCommand("select room.*, roomtype.roomprice from room join roomtype on roomtype.id = room.roomtypeid where roomtypeId = " + comboBox1.SelectedValue + " and status = 'Avail'", connection);
            dataGridView2.DataSource = Command.getdata(command);
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView2.CurrentRow.Selected = true;
            idRoom = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[0].Value);
            roomNumber = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[2].Value);
            roomPrice = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[6].Value);
            desc = dataGridView2.SelectedRows[0].Cells[4].Value.ToString();
        }

        void loaditems()
        {
            command = new SqlCommand("select * from item", connection);
            comboBox2.DisplayMember = "name";
            comboBox2.ValueMember = "id";
            comboBox2.DataSource = Command.getdata(command);
            loadprice();
        }
    }
}
