using System.Security.Claims;
using CarRental.Application.Bookings;
using CarRental.Application.DTOs;
using CarRental.Application.Exceptions;
using CarRental.Domain.Exceptions;
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
        try
        {
            var agentId = GetCurrentUserId();
            var response = await _bookingService.RegisterPickupAsync(request, agentId);
            return Ok(response);
        }
        catch (ApplicationValidationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (DomainValidationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("return")]
    public async Task<ActionResult<BookingResponse>> RegisterReturn([FromBody] RegisterReturnRequest request)
    {
        try
        {
            var response = await _bookingService.RegisterReturnAsync(request);
            return Ok(response);
        }
        catch (ApplicationValidationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (DomainValidationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    private int? GetCurrentUserId()
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(idClaim, out var id) ? id : null;
    }
}
