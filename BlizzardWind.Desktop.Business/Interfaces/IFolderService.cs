using BlizzardWind.Desktop.Business.Entities;

namespace BlizzardWind.Desktop.Business.Interfaces
{
    public interface IFolderService
    {
        public Task<bool> AddAsync(ArticleFolder family);

        public Task<bool> UpdateAsync(ArticleFolder family);

        public Task<bool> DeleteAsync(Guid id);

        public Task<List<ArticleFolder>> GetListAsync(Guid familyId, string name);

        public Task<List<ArticleFolder>> GetAllListAsync();
    }
}
