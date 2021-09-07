using Spire.Pdf;
using Spire.Pdf.Graphics;
using System.Drawing;

namespace BlizzardWind.App.Common.Tools
{
    public class PdfTool
    {
        public static void Paginate(string inPath,string outPath)
        {
            if (!File.Exists(inPath))
                throw new Exception("文件不存在");
            //加载PDF文档
            PdfDocument pdf = new PdfDocument();
            pdf.LoadFromFile(inPath);

            //获取第一页
            PdfPageBase page = pdf.Pages[0];

            //创建新PDF文档
            PdfDocument newPdf = new PdfDocument();

            //移除新文档的页边距
            newPdf.PageSettings.Margins.Left = 0;
            newPdf.PageSettings.Margins.Right = 0;
            newPdf.PageSettings.Margins.Top = 10;
            newPdf.PageSettings.Margins.Bottom = 10;


            //横向拆分：设置新文档页面的宽度等于原文档第一页的宽度，页面高度等于原文档第一页高度的二分之一
            newPdf.PageSettings.Width = page.Size.Width;
            newPdf.PageSettings.Height = page.Size.Width * 1.4f;

            //添加新页面到新文档
            PdfPageBase newPage = newPdf.Pages.Add();

            PdfTextLayout format = new PdfTextLayout();
            format.Break = PdfLayoutBreakType.FitPage;
            format.Layout = PdfLayoutType.Paginate;

            //根据原文档第一页创建模板，并将模板画到新文档的新添加页面，页面画满之后自动分页
            page.CreateTemplate().Draw(newPage, new PointF(0, 0), format);

            //保存
            newPdf.SaveToFile(outPath, FileFormat.PDF);
        }
    }
}
