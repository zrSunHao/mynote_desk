using BlizzardWind.Desktop.Business.Entities;
using BlizzardWind.Desktop.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlizzardWind.Desktop.Business.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IDatabaseService _dbService;

        public ArticleService(IDatabaseService service)
        {
            _dbService = service;
        }

        public async Task<bool> AddAsync(Article entity)
        {
            var db = await _dbService.GetConnectionAsync();
            await db.InsertAsync(entity);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var db = await _dbService.GetConnectionAsync();
            var entity = await db.Table<Article>()
                .FirstOrDefaultAsync(x => x.ID == id);
            if(entity != null)
            {
                entity.DeletedAt = DateTime.Now;
                entity.Deleted = true;
                await db.UpdateAsync(entity);
            }
            return true;
        }

        public async Task<Article> GetAsync(Guid id)
        {
            var db = await _dbService.GetConnectionAsync();
            return await db.Table<Article>().FirstOrDefaultAsync(x => x.ID == id);
        }

        public async Task<bool> UpdateAsync(Article entity)
        {
            entity.UpdatedAt = DateTime.Now;
            var db = await _dbService.GetConnectionAsync();
            await db.UpdateAsync(entity);
            return true;
        }
    }
}
