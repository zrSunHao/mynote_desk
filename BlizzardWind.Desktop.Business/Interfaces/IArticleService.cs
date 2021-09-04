using BlizzardWind.Desktop.Business.Entities;

namespace BlizzardWind.Desktop.Business.Interfaces
{
    public interface IArticleService
    {
        public Task<bool> AddAsync(Article entity);

        public Task<Article> GetAsync(Guid id);

        public Task<bool> UpdateAsync(Article entity);

        public Task<bool> DeleteAsync(Guid id);

        public Task<int> GetFolderCountAsync(Guid folderId);

        public Task<int> GetFamilyCountAsync(Guid familyId);
    }
}
