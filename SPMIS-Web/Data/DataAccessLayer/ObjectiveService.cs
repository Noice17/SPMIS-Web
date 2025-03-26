using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPMIS_Web.Models.Entities;
using SPMIS_Web.Models.ViewModels;

namespace SPMIS_Web.Data.DataAccessLayer
{
    public class ObjectiveService
    {
        public readonly ApplicationDbContext _dbContext;
        public ObjectiveService(ApplicationDbContext dbContext)
        {
             _dbContext = dbContext;
        }


        // UPDATE Objective
        //public async Task UpdateObjective(Objective objective)
        //{
        //    Console.WriteLine($"Updating Objective with ID: {objective.ObjectiveId}"); // Debugging Log

        //    var existingObjective = await _dbContext.Objectives.FindAsync(objective.ObjectiveId);

        //    if (existingObjective == null)
        //    {
        //        Console.WriteLine($"Objective not found: {objective.ObjectiveId}"); // Debugging Log
        //        throw new KeyNotFoundException("Objective not found.");
        //    }

        //    existingObjective.ObjectiveDescription = objective.ObjectiveDescription;
        //    existingObjective.ObjectiveTypeId = objective.ObjectiveTypeId;

        //    _dbContext.Objectives.Update(existingObjective);
        //    await _dbContext.SaveChangesAsync();
        //}


        // Add this method inside ObjectiveService.cs
        public async Task AddObjective(Objective objective)
        {
            if (objective == null)
            {
                throw new ArgumentException("Objective cannot be null.");
            }

            _dbContext.Objectives.Add(objective);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateObjectiveAsync(Objective objective)
        {
            _dbContext.Objectives.Update(objective);
            await _dbContext.SaveChangesAsync();            
        }


        //Add Objective Type
        //public async Task AddObjectiveType(ObjectiveType objectiveType)
        //{
        //    if (objectiveType == null || string.IsNullOrWhiteSpace(objectiveType.ObjectiveTypeName))
        //    {
        //        throw new ArgumentException("Objective Type cannot be null or empty.");
        //    }

        //    var newObjectiveType = new ObjectiveType
        //    {
        //        ObjectiveTypeName = objectiveType.ObjectiveTypeName
        //    };

        //    _dbContext.ObjectiveTypes.Add(newObjectiveType);
        //    await _dbContext.SaveChangesAsync();
        //}
        // Add Objective Type
        public async Task AddObjectiveType(ObjectiveType objectiveType)
        {
            if (objectiveType == null || string.IsNullOrWhiteSpace(objectiveType.ObjectiveTypeName))
            {
                throw new ArgumentException("Objective Type cannot be null or empty.");
            }

            var newObjectiveType = new ObjectiveType
            {
                ObjectiveTypeName = objectiveType.ObjectiveTypeName,
                IsActive = objectiveType.IsActive // Ensure IsActive is set
            };

            _dbContext.ObjectiveTypes.Add(newObjectiveType);
            await _dbContext.SaveChangesAsync();
        }


        //Retrieve
        public async Task<List<ObjectiveType>> GetObjectiveTypes()
        {
            return await _dbContext.ObjectiveTypes.ToListAsync();
        }
        //Edit the same page
        public async Task<bool> UpdateObjectiveType(Guid objectiveTypeId, string objectiveTypeName, bool isActive)
        {
            var objectiveType = await _dbContext.ObjectiveTypes.FindAsync(objectiveTypeId);
            if (objectiveType == null)
            {
                return false;
            }

            objectiveType.ObjectiveTypeName = objectiveTypeName;
            objectiveType.IsActive = isActive; // Update IsActive field

            _dbContext.ObjectiveTypes.Update(objectiveType);
            await _dbContext.SaveChangesAsync();

            return true;
        }




        public async Task<Objective?> GetObjectiveByIdAsync(Guid objectiveId)
        {
            return await _dbContext.Objectives
                                 .Include(o => o.Type)
                                 .FirstOrDefaultAsync(o => o.ObjectiveId == objectiveId);
        }



    }
}
