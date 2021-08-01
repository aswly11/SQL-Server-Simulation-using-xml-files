using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication9
{
    public partial class loading : Form
    {
        public loading()
        {
            InitializeComponent();
        }

       
        

     

        private void nextform()
        {
            Main f2 = new Main();
            this.Hide();
            f2.Show();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            this.timer1.Start();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {

            this.progressBar1.Increment(99);

            
            if (progressBar1.Value == 80)
            {
                label2.Text = "Welcome !";
                label2.ForeColor = Color.Green;
            }
            if (progressBar1.Value == progressBar1.Maximum)
            {
                this.timer1.Stop();
                nextform();
            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }


    
}
