using System.Collections.Generic;

namespace FW.Bridge.Configuration;

public class ConfigData
{
    public List<(string id, int columnOffset)> DevicesList { get; set; } = new();
}