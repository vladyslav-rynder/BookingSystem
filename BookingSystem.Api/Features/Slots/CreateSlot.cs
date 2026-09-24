using BookingSystem.Api.Domain;
using BookingSystem.Api.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Api.Features.Slots;

public static class CreateSlot
{
    public sealed record Request(DateTimeOffset StartTime, DateTimeOffset EndTime);

    public sealed record Response(int Id, DateTimeOffset StartTime, DateTimeOffset EndTime);

    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/slots", async (
            [FromBody] Request request,
            AppDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            if (request.EndTime <= request.StartTime)
            {
                return Results.BadRequest(new ProblemDetails
                {
                    Title = "Invalid time range",
                    Detail = "EndTime must be greater than StartTime."
                });
            }

            if (request.StartTime < DateTimeOffset.UtcNow)
            {
                return Results.BadRequest(new ProblemDetails
                {
                    Title = "Invalid time range",
                    Detail = "Cannot create slots in the past."
                });
            }

            var slot = new Slot
            {
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                IsBooked = false
            };

            dbContext.Slots.Add(slot);
            await dbContext.SaveChangesAsync(cancellationToken);

            var response = new Response(slot.Id, slot.StartTime, slot.EndTime);

            return Results.Created($"/api/slots/{slot.Id}", response);
        })
        .WithTags("Slots");
    }
}