namespace BookingSystem.Api.Domain;
public class Booking
{
    public int Id { get; set; }
    public int SlotId { get; set; }
    public Slot? Slot { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}