using System;

namespace AGVDistributionSystem.DTO
{
    public class RunningPODTO
    {
        public string Id {get; set;}
        public string PrepStatId {get; set;}
        public string StiStatId {get; set;}
        public string PO {get; set;}
        public string Factory {get; set;}
        public string Line {get; set;}
        public string Article {get; set;}
    }
}