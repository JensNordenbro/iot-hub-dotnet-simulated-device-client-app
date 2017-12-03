namespace AnomalyDeviceLib
{
    public interface IValueConf
    {
        float Value { get; set; }
        float Deviation { get; set; }
        void Pingback(double value);
    }
}
