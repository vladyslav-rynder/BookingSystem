using BookingSystem.Api.Domain;
using BookingSystem.Api.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Api.Features.Bookings;

public static class CreateBooking
{
    public sealed record Request(
        int SlotId,
        string CustomerName,
        string CustomerEmail
    );
    public sealed record Response(
        int Id,
        int SlotId,
        string CustomerName,
        string CustomerEmail,
        DateTimeOffset CreatedAt
    );

    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/bookings", async (
            [FromBody] Request request,
            AppDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            if (string.IsNullOrWhiteSpace(request.CustomerName))
            {
                return Results.BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = "Customer name is required."
                });
            }

            if (string.IsNullOrWhiteSpace(request.CustomerEmail) || !request.CustomerEmail.Contains('@'))
            {
                return Results.BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = "A valid customer email is required."
                });
            }

            var slot = await dbContext.Slots
                .FirstOrDefaultAsync(s => s.Id == request.SlotId, cancellationToken);

            if (slot is null)
            {
                return Results.NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Slot with ID {request.SlotId} does not exist."
                });
            }

            if (slot.IsBooked)
            {
                return Results.Conflict(new ProblemDetails
                {
                    Title = "Slot Already Booked",
                    Detail = "This slot has already been reserved."
                });
            }

            if (slot.StartTime <= DateTimeOffset.UtcNow)
            {
                return Results.BadRequest(new ProblemDetails
                {
                    Title = "Invalid Booking Time",
                    Detail = "Cannot book a slot that has already started or passed."
                });
            }

            slot.IsBooked = true;

            var booking = new Booking
            {
                SlotId = slot.Id,
                CustomerName = request.CustomerName.Trim(),
                CustomerEmail = request.CustomerEmail.Trim().ToLowerInvariant(),
                CreatedAt = DateTimeOffset.UtcNow
            };

            dbContext.Bookings.Add(booking);

            try
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException)
            {
                return Results.Conflict(new ProblemDetails
                {
                    Title = "Conflict",
                    Detail = "The slot was booked concurrently by another request."
                });
            }

            var response = new Response(
                booking.Id,
                booking.SlotId,
                booking.CustomerName,
                booking.CustomerEmail,
                booking.CreatedAt
            );

            return Results.Created($"/api/bookings/{booking.Id}", response);
        })
        .WithTags("Bookings");
    }
}