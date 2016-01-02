using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AucklandBuses.UserControls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapIconUserControl : UserControl
    {
        public static readonly DependencyProperty DirectoryMapIconProperty =
                    DependencyProperty.Register("DisplayText", typeof(string), typeof(MapIconUserControl), null);

        public string DisplayText
        {
            get { return this.GetValue(DirectoryMapIconProperty) as string; }
            set { this.SetValueDp(DirectoryMapIconProperty, value); }
        }

        public MapIconUserControl(string displayText)
        {
            this.InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;

            DisplayText = displayText;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetValueDp(DependencyProperty property, object value, [CallerMemberName]string propertyName = null)
        {
            SetValue(property, value);
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
