using BlizzardWind.App.Common.MarkText;
using BlizzardWind.Desktop.Business.Models;
using System.Collections.ObjectModel;

namespace BlizzardWind.Desktop.Business.Helpers
{
    public static class OptionHelper
    {
        public static ObservableCollection<OptionTypeItem> GetEditorFileTypes()
        {
            return new ObservableCollection<OptionTypeItem>()
            {
                new OptionTypeItem(){Name = "全部",Type = -1 },
                new OptionTypeItem(){Name = "封面",Type = MarkResourceType.Cover },
                new OptionTypeItem(){Name = "图片",Type = MarkResourceType.Image },
                new OptionTypeItem(){Name = "office文件",Type = MarkResourceType.OfficeFile },
                new OptionTypeItem(){Name = "文本文件",Type = MarkResourceType.Txt },
                new OptionTypeItem(){Name = "PDF",Type = MarkResourceType.PDF },
                new OptionTypeItem(){Name = "音频",Type = MarkResourceType.Audio },
                new OptionTypeItem(){Name = "视频",Type = MarkResourceType.Video },
            };
        }
    }
}
