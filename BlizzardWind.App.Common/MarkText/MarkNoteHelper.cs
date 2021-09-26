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

        /// <summary>
        /// 根据符号获取元素类型
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static MarkNoteElementType GetElementType(string symbol)
        {
            MarkNoteElementType type = symbol switch
            {
                MarkNoteSymbolConsts.H1 => MarkNoteElementType.h1,
                MarkNoteSymbolConsts.H2 => MarkNoteElementType.h2,
                MarkNoteSymbolConsts.H3 => MarkNoteElementType.h3,
                MarkNoteSymbolConsts.KEY => MarkNoteElementType.key,
                MarkNoteSymbolConsts.PROFILE => MarkNoteElementType.profile,
                MarkNoteSymbolConsts.P => MarkNoteElementType.p,
                MarkNoteSymbolConsts.IMG => MarkNoteElementType.img,
                MarkNoteSymbolConsts.TXT => MarkNoteElementType.txt,
                MarkNoteSymbolConsts.LINK => MarkNoteElementType.link,
                MarkNoteSymbolConsts.LIST => MarkNoteElementType.list,
                MarkNoteSymbolConsts.SUMMARY => MarkNoteElementType.summary,
                MarkNoteSymbolConsts.QUOTE => MarkNoteElementType.quote,
                MarkNoteSymbolConsts.END => MarkNoteElementType.end,
                _ => MarkNoteElementType.other,
            };
            return type;
        }

        /// <summary>
        /// 判断是否为单行元素
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsSingleRowElement(MarkNoteElementType type)
        {
            bool isSingle;
            switch (type)
            {
                case MarkNoteElementType.profile:
                case MarkNoteElementType.list:
                case MarkNoteElementType.summary:
                case MarkNoteElementType.quote:
                case MarkNoteElementType.other:
                    isSingle = false;
                    break;
                default:
                    isSingle = true;
                    break;
            }
            return isSingle;
        }
    }
}
