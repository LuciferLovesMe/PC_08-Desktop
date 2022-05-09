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
    public partial class MasterRoom : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlDataReader reader;
        SqlCommand command;
        int id, cond;

        public MasterRoom()
        {
            InitializeComponent();
            dis();
            loadgrid();
            loadtype();

            lbltime.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainAdmin main = new MainAdmin();
            this.Hide();
            main.ShowDialog();
        }

        void loadgrid()
        {
            command = new SqlCommand("select * from room join roomtype on room.roomtypeId = roomtype.id where roomnumber like '%' + @params + '%' or roomtype.name like '%' + @params + '%' ", connection);
            command.Parameters.AddWithValue("@params", textBox1.Text);
            dataGridView1.DataSource = Command.getdata(command);
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[10].Visible = false;
        }

        void loadtype()
        {
            command = new SqlCommand("select * from roomtype", connection);
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "ID";
            comboBox1.DataSource = Command.getdata(command);
        }

        void clear()
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
        }

        bool val()
        {
            if (textBox2.TextLength < 1 || textBox3.TextLength < 1 || textBox4.TextLength < 1 || comboBox1.Text.Length < 1)
            {
                MessageBox.Show("All fields must be filled!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) ;
                return false;
            }

            command = new SqlCommand("select * from room where roomnumber = '" + textBox2.Text + "'", connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                connection.Close();
                MessageBox.Show("Room number is already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            connection.Close();
            return true;
        }

        bool valUp()
        {
            if (textBox2.TextLength < 1 || textBox3.TextLength < 1 || textBox4.TextLength < 1 || comboBox1.Text.Length < 1)
            {
                MessageBox.Show("All fields must be filled!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            command = new SqlCommand("select * from room where roomnumber = '" + textBox2.Text + "'", connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows && reader.GetInt32(0) != id)
            {
                connection.Close();
                MessageBox.Show("Room number is already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            connection.Close();
            return true;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == 8);
        }

        private void btn_insert_Click(object sender, EventArgs e)
        {
            cond = 1;
            enable();
        }

        void dis()
        {
            btn_insert.Enabled = true;
            btn_up.Enabled = true;
            btn_del.Enabled = true;
            btn_save.Enabled = false;
            btn_cancel.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            comboBox1.Enabled = false;
        }

        private void btn_up_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected)
            {
                cond = 2;
                enable();
            }
            else
                MessageBox.Show("Please select an item!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btn_del_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected)
            {
                DialogResult result = MessageBox.Show("Are you sure to delete?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    command = new SqlCommand("delete from Room where id = " + id, connection);
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Successfully deleted!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                        dis();
                        loadgrid();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            else
                MessageBox.Show("Please select an item!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if(cond == 1 && val())
            {
                command = new SqlCommand("insert into room values(" + comboBox1.SelectedValue + ", '" + textBox2.Text + "', '" + textBox3.Text + "', @desc, 'Avail')", connection);
                command.Parameters.AddWithValue("@desc", textBox4.Text);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Successfully inserted!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dis();
                    clear();
                    loadgrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            else if(cond == 2 && valUp())
            {
                DialogResult result = MessageBox.Show("Are you sure to update?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    command = new SqlCommand("update room set roomNumber = '" + textBox2.Text + "', roomTypeId = " + comboBox1.SelectedValue + ", roomFloor = '" + Convert.ToInt32(textBox3.Text) + "', description = @desc where id = " + id, connection);
                    command.Parameters.AddWithValue("@desc", textBox4.Text);
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Successfully Updated!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dis();
                        clear();
                        loadgrid();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            dis();
            clear();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            loadgrid();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            comboBox1.SelectedValue = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            textBox4.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
        }

        void enable()
        {
            btn_insert.Enabled = false;
            btn_up.Enabled = false;
            btn_del.Enabled = false;
            btn_save.Enabled = true;
            btn_cancel.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            comboBox1.Enabled = true;
        }
    }
}
