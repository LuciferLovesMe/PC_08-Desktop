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
    public partial class CheckOut : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;
        int idReser;
        
        public CheckOut()
        {
            InitializeComponent();
            loadroom();
            loadstatus();
            loadFD();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainFrontOffice main = new MainFrontOffice();
            this.Hide();
            main.ShowDialog();
        }

        void loadroom()
        {
            command = new SqlCommand("select * from room where status = 'unavail'", connection);
            comboBox1.ValueMember = "ID";
            comboBox1.DisplayMember = "roomNumber";
            comboBox1.DataSource = Command.getdata(command);

            loadItem();
        }

        void loadstatus()
        {
            command = new SqlCommand("select * from itemStatus", connection);
            comboBox3.ValueMember = "ID";
            comboBox3.DisplayMember = "name";
            comboBox3.DataSource = Command.getdata(command);
        }

        int getReservationID()
        {
            command = new SqlCommand("select top(1) id from reservationroom where roomId = " + comboBox1.SelectedValue + " order by id desc", connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            idReser = reader.GetInt32(0);
            connection.Close();
            return idReser;
        }

        void loadItem()
        {
            command = new SqlCommand("select item.* from reservationRequestItem join item on item.id = reservationRequestItem.itemID where reservationRoomID = " + getReservationID(), connection);
            comboBox2.ValueMember = "ID";
            comboBox2.DisplayMember = "name";
            comboBox2.DataSource = Command.getdata(command);

            loadprice();
            loadCompe();
        }

        void loadFD()
        {
            command = new SqlCommand("select * from FDCheckOut join FoodsAndDrinks on FoodsAndDrinks.ID = FDCheckOut.FDID where reservationRoomID = " + getReservationID(), connection);
            dataGridView1.DataSource = Command.getdata(command);

            lbltotalfd.Text = getFDTotal().ToString();
            lbltotalfd.Text = String.Format("{0:n}", lbltotalfd.Text);
            
        }

        int loadprice()
        {
            if (Convert.ToInt32(comboBox2.SelectedValue) != 0 || comboBox2.SelectedValue != null)
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

            return 0;
        }

        int loadCompe()
        {
            if (Convert.ToInt32(comboBox2.SelectedValue) != 0 || comboBox2.SelectedValue != null)
            {
                command = new SqlCommand("select compensationFee from item where id = " + comboBox2.SelectedValue, connection);
                connection.Open();
                reader = command.ExecuteReader();
                reader.Read();
                int p = reader.GetInt32(0);
                connection.Close();
                textBox1.Text = p.ToString();
                return p;
            }

            return 0;
        }

        double getItemTotal()
        {
            double a = 0;
            for (int i = 0; i < dataGridView4.RowCount; i++)
            {
                a += Convert.ToInt32(dataGridView4.Rows[i].Cells[5].Value);
            }

            return a;
        }

        double getFDTotal()
        {
            double a = 0;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                a += Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value);
            }

            return a;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadItem();
            loadFD();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int a = 0;
            if (comboBox3.Text != "Available")
            {
                a = Convert.ToInt32(textBox1.Text);
            }
            else
            {
                a = 0;
            }
            int row = dataGridView4.Rows.Add();
            dataGridView4.Rows[row].Cells[0].Value = comboBox2.SelectedValue;
            dataGridView4.Rows[row].Cells[1].Value = comboBox2.Text;
            dataGridView4.Rows[row].Cells[2].Value = textBox1.Text;
            dataGridView4.Rows[row].Cells[3].Value = textBox3.Text;
            dataGridView4.Rows[row].Cells[4].Value = a;
            dataGridView4.Rows[row].Cells[5].Value = numericUpDown1.Value * (a + loadprice());
            dataGridView4.Rows[row].Cells[6].Value = comboBox3.SelectedValue;

            lbltotalitems.Text = getItemTotal().ToString();
            lbltotalitems.Text = String.Format("{0:n}", Convert.ToDouble(lbltotalitems.Text));


            lbltotal.Text = (getItemTotal() + getFDTotal()).ToString();
            lbltotal.Text = String.Format("{0:n}", Convert.ToDouble(lbltotalitems.Text));
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadItem();
        }

        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView4.CurrentRow.Selected = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView4.CurrentRow.Selected)
            {
                    dataGridView4.Rows.Remove(dataGridView4.SelectedRows[0]);

                lbltotalitems.Text = getItemTotal().ToString();
                lbltotalitems.Text = String.Format("{0:n}", Convert.ToDouble(lbltotalitems.Text));


                lbltotal.Text = (getItemTotal() + getFDTotal()).ToString();
                lbltotal.Text = String.Format("{0:n}", Convert.ToDouble(lbltotalitems.Text));
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected)
            {
                command = new SqlCommand("delete from FDCheckOut where id = "+dataGridView1.SelectedRows[0].Cells[0].Value);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

                loadFD();

                lbltotal.Text = (getItemTotal() + getFDTotal()).ToString();
                lbltotal.Text = String.Format("{0:n}", Convert.ToDouble(lbltotalitems.Text));
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView4.RowCount <1)
            {
                command = new SqlCommand("insert into reservationCheckOut values(" + getReservationID() + ", null, null, null, null)", connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

                
            }
            else
            {
                for (int i = 0; i < dataGridView4.RowCount; i++)
                {
                    command = new SqlCommand("insert into reservationCheckOut values(" + getReservationID() + ", "+dataGridView4.Rows[i].Cells[0].Value+", "+ dataGridView4.Rows[i].Cells[6].Value + ", " + dataGridView4.Rows[i].Cells[2].Value + ", "+ dataGridView4.Rows[i].Cells[5].Value+")", connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            command = new SqlCommand("update reservationRoom set checkOutDateTime = getdate() where id = " + getReservationID(), connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            command = new SqlCommand("update room set status = 'Avail' where id = " + comboBox1.SelectedValue, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            MessageBox.Show("Successfully Checked Out", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            loadroom();
            loadFD();
            dataGridView4.DataSource = null;
            dataGridView4.Rows.Clear();
        }
    }
}
