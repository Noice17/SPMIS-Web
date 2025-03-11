﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPMIS_Web.Models.Entities;

namespace SPMIS_Web.Data.DataAccessLayer
{
    public class ObjectiveService
    {
        public readonly ApplicationDbContext _dbContext;
        public ObjectiveService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        //Create
        public async Task AddObjectiveType(ObjectiveType objectiveType)
        {
            if (objectiveType == null || string.IsNullOrWhiteSpace(objectiveType.ObjectiveTypeName))
            {
                throw new ArgumentException("Objective Type cannot be null or empty.");
            }

            var newObjectiveType = new ObjectiveType
            {
                ObjectiveTypeName = objectiveType.ObjectiveTypeName
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
        public async Task<bool> UpdateObjectiveType(Guid id, string newName)
        {
            var objectiveType = await _dbContext.ObjectiveTypes.FindAsync(id);
            if (objectiveType == null)
            {
                return false;
            }

            objectiveType.ObjectiveTypeName = newName;
            await _dbContext.SaveChangesAsync();
            return true;
        }

    }
}
