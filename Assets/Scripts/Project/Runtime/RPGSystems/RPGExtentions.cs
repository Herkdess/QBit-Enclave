using Base;
public static class RPGExtentions {
    public static void AdjustValues(this B_ATModifier atModifier) {
        atModifier.CalculationOrder = (int)atModifier.Type;
    }
}