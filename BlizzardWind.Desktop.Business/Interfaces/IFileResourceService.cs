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
    }
}
