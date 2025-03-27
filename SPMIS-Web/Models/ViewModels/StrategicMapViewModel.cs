using SPMIS_Web.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace SPMIS_Web.Models.ViewModels
{
    public class StrategicMapViewModel
    {
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

        public bool IsActive { get; set; }

        public List<ObjectiveType> ObjectiveType { get; set; } = new List<ObjectiveType>();

        public List<Objective> Objective { get; set; } = new List<Objective>();

        // Constructor with default values
        public StrategicMapViewModel()
        {
            MapDescription = string.Empty;
            MapTitle = string.Empty;
            ObjectiveType = new List<ObjectiveType>();
            Objective = new List<Objective>();
        }
    }
}
