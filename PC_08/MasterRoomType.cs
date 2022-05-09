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
    public partial class MasterRoomType : Form
    {
        int cond, id;
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;

        public MasterRoomType()
        {
            InitializeComponent();
            dis();
            loadgrid();

            lbltime.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");
        }

        void dis()
        {
            btn_insert.Enabled = true;
            btn_up.Enabled = true;
            btn_del.Enabled = true;
            btn_cancel.Enabled = false;
            btn_save.Enabled = false;
            button2.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            numericUpDown1.Enabled = false;
        }

        void enable()
        {
            btn_insert.Enabled = false;
            btn_up.Enabled = false;
            btn_del.Enabled = false;
            btn_cancel.Enabled = true;
            btn_save.Enabled = true;
            button2.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            numericUpDown1.Enabled = true;
        }

        void clear()
        {
            textBox2.Text = "";
            numericUpDown1.Value = 0;
            textBox3.Text = "";
            pictureBox1.Image = null;
        }

        void loadgrid()
        {
            command = new SqlCommand("select * from roomType where name like '%' + @params + '%'", connection);
            command.Parameters.AddWithValue("@params", textBox1.Text);
            dataGridView1.DataSource = Command.getdata(command);
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[4].Visible = false;
        }

        bool val()
        {
            if (textBox2.TextLength < 1 || textBox3.TextLength < 1 || pictureBox1.Image == null || numericUpDown1.Value < 1)
            {
                MessageBox.Show("All fields must be filled!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; 
            }

            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainAdmin main = new MainAdmin();
            this.Hide();
            main.ShowDialog();
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
                DialogResult result= MessageBox.Show("Are you sure to delete?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(result == DialogResult.Yes)
                {
                    command = new SqlCommand("delete from roomtype where id = " + id, connection);
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
                command = new SqlCommand("insert into roomtype values(@name, " + numericUpDown1.Value + ", " + Convert.ToInt32(textBox3.Text) + ", @img)", connection);
                command.Parameters.AddWithValue("@name", textBox2.Text);
                command.Parameters.AddWithValue("@img", Encrypt.encode(pictureBox1.Image));
                connection.Open();
                try
                {
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
            else if (cond == 2 && val())
            {
                DialogResult result = MessageBox.Show("Are you sure to update?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    command = new SqlCommand("update roomtype set name = @name, capacity = " + numericUpDown1.Value + ", roomprice = " + Convert.ToInt32(textBox3.Text) + ", photo = @img where id = "+id, connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@name", textBox2.Text);
                    command.Parameters.AddWithValue("@img", Encrypt.encode(pictureBox1.Image));
                    try
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Successfully Updated", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dis();
                        clear();
                        loadgrid();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("" + ex);
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

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images|*.jpg;*.jpeg;*png";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Image img = Image.FromFile(ofd.FileName);
                Bitmap bmp = (Bitmap)img;
                pictureBox1.Image = bmp;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == 8);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            numericUpDown1.Value = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[2].Value);

            if(dataGridView1.SelectedRows[0].Cells[4].Value == System.DBNull.Value)
            {
                pictureBox1.Image = null;
            }
            else
            {
                byte[] b = (byte[])dataGridView1.SelectedRows[0].Cells[4].Value;
                MemoryStream stream = new MemoryStream(b);
                pictureBox1.Image = Image.FromStream(stream);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void btn_insert_Click(object sender, EventArgs e)
        {
            cond = 1;
            enable();
        }
    }
}
