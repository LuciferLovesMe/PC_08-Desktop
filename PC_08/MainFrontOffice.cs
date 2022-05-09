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
    public partial class MainFrontOffice : Form
    {
        public MainFrontOffice()
        {
            InitializeComponent();
            lblname.Text = Model.Name;
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to logout?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                MainLogin main = new MainLogin();
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
            Reservation reservation = new Reservation();
            this.Hide();
            reservation.ShowDialog();
        }

        private void foodsAndDrinksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckIn check = new CheckIn();
            this.Hide();
            check.ShowDialog();
        }

        private void itemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RequestAdditionalItem requestAdditionalItem = new RequestAdditionalItem();
            this.Hide();
            requestAdditionalItem.ShowDialog();
        }

        private void roomTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckOut check = new CheckOut();
            this.Hide();
            check.ShowDialog();
        }

        private void checkInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportForm report = new ReportForm();
            this.Hide();
            report.ShowDialog();
        }

        private void guessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportInChart report = new ReportInChart(); 
            this.Hide();
            report.ShowDialog();
        }
    }
}
