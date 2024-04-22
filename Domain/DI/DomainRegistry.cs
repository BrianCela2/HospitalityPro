using DAL.Contracts;
using DAL.DI;
using Domain.Concrete;
using Domain.Contracts;
using Domain.Notifications;
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
			For<IReservationRoomDomain>().Use<ReservationRoomDomain>();
			For<IReservationDomain>().Use<ReservationDomain>();
            For<IHotelServiceDomain>().Use<HotelServiceDomain>();		
            For<INotificationDomain>().Use<NotificationDomain>();
            For<IReservationServiceDomain>().Use<ReservationServiceDomain>();
            For<IUserHistoryDomain>().Use<UserHistoryDomain>();
            For<NotificationHub>();
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
