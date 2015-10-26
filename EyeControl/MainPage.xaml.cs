using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace EyeControl
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public IScreen userScreen { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            userScreen = new UserScreen();
            userScreen.test();
        }

        private void Play(object sender, RoutedEventArgs e)
        {
            //if (lineViewTxt.Text == "")
            //{
            //    return;
            //}
            //logViewTxt.Text += "\n"+lineViewTxt.Text;
            //this._ClearLineView();
        }
        
        private void northEvent(object sender, RoutedEventArgs e)
        {
            userScreen.HandleClusterEvent(userScreen.clusterSection.north);
        }

        private void eastEvent(object sender, RoutedEventArgs e)
        {
            userScreen.HandleClusterEvent(userScreen.clusterSection.east);
        }

        private void westEvent(object sender, RoutedEventArgs e)
        {
            userScreen.HandleClusterEvent(userScreen.clusterSection.west);
        }

        private void southEvent(object sender, RoutedEventArgs e)
        {
            userScreen.HandleClusterEvent(userScreen.clusterSection.south);
        }

        private void centerEvent(object sender, RoutedEventArgs e)
        {
            userScreen.HandleClusterEvent(userScreen.clusterSection.center);
        }

        private void SpeakEvent(object sender, RoutedEventArgs e)
        {
            userScreen.HandleSpeakEvent();
        }
    }
}
