﻿using Autofac;
using AutoMapper;
using Core.Infra;
using DataAccessLayer;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace DemoPattern
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(new RegisterFluentValidation(
                    new List<Type>
                    {
                        typeof(DomainLogicLayer.AutofacModule),
                        typeof(ApplicationLogicLayer.AutofacModule),
                    }).ConfigurationExpression);

            services.AddAutoMapper();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString")));

            //services.AddDbContext<AppDbContext>(opt =>
            //    opt.UseInMemoryDatabase("TodoList"));
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<DemoPattern.AutofacModule>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
