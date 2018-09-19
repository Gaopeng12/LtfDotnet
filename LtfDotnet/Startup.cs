using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace LtfDotnet
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            //app版本控制
            services.AddApiVersioning(option =>
            {
                option.ReportApiVersions = true;//当设置为 true 时, API 将返回响应标头中支持的版本信息
                //option.ApiVersionReader = new HeaderApiVersionReader("api-version");
                option.AssumeDefaultVersionWhenUnspecified = true;//此选项将用于不提供版本的请求。默认情况下, 假定的 API 版本为1.0。
                option.DefaultApiVersion = new ApiVersion(1, 0);//此选项用于指定在请求中未指定版本时要使用的默认 API 版本。这将默认版本为1.0。
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "LtfApi_v1", Version = "v1" });
                //c.SwaggerDoc("v2", new Info { Title = "LtfApi_v2", Version = "v2" });
                c.CustomSchemaIds(t => t.FullName);// 解决相同类名会报错的问题
                string xmlPath = Path.Combine(AppContext.BaseDirectory, "LtfDotnet.xml");
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            //app.UseApiVersioning();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "LtfApi_v1");
                //c.SwaggerEndpoint("/swagger/v2/swagger.json", "LtfApi_v2");
            });
        }
    }
}
