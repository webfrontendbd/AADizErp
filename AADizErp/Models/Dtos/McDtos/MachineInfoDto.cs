using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AADizErp.Models.Dtos.McDtos
{
    public class MachineInfoDto
    {
        public int Mcnum { get; set; }
        public string Mcid { get; set; }
        public int Orgid { get; set; }
        public string Orgname { get; set; }
        public int Floorid { get; set; }
        public string Floorname { get; set; }
        public string Line { get; set; }
        public string Status { get; set; }
    }
}
