namespace BlizzardWind.App.Common.MarkText
{
    public static class MarkNoteHelper
    {
        /// <summary>
        /// 获取文件过滤条件
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetWinFileFilter(int type)
        {
            string filter = "图像文件|*.jpg;*.jpeg;*.gif;*.png;";
            switch (type)
            {
                case MarkResourceType.Cover:
                    filter = "图像文件(*.jpg;*.jpeg;*.gif;*.png;)|*.jpg;*.jpeg;*.gif;*.png;";
                    break;
                case MarkResourceType.Image:
                    filter = "图像文件(*.jpg;*.jpeg;*.gif;*.png;)|*.jpg;*.jpeg;*.gif;*.png;";
                    break;
                case MarkResourceType.OfficeFile:
                    filter = "office文件(word,excel.ppt)|*.docx;*.doc;*.xlsx;*.xls;*.pptx;*.ppt;";
                    break;
                case MarkResourceType.Txt:
                    filter = "文本文件(*.txt;*.cs;)| *.txt;*.cs;";
                    break;
                case MarkResourceType.PDF:
                    filter = "PDF(*.pdf;)|*.pdf;";
                    break;
                case MarkResourceType.Audio:
                    filter = "音频文件(*.mp3;*.flac;)|*.mp3;*.flac;";
                    break;
                case MarkResourceType.Video:
                    filter = "图像文件(*.mp4;*.flv;)|*.mp4;*.flv;";
                    break;
            }
            return filter;
        }
    }
}
