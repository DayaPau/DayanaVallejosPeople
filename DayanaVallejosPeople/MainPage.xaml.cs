﻿using DayanaVallejosPeople.Models;
using DayanaVallejosPeople.ViewModels; 

namespace DayanaVallejosPeople;

public partial class MainPage : ContentPage
{
    private MainPageViewModel _viewModel;

    public MainPage()
    {
        InitializeComponent();

        // Asignamos el ViewModel como el BindingContext de la página
        _viewModel = new MainPageViewModel();
        BindingContext = _viewModel;


        
        dvallejos_people_list.ItemSelected -= OnItemSelected; 
        dvallejos_people_list.ItemSelected += OnItemSelected;

    }

    // Método para manejar clic en el botón "Agregar"
    public void OnNewButtonClicked(object sender, EventArgs args)
    {
        // Asegúrate de que 'dvallejos_new_person_entry' tenga el nombre correcto en el XAML
        if (!string.IsNullOrEmpty(dvallejos_new_person_entry.Text))
        {
            _viewModel.AddPersonCommand.Execute(dvallejos_new_person_entry.Text);
        }
    }



    public void OnGetButtonClicked(object sender, EventArgs args)
    {
        _viewModel.RefreshPeopleCommand.Execute(null);
    }


    // Método para manejar la eliminación de un elemento seleccionado
    private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Person person)
        {
            // Limpiamos la selección inmediatamente para evitar múltiples disparos
            dvallejos_people_list.SelectedItem = null;

            // Mostramos la alerta de confirmación
            bool confirm = await DisplayAlert("Eliminar registro",
                $"¿Estás seguro de que deseas eliminar a {person.Name}?",
                "Sí", "No");

            if (confirm)
            {
                
                _viewModel.DeletePersonCommand.Execute(person);

                
                await DisplayAlert("Registro eliminado",
                    $"Dayana Vallejos acaba de eliminar un registro.",
                    "OK");
            }
        }
    }

}
