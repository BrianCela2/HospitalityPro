using DTO.ReservationsDTOS;
using DTO.RoomDTOs;
using Entities.Models;
using DTO.SearchParametersList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.ReservationServiceDTOs;

namespace Domain.Contracts
{
	public interface IReservationDomain
	{
		Task AddReservationAsync(CreateReservationDTO reservationDto);
	    Task<PaginatedReservationDTO> GetAllReservationsAsync(int page, int pageSize, string sortField, string sortOrder);
		Task<ReservationDTO> GetReservationByIdAsync(Guid id);
		Task DeleteReservation(Guid reservationId);
		Task AddExtraService(Guid ReservationID,Guid serviceId);
        IEnumerable<ReservationDTO> GetReservationsOfUser();
		Task<PaginatedReservationDTO> ReservationsRoomAndService(int page, int pageSize, string sortField, string sortOrder, string searchString);
		Task UpdateReservation(UpdateReservationDTO updateReservationDTO);
		Task UpdateReservationStatus(Guid id, int status);

        public int GetStaysCountWithinDateRange(DateTime startDate, DateTime endDate);
		public decimal GetTotalRevenueWithinDateRange(DateTime startDate, DateTime endDate);
		public decimal getTotalReservationPrice(ReservationSampleDTO reservation);

    }
}
