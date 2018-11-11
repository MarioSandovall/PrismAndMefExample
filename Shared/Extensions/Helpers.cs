namespace Shared.Extensions
{
    public static class Helpers
    {
        public static string ClearData(this string data)
        {
            return (data.Contains("\r") || data.Contains("\n"))
                ? data.Replace("\n", "").Replace("\r", "") : data;
        }
    }
}
