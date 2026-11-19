using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using GestionVideojuegos.Data;
using GestionVideojuegos.Models;
using GestionVideojuegos.Services;

namespace GestionVideojuegos.Views;

public partial class MainWindow : Window
{
    private readonly VideojuegoServicio _servicio;
    private readonly TextBox _nombre;
    private readonly TextBox _genero;
    private readonly TextBox _plataforma;
    private readonly TextBox _precio;
    private readonly ListBox _lista;
    private readonly TextBlock _mensaje;

    public MainWindow()
    {
        InitializeComponent();
        _nombre = this.FindControl<TextBox>("TxtNombre")!;
        _genero = this.FindControl<TextBox>("TxtGenero")!;
        _plataforma = this.FindControl<TextBox>("TxtPlataforma")!;
        _precio = this.FindControl<TextBox>("TxtPrecio")!;
        _lista = this.FindControl<ListBox>("ListaVideojuegos")!;
        _mensaje = this.FindControl<TextBlock>("TxtMensaje")!;
        _servicio = new VideojuegoServicio(new VideojuegoDao());
        Recargar();
    }

    private void Recargar()
    {
        _lista.ItemsSource = _servicio.Consultar();
    }

    private void Notificar(string texto, bool error = false)
    {
        _mensaje.Text = texto;
        _mensaje.Foreground = error ? Brushes.Salmon : Brushes.LightGreen;
    }

    private void Limpiar()
    {
        _lista.SelectedItem = null;
        _nombre.Text = string.Empty;
        _genero.Text = string.Empty;
        _plataforma.Text = string.Empty;
        _precio.Text = string.Empty;
    }

    private void Registrar_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            _servicio.Registrar(_nombre.Text, _genero.Text, _plataforma.Text, _precio.Text);
            Limpiar();
            Recargar();
            Notificar("Videojuego registrado correctamente.");
        }
        catch (Exception ex) { Notificar(ex.Message, true); }
    }

    private void Actualizar_Click(object? sender, RoutedEventArgs e)
    {
        if (_lista.SelectedItem is not Videojuego seleccionado)
        {
            Notificar("Selecciona primero un videojuego de la lista.", true);
            return;
        }
        try
        {
            bool actualizado = _servicio.Actualizar(seleccionado.Id, _nombre.Text, _genero.Text, _plataforma.Text, _precio.Text);
            Limpiar();
            Recargar();
            Notificar(actualizado ? "Videojuego actualizado correctamente." : "No se encontró el registro.", !actualizado);
        }
        catch (Exception ex) { Notificar(ex.Message, true); }
    }

    private void Eliminar_Click(object? sender, RoutedEventArgs e)
    {
        if (_lista.SelectedItem is not Videojuego seleccionado)
        {
            Notificar("Selecciona primero un videojuego de la lista.", true);
            return;
        }
        try
        {
            bool eliminado = _servicio.Eliminar(seleccionado.Id);
            Limpiar();
            Recargar();
            Notificar(eliminado ? "Videojuego eliminado correctamente." : "No se encontró el registro.", !eliminado);
        }
        catch (Exception ex) { Notificar(ex.Message, true); }
    }

    private void Limpiar_Click(object? sender, RoutedEventArgs e)
    {
        Limpiar();
        Notificar("Formulario limpio.");
    }

    private void Recargar_Click(object? sender, RoutedEventArgs e)
    {
        try { Recargar(); Notificar("Lista actualizada."); }
        catch (Exception ex) { Notificar(ex.Message, true); }
    }

    private void ListaVideojuegos_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_lista?.SelectedItem is Videojuego seleccionado)
        {
            _nombre.Text = seleccionado.Nombre;
            _genero.Text = seleccionado.Genero;
            _plataforma.Text = seleccionado.Plataforma;
            _precio.Text = seleccionado.Precio.ToString();
        }
    }
}
