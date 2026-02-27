namespace Back_end.Util;

public sealed class AppOptions
{
  public static readonly string OptionsJSON = "appsettings.json";
  public string? DBEnvConnectionString { get; set; }
}