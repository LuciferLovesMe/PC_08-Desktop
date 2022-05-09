using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PC_08
{
    public partial class MainAdmin : Form
    {
        public MainAdmin()
        {
            InitializeComponent();
            lblname.Text = Model.Name;
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to logout?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                MainLogin main =new MainLogin();
                this.Hide();
                main.ShowDialog();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void employeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MasterEmployee employee = new MasterEmployee();
            this.Hide();
            employee.ShowDialog();
        }

        private void foodsAndDrinksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MasterFoodAndDrink master = new MasterFoodAndDrink();
            this.Hide();
            master.ShowDialog();
        }

        private void itemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MasterItem item = new MasterItem();
            this.Hide();
            item.ShowDialog();
        }

        private void roomTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MasterRoomType roomType = new MasterRoomType();
            this.Hide();
            roomType.ShowDialog();
        }

        private void roomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MasterRoom room = new MasterRoom();
            this.Hide();
            room.ShowDialog();
        }
    }
}
