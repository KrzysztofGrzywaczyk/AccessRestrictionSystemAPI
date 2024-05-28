
namespace AccessControlSystem.Api.Entities;

public class AccessMapping
{
    public int SlotId { get; set; }

    public int AccessCardId { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        AccessMapping other = (AccessMapping)obj;
        return SlotId == other.SlotId && AccessCardId == other.AccessCardId;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = (hash * 23) + SlotId.GetHashCode();
            hash = (hash * 23) + AccessCardId.GetHashCode();
            return hash;
        }
    }
}