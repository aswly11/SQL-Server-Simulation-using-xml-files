using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
namespace WindowsFormsApplication9
{
    public partial class Relations : Form
    {
        Main frm;
        string tablename, databasename,relatoinname;
        public Relations()
        {
            InitializeComponent();
        }
        databaseContent currnentdatabase;
        public Relations(Main frm, string tablename, string databasename, databaseContent currnentdatabase)
        {
            InitializeComponent();
            this.frm = frm;
            this.tablename = tablename;
            this.databasename = databasename;
            this.currnentdatabase = currnentdatabase;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        string colsprimary = "";
        Dictionary<string, string> returnsprim;
        private void Relations_Load(object sender, EventArgs e)
        {
            frm.mydatabases.Clear();
            frm.loading_files();
            this.txtforeigntable.Text =this.tablename;
            for (int i = 0; i < frm.mydatabases[databasename].mydatabase[tablename].col.Count; i++)
            {
                cmbcolsforeign.Items.Add(frm.mydatabases[databasename].mydatabase[tablename].col[i].colname);
            
            
            }
            for (int i = 0; i < frm.mydatabases[databasename].mydatabase.Count; i++)
            { 
                for(int j =0 ; j <frm.mydatabases[databasename].mydatabase.ElementAt(j).Value.col.Count;j++ )
                {
                    if(frm.mydatabases[databasename].mydatabase.ElementAt(i).Value.col[j].hasprimarykey)
                    {
                        colsprimary = frm.mydatabases[databasename].mydatabase.ElementAt(i).Value.col[j].colname;
                        this.cmbprimarykeytables.Items.Add(frm.mydatabases[databasename].mydatabase.ElementAt(i).Key);
                        returnsprim.Add(frm.mydatabases[databasename].mydatabase.ElementAt(i).Key, colsprimary);
                    }
                }         
            }            
        }

        private void cmbprimarykeytables_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < returnsprim.Count; i++)
                if (cmbprimarykeytables.SelectedItem.ToString() == returnsprim.ElementAt(i).Key)
                    cmb1colsprimary.Items.Add(returnsprim.ElementAt(i).Value);     
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("DataBases.xml");
            XmlNodeList mytables = doc.GetElementsByTagName("Table");
            foreach (XmlNode i in mytables)
            {
                if(i.Attributes[0].Value==tablename)
                {
                    XmlNodeList cols = i.ChildNodes;
                    foreach (XmlNode col in cols){
                     if(col.Attributes[0].Value== cmbcolsforeign.SelectedItem.ToString())
                     {
                         col.Attributes[4].Value = col.Attributes[4].Value + "&&" + cmb1colsprimary.SelectedItem.ToString()
                             + "\\\\" + cmbcolsforeign.SelectedItem.ToString();
                     }
                    
                    }
                 

                }
            }
            doc.Save("DataBases.xml");
            this.Dispose();
          
        }
    }
}
