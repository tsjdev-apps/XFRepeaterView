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

        private void LoadPersons()
        {
            var persons = new List<Person>();
            var ageRandom = new Random();

            for (int i = 0; i < 100; i++)
            {
                persons.Add(new Person { Name = $"Person {i + 1}", Age = ageRandom.Next(1, 100) });
            }

            Persons = persons;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
