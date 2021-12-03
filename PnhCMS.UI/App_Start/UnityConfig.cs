using PnhCMS.Repository;
using System.Web.Mvc;
using Unity;
using Unity.log4net;
using Unity.Mvc5;

namespace PnhCMS.UI
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.AddNewExtension<Log4NetExtension>();
            container.RegisterType(typeof(IRepository<>), typeof(Repository<>));

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}