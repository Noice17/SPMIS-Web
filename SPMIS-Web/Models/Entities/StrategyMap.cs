using System.ComponentModel.DataAnnotations;

namespace SPMIS_Web.Models.Entities
{
    public class StrategyMap
    {
        [Key]
        public Guid MapId { get; set; }
        public string MapDescription { get; set; }
        public string MapTitle { get; set; }
        public DateTime MapStart { get; set; }
        public DateTime MapEnd { get; set; }
        public bool? IsActive { get; set; }

    }
}
