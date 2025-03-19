using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPMIS_Web.Models.Entities
{
    public class Objective
    {
        [Key]
        public Guid ObjectiveId { get; set; }
        public string ObjectiveDescription { get; set; }
        public string? ObjectiveCode { get; set; }
        
        [ForeignKey("StrategyMap")]
        public Guid MapId { get; set; }
        public StrategyMap Map { get; set; }

        [ForeignKey("ObjectiveType")]
        public Guid ObjectiveTypeId { get; set; }
        public ObjectiveType Type { get; set; }
    }
}
