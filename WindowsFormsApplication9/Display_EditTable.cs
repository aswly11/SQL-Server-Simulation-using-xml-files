using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace WindowsFormsApplication9
{
    public partial class Display_EditTable : UserControl
    {
       Dictionary <string,databaseContent >mydatabases;
        string currentdatabase;
        string tableName;
        Main frmmain;
        
        public Display_EditTable(Dictionary<string, databaseContent> c,Main frm)
        {
            InitializeComponent();
            this.mydatabases = c;
            this.frmmain = frm;
          //  this.tableName = this.Parent.Name;

        }
        public Display_EditTable()
        {
            InitializeComponent();
            this.tableName = this.Parent.Name;
        }
         public string getdatabasename(string x)
        {
            string tablename = x;

             XmlDocument doc = new XmlDocument();
             doc.Load("DataBases.xml");
            XmlNodeList mytables = doc.GetElementsByTagName("Table");
                foreach (XmlNode i in mytables)
                    if (tablename == i.Attributes[0].Value)
                    {
                        currentdatabase = i.Attributes[1].Value;
                    }

                return currentdatabase;
        }
        public void addcoulnms_to_my_dataGridView(int num_of_columns, List<string> names)
        {
            for (int i = 0; i < num_of_columns; i++)
            {
                dataGridView1.Columns.Add(names[i],names[i]);
                dataGridView1.Columns[i].HeaderCell.ContextMenuStrip = contextMenuStrip1;
            }
        }
        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
           // if (e.Button == MouseButtons.Right)
             //   contextMenuStrip1.Show(dataGridView1, new Point(e.X, e.Y));
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = false;

            ///why ???
        }
        private void Display_EditTable_Load_1(object sender, EventArgs e)
        {
            List<string>columns= new  List<string>();
            this.tableName = this.Parent.Text;
             string database_name = getdatabasename(tableName);
             List<cols> mycols = frmmain.get_table_cols(tableName);

             for (int i = 0; i < mycols.Count; i++)
             {
                 columns.Add(mycols[i].colname);
                
             }
            addcoulnms_to_my_dataGridView(columns.Count, columns);
            for (int i = 0; i < dataGridView1.Columns.Count;i++ )
            {
                 if(mycols[i].hasprimarykey)
                 {
                     this.dataGridView1.Columns[i].HeaderCell.Style.BackColor = Color.Gold;
                 }
            }
                if (mydatabases.ContainsKey(database_name))
                {
                    if (mydatabases[database_name].mydatabase.ContainsKey(tableName))
                        for (int i = 0; i < mydatabases[database_name].mydatabase[tableName].data.Count; i++)
                        {

                            string[] row = new string[mydatabases[database_name].mydatabase[tableName].data[i].Count];
                            for (int j = 0; j < mydatabases[database_name].mydatabase[tableName].data[i].Count; j++)
                                row[j] = mydatabases[database_name].mydatabase[this.tableName].data[i][j];
                            dataGridView1.Rows.Add(row);
                        }

                }
           

        }
     
        void savedata()
        {
            if (!validation_is_correct())
            {
                MessageBox.Show("Values Dose Not Fit your DataTypes");
                return;
            }
            XmlWriter writer = XmlWriter.Create(currentdatabase+ ".xml");
            writer.WriteStartDocument();
            writer.WriteStartElement("Tables");
            writer.WriteAttributeString("Databasename", currentdatabase);
            this.mydatabases[currentdatabase].mydatabase[tableName].data.Clear();
            for (int i = 0; i < this.dataGridView1.Rows.Count - 1; i++)
            {
                List<string> temp = new List<string>();
                for (int j = 0; j < this.dataGridView1.Columns.Count; j++)
                    temp.Add(dataGridView1.Rows[i].Cells[j].Value.ToString());
                this.mydatabases[currentdatabase].mydatabase[tableName].data.Add(temp);
            }

            for (int i = 0; i < mydatabases[currentdatabase].mydatabase.Count; i++)
            {
                writer.WriteStartElement("Table");
                //   MessageBox.Show("TableName " + mydatabase.mydatabase.ElementAt(i).Key);
                writer.WriteAttributeString("TableName", mydatabases[currentdatabase].mydatabase.ElementAt(i).Key);
                for (int c = 0; c < mydatabases[currentdatabase].mydatabase.ElementAt(i).Value.data.Count; c++)
                {
                    writer.WriteStartElement("row");
                    for (int k = 0; k < mydatabases[currentdatabase].mydatabase.ElementAt(i).Value.data[c].Count; k++)
                    {
                       // MessageBox.Show("cell" + k, mydatabases[currentdatabase].mydatabase.ElementAt(i).Value.data[c][k]);
                        
                        writer.WriteAttributeString("cell" + k, mydatabases[currentdatabase].mydatabase.ElementAt(i).Value.data[c][k]);//adding attr insteadof elments because of some errors
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
            mydatabases.Clear();
            frmmain.loading_files();

            MessageBox.Show("saved");

               

        }
        bool validation_is_correct() {

            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                for (int j = 0; j < dataGridView1.Columns.Count;j++)
                    if (dataGridView1.Rows[i].Cells[j].ErrorText != "")
                        return false;

            return true;
        }
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                //databaseContent mydatabase1 = this.mydatabase;
                savedata();
            }
          
        }

         void primarykey_validations(int rowindex,int columindex)
        {

             if(mydatabases[currentdatabase].mydatabase[tableName].col[columindex].hasprimarykey)
             {
                
                     string currentvalue = dataGridView1.Rows[rowindex].Cells[columindex].Value.ToString();
                     for(int j =0 ; j<dataGridView1.Rows.Count-1;j++)
                     {
                         if (currentvalue == dataGridView1.Rows[j].Cells[columindex].Value.ToString() && rowindex != j)
                         {
                             string error = "this column is Primarykey .. values must be unique";
                             dataGridView1.Rows[rowindex].Cells[columindex].ErrorText = error;
                             return;

                         }
                    }


             }

        }
        void constraints_validations(int rowindex,int columindex)
        {
            if (mydatabases[this.currentdatabase].mydatabase[tableName].col[columindex].constraint != "null")
            {
                string[] arr = mydatabases[this.currentdatabase].mydatabase[tableName].col[columindex].constraint.Split('@');
                switch (arr[0])
                {
                    case ">":
                        {
                            if (!(Int32.Parse(dataGridView1.Rows[rowindex].Cells[columindex].Value.ToString()) > Int32.Parse(arr[1])))
                            {
                                string error = "this value dose Not fit its constraints !!" + "\n Value " + arr[0] + " " + arr[1];
                                dataGridView1.Rows[rowindex].Cells[columindex].ErrorText = error;
                            }

                            break;
                        }
                    case "<":
                        if (!(Int32.Parse(dataGridView1.Rows[rowindex].Cells[columindex].Value.ToString()) < Int32.Parse(arr[1])))
                        {
                            string error = "this value dose Not fit its constraints !!" + "\n Value " + arr[0] + " " + arr[1];
                            dataGridView1.Rows[rowindex].Cells[columindex].ErrorText = error;
                        }
                        break;
                    case "=":
                        if (!(Int32.Parse(dataGridView1.Rows[rowindex].Cells[columindex].Value.ToString()) == Int32.Parse(arr[1])))
                        {
                            string error = "this value dose Not fit its constraints !!" + "\n Value " + arr[0] + " " + arr[1];
                            dataGridView1.Rows[rowindex].Cells[columindex].ErrorText = error;
                        }
                        break;
                    case "!=":
                        if (!(Int32.Parse(dataGridView1.Rows[rowindex].Cells[columindex].Value.ToString()) != Int32.Parse(arr[1])))
                        {
                            string error = "this value dose Not fit its constraints !!" + "\n Value " + arr[0] + " " + arr[1];
                            dataGridView1.Rows[rowindex].Cells[columindex].ErrorText = error;
                        }
                        break;
                    case ">=":
                        if (!(Int32.Parse(dataGridView1.Rows[rowindex].Cells[columindex].Value.ToString()) >= Int32.Parse(arr[1])))
                        {
                            string error = "this value dose Not fit its constraints !!" + "\n Value " + arr[0] + " " + arr[1];
                            dataGridView1.Rows[rowindex].Cells[columindex].ErrorText = error;
                        }
                        break;
                    case "<=":
                        if (!(Int32.Parse(dataGridView1.Rows[rowindex].Cells[columindex].Value.ToString()) <= Int32.Parse(arr[1])))
                        {
                            string error = "this value dose Not fit its constraints !!" + "\n Value " + arr[0] + " " + arr[1];
                            dataGridView1.Rows[rowindex].Cells[columindex].ErrorText = error;
                        }
                        break;
                }
            }



        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
         {
             frmmain.mydatabases.Clear();
             frmmain.loading_files();
           

            try
            {

                if (!mydatabases.ContainsKey(currentdatabase) || !mydatabases[currentdatabase].mydatabase.ContainsKey(tableName))
                {

                    if (!mydatabases.ContainsKey(currentdatabase))
                    {
                        databaseContent temp1 = new databaseContent();
                        temp1.name = currentdatabase;
                        table t1 = new table();
                        t1.col = frmmain.get_table_cols(this.tableName);
                        temp1.mydatabase.Add(this.tableName, t1);
                        mydatabases.Add(temp1.name, temp1);
                    }
                    else
                    {
                        if (!mydatabases[currentdatabase].mydatabase.ContainsKey(tableName))
                        {

                            databaseContent temp = new databaseContent();
                            temp.name = currentdatabase;
                            table t = new table();
                            t.col = frmmain.get_table_cols(this.tableName);
                            mydatabases[currentdatabase].mydatabase.Add(tableName, t);
                        }
                    }

                }
                int r;
              

                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {

                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        if ((dataGridView1.Rows[i].Cells[j].Value == null || dataGridView1.Rows[i].Cells[j].Value == "") && mydatabases[this.currentdatabase].mydatabase[this.tableName].col[j].allownull == "False")
                        {
                            string error = "this field Dose Not Allow Null Value";
                            dataGridView1.Rows[i].Cells[j].ErrorText = error;
                          

                        }
                        else if ((dataGridView1.Rows[i].Cells[j].Value == null || dataGridView1.Rows[i].Cells[j].Value == "") && mydatabases[this.currentdatabase].mydatabase[this.tableName].col[j].allownull == "True")
                        {
                            dataGridView1.Rows[i].Cells[j].Value = "NULL";
                           
                        }else
                            dataGridView1.Rows[i].Cells[j].ErrorText = null;

                    }
                }
                if (mydatabases[currentdatabase].mydatabase.ContainsKey(tableName))
                    for (int i = 0; i < mydatabases[currentdatabase].mydatabase[this.tableName].col.Count; i++)
                    {

                        if (mydatabases[currentdatabase].mydatabase.ContainsKey(tableName))
                            if (dataGridView1.Columns[e.ColumnIndex].HeaderText == mydatabases[currentdatabase].mydatabase[this.tableName].col[i].colname)
                            {
                                switch (mydatabases[currentdatabase].mydatabase[this.tableName].col[i].datatype)
                                {
                                    case "string":
                                        {
                                            int x;
                                            float y;
                                            if ((int.TryParse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out x)) || (float.TryParse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out y)))
                                            {
                                                string error = "this field must be " + mydatabases[currentdatabase].mydatabase[this.tableName].col[i].datatype;
                                                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = error;

                                            }
                                            else
                                                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = null;

                                            break;
                                        }
                                    case "Int32":
                                    case "int32":
                                    case "int":
                                        {
                                            int x;
                                            float y;
                                            if (!(int.TryParse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out x)))
                                            {
                                                string error = "this field must be " + mydatabases[currentdatabase].mydatabase[this.tableName].col[i].datatype;
                                                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = error;

                                            }
                                            else
                                                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = null;

                                            break;
                                        }
                                    case "float":
                                        {
                                            int x;
                                            float y;
                                            if (!(float.TryParse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out y)))
                                            {
                                                string error = "this field must be " + mydatabases[currentdatabase].mydatabase[this.tableName].col[i].datatype;
                                                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = error;

                                            }
                                            else
                                                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = null;

                                            break;
                                        }
                                    case "char": dataGridView1.Columns[i].ValueType = typeof(char);//error bosiballe
                                        break;
                                    case "Date": dataGridView1.Columns[i].ValueType = typeof(DateTime);
                                        break;
                                }


                            }
                    }
                // validation of constraints
                        constraints_validations(e.RowIndex, e.ColumnIndex);
                //valdation on primarykey if exist
                        primarykey_validations(e.RowIndex, e.ColumnIndex);

            }          
            catch (NullReferenceException ex) {

                string error = "this field Dose Not Allow Null Value";
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = error;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
          
           
        }

        private void Display_EditTable_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try { if (e.Button == MouseButtons.Right)
               contextMenuStrip1.Show(dataGridView1, new Point(e.X, e.Y));
            colindex = e.ColumnIndex;
            contextMenuStrip1.Items[2].Text = "Set Primary Key";
           
            for (int i = 0; i < mydatabases[currentdatabase].mydatabase[this.tableName].col.Count; i++)
                if (mydatabases[currentdatabase].mydatabase[tableName].col[i].hasprimarykey)
                {
                    if (i == colindex)
                    { contextMenuStrip1.Items[2].Text = "Remove Primary Key";
                    break;
                    }           
                 }
            dataGridView1.Columns[e.ColumnIndex].HeaderCell.ContextMenuStrip.Show(dataGridView1, new Point(e.X, e.Y));
            }
            catch(ArgumentOutOfRangeException ex)
            {

            }
            
        }

        private void relationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Relations frm = new Relations(this.frmmain, this.tableName, this.currentdatabase,mydatabases[currentdatabase]);
            frm.Show();
        }

        int colindex;
        private void setPrimaryKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmmain.mydatabases.Clear();
            frmmain.loading_files();
            int index = -1;

            for (int i = 0; i < mydatabases[currentdatabase].mydatabase[this.tableName].col.Count; i++)
                if (mydatabases[currentdatabase].mydatabase[tableName].col[i].hasprimarykey)
                {
                    index = i;
                   
                }
            if (index == colindex || index==-1)
            {
                if (!mydatabases[currentdatabase].mydatabase[tableName].col[colindex].hasprimarykey)
                {
                    mydatabases[currentdatabase].mydatabase[tableName].col[colindex].hasprimarykey = true;
                    dataGridView1.Columns[colindex].HeaderCell.Style.BackColor = Color.Gold;
                    XmlDocument doc = new XmlDocument();
                    doc.Load("DataBases.xml");
                    XmlNodeList mytables = doc.GetElementsByTagName("Table");
                    foreach (XmlNode i in mytables)
                    {
                        if (tableName == i.Attributes[0].Value)
                        {
                            XmlNodeList mycols = i.ChildNodes;
                            foreach (XmlNode col in mycols)
                                if (mydatabases[currentdatabase].mydatabase[tableName].col[colindex].colname == col.Attributes[0].Value)
                                {
                                      col.Attributes[3].Value = "True";
                                }
                        }
                    }
                    doc.Save("DataBases.xml");
          
                }
                else
                {
                    mydatabases[currentdatabase].mydatabase[tableName].col[colindex].hasprimarykey = false;
                    dataGridView1.Columns[colindex].HeaderCell.Style.BackColor = Color.Gainsboro;
                    XmlDocument doc = new XmlDocument();
                    doc.Load("DataBases.xml");
                    XmlNodeList mytables = doc.GetElementsByTagName("Table");
                    foreach (XmlNode i in mytables)
                    {
                        if (tableName == i.Attributes[0].Value)
                        {
                            XmlNodeList mycols = i.ChildNodes;
                            foreach (XmlNode col in mycols)
                                if (mydatabases[currentdatabase].mydatabase[tableName].col[colindex].colname == col.Attributes[0].Value)
                                {
                                    col.Attributes[3].Value = "False";
                                }
                        }
                    }
                    doc.Save("DataBases.xml");
                }
                dataGridView1.EnableHeadersVisualStyles = false;
            }else
                MessageBox.Show("A Table Has Already A Primary Key");
                 
        }

        private void dataGridView1_ColumnHeaderCellChanged(object sender, DataGridViewColumnEventArgs e)
        {

        }

       
    }
}
