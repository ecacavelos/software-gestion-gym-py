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
    /// Interaction logic for VistaAdmUsuarios.xaml
    /// </summary>
    public partial class VistaAdmUsuarios : Window
    {
        Gimnasio.Database1Entities database1Entities = new Gimnasio.Database1Entities();
        System.Data.Objects.ObjectQuery<Gimnasio.usuario> usuariosVar2;
        public static bool IsOpen { get; private set; }
        int isChecked = 0;
        int contIntentos = 0;

        public VistaAdmUsuarios()
        {
            InitializeComponent();
            checkBox1.Visibility = Visibility.Hidden;
            info1Label.Visibility = Visibility.Hidden;
            btnAdd.Visibility = Visibility.Hidden;
            info2Label.Visibility = Visibility.Hidden;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            
            String usertxt = txtUsr.Text;
            String pass = txtPasswd.Password;
            usuario usuario = new usuario();
            usuario.user = usertxt;
            usuario.password = pass;
            usuario.root = isChecked;
            string esql4 = "select value u from usuarios as u";
            var cantUsuariosVar = database1Entities.CreateQuery<usuario>(esql4);

            // Se controla que el cliente que se haya traido sea un cliente valido. 
            if (cantUsuariosVar.ToList().Count == 0)
            {
                usuario.id = 1;
            }
            //guarda los cambios 
            database1Entities.AddTousuarios(usuario);
            int result = database1Entities.SaveChanges();
            if (result == 0)
            {
                MessageBox.Show("Hubo un problema de base de datos, por favor consule con los responsables de la aplicación");
            }
            else
            {
                MessageBox.Show("El usuario se agregó correctamente");
                //button2.Visibility = Visibility.Hidden;
                //button1.Visibility = Visibility.Visible;
                this.Close();
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
             
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            isChecked = 1;
        }

        private void checkBox1_unChecked(object sender, RoutedEventArgs e)
        {
            isChecked = 0;
        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
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
                string esql2 = "select value u from usuarios as u where u.user= '" + txtUsr.Text + "\' and u.root = 1";
                var usuariosVar3 = database1Entities.CreateQuery<usuario>(esql2);

                // Se controla que el cliente que se haya traido sea un cliente valido. 
                if (usuariosVar3.ToList().Count == 0)
                {
                    info2Label.Visibility = Visibility.Visible;
                }
                else
                {

                    esql = "select value u from usuarios as u where u.user= '" + txtUsr.Text + "\' and u.password='" + txtPasswd.Password + "\'";
                    var usuariosVar2 = database1Entities.CreateQuery<usuario>(esql);
                    if (usuariosVar2.ToList().Count == 0)
                    {
                        info1Label.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        //Mostrar los campos ocultos de agregar usuario
                        btnAdd.Visibility = Visibility.Visible;
                        checkBox1.Visibility = Visibility.Visible;
                        txtUsr.Text = "";
                        txtPasswd.Password = "";
                        //Ocultar campos de login
                        btnLogin.Visibility = Visibility.Hidden;
                        info1Label.Visibility = Visibility.Hidden;
                        info1Label.Visibility = Visibility.Hidden;
                    }
                }
            }
        }
    }
}
