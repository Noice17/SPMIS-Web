using SPMIS_Web.Models.Entities;

namespace SPMIS_Web.Models.ViewModels
{
    public class AddObjectiveViewModel
    {
        public string ObjectiveDescription { get; set; }
        public Guid ObjectTypeId { get; set; }  // Ensure it's a Guid to match ObjectiveTypeId
        //public List<ObjectiveType> ObjectiveType { get; set; }
        public List<ObjectiveType> ObjectiveType { get; set; } = new List<ObjectiveType>();
        public Guid MapId { get; set; }
    }


}
