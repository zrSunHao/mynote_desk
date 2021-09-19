using BlizzardWind.Desktop.Business.Entities;
using BlizzardWind.Desktop.Business.Interfaces;
using BlizzardWind.Desktop.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlizzardWind.Desktop.Business.Services
{
    public class FamilyService : IFamilyService
    {
        private readonly IDatabaseService _dbService;

        public FamilyService(IDatabaseService service)
        {
            _dbService = service;
        }

        public async Task<bool> AddAsync(NoteFamily family)
        {
            var db = await _dbService.GetConnectionAsync();
            await db.InsertAsync(family);
            return true;
        }

        public async Task<List<NoteFamily>> GetListAsync()
        {
            var db = await _dbService.GetConnectionAsync();
            var query = db.Table<NoteFamily>()
                .Where(x=> !x.Deleted)
                .OrderBy(x => x.Name);
            return await query.ToListAsync(); ;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var db = await _dbService.GetConnectionAsync();
            var entity = await db.Table<NoteFamily>()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                return false;
            entity.DeletedAt = DateTime.Now;
            entity.Deleted = true;
            await db.UpdateAsync(entity);

            var folsers = await db.Table<NoteFolder>()
                .Where(x => !x.Deleted && x.FamilyId == id)
                .ToListAsync();
            var deleteFolderSql = $"UPDATE NoteFolder SET Deleted= '1', DeletedAt = '{DateTime.Now.Ticks.ToString()}' WHERE FamilyId = '{entity.Id.ToString()}'";
            await db.ExecuteAsync(deleteFolderSql);

            if (!folsers.Any())
                return true;
            foreach (var folder in folsers)
            {
                var deleteNoteSql = $"UPDATE Note SET Deleted= '1', DeletedAt = '{DateTime.Now.Ticks.ToString()}' WHERE FolderId = '{folder.Id.ToString()}'";
                await db.ExecuteAsync(deleteNoteSql);
            }
            return true;
        }

        public async Task<bool> UpdateAsync(NoteFamily family)
        {
            family.UpdatedAt = DateTime.Now;
            var db = await _dbService.GetConnectionAsync();
            await db.UpdateAsync(family);
            return true;
        }
    }
}
