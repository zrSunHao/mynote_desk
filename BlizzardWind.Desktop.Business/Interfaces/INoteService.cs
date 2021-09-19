using BlizzardWind.Desktop.Business.Entities;
using BlizzardWind.Desktop.Business.Models;
using System;
using System.Threading.Tasks;

namespace BlizzardWind.Desktop.Business.Interfaces
{
    public interface INoteService
    {
        public Task<bool> AddAsync(Note entity);

        public Task<Note> GetAsync(Guid id);

        public Task<PagingResult<Note>> GetListAsync(Guid? folderId, string sortColumn, string key, string content);

        public Task<bool> UpdateAsync(Note entity);

        public Task<bool> DeleteAsync(Guid id);

        public Task<int> GetFolderCountAsync(Guid folderId);

        public Task<int> GetFamilyCountAsync(Guid familyId);
    }

    public class NoteColumnConsts
    {
        public const string Title = "Title";

        public const string UpdatedAt = "UpdatedAt";
    }
}
