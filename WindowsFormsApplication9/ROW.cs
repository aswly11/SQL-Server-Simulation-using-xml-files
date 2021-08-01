using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication9
{
   public class ROW
    {
        public string columnName;
        public string dataType;
        public bool allownull;
       public bool primarykeystate=false;
       public string constraint;
     
        public ROW(string columnname, string datatype, string allownull ,string constraint) {

            this.constraint = constraint;
            if (allownull == "True")
                this.allownull = true;
            else
                this.allownull = false;
            this.columnName = columnname;
            this.dataType = datatype;

        }
    }
}
