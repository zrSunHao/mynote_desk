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
        /// <param name="fileNames"></param>
        /// <returns></returns>
        public Task<List<MarkTextFileModel>> AddArticleFileAsync(int type,  List<string> fileNames, Guid? articleId = null);

        /// <summary>
        /// 获取文本资源
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Task<List<MarkTextFileModel>> GetArticleFilesAsync(Guid articleId,int type = -1);

        /// <summary>
        /// 根据id获取文件路径
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<string> GetPathByIdAsync(Guid id);

        /// <summary>
        /// 替换文件
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fileName">新文件路径</param>
        /// <returns></returns>
        public Task<bool> RelaceAsync(MarkTextFileModel model, string fileName);

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
