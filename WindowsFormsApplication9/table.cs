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
   public class table
   {
       
        public List<cols> col;     
        public List<List<string>> data;
       
        public  table()
        {
            col = new List<cols>();
            data = new List<List<string>>();
        }

        
    }
}
