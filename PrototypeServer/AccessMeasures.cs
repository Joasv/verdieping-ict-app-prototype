using System;

public class AccessMeasures
{
    public static int getGlucoseValue()
    {
        //* Glucose values are around 4 - 9
        Random rnd = new Random();
        return rnd.Next(4, 9);
    }

    public static int getInsulinValue()
    {
        //* Insulin value is calculated a bit different/
        //! for simplicity the ratio is set between 2 to 10
        Random rnd = new Random();
        return rnd.Next(2, 10);
    }

}