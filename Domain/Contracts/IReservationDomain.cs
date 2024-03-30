using DTO.ReservationsDTOS;
using DTO.RoomDTOs;
using Entities.Models;
using DTO.SearchParametersList;
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

		//
		public int GetStaysCountWithinDateRange(DateTime startDate, DateTime endDate);
		public decimal GetTotalRevenueWithinDateRange(DateTime startDate, DateTime endDate);

    }
}
