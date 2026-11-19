using System;
using System.Collections.Generic;
using GestionVideojuegos.Data;
using GestionVideojuegos.Models;

namespace GestionVideojuegos.Services;

public class VideojuegoServicio
{
    private readonly VideojuegoDao _dao;

    public VideojuegoServicio(VideojuegoDao dao)
    {
        _dao = dao;
    }

    public List<Videojuego> Consultar() => _dao.ObtenerTodos();

    public void Registrar(string? nombre, string? genero, string? plataforma, string? precio)
    {
        _dao.Registrar(Validar(nombre, genero, plataforma, precio));
    }

    public bool Actualizar(int id, string? nombre, string? genero, string? plataforma, string? precio)
    {
        var videojuego = Validar(nombre, genero, plataforma, precio);
        videojuego.Id = id;
        return _dao.Actualizar(videojuego);
    }

    public bool Eliminar(int id) => _dao.Eliminar(id);

    private static Videojuego Validar(string? nombre, string? genero, string? plataforma, string? precio)
    {
        nombre = nombre?.Trim();
        genero = genero?.Trim();
        plataforma = plataforma?.Trim();
        precio = precio?.Trim();

        if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(genero) || string.IsNullOrWhiteSpace(plataforma))
            throw new ArgumentException("Completa nombre, género y plataforma.");
        if (nombre.Length > 100 || genero.Length > 60 || plataforma.Length > 60)
            throw new ArgumentException("Nombre: máximo 100 caracteres. Género y plataforma: máximo 60.");
        if (!int.TryParse(precio, System.Globalization.NumberStyles.None,
                System.Globalization.CultureInfo.InvariantCulture, out int valor))
            throw new ArgumentException("El precio debe ser un número entero sin puntos ni comas (ejemplo: 50000).");
        return new Videojuego(nombre, genero, plataforma, valor);
    }
}
