using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Concrete;
using DAL.Contracts;
using Lamar;
using Microsoft.Extensions.DependencyInjection;

namespace DAL.DI
{
    public class RepositoryRegistry : ServiceRegistry
    {
        public RepositoryRegistry()
        {
            IncludeRegistry<UnitOfWorkRegistry>();

            For<IUserRepository>().Use<UserRepository>();
            this.AddScoped<IRoomPhotoRepository, RoomPhotoRepository>();

            For<IRoomPhotoRepository>().Use<RoomPhotoRepository>();
            For<IRoomRepository>().Use<RoomRepository>();
        
			For<IUserRolesRepository>().Use<UserRolesRepository>();

            //
            For<IHotelServiceRepository>().Use<HotelServiceRepository>();
        }


    }
}
