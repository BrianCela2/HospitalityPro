using DAL.Contracts;
using DAL.DI;
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
			For<IUserRolesDomain>().Use<UserRolesDomain>();
			For<IAuthDomain>().Use<AuthDomain>();
			For<IReservationDomain>().Use<ReservationDomain>();
			For<IReservationRoomDomain>().Use<ReservationRoomDomain>();

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
