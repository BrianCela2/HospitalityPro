using DTO.RoomPhotoDTOs;
using Helpers.Enumerations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DTO.RoomDTOs
{
    public record UpdateRoomDTO
    {
        public Guid RoomId { get; set; }
        [Required]
        public int RoomNumber { get; set; }
        public int? Capacity { get; set; }
        [Required]
        public decimal Price { get; set; }
        public RoomStatus? RoomStatus { get; set; }
        public RoomCategory? Category { get; set; }
       
    }
}
