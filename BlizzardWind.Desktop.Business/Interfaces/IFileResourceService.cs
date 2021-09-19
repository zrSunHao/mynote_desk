using BlizzardWind.Desktop.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlizzardWind.Desktop.Business.Interfaces
{
    public interface IFileResourceService
    {
        /// <summary>
        /// 添加文本资源
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fileNames"></param>
        /// <param name="noteId"></param>
        /// <returns></returns>
        public Task<List<MarkNoteFileModel>> AddNoteFileAsync(int type,  List<string> fileNames, Guid? noteId = null);

        /// <summary>
        /// 获取文本资源
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Task<List<MarkNoteFileModel>> GetNoteFilesAsync(Guid noteId, int type = -1);

        /// <summary>
        /// 根据id获取文件路径
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<MarkNoteFileModel?> GetByIdAsync(Guid id);

        /// <summary>
        /// 替换文件
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fileName">新文件路径</param>
        /// <returns></returns>
        public Task<bool> RelaceAsync(MarkNoteFileModel model, string fileName);

        /// <summary>
        /// 改名
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<bool> RenameAsync(Guid id, string name);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> DeleteAsync(Guid id);
    }
}
