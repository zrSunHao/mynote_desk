using BlizzardWind.Desktop.Business.Entities;
using BlizzardWind.Desktop.Business.Models;

namespace BlizzardWind.Desktop.Business.Interfaces
{
    public interface IFolderService
    {
        public Task<bool> AddAsync(ArticleFolder family);

        public Task<bool> UpdateAsync(ArticleFolder family);

        public Task<bool> DeleteAsync(Guid id);

        public Task<List<ArticleFolder>> GetListAsync(Guid familyId, string name);
    }
}
