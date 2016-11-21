namespace PhotoGallery.DependencyResolution {
    using System.Web;


    using StructureMap.Web.Pipeline;

    public class StructureMapScopeModule : IHttpModule {
        #region Public Methods and Operators

        public void Dispose() {
        }

        public void Init(HttpApplication context) {
            context.BeginRequest += (sender, e) => IoCInitializer.StructureMapDependencyScope.CreateNestedContainer();
            context.EndRequest += (sender, e) => {
                HttpContextLifecycle.DisposeAndClearAll();
                IoCInitializer.StructureMapDependencyScope.DisposeNestedContainer();
            };
        }

        #endregion
    }
}