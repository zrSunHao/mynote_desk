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
    public class FolderService : IFolderService
    {
        private readonly IDatabaseService _dbService;

        public FolderService(IDatabaseService service)
        {
            _dbService = service;
        }

        public async Task<bool> AddAsync(ArticleFolder folder)
        {
            var db = await _dbService.GetConnectionAsync();
            await db.InsertAsync(folder);
            return true;
        }

        public async Task<List<ArticleFolder>> GetListAsync(Guid familyId, string name)
        {
            var db = await _dbService.GetConnectionAsync();
            var query = db.Table<ArticleFolder>()
                .Where(x=>x.FamilyId == familyId && !x.Deleted)
                .OrderBy(x => x.Name);
            if (!string.IsNullOrEmpty(name))
                query = query.Where(x => x.Name.Contains(name));
            return await query.ToListAsync(); ;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var db = await _dbService.GetConnectionAsync();
            var entity = await db.Table<ArticleFolder>()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                entity.DeletedAt = DateTime.Now;
                entity.Deleted = true;
                await db.UpdateAsync(entity);
            }
            var sql = $"UPDATE Article SET Deleted= '1', DeletedAt = '{DateTime.Now.Ticks.ToString()}' WHERE FolderId = '{id.ToString()}'";
            await db.ExecuteAsync(sql);
            return true;
        }

        public async Task<bool> UpdateAsync(ArticleFolder folder)
        {
            folder.UpdatedAt = DateTime.Now;
            var db = await _dbService.GetConnectionAsync();
            await db.UpdateAsync(folder);
            return true;
        }
    }
}
