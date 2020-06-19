using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace Jareds.Swagger.Extension
{
    public static class ExtensionFunctions
    {
        public static void AddJaredsSwagger(this IServiceCollection services, Type groupEnumType, string title, string version, string[] docPaths, Action<SwaggerGenOptions> action = null)
        {
            services.AddSwaggerGen(o =>
            {
                groupEnumType.GetFields().Skip(1).ToList().ForEach(f =>
                {
                    //获取枚举值上的特性
                    var info = f.GetCustomAttributes(typeof(GroupInfoAttribute), false).OfType<GroupInfoAttribute>().FirstOrDefault();
                    o.SwaggerDoc(f.Name, new OpenApiInfo
                    {
                        Title = info?.Title,
                        Version = info?.Version,
                        Description = info?.Description
                    });
                });
                //没有加特性的分到这个NoGroup上
                o.SwaggerDoc("NoGroup", new OpenApiInfo
                {
                    Title = "无分组"
                });
                //判断接口归于哪个分组
                o.DocInclusionPredicate((docName, apiDescription) =>
                {
                    if (docName == "NoGroup")
                    {
                        //当分组为NoGroup时，只要没加特性的都属于这个组
                        return string.IsNullOrEmpty(apiDescription.GroupName);
                    }
                    else
                    {
                        return apiDescription.GroupName == docName;
                    }
                });
                o.CustomSchemaIds(type =>
                {
                    return type.FullName;
                });
                o.SwaggerDoc(version, new OpenApiInfo() { Title = title, Version = version });
                if (docPaths != null)
                {
                    foreach (var path in docPaths)
                    {
                        o.IncludeXmlComments(path);
                    }
                }
                action?.Invoke(o);
            });
        }

        public static void UseJaredsSwagger(this IApplicationBuilder app, Type groupEnumType)
        {
            app.UseSwagger();
            app.UseSwaggerUI(o =>
            {
                //遍历ApiGroupNames所有枚举值生成接口文档，Skip(1)是因为Enum第一个FieldInfo是内置的一个Int值
                groupEnumType.GetFields().Skip(1).ToList().ForEach(f =>
                {
                    //获取枚举值上的特性
                    var info = f.GetCustomAttributes(typeof(GroupInfoAttribute), false).OfType<GroupInfoAttribute>().FirstOrDefault();
                    o.SwaggerEndpoint($"/swagger/{f.Name}/swagger.json", info != null ? info.Title : f.Name);

                });
                o.SwaggerEndpoint("/swagger/NoGroup/swagger.json", "无分组");
            });
        }
    }
}
