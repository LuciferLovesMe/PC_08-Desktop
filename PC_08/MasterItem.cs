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
    public partial class MasterItem : Form
    {
        int id, cond;
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;

        public MasterItem()
        {
            InitializeComponent();
            loadgrid();
            dis();

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
            command = new SqlCommand("select * from item where name like '%' + @params + '%'", connection);
            command.Parameters.AddWithValue("@params", textBox1.Text);
            dataGridView1.DataSource = Command.getdata(command);

            dataGridView1.Columns[0].Visible = false;
        }

        void dis()
        {
            btn_insert.Enabled = true;
            btn_del.Enabled = true;
            btn_up.Enabled = true;
            btn_save.Enabled = false;
            btn_cancel.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
        }

        void enable()
        {
            btn_insert.Enabled = false;
            btn_del.Enabled = false;
            btn_up.Enabled = false;
            btn_save.Enabled = true;
            btn_cancel.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
        }

        void clear()
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
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
            }
            else
                MessageBox.Show("Please select an item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btn_del_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected)
            {
                DialogResult result = MessageBox.Show("Are you sure to delete?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    command = new SqlCommand("delete from item where id = " + id, connection);
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Successfully deleted!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dis();
                        clear();
                        loadgrid();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("This Item Can not be deleted because it has been used in transaction!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            else
                MessageBox.Show("Please select an item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (cond == 1 && val())
            {
                command = new SqlCommand("insert into item values(@name, " + Convert.ToInt32(textBox3.Text) + ", " + Convert.ToInt32(textBox4.Text) + ")", connection);
                command.Parameters.AddWithValue("@name", textBox2.Text);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Successfully Inserted!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dis();
                    clear();
                    loadgrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(""+ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
            else if (cond == 2 && val())
            {
                DialogResult result = MessageBox.Show("Are you sure to Update?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    command = new SqlCommand("update item set name = @name, requestPrice = "+Convert.ToInt32(textBox3.Text)+", compensationFee = "+Convert.ToInt32(textBox4.Text)+" where id = " + id, connection);
                    command.Parameters.AddWithValue("@name", textBox2.Text);
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Successfully updated!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dis();
                        clear();
                        loadgrid();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == 8);
        }

        bool val()
        {
            if (textBox2.TextLength < 1 || textBox3.TextLength < 1 || textBox4.TextLength < 1)
            {
                MessageBox.Show("All fields must be filled!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
    }
}
