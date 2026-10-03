// Services/PasswordHasher.cs
// Hash de contraseñas con PBKDF2-HMAC-SHA256 (incluido en .NET, sin paquetes extra).

using System.Security.Cryptography;
using System.Text;

namespace PromacoHerra.Services
{
    // Formato guardado en Usuario.PasswordHash:
    //   pbkdf2-sha256$<iteraciones>$<sal base64>$<hash base64>
    // Guardar las iteraciones permite subirlas en el futuro: los hashes viejos siguen
    // verificándose y se recalculan solos en el siguiente inicio de sesión (NecesitaRehash).
    public static class PasswordHasher
    {
        private const string Algoritmo = "pbkdf2-sha256";
        private const int Iteraciones = 600_000;   // recomendación OWASP para PBKDF2-HMAC-SHA256
        private const int BytesSal = 16;
        private const int BytesHash = 32;

        public const int LongitudMinima = 8;

        public static string Hash(string password)
        {
            byte[] sal = RandomNumberGenerator.GetBytes(BytesSal);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, sal, Iteraciones, HashAlgorithmName.SHA256, BytesHash);
            return $"{Algoritmo}${Iteraciones}${Convert.ToBase64String(sal)}${Convert.ToBase64String(hash)}";
        }

        public static bool Verificar(string password, string guardado)
        {
            if (!Leer(guardado, out int iteraciones, out byte[] sal, out byte[] esperado)) return false;

            byte[] calculado = Rfc2898DeriveBytes.Pbkdf2(password, sal, iteraciones, HashAlgorithmName.SHA256, esperado.Length);
            // Tiempo constante: no revela cuántos bytes coincidieron
            return CryptographicOperations.FixedTimeEquals(calculado, esperado);
        }

        public static bool NecesitaRehash(string guardado) =>
            !Leer(guardado, out int iteraciones, out _, out _) || iteraciones < Iteraciones;

        // Comparación de las contraseñas en texto plano previas a la migración 015.
        // Exacta (distingue mayúsculas) y en tiempo constante.
        public static bool IgualesTextoPlano(string a, string b) =>
            CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(a), Encoding.UTF8.GetBytes(b));

        /// <summary>Contraseña temporal legible: sin caracteres que se confunden (0/O, 1/l/I).</summary>
        public static string GenerarTemporal(int longitud = 10)
        {
            const string letras = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz";
            const string digitos = "23456789";
            const string todos = letras + digitos;

            var chars = new char[longitud];
            for (int i = 0; i < longitud; i++)
                chars[i] = todos[RandomNumberGenerator.GetInt32(todos.Length)];
            // Al menos un dígito, en una posición al azar
            chars[RandomNumberGenerator.GetInt32(longitud)] = digitos[RandomNumberGenerator.GetInt32(digitos.Length)];
            return new string(chars);
        }

        /// <summary>Reglas para contraseñas elegidas por el usuario. null = válida.</summary>
        public static string? ValidarNueva(string password, string username)
        {
            if (password.Length < LongitudMinima)
                return $"La contraseña debe tener al menos {LongitudMinima} caracteres.";
            if (!password.Any(char.IsLetter) || !password.Any(char.IsDigit))
                return "La contraseña debe combinar letras y números.";
            if (password.Contains(username, StringComparison.OrdinalIgnoreCase))
                return "La contraseña no puede contener el nombre de usuario.";
            return null;
        }

        private static bool Leer(string guardado, out int iteraciones, out byte[] sal, out byte[] hash)
        {
            iteraciones = 0;
            sal = hash = Array.Empty<byte>();

            var partes = guardado.Split('$');
            if (partes.Length != 4 || partes[0] != Algoritmo || !int.TryParse(partes[1], out iteraciones) || iteraciones <= 0)
                return false;
            try
            {
                sal = Convert.FromBase64String(partes[2]);
                hash = Convert.FromBase64String(partes[3]);
                return hash.Length > 0;
            }
            catch (FormatException) { return false; }
        }
    }
}
