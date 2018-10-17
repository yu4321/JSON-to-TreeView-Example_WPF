/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:JSONVisualizer.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>

  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace JSONVisualizer.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ContentAddViewModel>();
            SimpleIoc.Default.Register<ContentDeleteViewModel>();
            SimpleIoc.Default.Register<ContentModifyViewModel>();
            SimpleIoc.Default.Register<ContentViewViewModel>();
            SimpleIoc.Default.Register<ContentListViewModel>();
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
            System.Console.WriteLine("Cleanup invoked");
            SimpleIoc.Default.Unregister<MainViewModel>();
            SimpleIoc.Default.Unregister<ContentAddViewModel>();
            SimpleIoc.Default.Unregister<ContentDeleteViewModel>();
            SimpleIoc.Default.Unregister<ContentModifyViewModel>();
            SimpleIoc.Default.Unregister<ContentViewViewModel>();
            SimpleIoc.Default.Unregister<ContentListViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ContentAddViewModel>();
            SimpleIoc.Default.Register<ContentDeleteViewModel>();
            SimpleIoc.Default.Register<ContentModifyViewModel>();
            SimpleIoc.Default.Register<ContentViewViewModel>();
            SimpleIoc.Default.Register<ContentListViewModel>();
        }
    }
}