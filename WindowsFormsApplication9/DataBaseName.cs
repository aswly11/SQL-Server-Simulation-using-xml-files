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
    public partial class DataBaseName : Form
    {
        Main formMMain;
        public DataBaseName()
        {
            InitializeComponent();
        }
        public DataBaseName(Main m)
        {
            InitializeComponent();
            formMMain = m;
        }
        public string getname_ofDataBase() {
            return this.NameTxt.Text;
        }
         
        private void DataBaseName_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
        }

        
        private void Savebtn_Click(object sender, EventArgs e)
        {
            if (File.Exists("DataBases.xml"))
            {
                XmlDocument Doc = new XmlDocument();
                Doc.Load("DataBases.xml");
                XmlNodeList db = Doc.GetElementsByTagName("Database");
                foreach (XmlNode i in db)
                {
                    if (i.Attributes[0].Value == this.getname_ofDataBase())
                    {
                        MessageBox.Show("This Database Is Already Existed");
                        
                        Doc.Save("DataBases.xml");
                        this.Hide();
                        return;
                    }
                }
                XmlElement database = Doc.CreateElement("Database");
                database.SetAttribute("name", getname_ofDataBase());              
                XmlElement root = Doc.DocumentElement;
                root.AppendChild(database);
                Doc.Save("DataBases.xml");
            
            }else
            {
                XmlWriter writer = XmlWriter.Create("DataBases.xml");
                writer.WriteStartDocument();
                writer.WriteStartElement("Databases");
                writer.WriteStartElement("Database");
                writer.WriteAttributeString("name", getname_ofDataBase());                
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
            }
            this.Hide();

            formMMain.refresh();
            
        }
    }
}
