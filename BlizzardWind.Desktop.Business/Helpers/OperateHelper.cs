using BlizzardWind.App.Common.MarkText;
using BlizzardWind.Desktop.Business.Consts;
using BlizzardWind.Desktop.Business.Models;
using System.Collections.ObjectModel;

namespace BlizzardWind.Desktop.Business.Helpers
{
    public static class OperateHelper
    {
        /// <summary>
        /// 编辑器主要操作
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<OperateModel> GetEditorMainOperate()
        {
            return new ObservableCollection<OperateModel>()
            {
                new OperateModel(){Name = "保存",  Type = EditorOperateType.Save},
                new OperateModel(){Name = "云同步",  Type = EditorOperateType.CloudSync},
                new OperateModel(){Name = "查看",  Type = EditorOperateType.See},
            };
        }

        /// <summary>
        /// 编辑器文件上传操作
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<OperateModel> GetEditorFileUploadOperate()
        {
            return new ObservableCollection<OperateModel>()
            {
                new OperateModel(){Name = "图片", Type = MarkResourceType.Image},
                new OperateModel(){Name = "office文件", Type = MarkResourceType.OfficeFile},
                new OperateModel(){Name = "文本文档", Type = MarkResourceType.Txt},
                new OperateModel(){Name = "PDF", Type = MarkResourceType.PDF},
                new OperateModel(){Name = "音频", Type = MarkResourceType.Audio},
                new OperateModel(){Name = "视频", Type = MarkResourceType.Video},
            };
        }
    }
}
