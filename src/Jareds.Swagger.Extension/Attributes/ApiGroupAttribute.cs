using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jareds.Swagger.Extension
{
    /// <summary>
    /// 系统分组特性
    /// </summary>
    public class ApiGroupAttribute : Attribute, IApiDescriptionGroupNameProvider
    {
        public ApiGroupAttribute(object name)
        {
            GroupName = name.ToString();
        }
        public string GroupName { get; set; }
    }
}
