using BlizzardWind.App.Common.MarkText;
using BlizzardWind.Desktop.Business.Models;
using Microsoft.VisualBasic;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlizzardWind.Desktop.Business.ViewModels
{
    public partial class MarkTextPageViewModel : MvxViewModel
    {
        public ObservableCollection<MarkTextHeadlineModel> HeadlineCollection { get; set; }
        public ObservableCollection<MarkElement> ElementCollection { get; set; }
    }

    public partial class MarkTextPageViewModel
    {
        public MarkTextPageViewModel()
        {
            HeadlineCollection = new ObservableCollection<MarkTextHeadlineModel>()
            {
                new MarkTextHeadlineModel(){ 
                    Name = "大飒飒打撒打撒打撒",
                    Children = new List<MarkTextHeadlineModel>(){
                        new MarkTextHeadlineModel(){Name ="fsdfdss" },
                        new MarkTextHeadlineModel(){Name ="fsdfdss" },
                        new MarkTextHeadlineModel(){Name ="fsdfdss" },
                        new MarkTextHeadlineModel(){Name ="fsdfdss" },
                    }
                },
                new MarkTextHeadlineModel(){
                    Name = "发生发射点发射点发生",
                    Children = new List<MarkTextHeadlineModel>(){
                        new MarkTextHeadlineModel(){Name ="fsdfdss" },
                        new MarkTextHeadlineModel(){Name ="fsdfdss" },
                        new MarkTextHeadlineModel(){Name ="fsdfdss" },
                        new MarkTextHeadlineModel(){Name ="fsdfdss" },
                    }
                },
                new MarkTextHeadlineModel(){
                    Name = "发生发射点发射点发生",
                    Children = new List<MarkTextHeadlineModel>(){
                        new MarkTextHeadlineModel(){Name ="fsdfdss" },
                        new MarkTextHeadlineModel(){Name ="fsdfdss" },
                        new MarkTextHeadlineModel(){Name ="fsdfdss" },
                        new MarkTextHeadlineModel(){Name ="fsdfdss" },
                    }
                },
            };
            ElementCollection = new ObservableCollection<MarkElement>();
        }

        public void OnPageLoaded()
        {
            string text = "#h1] 文章总标题\r\n#h2] 二级标题\r\n#h3] 三级总标题\r\n\r\n#key] 关键词,说明\r\n#profile] 测试文件\r\n#p] 段落发射点发射点士大夫士大夫士大夫大师傅士大夫士大夫撒旦\r\nfsdfsdfsdfsdfdsf\r\n#img] <图片名>(images/avatar_cat.jpeg)\r\n#link] <链接名>(images/avatar_cat.jpeg)\r\n\r\n#list]\r\n1、大师傅士大夫士大夫的是\r\n2、的撒范德萨范德萨范德萨\r\n--\r\n#table] <名字|特点|称呼>\r\n刘备|哭|大哥\r\n关羽|打|二哥\r\n张飞|骂|三弟\r\n--\r\n#summary] 总结\r\n#quote]引用\r\n<引用1>(url)\r\n<引用2>(url)\r\n--\r\n";
            var parser = new MarkTextParser();
            var list = parser.GetMarkElements(text);
            foreach (var item in list)
            {
                ElementCollection.Add(item);
            }
            var z = ElementCollection.Count;
        }
    }
}
