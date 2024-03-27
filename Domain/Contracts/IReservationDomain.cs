using DTO.ReservationsDTOS;
using DTO.RoomDTOs;
using Entities.Models;
using Entities.SearchParametersList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
	public interface IReservationDomain
	{
		Task AddReservationAsync(CreateReservationDTO reservationDto);
		Task<IEnumerable<ReservationDTO>> GetAllReservationsAsync();
		Task<ReservationDTO> GetReservationByIdAsync(Guid id);

    }
}
