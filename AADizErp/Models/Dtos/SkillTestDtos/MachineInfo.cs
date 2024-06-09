namespace AADizErp.Models.Dtos.SkillTestDtos
{
    public class MachineInfo
    {
        public int MachineId { get; set; }
        public string MachineName { get; set; }
        public MachineInfo(int mcid, string name)
        {
            this.MachineId = mcid;
            this.MachineName = name;
        }
    }
}
