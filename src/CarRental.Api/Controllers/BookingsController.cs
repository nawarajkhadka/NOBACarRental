using System.Security.Claims;
using CarRental.Application.Bookings;
using CarRental.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Api.Controllers;

[ApiController]
[Route("api/bookings")]
[Authorize(Roles = "Agent,Manager")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost("pickup")]
    public async Task<ActionResult<BookingResponse>> RegisterPickup([FromBody] RegisterPickupRequest request)
    {
        var agentId = GetCurrentUserId();
        var response = await _bookingService.RegisterPickupAsync(request, agentId);
        return Ok(response);
    }

    [HttpPost("return")]
    public async Task<ActionResult<BookingResponse>> RegisterReturn([FromBody] RegisterReturnRequest request)
    {
        var response = await _bookingService.RegisterReturnAsync(request);
        return Ok(response);
    }

    private int? GetCurrentUserId()
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(idClaim, out var id) ? id : null;
    }
}
