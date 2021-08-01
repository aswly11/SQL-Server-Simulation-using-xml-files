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
using System.Xml.Serialization;
namespace WindowsFormsApplication9
{
    public partial class Main : Form
    {
        public Dictionary<string, databaseContent> mydatabases;
        public string tableclicked;
        public Main()
        {
            InitializeComponent();
            mydatabases = new Dictionary<string, databaseContent>();
            loading_files();
        }
        public List<cols> get_table_cols(String tablename){
                 List<cols> columns=new List<cols>();
             if (File.Exists("DataBases.xml"))
                 {

                XmlDocument doc = new XmlDocument();
                doc.Load("DataBases.xml");
                XmlNodeList myxmltables= doc.GetElementsByTagName("Table");
                 foreach(XmlNode i in myxmltables)
                 {
                     if(i.Attributes[0].Value.ToString()==tablename)
                     {
                          XmlNodeList myxmlcols=i.ChildNodes;
                         foreach(XmlNode col in myxmlcols)
                         {

                             cols column = new cols(col.Attributes[0].Value, col.Attributes[1].Value, col.Attributes[2].Value, col.Attributes[3].Value, col.Attributes[4].Value);
                             columns.Add(column);
                         }

                     }
                 
                 
                 }

            
             }
            return columns;
        }
        public void loading_files(){

            if (File.Exists("DataBases.xml"))
            {
         

                XmlDocument doc = new XmlDocument();
                doc.Load("DataBases.xml");
                XmlNodeList myxmldatabases = doc.GetElementsByTagName("Database");
                foreach (XmlNode i in myxmldatabases)
                {
                         if (File.Exists(i.Attributes[0].Value+".xml"))
                         {
                             XmlDocument doc1 = new XmlDocument();
                             doc1.Load(i.Attributes[0].Value + ".xml");              
                             databaseContent temp = new databaseContent();
                              XmlNodeList myxmltables = doc1.GetElementsByTagName("Table");
                             foreach (XmlNode table in myxmltables)
                             {
                                 string tablename=table.Attributes[0].Value;                               
                                 XmlNodeList myxmlrows=table.ChildNodes;
                                   List<List<string>>data = new List<List<string>>();
                                 foreach(XmlNode row in myxmlrows)
                                 {
                                     List<string> record = new List<string>();
                                     for(int cell=0;cell<row.Attributes.Count;cell++)
                                     {
                                         record.Add(row.Attributes[cell].Value);
                                     }  
                                     data.Add(record);                           
                                 }                            
                                 table mytable = new table();
                                 mytable.col= get_table_cols(tablename);
                                 mytable.data=data;
                                 temp.mydatabase.Add(tablename,mytable);
                                 temp.name=i.Attributes[0].Value;
                         
                            }
                             if(i.ChildNodes.Count!=temp.mydatabase.Count)
                             {
                                 table mytable = new table();
                                 XmlNodeList mytables = i.ChildNodes;
                                 foreach(XmlNode x in mytables)
                                 {
                                     if(!temp.mydatabase.ContainsKey(x.Attributes[0].Value))
                                     {
                                         mytable.col = get_table_cols(x.Attributes[0].Value);
                                         mytable.data = new List<List<string>>();
                                         temp.mydatabase.Add(x.Attributes[0].Value, mytable);
                                         temp.name = i.Attributes[0].Value;
                                     }
                                   
                                 }
                                

                             }
                             if(temp.name!=null)
                                mydatabases.Add(temp.name,temp);
                         }
                         else
                         {

                             databaseContent temp = new databaseContent();
                             foreach (XmlNode table in i.ChildNodes) {
                                 table mytable = new table();
                                 mytable.col = get_table_cols(table.Attributes[0].Value);
                                 temp.mydatabase.Add(table.Attributes[0].Value, mytable);
                                 temp.name = table.Attributes[1].Value;
                             }
                             mydatabases.Add(temp.name, temp);
                           

                         }
                     }
                
                }
            }
        
        
        

        private void Form2_Load(object sender, EventArgs e)
        {
            this.TopMost = false;            
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
          //  this.WindowState = FormWindowState.Maximized;
            refresh();
        }
        public void refresh()
        {
            treeView1.Nodes[0].Nodes.Clear();
            treeView1.Nodes[0].Nodes.Add("Show All Databases");
            if (File.Exists("DataBases.xml"))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("DataBases.xml");
                XmlNodeList mydatabases = doc.GetElementsByTagName("Database");
                TreeNode t = new TreeNode();
                for (int i = 0; i < mydatabases.Count; i++)
                {                    
                    t.Text = mydatabases[i].Attributes[0].Value;
                    t.ContextMenuStrip=contextMenuStrip2;
                    XmlNodeList mydatabaseschild = mydatabases[i].ChildNodes;
                    for (int j = 0; j < mydatabaseschild.Count; j++)
                    {                       
                            t.Nodes.Add(mydatabaseschild[j].Attributes[0].Value);
                            t.Nodes[j].ContextMenuStrip = contextMenuStrip3;                       
                    }
                    treeView1.Nodes[0].Nodes.Add(t);
                    t = new TreeNode();

                }
            }
        }
   
        private void newDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
           DataBaseName f = new DataBaseName(this);
            f.Show();
        }

        private string databaseclicked;
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                databaseclicked = e.Node.Text;
                if(databaseclicked.Contains("\\.."))
                {
                    tableclicked = databaseclicked;
                    databaseclicked = e.Node.Parent.Text;
                }
                    
                 
            }
        }

        private void deleteDatabaseToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string database_wantToDelete;
            database_wantToDelete = databaseclicked;
            XmlDocument doc = new XmlDocument();
            doc.Load("DataBases.xml");
            XmlNodeList mydatabasesformxml = doc.GetElementsByTagName("Database");
            foreach (XmlNode i in mydatabasesformxml)
            {
                if (database_wantToDelete == i.Attributes[0].Value)
                {
                    XmlNodeList tables=i.ChildNodes;
                    foreach(XmlNode table in tables)
                     for(int j =0;j<tabControl1.Controls.Count;j++)
                      {
                          if(tabControl1.Controls[j].Name==table.Attributes[0].Value)
                              tabControl1.Controls.Remove(tabControl1.Controls[j]);
                      }
                    XmlNode parent = i.ParentNode;
                    parent.RemoveChild(i);
                    break;                  
                }
            }
            doc.Save("DataBases.xml");
            mydatabases.Remove(database_wantToDelete);
            if (File.Exists(database_wantToDelete + ".xml"))
                File.Delete(database_wantToDelete + ".xml");
           
            refresh();

        }

        private void addNewTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage t = new TabPage();
            tabControl1.ContextMenuStrip = contextMenuStrip4;            
            CreateNewTable table = new CreateNewTable(databaseclicked, this, true,mydatabases);
            t.Controls.Add(table);
            tabControl1.Controls.Add(t);
        }

        private void editTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage t = new TabPage();
            tabControl1.ContextMenuStrip = contextMenuStrip4;
            CreateNewTable mytable = new CreateNewTable(databaseclicked,this,mydatabases);         
            t.Controls.Add(mytable);
            t.Text = tableclicked;
            tabControl1.Controls.Add(t);
        }
        private void displayTableToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            TabPage t = new TabPage();
            t.Text = tableclicked;

            t.ContextMenuStrip = contextMenuStrip4;
            Display_EditTable d = new Display_EditTable(mydatabases,this);            
          //  addnewtabletodatabase();
                t.Controls.Add(d);             
            tabControl1.Controls.Add(t);

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("DataBases.xml");
            XmlNodeList mytables = doc.GetElementsByTagName("Table");
            foreach (XmlNode i in mytables)
            {
                if (tableclicked == i.Attributes[0].Value )
                {
                    if(File.Exists(i.Attributes[1].Value+".xml"))
                    {
                        XmlDocument mydoc = new XmlDocument();
                        mydoc.Load(i.Attributes[1].Value + ".xml");
                        XmlNodeList mydatatables = mydoc.GetElementsByTagName("Table");
                        foreach( XmlNode mytable in mydatatables )
                        {
                            if(tableclicked==mytable.Attributes[0].Value)
                            {

                                XmlNode parent = mytable.ParentNode;
                                parent.RemoveChild(mytable);
                                mydoc.Save(i.Attributes[1].Value + ".xml");
                                break;
                            }
                        }
                    
                    
                    }

                    XmlNode parent1 = i.ParentNode;
                    parent1.RemoveChild(i);
                    break;
                }
            }
            doc.Save("DataBases.xml");
            foreach(TabPage i in tabControl1.Controls)
            {
                if (tableclicked == i.Text)
                {
                    tabControl1.Controls.Remove(i);
                }
            }
            refresh();
            
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab.Name="";
            tabControl1.Controls.RemoveAt(toolStripMenuItem3.Owner.TabIndex); 

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dragonToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {

        }

        private void tabControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                contextMenuStrip4.Show(this.tabControl1, new Point(e.X, e.Y));

        }

        private void tabControl1_ControlAdded(object sender, ControlEventArgs e)
        {
            tabControl1.SelectedIndex = tabControl1.Controls.Count - 1;
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refresh();
        }
    }
}
