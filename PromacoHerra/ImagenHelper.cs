using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace PromacoHerra
{
    // Fotos de herramientas: archivos en la carpeta Fotos\ junto al .exe; la BD guarda solo el
    // nombre del archivo (Herramienta.FotoNombre).
    // Las imágenes se leen con File.ReadAllBytes + MemoryStream y se copian a un Bitmap propio:
    // Image.FromFile deja el archivo bloqueado y luego no se puede sobrescribir al cambiar la foto.
    public static class ImagenHelper
    {
        public static readonly string FotosDir = Path.Combine(AppContext.BaseDirectory, "Fotos");

        // GDI+ no lee .webp ni .avif
        private const string FiltroImagenes = "Imágenes (*.jpg; *.jpeg; *.png; *.bmp; *.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

        private static readonly Color PlaceholderFondo = Color.FromArgb(240, 242, 246);
        private static readonly Color PlaceholderIcono = Color.FromArgb(154, 160, 173);

        private static Image? _defaultImage;

        // Miniaturas compartidas por tarjetas y filas: (ruta, lado) → imagen + fecha del archivo.
        // Quien las recibe NO debe liberarlas.
        private static readonly Dictionary<(string Ruta, int Lado), (DateTime Fecha, Image Imagen)> _miniaturas = new();

        // Placeholder cuando no hay foto (compartido: no liberar)
        public static Image DefaultImage => _defaultImage ??= CreatePlaceholder();

        /// <summary>
        /// Carga la foto escalada para que su lado mayor no pase de maxLado.
        /// Devuelve una imagen nueva (el llamador la libera) o null si no hay foto o no se pudo leer.
        /// </summary>
        public static Image? CargarFoto(string? fotoNombre, int maxLado = 600)
        {
            string? ruta = RutaDe(fotoNombre);
            if (ruta == null) return null;

            try
            {
                byte[] bytes = File.ReadAllBytes(ruta);
                using var ms = new MemoryStream(bytes);
                using var original = Image.FromStream(ms);
                return Escalar(original, maxLado);
            }
            catch { return null; }
        }

        /// <summary>
        /// Miniatura cacheada para listas (tarjetas de préstamo, filas de devolución).
        /// Se vuelve a leer si el archivo cambió. No liberar la imagen devuelta.
        /// </summary>
        public static Image? ObtenerMiniatura(string? fotoNombre, int lado)
        {
            string? ruta = RutaDe(fotoNombre);
            if (ruta == null) return null;

            var clave = (ruta.ToLowerInvariant(), lado);
            DateTime fecha = File.GetLastWriteTimeUtc(ruta);
            if (_miniaturas.TryGetValue(clave, out var cache) && cache.Fecha == fecha)
                return cache.Imagen;

            var imagen = CargarFoto(fotoNombre, lado);
            if (imagen == null) return null;

            // La versión anterior no se libera: otro formulario abierto puede seguir pintándola
            _miniaturas[clave] = (fecha, imagen);
            return imagen;
        }

        public static void MostrarEn(PictureBox pb, string? fotoNombre)
        {
            var anterior = pb.Image;
            pb.SizeMode = PictureBoxSizeMode.Zoom;
            pb.Image = CargarFoto(fotoNombre) ?? DefaultImage;

            // La anterior se libera después de reemplazarla, y nunca el placeholder compartido
            if (anterior != null && anterior != DefaultImage && anterior != pb.Image)
                anterior.Dispose();
        }

        /// <summary>
        /// Dibuja la imagen completa dentro de r, centrada y sin deformar (como PictureBoxSizeMode.Zoom).
        /// </summary>
        public static void DibujarAjustada(Graphics g, Image imagen, Rectangle r)
        {
            float escala = Math.Min((float)r.Width / imagen.Width, (float)r.Height / imagen.Height);
            float w = imagen.Width * escala, h = imagen.Height * escala;
            var modo = g.InterpolationMode;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imagen, r.X + (r.Width - w) / 2, r.Y + (r.Height - h) / 2, w, h);
            g.InterpolationMode = modo;
        }

        /// <summary>
        /// Abre un diálogo para elegir la foto y la copia a Fotos\ como {codigoHerramienta}.{ext}
        /// (el código evita problemas con nombres largos o caracteres especiales).
        /// Devuelve el nombre del archivo copiado, o null si el usuario canceló.
        /// Lanza InvalidOperationException si el archivo elegido no es una imagen legible.
        /// </summary>
        public static string? SeleccionarYCopiarFoto(string codigoHerramienta, IWin32Window? owner = null)
        {
            using var dlg = new OpenFileDialog
            {
                Title = "Seleccionar imagen",
                Filter = FiltroImagenes,
                Multiselect = false
            };

            if (dlg.ShowDialog(owner) != DialogResult.OK) return null;

            // Validar antes de copiar: un archivo dañado o con otra extensión no debe reemplazar la foto actual
            try
            {
                using var ms = new MemoryStream(File.ReadAllBytes(dlg.FileName));
                using var prueba = Image.FromStream(ms);
            }
            catch
            {
                throw new InvalidOperationException("El archivo seleccionado no es una imagen válida o su formato no es compatible.");
            }

            Directory.CreateDirectory(FotosDir);

            string nombreArchivo = codigoHerramienta + Path.GetExtension(dlg.FileName).ToLowerInvariant();
            string destino = Path.Combine(FotosDir, nombreArchivo);

            if (!string.Equals(Path.GetFullPath(dlg.FileName), Path.GetFullPath(destino), StringComparison.OrdinalIgnoreCase))
                File.Copy(dlg.FileName, destino, overwrite: true);

            return nombreArchivo;
        }

        // Ruta completa de una foto existente; solo el nombre del archivo, para no salir de Fotos\
        private static string? RutaDe(string? fotoNombre)
        {
            if (string.IsNullOrWhiteSpace(fotoNombre)) return null;
            string ruta = Path.Combine(FotosDir, Path.GetFileName(fotoNombre.Trim()));
            return File.Exists(ruta) ? ruta : null;
        }

        private static Bitmap Escalar(Image original, int maxLado)
        {
            float escala = Math.Min(1f, (float)maxLado / Math.Max(original.Width, original.Height));
            int w = Math.Max(1, (int)Math.Round(original.Width * escala));
            int h = Math.Max(1, (int)Math.Round(original.Height * escala));

            var bmp = new Bitmap(w, h);
            using var g = Graphics.FromImage(bmp);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawImage(original, 0, 0, w, h);
            return bmp;
        }

        // Fondo gris con el ícono de llave, del mismo estilo que el resto de la app
        private static Image CreatePlaceholder()
        {
            var bmp = new Bitmap(200, 200);
            using var g = Graphics.FromImage(bmp);
            g.Clear(PlaceholderFondo);
            using var icono = IconChar.Wrench.ToBitmap(PlaceholderIcono, 64);
            g.DrawImage(icono, (bmp.Width - 64) / 2, (bmp.Height - 64) / 2, 64, 64);
            return bmp;
        }
    }
}
