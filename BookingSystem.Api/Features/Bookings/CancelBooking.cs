using BookingSystem.Api.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Api.Features.Bookings;

public static class CancelBooking
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/bookings/{id:int}", async (
            [FromRoute] int id,
            AppDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var booking = await dbContext.Bookings
                .Include(b => b.Slot)
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

            if (booking is null)
            {
                return Results.NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Booking with ID {id} was not found."
                });
            }

            if (booking.Slot is not null && booking.Slot.StartTime <= DateTimeOffset.UtcNow)
            {
                return Results.BadRequest(new ProblemDetails
                {
                    Title = "Invalid Operation",
                    Detail = "Cannot cancel a booking for a slot that has already started or passed."
                });
            }

            if (booking.Slot is not null)
            {
                booking.Slot.IsBooked = false;
            }

            dbContext.Bookings.Remove(booking);

            await dbContext.SaveChangesAsync(cancellationToken);

            return Results.NoContent();
        })
        .WithTags("Bookings");
    }
}