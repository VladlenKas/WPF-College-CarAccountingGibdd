using CarAccountingGibdd.Model;
using System.Configuration;
using System.Data;
using System.Windows;

namespace CarAccountingGibdd
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        // Окно текущего меню
        public static Window MenuWindow { get; set; } = null!;
        
        // Быстрое получения актуального контекста бд
        private static readonly GibddContext _instance = new GibddContext();
        public static GibddContext DbContext => _instance;
    }
}
