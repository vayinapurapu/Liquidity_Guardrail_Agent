
using System.Text.Json.Serialization;

namespace FunctionApp;

public class StatementRecord
{
    public DateOnly Date { get; set; }
    public string Bank { get; set; } = string.Empty;
    public string Account_Id { get; set; } = string.Empty;
    public string Business_Unit { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public double Opening_Balance { get; set; }
    public double Closing_Balance { get; set; }
    public double Inflow { get; set; }
    public double Outflow { get; set; }
}

public class SummaryRow
{
    [JsonPropertyName("business_unit")] public string BusinessUnit { get; set; } = string.Empty;
    [JsonPropertyName("bank")] public string Bank { get; set; } = string.Empty;
    [JsonPropertyName("currency")] public string Currency { get; set; } = string.Empty;
    [JsonPropertyName("closing_balance")] public double ClosingBalance { get; set; }
    [JsonPropertyName("avg_7d")] public double Avg7d { get; set; }
    [JsonPropertyName("variance_pct")] public double VariancePct { get; set; }
    [JsonPropertyName("threshold")] public double Threshold { get; set; }
    [JsonPropertyName("breach")] public bool Breach { get; set; }
}

public class DetailRow
{
    [JsonPropertyName("date")] public string Date { get; set; } = string.Empty;
    [JsonPropertyName("business_unit")] public string BusinessUnit { get; set; } = string.Empty;
    [JsonPropertyName("bank")] public string Bank { get; set; } = string.Empty;
    [JsonPropertyName("currency")] public string Currency { get; set; } = string.Empty;
    [JsonPropertyName("inflow")] public double Inflow { get; set; }
    [JsonPropertyName("outflow")] public double Outflow { get; set; }
    [JsonPropertyName("closing_balance")] public double ClosingBalance { get; set; }
}

public class AggregateResponse
{
    [JsonPropertyName("asOf")] public string AsOf { get; set; } = string.Empty;
    [JsonPropertyName("summary")] public List<SummaryRow> Summary { get; set; } = new();
    [JsonPropertyName("details")] public List<DetailRow> Details { get; set; } = new();
}
