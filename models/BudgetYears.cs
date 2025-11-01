namespace BudgetP;

public class BudgetYear
{
    public string FiscalYear { get; set; }
    public DateTime openingDate { get; set; }
    public DateTime closingDate { get; set; }

}
public class updateBudgetYearModel
{
    public string budgetYear { get; set; }
    public DateTime openingDate { get; set; }
    public DateTime closingDate { get; set; }
}