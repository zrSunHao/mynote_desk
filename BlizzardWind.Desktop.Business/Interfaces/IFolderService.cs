using BlizzardWind.Desktop.Business.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlizzardWind.Desktop.Business.Interfaces
{
    public interface IFolderService
    {
        public Task<bool> AddAsync(NoteFolder family);

        public Task<bool> UpdateAsync(NoteFolder family);

        public Task<bool> DeleteAsync(Guid id);

        public Task<List<NoteFolder>> GetListAsync(Guid familyId, string name);

        public Task<List<NoteFolder>> GetAllListAsync();
    }
}
