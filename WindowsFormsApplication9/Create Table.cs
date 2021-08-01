using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using Microsoft.VisualBasic;
namespace WindowsFormsApplication9
{
    public partial class CreateNewTable : UserControl
    {
        public string databasename;
        public string tablename;
        public Main frmmain;
        bool isnew = false;
        public  List<ROW> table;

        Dictionary<string, databaseContent> mydatabases;
     
        public CreateNewTable()
        {
            InitializeComponent();
             
           
        }
        public CreateNewTable(string databaseName, Main frm, bool isnew, Dictionary<string, databaseContent> c)
        {
            InitializeComponent();
            this.mydatabases = c;
            this.databasename = databaseName;
            this.frmmain = frm;
            this.isnew = isnew;
            if(!this.isnew)
                this.tablename = this.Parent.Name;

           
        }

        public CreateNewTable(string databaseName, Main frm, Dictionary<string, databaseContent> c)
        {
            InitializeComponent();
            this.databasename = databaseName;
            this.frmmain = frm;
            this.mydatabases = c;
           
        }

        private void CreateNewTable_Load(object sender, EventArgs e)
        {
           if(!isnew)
           {
               this.tablename = this.Parent.Text;
               XmlDocument doc = new XmlDocument();
               doc.Load("DataBases.xml");
               XmlNodeList tables = doc.GetElementsByTagName("Table");
               foreach (XmlNode i in tables)
                   if (this.tablename == i.Attributes[0].Value)
                   {
                       XmlNodeList cols = i.ChildNodes;
                       for (int j = 0; j < cols.Count; j++)
                       {
                           this.dataGridView1.Rows.Add();
                           this.dataGridView1.Rows[j].HeaderCell.ContextMenuStrip = contextMenuStrip1;
                           this.dataGridView1.Rows[j].Cells[0].Value = cols[j].Attributes[0].Value;                           
                           (this.dataGridView1.Rows[j].Cells[1] as DataGridViewComboBoxCell).Value = cols[j].Attributes[1].Value;
                           if (cols[j].Attributes[2].Value.ToString() == "True")
                           {
                               this.dataGridView1.Rows[j].Cells[2].Value = true;
                           }
                           else if (cols[j].Attributes[2].Value.ToString() == "False")
                           {
                               this.dataGridView1.Rows[j].Cells[2].Value = false;
                           }

                           if (cols[j].Attributes[3].Value.ToString() == "True")
                           {
                               this.dataGridView1.Rows[j].HeaderCell.Style.BackColor=Color.Gold;
                               contextMenuStrip1.Items[2].Text = "Remove Primary Key";
                           }
                       }
                       doc.Save("DataBases.xml");
                   }
           }
       else
            {
               XmlDocument doc = new XmlDocument();                      
                doc.Load("DataBases.xml");
                XmlNodeList myxmldatabases = doc.GetElementsByTagName("Database");
                foreach (XmlNode database in myxmldatabases)
                    if (this.databasename == database.Attributes[0].Value)
                    {
                        this.Parent.Text = "Table" + "\\.." +this.databasename;//default name setting
                        this.tablename = this.Parent.Text;
                        XmlElement table = doc.CreateElement("Table"); ;
                        table.SetAttribute("Name", this.tablename);
                        table.SetAttribute("databaseName", this.databasename);
                        database.AppendChild(table);
                        doc.Save("DataBases.xml");
                        this.frmmain.refresh();
                    }
            }     

       }

        int Rowindex=-1;
        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
           // if (e.Button == MouseButtons.Right)
           //     contextMenuStrip1.Show(dataGridView1, new Point(e.X, e.Y));
            try
            {
                if (e.Button == MouseButtons.Right)
                    contextMenuStrip1.Show(dataGridView1, new Point(e.X, e.Y));
                Rowindex = e.RowIndex;
                contextMenuStrip1.Items[2].Text = "Set Primary Key";

                for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].HeaderCell.Style.BackColor == Color.Gold && Rowindex==i)
                    {
                        contextMenuStrip1.Items[2].Text = "Remove Primary Key";
                        return;

                    }

                }
                contextMenuStrip1.Items[2].Text = "Set Primary Key";
                return;
                //    if (!mydatabases.ContainsKey(databasename) || !mydatabases[databasename].mydatabase.ContainsKey(tablename))
                //    {

                //        if (!mydatabases.ContainsKey(databasename))
                //        {
                //            databaseContent temp1 = new databaseContent();
                //            temp1.name = databasename;
                //            table t1 = new table();
                //            t1.col = frmmain.get_table_cols(this.tablename);
                //            temp1.mydatabase.Add(this.tablename, t1);
                //            mydatabases.Add(temp1.name, temp1);
                //        }
                //        else
                //        {
                //            if (!mydatabases[databasename].mydatabase.ContainsKey(tablename))
                //            {

                //                databaseContent temp = new databaseContent();
                //                temp.name = databasename;
                //                table t = new table();
                //                t.col = frmmain.get_table_cols(this.tablename);
                //                mydatabases[databasename].mydatabase.Add(tablename, t);
                //            }
                //        }

                //    }
                //for (int i = 0; i < mydatabases[databasename].mydatabase[this.tablename].col.Count; i++)
                //    if (mydatabases[databasename].mydatabase[tablename].col[i].hasprimarykey)
                //    {
                //        if (i == Rowindex)
                //        {
                //            contextMenuStrip1.Items[2].Text = "Remove Primary Key";
                //            break;
                //        }
                //    }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            dataGridView1.Rows[e.RowIndex].HeaderCell.ContextMenuStrip = contextMenuStrip1;
            dataGridView1.Rows[e.RowIndex].HeaderCell.ContextMenuStrip.Show(dataGridView1, new Point(e.X, e.Y));
            
 
        }

       

       public List<ROW> getdatafromdatagridview()
        {
                frmmain.mydatabases.Clear();
                frmmain.loading_files();

                  table = new List<ROW>();
         
                for (int i = 0; i < dataGridView1.Rows.Count - 1;i++ )
                {
                    bool completed;
                    string allownull = "";
                   // (dataGridView1.Rows[i].Cells[2] as DataGridViewCheckBoxCell);
                    try
                    {
                         completed = Convert.ToBoolean(dataGridView1.Rows[i].Cells[2].Value.ToString());
                    }
                    catch (NullReferenceException ex)
                    {
                        completed = false;
                    }
                    if (completed)
                    {
                         allownull = "True";
                    }
                    else
                        allownull = "False";
                  
                

                    ROW temp;
                    if(isnew)
                    {
                        temp = new ROW(dataGridView1.Rows[i].Cells[0].Value.ToString(),
                     dataGridView1.Rows[i].Cells[1].Value.ToString(),allownull,"null" );
                    if (dataGridView1.Rows[i].HeaderCell.Style.BackColor == Color.Gold)
                        temp.primarykeystate = true;
                    else
                        temp.primarykeystate = false;
                     }else
                    {
                      try {
                            temp = new ROW(dataGridView1.Rows[i].Cells[0].Value.ToString(),
                        dataGridView1.Rows[i].Cells[1].Value.ToString(), allownull, mydatabases[databasename].mydatabase[tablename].col[i].constraint);
                        }catch(ArgumentOutOfRangeException e)
                      {
                          temp = new ROW(dataGridView1.Rows[i].Cells[0].Value.ToString(),
                              dataGridView1.Rows[i].Cells[1].Value.ToString(), allownull, "null");
                        
                        }
                     
                    if (dataGridView1.Rows[i].HeaderCell.Style.BackColor == Color.Gold)
                        temp.primarykeystate = true;
                    else
                        temp.primarykeystate = false;
                    }
                   
                    table.Add(temp);
                    dataGridView1.Rows[i].HeaderCell.ContextMenuStrip = contextMenuStrip1;
                   

                }

                return table;

        }
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
         {
             if (e.Control)
             {
                 //&&e.KeyCode==Keys.S

                 if (tablename.Contains("Table"))
                {
                    tableNameRecive d = new tableNameRecive(this);
                    d.Show();
                }
                else
                    savetableAttributs(getdatafromdatagridview());
               
             
            
            }
        }

        public void savetableAttributs(List<ROW> rowlist)
        {
            if (File.Exists("DataBases.xml"))
            {

                // we need to deletث all values of the row deleted from the file that carry data
                XmlDocument doc = new XmlDocument();
                doc.Load("DataBases.xml");
                XmlNodeList xmldatabases = doc.GetElementsByTagName("Database");
                foreach (XmlNode mydatabase in xmldatabases)
                        {
                            if (mydatabase.Attributes[0].Value == this.databasename)
                            {
                               
                                XmlNodeList tables=mydatabase.ChildNodes;
                                foreach(XmlNode mytable in tables)
                                { 
                                    if(mytable.Attributes[0].Value==this.Parent.Text)
                                    {
                                        string[] arr = this.tablename.Split('\\');
                                        this.tablename = arr[0];
                                        mytable.Attributes[0].Value = this.tablename + "\\.." + this.databasename;
                                        this.tablename = mytable.Attributes[0].Value;
                                        this.Parent.Text = this.tablename;
                                        while (mytable.ChildNodes.Count != 0)
                                        {
                                            mytable.RemoveChild(mytable.ChildNodes[mytable.ChildNodes.Count - 1]);                                    
                                        }
                                        for (int h = 0; h < rowlist.Count; h++)
                                        {
                                        XmlElement colmn = doc.CreateElement("colmn");
                                        string col = "col" + h;
                                        XmlAttribute atter1 = doc.CreateAttribute("ColmnName");
                                        atter1.Value = rowlist[h].columnName;
                                        XmlAttribute atter2 = doc.CreateAttribute("datatype");
                                        atter2.Value = rowlist[h].dataType;
                                        XmlAttribute atter3 = doc.CreateAttribute("allownull");
                                        atter3.Value = rowlist[h].allownull.ToString();
                                        XmlAttribute atter4 = doc.CreateAttribute("PrimaryState");
                                        XmlAttribute atter5 = doc.CreateAttribute("constraint");
                                        atter5.Value = rowlist[h].constraint;
                                        if(rowlist[h].primarykeystate)
                                            atter4.Value = "True";
                                         else
                                             atter4.Value = "False";                                   
                                        colmn.Attributes.Append(atter1);
                                        colmn.Attributes.Append(atter2);
                                        colmn.Attributes.Append(atter3);
                                        colmn.Attributes.Append(atter4);
                                        colmn.Attributes.Append(atter5);                                      
                                        mytable.AppendChild(colmn);                                       
                                        }
                                    }
                                }                                   
                            } 
                }
                doc.Save("DataBases.xml");
                mydatabases.Clear();
                frmmain.loading_files();
                this.frmmain.refresh();
                MessageBox.Show("Saved");
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = false;
            
        }

        private void Editprimarykeystatinxml(string databasename, string tablename,int colindex, string colstate)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("DataBases.xml");
            XmlNodeList mytables = doc.GetElementsByTagName("Table");
            foreach (XmlNode i in mytables)
            {
                if (tablename == i.Attributes[0].Value)
                {
                    XmlNodeList mycols = i.ChildNodes;
                    foreach (XmlNode col in mycols)
                        if (mydatabases[databasename].mydatabase[tablename].col[Rowindex].colname == col.Attributes[0].Value)
                        {
                            col.Attributes[3].Value = colstate;
                        }
                }
            }
            doc.Save("DataBases.xml");


        }
   
        private void setPrimaryKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            savetableAttributs(getdatafromdatagridview());
            frmmain.mydatabases.Clear();
            frmmain.loading_files();
          if(contextMenuStrip1.Items[2].Text=="Remove Primary Key")
          {
              if (Rowindex == -1)
              {
                  MessageBox.Show("Please Select Row First");
                  return;
              }
               for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                   if (dataGridView1.Rows[i].HeaderCell.Style.BackColor == Color.Gold && Rowindex == i)
                   {
                       dataGridView1.Rows[Rowindex].HeaderCell.Style.BackColor = Color.Gainsboro;
                       mydatabases[databasename].mydatabase[tablename].col[Rowindex].hasprimarykey = false;
                       contextMenuStrip1.Items[2].Text = "Set Primary Key";
                       return;
                   }

          }
          else
          {
              if (Rowindex == -1)
              {
                  MessageBox.Show("Please Select Row First");
                  return;
              }
              for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                  if (dataGridView1.Rows[i].HeaderCell.Style.BackColor == Color.Gold && Rowindex != i)
                  {
                      MessageBox.Show("A Table Has Already A Primary Key");
                      return;
                  }
                 

              this.dataGridView1.Rows[Rowindex].HeaderCell.Style.BackColor = Color.Gold;
              mydatabases[databasename].mydatabase[tablename].col[Rowindex].hasprimarykey = true;
              contextMenuStrip1.Items[2].Text = "Remove Primary Key";

          }
            
           

        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].HeaderCell.ContextMenuStrip = contextMenuStrip1;
            
        }

        private void deleteColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Rowindex==-1)
            {
                MessageBox.Show("Please Select Row First");
                return;
            }
            //show message dialoge to warn him if he deleted a colunm and ther a data in a data file then itll be deleted 

            DialogResult ds = MessageBox.Show("Deleting This Column Will Cause The Erasing Of Its Data ,Are you Sure you Want To Do This", "Caution", MessageBoxButtons.YesNo);
            if (ds == DialogResult.Yes)
            {
                if (File.Exists("DataBases.xml"))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load("DataBases.xml");
                    XmlNodeList myxmltables = doc.GetElementsByTagName("Table");
                    foreach (XmlNode table in myxmltables)
                    {
                        if (table.Attributes[0].Value == this.tablename)
                        {
                            XmlNodeList myxmlcols = table.ChildNodes;
                            foreach (XmlNode xmlcol in myxmlcols)
                            {
                                if (xmlcol.Attributes[0].Value == dataGridView1.Rows[Rowindex].Cells[0].Value.ToString())
                                {
                                    XmlNode parnet = xmlcol.ParentNode;
                                    parnet.RemoveChild(xmlcol);
                                    break;
                                }


                            }
                            break;
                        }
                    }
                    doc.Save("DataBases.xml");

                }
                dataGridView1.Rows.RemoveAt(Rowindex);

                //open and delete  data of this colunm 
                if (File.Exists(this.databasename + ".xml"))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(this.databasename + ".xml");
                    XmlNodeList myxmltables = doc.GetElementsByTagName("Table");
                    foreach (XmlNode table in myxmltables)
                    {
                        if (table.Attributes[0].Value == this.tablename)
                        {
                            XmlNodeList myxmlrows = table.ChildNodes;
                            foreach (XmlNode xmlrow in myxmlrows)
                            {
                                xmlrow.Attributes.RemoveAt(Rowindex);
                            }
                        }
                    }
                    doc.Save(this.databasename + ".xml");

                }
                mydatabases.Clear();
                frmmain.loading_files();
                frmmain.refresh();
                MessageBox.Show("Deleted Successfully");
            
            }
           
            // delete the col from databasesxml 

          

        }

        private void constrainsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            savetableAttributs(getdatafromdatagridview());
            frmmain.mydatabases.Clear();
            frmmain.loading_files();
           string colwanttocheckconstraint= dataGridView1.Rows[Rowindex].Cells[0].Value.ToString();
           if (dataGridView1.Rows[Rowindex].Cells[1].Value.ToString() == "Int32" || dataGridView1.Rows[Rowindex].Cells[1].Value.ToString() == "float")
            {
                if (File.Exists(this.databasename + ".xml"))
                {
                    frmmain.mydatabases.Clear();
                    frmmain.loading_files();
                    if (!(Rowindex > frmmain.mydatabases[databasename].mydatabase[tablename].data[0].Count))
                    {
                        //not finished
                        constraints frmcon = new constraints(frmmain, this.databasename, this.tablename, colwanttocheckconstraint, mydatabases);
                        frmcon.Show();
                        return;

                    }
                    MessageBox.Show("You Can Not Upadet Constraints After Entering Data !!"
                        + "\n Delete This Table And Make All Operations Before Entering Data !!");
                    return;
                }
                else {
                    constraints frmcon = new constraints(frmmain, this.databasename, this.tablename, colwanttocheckconstraint, mydatabases);
                    frmcon.Show();
                }
              

            }
           else
           {
               MessageBox.Show("To Make Check Constraint The Field Must Be Int32 or Float ");
           }
           
        }

        private void relationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Relations r = new Relations(frmmain, tablename, databasename, mydatabases[databasename]);
            r.Show();
        }
    }
}
