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

        /// <summary>
        /// this is invoked when the media element state is changing
        /// </summary>
        /// <remarks>
        /// this simply is looking for the media element to go to a paused state, then it can enable the button
        /// </remarks>
        /// <param name="sender">unused object parameter</param>
        /// <param name="e">unused object parameter</param>
        void media_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            if (this.media.CurrentState == Windows.UI.Xaml.Media.MediaElementState.Paused)
            {
                this.speakButton.IsEnabled = true;
                this.media.CurrentStateChanged -= this.media_CurrentStateChanged;
            }
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
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
            userScreen.HandleSpeakEvent(this.media);
        }
    }
}
