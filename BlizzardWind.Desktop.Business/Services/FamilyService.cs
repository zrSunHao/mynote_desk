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

        public async Task<bool> AddAsync(ArticleFamily family)
        {
            var db = await _dbService.GetConnectionAsync();
            await db.InsertAsync(family);
            return true;
        }

        public async Task<List<ArticleFamily>> GetListAsync()
        {
            var db = await _dbService.GetConnectionAsync();
            var query = db.Table<ArticleFamily>()
                .Where(x=> !x.Deleted)
                .OrderBy(x => x.Name);
            return await query.ToListAsync(); ;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var db = await _dbService.GetConnectionAsync();
            var entity = await db.Table<ArticleFamily>()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                entity.DeletedAt = DateTime.Now;
                entity.Deleted = true;
                await db.UpdateAsync(entity);
            }
            return true;
        }

        public async Task<bool> UpdateAsync(ArticleFamily family)
        {
            family.UpdatedAt = DateTime.Now;
            var db = await _dbService.GetConnectionAsync();
            await db.UpdateAsync(family);
            return true;
        }
    }
}
