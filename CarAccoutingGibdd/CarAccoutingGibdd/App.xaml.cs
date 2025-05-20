using System.Configuration;
using System.Data;
using System.Windows;

namespace CarAccoutingGibdd
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Окно текущего меню
        public static Window MenuWindow { get; set; } = null!;
    }

}
