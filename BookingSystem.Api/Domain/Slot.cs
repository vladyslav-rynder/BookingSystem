namespace BookingSystem.Api.Domain;
public class Slot
{
    public int Id { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
    public bool IsBooked { get; set; } = false;
}