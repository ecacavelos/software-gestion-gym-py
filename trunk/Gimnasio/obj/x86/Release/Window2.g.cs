﻿#pragma checksum "..\..\..\Window2.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A560F0318A31CF10C7AAE9A97182A52D"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.1
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

using Gimnasio;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Gimnasio {
    
    
    /// <summary>
    /// Window2
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
    public partial class Window2 : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\Window2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid clientesDataGrid;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\..\Window2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTextColumn nombreColumn;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\Window2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTextColumn apellidoColumn;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\Window2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTextColumn direccionColumn;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\Window2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTextColumn telefonoColumn;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\Window2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTextColumn emailColumn;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\Window2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTemplateColumn fecha_nacimientoColumn;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\Window2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTemplateColumn fecha_ingresoColumn;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\Window2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTextColumn alturaColumn;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\Window2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTextColumn pesoColumn;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\Window2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button button1;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\Window2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button button2;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Gimnasio;component/window2.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Window2.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 4 "..\..\..\Window2.xaml"
            ((Gimnasio.Window2)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.clientesDataGrid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 9 "..\..\..\Window2.xaml"
            this.clientesDataGrid.RowEditEnding += new System.EventHandler<System.Windows.Controls.DataGridRowEditEndingEventArgs>(this.clientesDataGrid_RowEditEnding);
            
            #line default
            #line hidden
            return;
            case 3:
            this.nombreColumn = ((System.Windows.Controls.DataGridTextColumn)(target));
            return;
            case 4:
            this.apellidoColumn = ((System.Windows.Controls.DataGridTextColumn)(target));
            return;
            case 5:
            this.direccionColumn = ((System.Windows.Controls.DataGridTextColumn)(target));
            return;
            case 6:
            this.telefonoColumn = ((System.Windows.Controls.DataGridTextColumn)(target));
            return;
            case 7:
            this.emailColumn = ((System.Windows.Controls.DataGridTextColumn)(target));
            return;
            case 8:
            this.fecha_nacimientoColumn = ((System.Windows.Controls.DataGridTemplateColumn)(target));
            return;
            case 9:
            this.fecha_ingresoColumn = ((System.Windows.Controls.DataGridTemplateColumn)(target));
            return;
            case 10:
            this.alturaColumn = ((System.Windows.Controls.DataGridTextColumn)(target));
            return;
            case 11:
            this.pesoColumn = ((System.Windows.Controls.DataGridTextColumn)(target));
            return;
            case 12:
            this.button1 = ((System.Windows.Controls.Button)(target));
            return;
            case 13:
            this.button2 = ((System.Windows.Controls.Button)(target));
            
            #line 35 "..\..\..\Window2.xaml"
            this.button2.Click += new System.Windows.RoutedEventHandler(this.GuardarCambiosClientes);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
