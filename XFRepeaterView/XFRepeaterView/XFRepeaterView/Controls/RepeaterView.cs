using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace XFRepeaterView.Controls
{
    [ContentProperty(nameof(ItemTemplate))]
    public class RepeaterView : StackLayout
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(ICollection), typeof(RepeaterView), propertyChanged: ItemsSourcePropertyOnChanged);

        public ICollection ItemsSource
        {
            get => (ICollection)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(RepeaterView), default(DataTemplate));

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public static readonly BindableProperty ItemTappedCommandProperty =
            BindableProperty.Create(nameof(ItemTappedCommand), typeof(ICommand), typeof(RepeaterView));

        public ICommand ItemTappedCommand
        {
            get => GetValue(ItemTappedCommandProperty) as ICommand;
            set => SetValue(ItemTappedCommandProperty, value);
        }


        public RepeaterView()
        {
            Spacing = 0;
        }


        protected virtual View ViewFor(object item)
        {
            if (ItemTemplate == null)
                return null;

            var content = ItemTemplate.CreateContent();

            var view = content is ViewCell viewCell ? viewCell.View : content as View;

            view.BindingContext = item;

            if (ItemTappedCommand != null)
                AddTapGestureToChildView(view);

            return view;
        }

        private static void ItemsSourcePropertyOnChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is RepeaterView control))
                return;

            control.Children.Clear();

            var collection = (ICollection)newValue;
            if (collection == null)
                return;

            if (collection is INotifyCollectionChanged observableCollection)
                control.AddCollectionNotificationsListener(observableCollection);

            foreach (var item in collection)
                control.Children.Add(control.ViewFor(item));
        }

        private void AddCollectionNotificationsListener(INotifyCollectionChanged observableCollection)
        {
            if (observableCollection == null)
                throw new ArgumentNullException(nameof(observableCollection));

            observableCollection.CollectionChanged -= ObsersavableCollectionOnCollectionChanged;
            observableCollection.CollectionChanged += ObsersavableCollectionOnCollectionChanged;
        }

        private void ObsersavableCollectionOnCollectionChanged(object s, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var newItem in e.NewItems)
                        Children.Add(ViewFor(newItem));
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var oldItem in e.OldItems)
                    {
                        var viewToRemove = Children.FirstOrDefault(x => x.BindingContext == oldItem);
                        if (viewToRemove != null)
                            Children.Remove(viewToRemove);
                    }
                    break;
                default:
                    Children.Clear();
                    foreach (var item in (ICollection)s)
                        Children.Add(ViewFor(item));
                    break;
            }
        }
               
        private void AddTapGestureToChildView(View view)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));

            view.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    if (ItemTappedCommand.CanExecute(view.BindingContext))
                        ItemTappedCommand.Execute(view.BindingContext);
                })
            });
        }
    }
}