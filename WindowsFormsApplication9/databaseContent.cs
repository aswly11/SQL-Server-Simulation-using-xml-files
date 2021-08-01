using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
namespace WindowsFormsApplication9
{
 
   public class databaseContent
    {   
       
        public Dictionary< string, table> mydatabase;   
        public string name;
        public databaseContent() {
            mydatabase = new Dictionary<string, table>();
        }
       public databaseContent(string tableName)
       {
           mydatabase = new Dictionary<string, table>();          
       }

      
    }
}
