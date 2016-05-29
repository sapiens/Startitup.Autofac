using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using CavemanTools;

namespace StartItUp
{
    /// <summary>
    /// 
    /// </summary>
    public static class AutofacExtensions
    {
        const string Key = "__autofac";

        /// <summary>
        /// Configure autofac
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="cfg"></param>
        public static void ConfigureContainer(this StartupContext ctx, Action<ContainerBuilder> cfg)
        {
            var data = ctx.Container();
            var cb=new ContainerBuilder();
            cfg(cb);
            cb.Update(data.ComponentRegistry);            
        }

        /// <summary>
        /// Gets the built container
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static ILifetimeScope Container(this StartupContext ctx)
            => ctx.ContextData.GetValueOrCreate(Key, () =>
            {
                var builder = new ContainerBuilder();
                builder.RegisterInstance(ctx).As(ctx.GetType()).SingleInstance();
                return builder.Build();
            });

        ///  <summary>
        ///   Register types ending with specified suffix from the specified assemblies
        ///   </summary>
        ///  <param name="cb"></param>
        /// <param name="cfgSingletons">
        /// Implicit suffixes: Cache
        /// </param>
        /// <param name="cfgServices">
        /// Implicit suffixes: Service, Store, Query, Updater, Creator, Manager.
        /// </param>
        /// <param name="asm"></param>
        public static void RegisterServices(this ContainerBuilder cb,Action<List<string>> cfgServices=null, Action<List<string>> cfgSingletons =null,params Assembly[] asm)
        {
            var services = new List<string>() { "Service", "Store", "Query", "Updater", "Creator", "Manager" };
            var singletons=new List<string>() {"Cache"};

            cfgServices.GetOrEmpty()(services);
            cfgSingletons.GetOrEmpty()(singletons);

            asm.ForEach(a =>
            {
                cb.RegisterAssemblyTypes(asm).Where(t => services.Any(d => t.Name.EndsWith(d)))
             .AsSelf()
             .AsImplementedInterfaces();

                cb.RegisterAssemblyTypes(asm).Where(t => singletons.Any(d => t.Name.EndsWith(d)))
             .AsSelf()
             .AsImplementedInterfaces()
             .SingleInstance();

            });
         
        }

      
    }
}