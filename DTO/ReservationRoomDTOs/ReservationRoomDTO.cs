﻿using DTO.ReservationsDTOS;
using DTO.RoomDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ReservationRoomDTOs
{
	public class ReservationRoomDTO
	{
		public Guid RoomId { get; set; }
		public DateTime CheckInDate { get; set; }
		public DateTime CheckOutDate { get; set; }

	}
}
