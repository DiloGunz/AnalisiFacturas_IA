namespace ADIA.Model.Domain.Helpers;

/// <summary>
/// Helper para obtener un ID unico de tipo long
/// </summary>
internal class IdHelper
{
    private const long Epoch = 1288834974657L;

    private const int NodeIdBits = 10;
    private const int SequenceBits = 12;

    private const long MaxNodeId = -1L ^ (-1L << NodeIdBits);
    private const long SequenceMask = -1L ^ (-1L << SequenceBits);

    private const int NodeIdShift = SequenceBits;
    private const int TimestampLeftShift = SequenceBits + NodeIdBits;

    private static readonly object LockObject = new object();
    private static long lastTimestamp = -1L;
    private static long sequence = 0L;

    public static long NodeId { get; private set; } = 100;
    public long LastTimestamp => lastTimestamp;

    /// <summary>
    /// Genera El Id
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static long NextId()
    {
        lock (LockObject)
        {
            if (NodeId > MaxNodeId || NodeId < 0)
            {
                throw new InvalidOperationException($"Node ID must be between 0 and {MaxNodeId}.");
            }

            var timestamp = GetCurrentTimestamp();
            if (timestamp < lastTimestamp)
            {
                throw new InvalidOperationException("Clock moved backwards.");
            }

            if (lastTimestamp == timestamp)
            {
                sequence = (sequence + 1) & SequenceMask;
                if (sequence == 0)
                {
                    timestamp = WaitNextMillis(lastTimestamp);
                }
            }
            else
            {
                sequence = 0;
            }

            lastTimestamp = timestamp;

            return ((timestamp - Epoch) << TimestampLeftShift) |
                   (NodeId << NodeIdShift) |
                   sequence;
        }
    }

    private static long GetCurrentTimestamp()
    {
        return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
    }

    private static long WaitNextMillis(long lastTimestamp)
    {
        var timestamp = GetCurrentTimestamp();
        while (timestamp <= lastTimestamp)
        {
            timestamp = GetCurrentTimestamp();
        }
        return timestamp;
    }
}