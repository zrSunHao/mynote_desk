namespace BlizzardWind.App.Common.MarkText
{
    // 元素层级
    public class MarkElementLevel
    {
        // 需要跳过的元素
        public const int Skip = 1024;       // 文章标题

        // 一级元素
        public const int Title_1 = 512;     // H2
        public const int Single = 511;      // Profile Summary Quote

        // 二级元素
        public const int Title_2 = 256;     // H3

        // 叶子元素
        public const int Leaf = 0;          // 其他
    }
}
