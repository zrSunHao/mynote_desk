using BlizzardWind.Desktop.Business.Entities;
using BlizzardWind.Desktop.Business.Models;
using System;
using System.Threading.Tasks;

namespace BlizzardWind.Desktop.Business.Interfaces
{
    public interface IArticleService
    {
        public Task<bool> AddAsync(Article entity);

        public Task<Article> GetAsync(Guid id);

        public Task<PagingResult<Article>> GetListAsync(Guid? folderId, string sortColumn, string title, string key);

        public Task<bool> UpdateAsync(Article entity);

        public Task<bool> DeleteAsync(Guid id);

        public Task<int> GetFolderCountAsync(Guid folderId);

        public Task<int> GetFamilyCountAsync(Guid familyId);
    }

    public class ArticleColumnConsts
    {
        public const string Title = "Title";

        public const string UpdatedAt = "UpdatedAt";
    }
}
