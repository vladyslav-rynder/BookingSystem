using BookingSystem.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Api.Features.Slots;

public static class GetAvailableSlots
{
    public sealed record Response(int Id, DateTimeOffset StartTime, DateTimeOffset EndTime);

    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/slots", async (
            AppDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var now = DateTimeOffset.UtcNow;

            var availableSlots = await dbContext.Slots
                .AsNoTracking()
                .Where(s => !s.IsBooked && s.StartTime > now)
                .OrderBy(s => s.StartTime)
                .Select(s => new Response(s.Id, s.StartTime, s.EndTime))
                .ToListAsync(cancellationToken);

            return Results.Ok(availableSlots);
        })
        .WithTags("Slots");
    }
}