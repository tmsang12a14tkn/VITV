using Autofac;
using AutoMapper;
using System.Reflection;
using VITV.Data.Repositories;

namespace VITV.Web
{
    public static class Bootstrapper
    {
        public static void Run()
        {
            var builder = new ContainerBuilder();

            // assemblies
            var assemblies = new[] { Assembly.GetExecutingAssembly() };

            // modules
            builder.RegisterAssemblyTypes(assemblies)
                   .AsClosedTypesOf(typeof(ITypeConverter<,>))
                   .AsSelf();

            // AutoMapper initialization
            //Mapper.Initialize(x =>
            //{
            //    x.ConstructServicesUsing(type => container.Resolve(type));
            //});

            // startable which include mapper classes
            builder.RegisterAssemblyTypes(assemblies)
                   .Where(t => typeof(IStartable).IsAssignableFrom(t))
                   .As<IStartable>()
                   .SingleInstance();


            // repository
            builder.RegisterType<KeywordRepository>().As<IKeywordRepository>();

            builder.Build();
        }
    }
}