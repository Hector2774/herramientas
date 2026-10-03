namespace PromacoHerra
{
    // Moneda de la empresa: lempira hondureño (HNL). Único lugar donde se define el símbolo.
    public static class Dinero
    {
        public const string Simbolo = "L.";

        public static string Texto(decimal monto) => $"{Simbolo} {monto:N2}";
    }
}
