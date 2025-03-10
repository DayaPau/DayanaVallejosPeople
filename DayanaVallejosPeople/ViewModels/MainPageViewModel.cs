﻿using DayanaVallejosPeople.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace DayanaVallejosPeople.ViewModels;

public class MainPageViewModel : BaseViewModel
{
    // Propiedad observable para los datos de personas
    public ObservableCollection<Person> People { get; set; }

    // Comando para agregar persona
    public ICommand AddPersonCommand { get; }

    // Comando para eliminar persona
    public ICommand DeletePersonCommand { get; }

    // Comando para refrescar personas
    public ICommand RefreshPeopleCommand { get; }

    // Propiedad para el mensaje de estado
    private string _statusMessage;
    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    // Constructor
    public MainPageViewModel()
    {
        People = new ObservableCollection<Person>();

        // Inicializamos los comandos
        AddPersonCommand = new Command<string>(AddPerson);
        DeletePersonCommand = new Command<Person>(DeletePerson);
        RefreshPeopleCommand = new Command(RefreshPeople);

        // Cargar la lista inicial de personas
        RefreshPeople();
    }

    // Método para agregar una persona
    private void AddPerson(string name)
    {
        try
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("Nombre válido requeridos");

            // Agregar la persona a la base de datos
            App.PersonRepo.AddNewPerson(name);

            // Mostrar solo la última persona añadida en la lista
            People.Clear();
            People.Add(new Person { Name = name });

            // Mensaje de éxito
            StatusMessage = $"{name} fue añadido exitosamente.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error al añadir una persona: {ex.Message}";
        }
    }


    // Método para eliminar una persona
    private void DeletePerson(Person person)
    {
        try
        {
            // Eliminar la persona
            App.PersonRepo.DeletePerson(person.Id);

            // Personalizar el mensaje con el nombre 'Dayana Vallejos'
            StatusMessage = $"Dayana Vallejos acaba de eliminar un registro: {person.Name}.";

            // Refrescar la lista después de eliminar
            RefreshPeople();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error al eliminar persona: {ex.Message}";
        }
    }

    // Método para refrescar la lista de personas
    private void RefreshPeople()
    {
        try
        {
            var peopleList = App.PersonRepo.GetAllPeople();
            People.Clear();

            // Agregar todas las personas a la lista observable
            foreach (var person in peopleList)
            {
                People.Add(person);
            }

            StatusMessage = $"Recuperado {peopleList.Count} persona/s.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Failed to retrieve people: {ex.Message}";
        }
    }

}

