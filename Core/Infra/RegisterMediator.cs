using Autofac;
using MediatR;
using System;
using System.Collections.Generic;

namespace Core.Infra
{
    public class RegisterMediator : Module
    {
        private readonly IList<Type> _typesToScan;

        public RegisterMediator(IList<Type> typesToScan)
        {
            _typesToScan = typesToScan;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).Assembly).AsImplementedInterfaces();

            var mediatrOpenTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(INotificationHandler<>),
            };

            foreach (var mediatrOpenType in mediatrOpenTypes)
            {
                foreach (var type in _typesToScan)
                {
                    builder
                        .RegisterAssemblyTypes(type.Assembly)
                        .AsClosedTypesOf(mediatrOpenType)
                        .AsImplementedInterfaces();
                }
            }

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
        }
    }
}
