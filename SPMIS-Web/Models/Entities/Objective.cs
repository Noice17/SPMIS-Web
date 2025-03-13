using System.ComponentModel.DataAnnotations;

namespace SPMIS_Web.Models.Entities
{
    public class Objective
    {
        [Key]
        public Guid ObjectiveId { get; set; }
        public string ObjectiveDescription { get; set; }
        public string? ObjectiveCode { get; set; }
        public Guid MapId { get; set; }
        public StrategyMap Map { get; set; }

        public Guid ObjectiveTypeId { get; set; }
        public ObjectiveType Type { get; set; }
    }
}
