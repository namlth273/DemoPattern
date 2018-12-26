using Autofac;
using Core.Common;
using Core.Infra;
using DataAccessLayer;
using EntityFramework.DbContextScope;
using EntityFramework.DbContextScope.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace DemoPattern
{
    internal class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new RegisterMediator(new List<Type>
            {
                typeof(DemoPattern.AutofacModule),
                typeof(DomainLogicLayer.AutofacModule),
                typeof(ApplicationLogicLayer.AutofacModule),
            }));

            builder.RegisterType<DbContextFactory>().As<IDbContextFactory>();
            builder.RegisterType<DbContextScopeFactory>().As<IDbContextScopeFactory>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            //builder.Register(c =>
            //{
            //    var config = c.Resolve<Microsoft.Extensions.Configuration.IConfiguration>();
            //    var userService = c.Resolve<IUserService>();

            //    var opt = new DbContextOptionsBuilder<AppDbContext>();
            //    opt.UseSqlServer(config.GetConnectionString("DefaultConnectionString"));

            //    return new AppDbContext(opt.Options, userService);
            //}).AsSelf().InstancePerLifetimeScope();
        }
    }
}