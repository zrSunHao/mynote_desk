using BlizzardWind.Desktop.Business.Entities;
using BlizzardWind.Desktop.Business.Models;

namespace BlizzardWind.Desktop.Business.Interfaces
{
    public interface IFamilyService
    {
        public Task<bool> AddAsync(ArticleFamily family);

        public Task<bool> UpdateAsync(ArticleFamily family);

        public Task<bool> DeleteAsync(Guid id);

        public Task<List<ArticleFamily>> GetListAsync();
    }
}
