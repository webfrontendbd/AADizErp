namespace AADizErp.Models.Dtos.SkillTestDtos
{
    public class McProcess
    {
        public int ProcessId { get; set; }
        public string ProcessName { get; set; }
        public int MachineId { get; set; }
        public McProcess(int pid, string name, int mcid)
        {
            this.ProcessId = pid;
            this.ProcessName = name;
            this.MachineId= mcid;
        }
    }
}
