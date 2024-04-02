using Domain.Contracts;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly IHotelServiceDomain _hotelServiceDomain;
    private readonly IReservationDomain _reservationDomain;
    private readonly IReservationRoomDomain _reservationRoomDomain;
    private readonly IRoomDomain _roomDomain;
    private readonly IUserDomain _userDomain;
    private readonly IUserRolesDomain _userRolesDomain;

    public DashboardController(IHotelServiceDomain hotelServiceDomain,
                                IReservationDomain reservationDomain,
                                IReservationRoomDomain reservationRoomDomain,
                                IRoomDomain roomDomain,
                                IUserDomain userDomain,
                                IUserRolesDomain userRolesDomain)
    {
        _hotelServiceDomain = hotelServiceDomain;
        _reservationDomain = reservationDomain;
        _reservationRoomDomain = reservationRoomDomain;
        _roomDomain = roomDomain;
        _userDomain = userDomain;
        _userRolesDomain = userRolesDomain;
    }

    [HttpGet("service-usage")]
    public IActionResult GetServiceUsageCount(Guid serviceId)
    {
        int count = _hotelServiceDomain.GetServiceUsageCount(serviceId);
        return Ok(count);
    }

    [HttpGet("stays-count")]
    public IActionResult GetStaysCountWithinDateRange(DateTime startDate, DateTime endDate)
    {
        int count = _reservationDomain.GetStaysCountWithinDateRange(startDate, endDate);
        return Ok(count);
    }

    [HttpGet("total-revenue")]
    public IActionResult GetTotalRevenueWithinDateRange(DateTime startDate, DateTime endDate)
    {
        decimal revenue = _reservationDomain.GetTotalRevenueWithinDateRange(startDate, endDate);
        return Ok(revenue);
    }

    [HttpGet("room-occupancy")]
    public IActionResult GetRoomOccupancyWithinDateRange(Guid roomId, DateTime startDate, DateTime endDate)
    {
        int occupancy = _reservationRoomDomain.GetRoomOccupancyWithinDateRange(roomId, startDate, endDate);
        return Ok(occupancy);
    }

    [HttpGet("room-reservations")]
    public IActionResult GetRoomReservationsWithinDateRange(DateTime startDate, DateTime endDate)
    {
        var reservations = _reservationRoomDomain.GetRoomReservationsWithinDateRange(startDate, endDate);
        return Ok(reservations);
    }

    [HttpGet("available-rooms")]
    public IActionResult GetAvailableRoomsCount()
    {
        int count = _roomDomain.GetAvailableRoomsCount();
        return Ok(count);
    }

    [HttpGet("active-users")]
    public IActionResult GetActiveUsersCount()
    {
        int count = _userDomain.GetActiveUsersCount();
        return Ok(count);
    }

    [HttpGet("role-users")]
    public IActionResult GetRoleUsersCount(int role)
    {
        int count = _userRolesDomain.GetRoleUsersCount(role);
        return Ok(count);
    }

}
