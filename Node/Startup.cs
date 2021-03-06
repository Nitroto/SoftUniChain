﻿using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Node.Interfaces;
using Node.Models;
using Node.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace Node
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
            services.AddSingleton(Configuration.GetSection("NodeInfo").Get<NodeInformation>());
            services.AddSingleton<INodeService, NodeService>();
            services.AddSingleton<IPeerService, PeerService>();
            services.AddSingleton<ITransactionService, TransactionService>();

            services.AddAutoMapper();

            services.AddCors();

            services.AddMvc();
//                .AddJsonOptions(options =>
//            {
//                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
//                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
//            });
            services.AddMvcCore()
                .AddApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "SoftUni Blockchain Node",
                    Version = "v1",
                    Description = "SoftUni Blockchain Node ASP.NET Core REST API",
                    TermsOfService = "None",
                    Contact = new Contact {Name = "Asen Todorov", Email = "", Url = ""},
                    License = new License {Name = "", Url = ""}
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

//             Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

//             Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "SoftUni Blockchain Node V1"); });

            app.UseCors(
                options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            );
            app.UseMvc();
        }
    }
}