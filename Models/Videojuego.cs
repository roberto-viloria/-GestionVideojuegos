using System.Globalization;

namespace GestionVideojuegos.Models;

public class Videojuego
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Genero { get; set; } = string.Empty;
    public string Plataforma { get; set; } = string.Empty;
    public int Precio { get; set; }
    public string PrecioVisible => Precio.ToString("N0", CultureInfo.GetCultureInfo("es-CO"));

    public Videojuego() { }

    public Videojuego(string nombre, string genero, string plataforma, int precio)
    {
        Nombre = nombre;
        Genero = genero;
        Plataforma = plataforma;
        Precio = precio;
    }
}
