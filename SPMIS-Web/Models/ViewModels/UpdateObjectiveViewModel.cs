using SPMIS_Web.Models.Entities;

namespace SPMIS_Web.Models.ViewModels
{
    public class UpdateObjectiveViewModel
    {
        public Guid ObjectiveId { get; set; }
        public string ObjectiveDescription { get; set; }

        //public Guid ObjectiveTypeId { get; set; }
        public List<ObjectiveType> ObjectiveType { get; set; } = new List<ObjectiveType>();
        public Guid ObjectiveTypeId { get; set; }

        public Guid MapId { get; set; }
    }
}