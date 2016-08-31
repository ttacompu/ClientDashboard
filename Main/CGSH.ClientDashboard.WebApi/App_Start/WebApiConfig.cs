using CGSH.ClientDashboard.BusinessLogic;
using CGSH.ClientDashboard.BusinessLogic.Util;
using CGSH.ClientDashboard.Interface.BusinessLogic;
using CGSH.ClientDashboard.Interface.DataAccess;
using CGSH.ClientDashboard.WebApi.Controllers;
using CGSH.ClientDashboard.WebApi.Resolver;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.Unity;
using System.Web.Http;
using System.Web.Http.Cors;


namespace CGSH.ClientDashboard.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            var container = new UnityContainer();
            ConfigureEnterpriseLib(container);
            
            container.RegisterTypes(AllClasses.FromLoadedAssemblies(), WithMappings.FromMatchingInterface, WithName.Default, WithLifetime.Hierarchical);
            // GetMapping(container);
            config.DependencyResolver = new UnityResolver(container);

            SpecialControllerRegister(container);

            // Web API configuration and services
            var cors = new EnableCorsAttribute("*", "*", "*");
            cors.SupportsCredentials = true;
            config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }


        public static void SpecialControllerRegister(IUnityContainer container) {
            var keyResults = System.Configuration.ConfigurationManager.AppSettings.AllKeys;
            foreach (var keyName in keyResults) {
                if (keyName.EndsWith("CacheName")) {
                    var cacheManagerName = keyName;
                    var cacheTimeValue = keyName.Replace("CacheName", "") + "CacheTime";
                    if (!string.IsNullOrEmpty(cacheManagerName) && !string.IsNullOrEmpty(cacheTimeValue)) {
                        int cacheTime = 30;
                        string cacheManager = System.Configuration.ConfigurationManager.AppSettings[cacheManagerName];
                        int.TryParse(System.Configuration.ConfigurationManager.AppSettings[cacheTimeValue], out cacheTime);

                        IMemCache memCache;
                        if (!string.IsNullOrEmpty(cacheManager)) {
                            switch (cacheManager) {
                                case "EntityManager":
                                    memCache = new MemCache(cacheManager, cacheTime);
                                    IEntityManager entityMgr = new EntityManager(container.Resolve<IApiKeyManager>(), container.Resolve<IEntityDataAccess>(), memCache, container.Resolve<ICustomExceptionManager>());
                                    container.RegisterType<EntitiesController>(
                                        new InjectionFactory(c => new EntitiesController(entityMgr, container.Resolve<ICustomExceptionManager>())));
                                    break;

                                case "SearchManager":
                                    memCache = new MemCache(cacheManager, cacheTime);
                                    ISearchManager searchMgr = new SearchManager(container.Resolve<IApiKeyManager>(), container.Resolve<ISearchDataAccess>(), memCache, container.Resolve<ICustomExceptionManager>());
                                    container.RegisterType<SearchController>(
                                        new InjectionFactory(c => new SearchController(searchMgr, container.Resolve<ICustomExceptionManager>())));
                                    break;
                                case "TimekeeperManager":
                                    memCache = new MemCache(cacheManager, cacheTime);
                                    ITimekeeperManager timekeeperMgr = new TimekeeperManager(container.Resolve<IApiKeyManager>(), container.Resolve<ITimekeeperDataAccess>(), memCache, container.Resolve<ICustomExceptionManager>());
                                    container.RegisterType<TimekeepersController>(
                                        new InjectionFactory(c => new TimekeepersController(timekeeperMgr, container.Resolve<ICustomExceptionManager>())));
                                    break;
                                default:
                                    break;
                            }
                        }

                    }
                }
            }
        }

        public static void ConfigureEnterpriseLib(IUnityContainer container)
        {
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory());
            IConfigurationSource configurationSource = ConfigurationSourceFactory.Create();
            ExceptionPolicyFactory factory = new ExceptionPolicyFactory(configurationSource);

            LogWriterFactory logWriterFactory = new LogWriterFactory(configurationSource);
            Logger.SetLogWriter(logWriterFactory.Create());
            container.RegisterType<ExceptionManager>(new InjectionFactory(x => factory.CreateManager()));
        }
    }
}
