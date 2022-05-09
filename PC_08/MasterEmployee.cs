using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PC_08
{
    public partial class MasterEmployee : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;
        string old;
        int id, cond;

        public MasterEmployee()
        {
            InitializeComponent();
            dis();
            loadgrid();
            loadjob();

            lbltime.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainAdmin main = new MainAdmin();
            this.Hide();
            main.ShowDialog();
        }

        void dis()
        {
            btn_insert.Enabled = true;
            btn_up.Enabled = true;
            btn_del.Enabled = true;
            btn_save.Enabled = false;
            btn_cancel.Enabled = false;
            button2.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            comboBox1.Enabled = false;
            dateTimePicker1.Enabled = false;
            checkBox1.Enabled = false;
        }

        void enable()
        {
            btn_insert.Enabled = false;
            btn_up.Enabled = false;
            btn_del.Enabled = false;
            btn_save.Enabled = true;
            btn_cancel.Enabled = true;
            button2.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            textBox6.Enabled = true;
            textBox7.Enabled = true;
            comboBox1.Enabled = true;
            dateTimePicker1.Enabled = true;
            checkBox1.Enabled = true;
        }

        void loadgrid()
        {
            command = new SqlCommand("select * from employee join job on job.id = employee.jobId where employee.name like '%' + @params + '%' or employee.username like '%' + @params + '%'", connection);
            command.Parameters.AddWithValue("@params", textBox1.Text);
            dataGridView1.DataSource = Command.getdata(command);
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].Visible = false;
            dataGridView1.Columns[9].Visible = false;

            dataGridView1.Columns[6].DefaultCellStyle.Format = "dddd, dd MMMM yyyy";
            dataGridView1.Columns[10].HeaderText = "Job";
        }

        void loadjob()
        {
            command = new SqlCommand("select * from job", connection);
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "ID";
            comboBox1.DataSource = Command.getdata(command);
        }

        bool val()
        {
            if(textBox2.TextLength < 1 || textBox3.TextLength < 1 || textBox4.TextLength < 1 ||textBox5.TextLength < 1 || textBox6.TextLength < 1 || comboBox1.Text.Length < 1 || dateTimePicker1.Value == null || comboBox1.Text.Length < 1 || pictureBox1.Image == null || textBox7.TextLength < 1)
            {
                MessageBox.Show("All fields must be filled!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (textBox4.Text != textBox3.Text)
            {
                MessageBox.Show("Confirmation password doesn't correct!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (dateTimePicker1.Value.Date > DateTime.Now)
            {
                MessageBox.Show("Insert a correct date!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            command = new SqlCommand("select * from employee where username = @user", connection);
            command.Parameters.AddWithValue("@user", textBox2.Text);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                connection.Close();
                MessageBox.Show("Username is already exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            connection.Close();
            return true;
        }

        bool valUp()
        {
            if (textBox2.TextLength < 1 || textBox5.TextLength < 1 || textBox6.TextLength < 1 || comboBox1.Text.Length < 1 || dateTimePicker1.Value == null || comboBox1.Text.Length < 1 || pictureBox1.Image == null || textBox7.TextLength < 1)
            {
                MessageBox.Show("All fields must be filled!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if(textBox3.TextLength > 0)
            {
                if (Encrypt.enc(textBox3.Text) != old)
                {
                    MessageBox.Show("Old password doesn't correct!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else if (dateTimePicker1.Value.Date > DateTime.Now)
            {
                MessageBox.Show("Insert a correct date!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            command = new SqlCommand("select * from employee where username = @user", connection);
            command.Parameters.AddWithValue("@user", textBox2.Text);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows && reader.GetInt32(0) != id)
            {
                connection.Close();
                MessageBox.Show("Username is already exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            connection.Close();
            return true;
        }

        void clear()
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            pictureBox1.Image = null;
            dateTimePicker1.Value = DateTime.Now;

            labelo.Text = "Password :";
            labelp.Text = "Confirm Password :";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images|*.jpg;*.jpeg;*.png";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Image img = Image.FromFile(ofd.FileName);
                Bitmap bmp = (Bitmap)img;
                pictureBox1.Image = bmp;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            loadgrid();
        }

        private void btn_insert_Click(object sender, EventArgs e)
        {
            cond = 1;
            enable();
        }

        private void btn_up_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected)
            {
                cond = 2;
                enable();
                labelo.Text = "Old Password :";
                labelp.Text = "New Password :";
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
                    command = new SqlCommand("delete from employee where id = " + id, connection);
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
                command = new SqlCommand("insert into employee values(@user, @pass, @name, @email, @address, @date, " + comboBox1.SelectedValue + ", @img)", connection);
                command.Parameters.AddWithValue("@user", textBox2.Text);
                command.Parameters.AddWithValue("@pass", Encrypt.enc(textBox3.Text));
                command.Parameters.AddWithValue("@name", textBox5.Text);
                command.Parameters.AddWithValue("@date", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@img", Encrypt.encode(pictureBox1.Image));
                command.Parameters.AddWithValue("@email", textBox7.Text);
                command.Parameters.AddWithValue("@address", textBox6.Text);
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
            if(cond == 2 && valUp())
            {
                DialogResult result = MessageBox.Show("Are you sure to update?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    command = new SqlCommand("update employee set username = @user, password = @pass, name = @name, email = @email, dateOfBirth = @date, jobId = " + comboBox1.SelectedValue + ", photo = @img, address = @address where id = " + id, connection);
                    command.Parameters.AddWithValue("@user", textBox2.Text);
                    if (textBox3.TextLength > 0)
                    {
                        command.Parameters.AddWithValue("@pass", Encrypt.enc(textBox4.Text));
                    }
                    else if (textBox3.TextLength < 1)
                    {
                        command.Parameters.AddWithValue("@pass", old);
                    }
                    command.Parameters.AddWithValue("@name", textBox5.Text);
                    command.Parameters.AddWithValue("@date", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@img", Encrypt.encode(pictureBox1.Image));
                    command.Parameters.AddWithValue("@email", textBox7.Text);
                    command.Parameters.AddWithValue("@address", textBox6.Text);
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
            clear();
            dis();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            old = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textBox5.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            textBox6.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            textBox7.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            dateTimePicker1.Value = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells[6].Value);
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[10].Value.ToString();
            comboBox1.SelectedValue = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[9].Value);

            if (dataGridView1.SelectedRows[0].Cells[8].Value == System.DBNull.Value)
            {
                pictureBox1.Image = null;
            }
            else
            {
                byte[] b = (byte[])dataGridView1.SelectedRows[0].Cells[8].Value;
                MemoryStream stream = new MemoryStream(b);
                pictureBox1.Image = Image.FromStream(stream);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || char.IsWhiteSpace(e.KeyChar) || char.IsSymbol(e.KeyChar) || e.KeyChar == 8);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox3.PasswordChar = '\0';
                textBox4.PasswordChar = '\0';
            }
            else if (!checkBox1.Checked)
            {
                textBox3.PasswordChar = '*';
                textBox4.PasswordChar = '*';
            }
        }
    }
}
