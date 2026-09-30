namespace SunamoDevCode._public;

internal interface IPercentCalculatorDC
{
    double OverallSum { get; set; }
    double Last { get; set; }
    IPercentCalculatorDC Create(double overallSum);
    void AddOnePercent();
    int PercentFor(double value, bool isLast);
    void ResetComputedSum();
}
