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
    public partial class constraints : Form
    {

        public string databasename;
        public string tablename;
        public Main frmmain;
        public string colname;
        Dictionary<string, databaseContent> mydatabases;


        public constraints(Main frm, string databaseName,string table,string colname, Dictionary<string, databaseContent> c)
        {
            InitializeComponent();
            this.mydatabases = c;
            this.databasename = databaseName;
            this.frmmain = frm;
            this.tablename = table;
            this.colname = colname;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (File.Exists("DataBases.xml"))
            {
                int colindex = 0;
                XmlDocument doc = new XmlDocument();
                doc.Load("DataBases.xml");
                XmlNodeList myxmltables = doc.GetElementsByTagName("Table");
                foreach (XmlNode i in myxmltables)
                {
                    if (i.Attributes[0].Value.ToString() == tablename)
                    {
                        XmlNodeList myxmlcols = i.ChildNodes;
                        foreach (XmlNode col in myxmlcols)
                        {       
                            if(col.Attributes[0].Value==colname)
                            {
                                XmlAttribute constraint = doc.CreateAttribute("constraint");
                                constraint.Value = cmboperation.SelectedItem.ToString()+"@"+txtvalue.Text;
                                if(col.Attributes.Count==4)
                                    col.Attributes.Append(constraint);
                                else
                                col.Attributes[4].Value=constraint.Value;


                                mydatabases[databasename].mydatabase[tablename].col[colindex].constraint = col.Attributes[4].Value;
                                doc.Save("DataBases.xml");
                                this.Hide();
                                return;
                            }
                            colindex++;
                        }
                    }
                }
            }
        }

        private void constraints_Load(object sender, EventArgs e)
        {
            this.label1.Text = colname;
        }
    }
}
