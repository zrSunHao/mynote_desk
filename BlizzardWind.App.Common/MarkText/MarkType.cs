﻿namespace BlizzardWind.App.Common.MarkText
{
    public enum MarkType
    {
        /// <summary>
        /// 段落
        /// </summary>
        p,

        /// <summary>
        /// 标题1,文章标题
        /// </summary>
        h1,

        /// <summary>
        /// 标题2
        /// </summary>
        h2,

        /// <summary>
        /// 标题3
        /// </summary>
        h3,

        /// <summary>
        /// 关键词
        /// </summary>
        key,

        /// <summary>
        /// 简介
        /// </summary>
        profile,

        /// <summary>
        /// 图片
        /// </summary>
        img,

        /// <summary>
        /// 链接
        /// </summary>
        link,

        /// <summary>
        /// 列表
        /// </summary>
        list,

        /// <summary>
        /// 表格
        /// </summary>
        table,

        /// <summary>
        /// 总结
        /// </summary>
        summary,

        /// <summary>
        /// 引用
        /// </summary>
        quote,

        /// <summary>
        /// 结尾
        /// </summary>
        end,

        /// <summary>
        /// 其他
        /// </summary>
        other,
    }

    public class MarkTypeConsts
    {
        public const string H1 = "h1";
        public const string H2 = "h2";
        public const string H3 = "h3";

        public const string KEY = "key";
        public const string PROFILE = "profile";
        public const string P = "p";
        public const string IMG = "img";
        public const string LINK = "link";
        public const string LIST = "list";
        public const string TABLE = "table";
        public const string SUMMARY = "summary";
        public const string QUOTE = "quote";
        public const string END = "--";
    }
}