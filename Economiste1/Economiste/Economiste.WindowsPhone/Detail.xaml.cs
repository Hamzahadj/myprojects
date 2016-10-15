using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Pour en savoir plus sur le modèle d’élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkID=390556

namespace Economiste
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class Detail : Page
    {
        public Detail()
        {
            this.InitializeComponent();
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri(ArticleImg.BaseUri, SharedEntite.shared.url);
            ArticleImg.Source = bitmapImage;
            
            title.Text = SharedEntite.shared.title;
            description.Text = "Detail:" + SharedEntite.shared.description;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;
            if (frame == null)
            {
                return;
            }

            if (frame.CanGoBack)
            {
                frame.GoBack();
                //Indicate the back button press is handled so the app does not exit
                e.Handled = true;
            }
        }

        /// <summary>
        /// Invoqué lorsque cette page est sur le point d'être affichée dans un frame.
        /// </summary>
        /// <param name="e">Données d'événement décrivant la manière dont l'utilisateur a accédé à cette page.
        /// Ce paramètre est généralement utilisé pour configurer la page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

      /*  private async void web_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Debug.WriteLine("web view");
            Debug.WriteLine(SharedEntite.shared.link);
            WebView webBrowserTask = new WebView();
            Uri targetUri = new Uri(SharedEntite.shared.link);
            var success = await Windows.System.Launcher.LaunchUriAsync(targetUri);
        }*/

        private async void WebClick(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("web view");
            Debug.WriteLine(SharedEntite.shared.link);
            WebView webBrowserTask = new WebView();
            Uri targetUri = new Uri(SharedEntite.shared.link);
            var success = await Windows.System.Launcher.LaunchUriAsync(targetUri);
        }

      
    }
}
