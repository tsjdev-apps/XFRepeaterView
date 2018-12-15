using Xamarin.Forms;
using XFRepeaterView.Models;

namespace XFRepeaterView.Utils
{
    public class PersonDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FemaleDataTemplate { get; set; }
        public DataTemplate MaleDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is Person person)
            {
                switch (person.Gender)
                {
                    case Gender.Male:
                        return MaleDataTemplate;
                    case Gender.Female:
                        return FemaleDataTemplate;
                }
            }

            return null;
        }
    }
}
