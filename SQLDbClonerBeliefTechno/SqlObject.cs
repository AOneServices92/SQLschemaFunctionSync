using Microsoft.SqlServer.Management.Smo;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace SQLDbClonerBeliefTechno.Core.Schema
{
    public class SqlObject
    {
        public Bitmap Status { get; set; }
        public string Name { get; set; }

        [Browsable(false)]
        public NamedSmoObject Object { get; set; }

        [Browsable(false)]
        public List<NamedSmoObject> SubObject { get; set; }

        public string Type { get; set; }
        public string Error { get; set; }

        public SqlObject()
        {
            //Status = Properties.Resources.unknown;
        }
    }
}