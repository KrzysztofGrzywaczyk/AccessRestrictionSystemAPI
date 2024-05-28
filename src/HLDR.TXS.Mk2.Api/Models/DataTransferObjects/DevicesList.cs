
using System.Collections.Generic;

namespace AccessControlSystem.Api.Models.DataTransferObjects;

public class DevicesList
{
    public List<GetDeviceDto>? AccessControlDevices { get; set; }
}
