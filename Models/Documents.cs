using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDocuments.Models
{
    public partial class Documents
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double size { get; set; }

        public string type { get; set; }


    }
}
