

public abstract class Provider
{
    private string _provType;

    protected Provider(string provType)
    {
        _provType = provType;
    }

    public int calculateInsulinValue()
    {
        return AccessMeasures.getInsulinValue();
    }

    public int getGlucoseValueFromDevice()
    {
        return AccessMeasures.getGlucoseValue();
    }

    public abstract void startSocket();
}