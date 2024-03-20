using DAL.DI;
using DAL.UoW;
using Domain.Concrete;
using Domain.Contracts;
using Lamar;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.DI
{
    public class DomainRegistry : ServiceRegistry
    {
        public DomainRegistry()
        {
            IncludeRegistry<DomainUnitOfWorkRegistry>();

            For<IUserDomain>().Use<UserDomain>();
            this.AddScoped<IRoomPhotoDomain, RoomPhotoDomain>();
            For<IRoomPhotoDomain>().Use<RoomPhotoDomain>();
            For<IRoomDomain>().Use<RoomDomain>();


            AddRepositoryRegistries();
            AddHttpContextRegistries();
        }
        private void AddRepositoryRegistries()
        {
            IncludeRegistry<RepositoryRegistry>();
        }
        private void AddHttpContextRegistries()
        {
            For<IHttpContextAccessor>().Use<HttpContextAccessor>();
        }
    }
}
