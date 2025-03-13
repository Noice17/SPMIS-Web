using System;
using System.ComponentModel.DataAnnotations;

namespace SPMIS_Web.Models.Entities
{
    public class StrategyMap
    {
        [Key]
        public Guid MapId { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string MapDescription { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string MapTitle { get; set; }

        [Required(ErrorMessage = "Start Date is required")]
        [DataType(DataType.Date)]
        public DateTime MapStart { get; set; }

        [Required(ErrorMessage = "End Date is required")]
        [DataType(DataType.Date)]
        public DateTime MapEnd { get; set; }

        public bool? IsActive { get; set; }

        public StrategyMap()
        {
            MapDescription = string.Empty; // Default value
            MapTitle = string.Empty; // Default value
        }
        public List<ObjectiveType> ObjectiveType { get; set; } = new List<ObjectiveType>();

        public List<Objective> Objective { get; set; } = new List<Objective>();    }
}
