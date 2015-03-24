using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Reflection;

namespace Gimnasio
{
    /// <summary>
    /// Interaction logic for VistaPermisoControlIngreso.xaml
    /// </summary>
    public partial class VistaPermisoControlIngreso : Window
    {
        int contIntentos = 0;
        Gimnasio.Database1Entities database1Entities = new Gimnasio.Database1Entities();
        System.Data.Objects.ObjectQuery<Gimnasio.usuario> usuariosVar2;
        public static bool IsOpen { get; private set; }
        Window winVistaControlIngreso = new Window();

        public VistaPermisoControlIngreso()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            contIntentos = contIntentos + 1;
            info1Label.Visibility = Visibility.Hidden;
            info2Label.Visibility = Visibility.Hidden;
            string esql = "select value u from usuarios as u where u.user= '" + txtUsr.Text + "\'";
            var usuariosVar = database1Entities.CreateQuery<usuario>(esql);

            // Se controla que el cliente que se haya traido sea un cliente valido. 
            if (usuariosVar.ToList().Count == 0)
            {
                info1Label.Visibility = Visibility.Visible;
            }
            else
            {
                esql = "select value u from usuarios as u where u.user= '" + txtUsr.Text + "\' and u.password='"+ txtPass.Password + "\'";
                var usuariosVar2 = database1Entities.CreateQuery<usuario>(esql);
                if (usuariosVar2.ToList().Count == 0)
                {
                    info2Label.Visibility = Visibility.Visible;
                }
                else
                {
                    if (VistaControlIngreso.IsOpen)// Se controla que una instancia de esta ventana no este abierta. 
                    {
                        this.winVistaControlIngreso.Activate();// Si esta abierta entonces activar, mandar al frente
                        return;
                    }
                    else // NO ESTA ABIERTA. Abrir una instancia de la ventana.
                    {
                        Type type = this.GetType();
                        Assembly assembly = type.Assembly;
                        this.winVistaControlIngreso = (Window)assembly.CreateInstance("Gimnasio.VistaControlIngreso");
                        this.winVistaControlIngreso.Show();
                    }
                    info1Label.Visibility = Visibility.Hidden;
                    info2Label.Visibility = Visibility.Hidden;
                    this.Close();
                }
            }
        }
    }
}
