using PhotoGallery.AppCommonCore.Contracts.DataAccess;
using PhotoGallery.AppCommonCore.Entities;
using PhotoGallery.DataAccess;
using StructureMap;
using System;
using System.Collections.Generic;

namespace PhotoGallery.IoC
{
    public static class IoCContainer
    {
        private static Container _container;
        private static bool _isReady;

        public static IContainer GetContainer()
        {
            if(_isReady)
            {
                return _container;
            }
            return null;
        }

        public static IContainer Initalize(Dictionary<Type, Type> bindings)
        {
            if (!_isReady)
            {
                _container = new Container();
                _container.Configure(x =>
                {
                    x.Scan(y =>
                    {
                        y.Assembly("PhotoGallery.AppCommonCore");
                        y.Assembly("PhotoGallery.Services");
                        y.Assembly("PhotoGallery.DataAccess");
                        y.Assembly("PhotoGallery.Logging");

                        y.RegisterConcreteTypesAgainstTheFirstInterface();
                    });

                    x.For<IRepository<User>>().Use <EFRepository<User>>();
                    x.For<IRepository<Photo>>().Use<EFRepository<Photo>>();
                    x.For<IRepository<Album>>().Use<EFRepository<Album>>();
                    x.For<ISearchRepository>().Use<EFSearchRepository>();

                    foreach (var element in bindings)
                    {
                        x.For(element.Key).Use(element.Value);
                    }
                });
                _isReady = true;
                return _container;
            }
            return null;
        }

        public static void ConfigureOwinAuthManager<T>(Type type,Func<T> func)
        {
            _container.Configure(x =>
            {
                x.For(type).Use(func.Invoke());
            });
        }

        public static T GetInstace<T>()
        {
            if(_isReady)
            {
                return _container.GetInstance<T>();
            }
            return default(T);
        }
    }
}
