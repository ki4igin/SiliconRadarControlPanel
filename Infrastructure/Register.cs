namespace SiliconRadarControlPanel.Infrastructure;

public static class Register
{
    public static int GetBitFiled(uint registerValue, int numberLSB, int fieldWith)
    {
        uint mask = (1U << (numberLSB + fieldWith)) - (1U << numberLSB);
        int field = (int)((registerValue & mask) >> numberLSB);
        return field;
    }
    public static uint SetBitFiled(uint registerValue, int field, int numberLSB, int fieldWith)
    {
        uint mask = (1U << (numberLSB + fieldWith)) - (1U << numberLSB);
        registerValue &= ~mask;
        registerValue |= ((uint)field << numberLSB) & mask;

        return registerValue;
    }

    public static bool IsBitSet(uint registerValue, int bit) => (registerValue & (1 << bit)) != 0;
    public static bool IsBitClear(uint registerValue, int bit) => (registerValue & (1 << bit)) == 0;
}
