using MaterialDesignThemes.Wpf.Transitions;
using SPAClientApp.InsumosService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SPAClientApp
{
    /// <summary>
    /// Interaction logic for InsumoContainer.xaml
    /// </summary>
    public partial class InsumosContenedor : Window
    {
        public EInsumo Insumo { get; set; } = null;
        public WListaInsumos ListaInsumosPage { get; set; }
        public WInsumo InsumoPage { get; set; }    

        public InsumosContenedor()
        {
            InitializeComponent();
            ListaInsumosPage = wListaInsumos;
            InsumoPage = wInsumo;
        }

        private void Salir(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Move(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Maximizar(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        private void Minimizar(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
