using System;
using System.Collections.Generic;
using System.Text;

namespace Jareds.Swagger.Extension
{
    /// <summary>
    /// 系统模块枚举注释
    /// </summary>
    public class GroupInfoAttribute : Attribute
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
