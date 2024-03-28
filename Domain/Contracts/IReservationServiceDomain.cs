using DTO.ReservationServiceDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
	public interface IReservationServiceDomain
	{
		Task AddReservationServiceAsync(List<ReservationServiceDTO> reservationServiceDto);
	}
}
