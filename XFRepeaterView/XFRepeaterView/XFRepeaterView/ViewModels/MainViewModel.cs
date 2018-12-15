using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using XFRepeaterView.Models;

namespace XFRepeaterView.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private List<Person> _persons;

        public List<Person> Persons
        {
            get { return _persons; }
            set { _persons = value; OnPropertyChanged(); }
        }

        private ICommand _loadPersonsCommand;
        public ICommand LoadPersonsCommand => _loadPersonsCommand ?? (_loadPersonsCommand = new Command(LoadPersons));

        private ICommand _showPersonDetailsCommand;
        public ICommand ShowPersonDetailsCommand => _showPersonDetailsCommand ?? (_showPersonDetailsCommand = new Command<Person>(ShowPersonDetails));

        private void LoadPersons()
        {
            var persons = new List<Person>();
            var ageRandom = new Random();

            for (int i = 0; i < 100; i++)
            {
                var age = ageRandom.Next(1, 100);
                persons.Add(new Person { Name = $"Person {i + 1}", Age = age, Gender = age % 2 == 0 ? Gender.Female : Gender.Male });
            }

            Persons = persons;
        }

        private async void ShowPersonDetails(Person person)
        {
            await UserDialogs.Instance.AlertAsync($"You've clicked the person '{person.Name}' with the age of {person.Age}.");
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
