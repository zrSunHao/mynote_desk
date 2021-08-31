using System;

namespace BlizzardWind.Desktop.Business.Models
{
    public class EditorOperateModel
    {
        public string Name { get; set; }

        public string Icon { get; set; }

        public int Type { get; set; }
    }

    public class EditorOperateType
    {
        #region 编辑器操作

        public const int Save = 1;

        public const int CloudSync = 2;

        public const int UploadCoverPicture = 3;

        #endregion

        #region 编辑器操作 上传附件操作

        public const int UploadImage = 11;

        public const int UploadWord = 12;

        public const int UploadExcel = 13;

        public const int UploadPPT = 14;

        public const int UploadTxt = 15;

        public const int UploadPDF = 15;

        public const int UploadAudio = 16;

        public const int UploadVideo = 17;

        #endregion

        #region 附件操作

        public const int CopyFileID = 21;

        public const int ExportFile = 22;

        public const int ReplaceFile = 23;

        public const int RemoveFile = 24;

        #endregion
    }
}
