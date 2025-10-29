namespace dotnetfeatures;

public record HeartRateRecord(int HeartRate, DateTime Timestamp)
{
    public static HeartRateRecord Create(int heartRate) => new(heartRate, DateTime.UtcNow);
}