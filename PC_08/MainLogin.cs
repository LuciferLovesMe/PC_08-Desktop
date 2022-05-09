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
    public partial class MainLogin : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;

        public MainLogin()
        {
            InitializeComponent();
        }

        bool val()
        {
            if(textBox1.TextLength < 1 || textBox2.TextLength < 1)
            {
                MessageBox.Show("All field must be filled!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBox2.PasswordChar = '\0';
            else
                textBox2.PasswordChar = '*';
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (val())
            {
                command = new SqlCommand("select * from employee where username = @username and password = @pass", connection);
                command.Parameters.AddWithValue("@username", textBox1.Text);
                command.Parameters.AddWithValue("@pass", Encrypt.enc(textBox2.Text));
                connection.Open();

                try
                {
                    reader = command.ExecuteReader();
                    reader.Read();
                    if (reader.HasRows)
                    {
                        Model.Name = reader.GetString(3);
                        Model.id = reader.GetInt32(0);
                        Model.jobID = reader.GetInt32(7);
                        
                        if(Model.jobID == 1)
                        {
                            MainAdmin main = new MainAdmin();
                            this.Hide();
                            main.ShowDialog();
                        }
                        else if (Model.jobID == 2)
                        {
                            MainManager main = new MainManager();
                            this.Hide();
                            main.ShowDialog();
                        }
                        else if (Model.jobID == 3)
                        {
                            MainFrontOffice main = new MainFrontOffice();
                            this.Hide();
                            main.ShowDialog();
                        }
                    }

                    else
                    {
                        connection.Close();
                        MessageBox.Show("Can't Find Employee!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
}
