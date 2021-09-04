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

        public async Task<PagingResult<ArticleFolder>> GetListAsync(Guid familyId,int index, int size, string name)
        {
            if (index < 0) index = 0;
            if (size < 0) size = 20;
            var result = new PagingResult<ArticleFolder>() { PageIndex = index, PageSize = size };

            var db = await _dbService.GetConnectionAsync();
            result.Total = await db.Table<ArticleFolder>()
                .Where(x => !x.Deleted).CountAsync();

            var query = db.Table<ArticleFolder>()
                .Where(x=>x.FamilyId == familyId && !x.Deleted)
                .OrderBy(x => x.Name)
                .Skip(index * size)
                .Take(size);
            if (!string.IsNullOrEmpty(name))
                query = query.Where(x => x.Name.Contains(name));
            result.Items = await query.ToListAsync();
            return result;
        }

        public async Task<bool> RemoveAsync(Guid id)
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
