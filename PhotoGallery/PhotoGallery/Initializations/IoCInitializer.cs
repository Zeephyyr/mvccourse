using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using PhotoGallery.DependencyResolution;
using PhotoGallery.IoC;
using PhotoGallery.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Mvc;

namespace PhotoGallery
{
    sealed class IoCInitializer
    {
        public static StructureMapDependencyScope StructureMapDependencyScope { get; set; }

        public void InitIoc()
        {
            Dictionary<Type, Type> bindings = new Dictionary<Type, Type>();

            bindings.Add(typeof(DbContext), typeof(ApplicationUsersDbContext));
            bindings.Add(typeof(IUserStore<ApplicationUser>), typeof(UserStore<ApplicationUser>));

            IoCContainer.Initalize(bindings);

            StructureMapDependencyScope = new StructureMapDependencyScope(IoCContainer.GetContainer());
            DependencyResolver.SetResolver(StructureMapDependencyScope);
            DynamicModuleUtility.RegisterModule(typeof(StructureMapScopeModule));
        }
    }
}