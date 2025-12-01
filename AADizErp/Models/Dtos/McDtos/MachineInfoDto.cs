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
        public string Serial { get; set; }
        public string Status { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Rpm { get; set; }
        public string MotorType { get; set; }
        public string ActivityMode { get; set; }
        public int Cost { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
    public class GroupMachineStatusDto
    {
        public int Orgid { get; set; }
        public string Orgname { get; set; }
        public string MachineName { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public int Quantity { get; set; }
    }

    public class MachineStatusUpdateDto
    {
        public string Mcid { get; set; }
        public int Floorid { get; set; }
        public string Floorname { get; set; }
        public string Line { get; set; }
        public string Status { get; set; }
        public string Serial { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Rpm { get; set; }
        public string MotorType { get; set; }
        public string ActivityMode { get; set; }
        public string UpdatedBy { get; set; }
    }
    public class MachineFloorDto
    {
        public int Floorid { get; set; }
        public string Floorname { get; set; }
    }
    public class MachineLineDto
    {
        public string Lineid { get; set; }
        public string Linename { get; set; }
    }
    public class ServiceHistoryModel
    {
        public DateTime ServiceDate { get; set; }
        public string ServiceType { get; set; }
        public int ServiceCost { get; set; }
        public string Remarks { get; set; }
    }

    public class SparePartsModel
    {
        public string PartName { get; set; }
        public int Quantity { get; set; }
        public int PartsCost { get; set; }
        public DateTime ChangedDate { get; set; }
        public string Remarks { get; set; }
        public int TotalCost
        {
            get { return Quantity * PartsCost; }
        }
    }

    public class MovementHistoryModel
    {
        public string Mcid { get; set; }
        public int Orgid { get; set; }
        public string Orgname { get; set; }
        public string MovedDate { get; set; }
    }
}
