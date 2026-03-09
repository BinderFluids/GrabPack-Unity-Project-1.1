using System;

public interface IPowerable
{
    bool IsPowered { get; }
    
    event Action onPoweredOn;
    event Action onPowerOff;
    event Action<bool> onSetPowered;
    void PowerOn(); 
    void PowerOff(); 
}