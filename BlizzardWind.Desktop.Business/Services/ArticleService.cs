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
                .FirstOrDefaultAsync(x => x.Id == id);
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
            return await db.Table<Article>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagingResult<Article>> GetListAsync(Guid? folderId,string sortColumn,string title,string key)
        {
            var result = new PagingResult<Article>();
            var db = await _dbService.GetConnectionAsync();
            var query = db.Table<Article>().Where(x=>!x.Deleted);
            result.Total = await query.CountAsync();

            if(folderId.HasValue && folderId != Guid.Empty)
                query = query.Where(x=>x.FolderId ==  folderId.Value);
            if (!string.IsNullOrEmpty(title))
                query = query.Where(x => x.Title.Contains(title));
            if (!string.IsNullOrEmpty(key))
                query = query.Where(x => x.Title.Contains(key));
            result.FilterTotal = await query.CountAsync();

            if (sortColumn == ArticleColumnConsts.Title)
                query = query.OrderBy(x=>x.Title);
            else
                query = query.OrderByDescending(x => x.UpdatedAt);
            result.Items = await query.Take(30).ToListAsync();
            return result;
        }

        public async Task<bool> UpdateAsync(Article entity)
        {
            entity.UpdatedAt = DateTime.Now;
            var db = await _dbService.GetConnectionAsync();
            await db.UpdateAsync(entity);
            return true;
        }

        public async Task<int> GetFamilyCountAsync(Guid familyId)
        {
            var db = await _dbService.GetConnectionAsync();
            var folsers = await db.Table<ArticleFolder>()
                .Where(x => !x.Deleted && x.FamilyId == familyId)
                .ToListAsync();
            if (!folsers.Any())
                return 0;
            var count = 0;
            foreach (var foler in folsers)
            {
                count += await db.Table<Article>()
                .Where(x => !x.Deleted && x.FolderId == foler.Id)
                .CountAsync();
            }
            return count;
        }

        public async Task<int> GetFolderCountAsync(Guid folderId)
        {
            var db = await _dbService.GetConnectionAsync();
            return await db.Table<Article>()
                .Where(x => !x.Deleted && x.FolderId == folderId)
                .CountAsync();
        }
    }
}
