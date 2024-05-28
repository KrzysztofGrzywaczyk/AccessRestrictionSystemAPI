namespace AccessControlSystem.Api.Models.DataTransferObjects;

public class GetDeviceDto
{
    public int? DeviceId { get; set; }

    public string? DeviceName { get; set; }

    public string? Description { get; set; }
}
