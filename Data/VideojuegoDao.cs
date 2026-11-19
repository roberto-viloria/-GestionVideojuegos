using System;
using System.Collections.Generic;
using System.IO;
using GestionVideojuegos.Models;
using Microsoft.Data.Sqlite;

namespace GestionVideojuegos.Data;

public class VideojuegoDao
{
    private readonly string _cadenaConexion;

    public string RutaBaseDatos { get; }

    public VideojuegoDao()
    {
        string carpeta = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "GestionVideojuegos");
        Directory.CreateDirectory(carpeta);
        RutaBaseDatos = Path.Combine(carpeta, "videojuegos.db");
        _cadenaConexion = new SqliteConnectionStringBuilder { DataSource = RutaBaseDatos }.ToString();
        Inicializar();
    }

    private void Inicializar()
    {
        using var conexion = new SqliteConnection(_cadenaConexion);
        conexion.Open();
        using var comando = conexion.CreateCommand();
        comando.CommandText = """
            CREATE TABLE IF NOT EXISTS Videojuegos (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre TEXT NOT NULL,
                Genero TEXT NOT NULL,
                Plataforma TEXT NOT NULL,
                Precio INTEGER NOT NULL CHECK (Precio >= 0)
            );
            """;
        comando.ExecuteNonQuery();
    }

    public List<Videojuego> ObtenerTodos()
    {
        var resultado = new List<Videojuego>();
        using var conexion = new SqliteConnection(_cadenaConexion);
        conexion.Open();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "SELECT Id, Nombre, Genero, Plataforma, Precio FROM Videojuegos ORDER BY Id DESC";
        using var lector = comando.ExecuteReader();
        while (lector.Read())
        {
            resultado.Add(new Videojuego
            {
                Id = lector.GetInt32(0),
                Nombre = lector.GetString(1),
                Genero = lector.GetString(2),
                Plataforma = lector.GetString(3),
                Precio = lector.GetInt32(4)
            });
        }
        return resultado;
    }

    public void Registrar(Videojuego videojuego)
    {
        using var conexion = new SqliteConnection(_cadenaConexion);
        conexion.Open();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "INSERT INTO Videojuegos (Nombre, Genero, Plataforma, Precio) VALUES ($nombre, $genero, $plataforma, $precio)";
        AgregarParametros(comando, videojuego);
        comando.ExecuteNonQuery();
    }

    public bool Actualizar(Videojuego videojuego)
    {
        using var conexion = new SqliteConnection(_cadenaConexion);
        conexion.Open();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "UPDATE Videojuegos SET Nombre=$nombre, Genero=$genero, Plataforma=$plataforma, Precio=$precio WHERE Id=$id";
        AgregarParametros(comando, videojuego);
        comando.Parameters.AddWithValue("$id", videojuego.Id);
        return comando.ExecuteNonQuery() > 0;
    }

    public bool Eliminar(int id)
    {
        using var conexion = new SqliteConnection(_cadenaConexion);
        conexion.Open();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "DELETE FROM Videojuegos WHERE Id=$id";
        comando.Parameters.AddWithValue("$id", id);
        return comando.ExecuteNonQuery() > 0;
    }

    private static void AgregarParametros(SqliteCommand comando, Videojuego videojuego)
    {
        comando.Parameters.AddWithValue("$nombre", videojuego.Nombre);
        comando.Parameters.AddWithValue("$genero", videojuego.Genero);
        comando.Parameters.AddWithValue("$plataforma", videojuego.Plataforma);
        comando.Parameters.AddWithValue("$precio", videojuego.Precio);
    }
}
