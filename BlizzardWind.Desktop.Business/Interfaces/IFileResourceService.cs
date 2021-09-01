using BlizzardWind.Desktop.Business.Entities;
using BlizzardWind.Desktop.Business.Models;

namespace BlizzardWind.Desktop.Business.Interfaces
{
    public interface IFileResourceService
    {
        /// <summary>
        /// 添加文本资源
        /// </summary>
        /// <param name="fileNames"></param>
        /// <returns></returns>
        public Task<List<MarkTextFileModel>> AddArticleFileAsync(int type,  List<string> fileNames, Guid? articleID = null);

        /// <summary>
        /// 获取文本资源
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Task<List<MarkTextFileModel>> GetArticleFilesAsync(Guid articleID,int type = -1);
    }
}
