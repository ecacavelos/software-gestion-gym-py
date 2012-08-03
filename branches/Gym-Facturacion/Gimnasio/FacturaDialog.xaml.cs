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

namespace Gimnasio
{
    /// <summary>
    /// Interaction logic for FacturaDialog.xaml
    /// </summary>
    public partial class FacturaDialog : Window
    {
        public FacturaDialog()
        {
            InitializeComponent();
            textBox1.Text = "0000001";
        }

        public string ResponseText
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }


        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            int temp = 0;
            try
            {
                temp = Convert.ToInt32(textBox1.Text);
            }
            catch (Exception h)
            {
                MessageBox.Show("Por favor introduzca sólo números.");
            }

            if (temp > 0)
            {
                this.DialogResult = true;
            }
            else
            {

            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
