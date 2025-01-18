using FW.Bridge.Data;
using FW.Bridge.USB;
using ReactiveUI;

namespace FW.Bridge.ViewModels;

public class DeviceViewModel : ViewModelBase
{
    private bool _isConnected;
    private DeviceData _data;
    private DeviceController _controller;

    public DeviceViewModel(int columnOffset, string serial = "")
    {
        _data.Serial = serial;
        _data.ColumnOffset = columnOffset;
        _controller = new(this);
    }
    
    
    
    public string Serial {get => _data.Serial; set => this.RaiseAndSetIfChanged(ref _data.Serial, value); }
    public int ColumnOffset {get => _data.ColumnOffset; set => this.RaiseAndSetIfChanged(ref _data.ColumnOffset, value); }
    public bool IsConnected {get => _isConnected; set => this.RaiseAndSetIfChanged(ref _isConnected, value); }
    
    #region Device state public properties
    
    public bool Button101Push {get => _data.Button101Push; set => this.RaiseAndSetIfChanged(ref _data.Button101Push, value); }
    public bool Button102Push {get => _data.Button102Push; set => this.RaiseAndSetIfChanged(ref _data.Button102Push, value); }
    public bool Button103Push {get => _data.Button103Push; set => this.RaiseAndSetIfChanged(ref _data.Button103Push, value); }
    public bool Button104Push {get => _data.Button104Push; set => this.RaiseAndSetIfChanged(ref _data.Button104Push, value); }
    public bool Button105Push {get => _data.Button105Push; set => this.RaiseAndSetIfChanged(ref _data.Button105Push, value); }
    
    public bool Button201Push {get => _data.Button201Push; set => this.RaiseAndSetIfChanged(ref _data.Button201Push, value); }
    public bool Button202Push {get => _data.Button202Push; set => this.RaiseAndSetIfChanged(ref _data.Button202Push, value); }
    public bool Button203Push {get => _data.Button203Push; set => this.RaiseAndSetIfChanged(ref _data.Button203Push, value); }
    public bool Button204Push {get => _data.Button204Push; set => this.RaiseAndSetIfChanged(ref _data.Button204Push, value); }
    public bool Button205Push {get => _data.Button205Push; set => this.RaiseAndSetIfChanged(ref _data.Button205Push, value); }
    
    public bool Button301Push {get => _data.Button301Push; set => this.RaiseAndSetIfChanged(ref _data.Button301Push, value); }
    public bool Button302Push {get => _data.Button302Push; set => this.RaiseAndSetIfChanged(ref _data.Button302Push, value); }
    public bool Button303Push {get => _data.Button303Push; set => this.RaiseAndSetIfChanged(ref _data.Button303Push, value); }
    public bool Button304Push {get => _data.Button304Push; set => this.RaiseAndSetIfChanged(ref _data.Button304Push, value); }
    public bool Button305Push {get => _data.Button305Push; set => this.RaiseAndSetIfChanged(ref _data.Button305Push, value); }

    public bool Rotary1Push {get => _data.Rotary1Push; set => this.RaiseAndSetIfChanged(ref _data.Rotary1Push, value); }
    public bool Rotary2Push {get => _data.Rotary2Push; set => this.RaiseAndSetIfChanged(ref _data.Rotary2Push, value); }
    public bool Rotary3Push {get => _data.Rotary3Push; set => this.RaiseAndSetIfChanged(ref _data.Rotary3Push, value); }
    public bool Rotary4Push {get => _data.Rotary4Push; set => this.RaiseAndSetIfChanged(ref _data.Rotary4Push, value); }
    public bool Rotary5Push {get => _data.Rotary5Push; set => this.RaiseAndSetIfChanged(ref _data.Rotary5Push, value); }
    
    public double Rotary1Rot {get => _data.Rotary1Rot; set => this.RaiseAndSetIfChanged(ref _data.Rotary1Rot, value); }
    public double Rotary2Rot {get => _data.Rotary2Rot; set => this.RaiseAndSetIfChanged(ref _data.Rotary2Rot, value); }
    public double Rotary3Rot {get => _data.Rotary3Rot; set => this.RaiseAndSetIfChanged(ref _data.Rotary3Rot, value); }
    public double Rotary4Rot {get => _data.Rotary4Rot; set => this.RaiseAndSetIfChanged(ref _data.Rotary4Rot, value); }
    public double Rotary5Rot {get => _data.Rotary5Rot; set => this.RaiseAndSetIfChanged(ref _data.Rotary5Rot, value); }

    public double Fader1Cur {get => _data.Fader1Cur; set => this.RaiseAndSetIfChanged(ref _data.Fader1Cur, value); }
    public double Fader2Cur {get => _data.Fader2Cur; set => this.RaiseAndSetIfChanged(ref _data.Fader2Cur, value); }
    public double Fader3Cur {get => _data.Fader3Cur; set => this.RaiseAndSetIfChanged(ref _data.Fader3Cur, value); }
    public double Fader4Cur {get => _data.Fader4Cur; set => this.RaiseAndSetIfChanged(ref _data.Fader4Cur, value); }
    public double Fader5Cur {get => _data.Fader5Cur; set => this.RaiseAndSetIfChanged(ref _data.Fader5Cur, value); }

    public double Fader1Tgt {get => _data.Fader1Tgt; set => this.RaiseAndSetIfChanged(ref _data.Fader1Tgt, value); }
    public double Fader2Tgt {get => _data.Fader2Tgt; set => this.RaiseAndSetIfChanged(ref _data.Fader2Tgt, value); }
    public double Fader3Tgt {get => _data.Fader3Tgt; set => this.RaiseAndSetIfChanged(ref _data.Fader3Tgt, value); }
    public double Fader4Tgt {get => _data.Fader4Tgt; set => this.RaiseAndSetIfChanged(ref _data.Fader4Tgt, value); }
    public double Fader5Tgt {get => _data.Fader5Tgt; set => this.RaiseAndSetIfChanged(ref _data.Fader5Tgt, value); }

    #endregion
}