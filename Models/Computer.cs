using System;

namespace pcman.Models
{
    public class Computer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Ram { get; set; }
        public int Hdd { get; set; }
        public string Cpu { get; set; }
        public string Psu { get; set; }
        public string Motherboard { get; set; }
        public string Monitor { get; set; }
        public string Description { get; set; }
    }
}