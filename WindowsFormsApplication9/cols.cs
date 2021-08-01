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
  public  class cols
    {
       
        public string colname;        
        public string datatype;     
        public string allownull;
        public bool hasprimarykey = false;
        public string constraint;
        public cols(string cname, string ctype, string callow, string isprimary, string constraint)
        {
            this.colname = cname;
            this.datatype = ctype;
            this.allownull = callow;
            this.constraint = constraint; 
            if (isprimary == "True")
                hasprimarykey = true;
            else
                hasprimarykey = false;

        }
    }
}
