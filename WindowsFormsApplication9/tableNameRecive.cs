using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace WindowsFormsApplication9
{
    public partial class tableNameRecive : Form
    {
        CreateNewTable table;

        public tableNameRecive(CreateNewTable x)
        {
            InitializeComponent();
            table = x;
        }

        public string tablename;

  
        private void Savebtn_Click(object sender, EventArgs e)
        {
            tablename = this.NameTxt.Text;
            this.table.tablename=tablename;
            this.table.savetableAttributs(table.getdatafromdatagridview());
            this.Dispose();

            
        }

        private void tableNameRecive_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            
        }
    }
}
