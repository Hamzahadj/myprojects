using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// Pour en savoir plus sur le modèle d’élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkID=390556

namespace Economiste
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class Main : Page
    {


        TextBlock feedStatus;
        StackPanel statusPanel;
        ListView lstRSS;
        Popup StandardPopup;

        //The Windows.Web.Http.HttpClient class provides the main class for  
        // sending HTTP requests and receiving HTTP responses from a resource identified by a URI. 
        private HttpClient httpClient;
        private HttpResponseMessage response;

        // This is the feed address that will be parsed and displayed 
        private String feedAddress = "http://www.leparisien.fr/economie/rss.xml";

        public Main()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            httpClient = new HttpClient();

            // Add a user-agent header 
            var headers = httpClient.DefaultRequestHeaders;

            // HttpProductInfoHeaderValueCollection is a collection of  
            // HttpProductInfoHeaderValue items used for the user-agent header 

            headers.UserAgent.ParseAdd("ie");
            headers.UserAgent.ParseAdd("Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
       /* protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Check if ExtendedSplashscreen.xaml is on the backstack  and remove 
            if (Frame.BackStack.Count() == 1)
            {
                Frame.BackStack.RemoveAt(Frame.BackStackDepth - 1);
            }
            base.OnNavigatedTo(e);
        }*/



      

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsInternetAvailable)
            {
                feedStatus = FindChildControl<TextBlock>(NewsHubSection, "feedStatus") as TextBlock;
                statusPanel = FindChildControl<StackPanel>(NewsHubSection, "statusPanel") as StackPanel;

                response = new HttpResponseMessage();

                // if 'feedAddress' value changed the new URI must be tested -------------------------------- 
                // if the new 'feedAddress' doesn't work, 'feedStatus' informs the user about the incorrect input. 

                feedStatus.Text = "Test if URI is valid";

                Uri resourceUri;
                if (!Uri.TryCreate(feedAddress.Trim(), UriKind.Absolute, out resourceUri))
                {
                    feedStatus.Text = "Invalid URI, please re-enter a valid URI";
                    return;
                }
                if (resourceUri.Scheme != "http" && resourceUri.Scheme != "https")
                {
                    feedStatus.Text = "Only 'http' and 'https' schemes supported. Please re-enter URI";
                    return;
                }
                // ---------- end of test--------------------------------------------------------------------- 

                string responseText;
                feedStatus.Text = "Waiting for response ...";

                try
                {
                    response = await httpClient.GetAsync(resourceUri);

                    response.EnsureSuccessStatusCode();

                    responseText = await response.Content.ReadAsStringAsync();
                    statusPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                }
                catch (Exception ex)
                {
                    // Need to convert int HResult to hex string 
                    feedStatus.Text = "Error = " + ex.HResult.ToString("X") +
                        "  Message: " + ex.Message;
                    responseText = "";
                }
                feedStatus.Text = response.StatusCode + " " + response.ReasonPhrase;

                // now 'responseText' contains the feed as a verified text. 
                // next 'responseText' is converted as the rssItems class model definition to be displayed as a list 

                List<rssItems> lstData = new List<rssItems>();
                XElement _xml = XElement.Parse(responseText);
                XNamespace content = "URI";
                foreach (XElement value in _xml.Elements("channel").Elements("item"))
                {
                    rssItems _item = new rssItems();

                    _item.title = value.Element("title").Value;

                    _item.description = value.Element("description").Value;

                    _item.link = value.Element("link").Value;

                   // _item.enclosure = value.Element("enclosure").Value;
                    string url = (string)value.Element("enclosure").Attribute("url");
                    _item.url = url;
                    lstData.Add(_item);


                }

                // lstRSS is bound to the lstData: the final result of the RSS parsing 
                lstRSS = FindChildControl<ListView>(NewsHubSection, "lstRSS") as ListView;
                lstRSS.DataContext = lstData;
            }
            else
            { //MessageBox("Internet connectivity is required with this fonctionnality");

                StandardPopup = FindChildControl<Popup>(NewsHubSection, "StandardPopup") as Popup;
                if (!StandardPopup.IsOpen) { StandardPopup.IsOpen = true; }
            }
        }

        private async void lstRSS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // if item clicked navigate to the webpage 

            var selected = lstRSS.SelectedItem as rssItems;

            WebView webBrowserTask = new WebView();
            Uri targetUri = new Uri(selected.link);

            //webbrowser task launcher for Windows 8.1 
            //http://msdn.microsoft.com/en-us/library/windows/apps/xaml/hh701480.aspx 
            var success = await Windows.System.Launcher.LaunchUriAsync(targetUri);



        }

        private DependencyObject FindChildControl<T>(DependencyObject control, string ctrlName)
        {
            int childNumber = VisualTreeHelper.GetChildrenCount(control);
            for (int i = 0; i < childNumber; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(control, i);
                FrameworkElement fe = child as FrameworkElement;
                // Not a framework element or is null
                if (fe == null) return null;

                if (child is T && fe.Name == ctrlName)
                {
                    // Found the control so return
                    return child;
                }
                else
                {
                    // Not found it - search children
                    DependencyObject nextLevel = FindChildControl<T>(child, ctrlName);
                    if (nextLevel != null)
                        return nextLevel;
                }
            }
            return null;
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

        private async void NewsHubSection_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsInternetAvailable)
            {
                feedStatus = FindChildControl<TextBlock>(NewsHubSection, "feedStatus") as TextBlock;
                statusPanel = FindChildControl<StackPanel>(NewsHubSection, "statusPanel") as StackPanel;

                response = new HttpResponseMessage();

                // if 'feedAddress' value changed the new URI must be tested -------------------------------- 
                // if the new 'feedAddress' doesn't work, 'feedStatus' informs the user about the incorrect input. 

                feedStatus.Text = "Test if URI is valid";

                Uri resourceUri;
                if (!Uri.TryCreate(feedAddress.Trim(), UriKind.Absolute, out resourceUri))
                {
                    feedStatus.Text = "Invalid URI, please re-enter a valid URI";
                    return;
                }
                if (resourceUri.Scheme != "http" && resourceUri.Scheme != "https")
                {
                    feedStatus.Text = "Only 'http' and 'https' schemes supported. Please re-enter URI";
                    return;
                }
                // ---------- end of test--------------------------------------------------------------------- 

                string responseText;
                feedStatus.Text = "Waiting for response ...";

                try
                {
                    response = await httpClient.GetAsync(resourceUri);

                    response.EnsureSuccessStatusCode();

                    responseText = await response.Content.ReadAsStringAsync();
                    statusPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                }
                catch (Exception ex)
                {
                    // Need to convert int HResult to hex string 
                    feedStatus.Text = "Error = " + ex.HResult.ToString("X") +
                        "  Message: " + ex.Message;
                    responseText = "";
                }
                feedStatus.Text = response.StatusCode + " " + response.ReasonPhrase;

                // now 'responseText' contains the feed as a verified text. 
                // next 'responseText' is converted as the rssItems class model definition to be displayed as a list 

                List<rssItems> lstData = new List<rssItems>();
                XElement _xml = XElement.Parse(responseText);
                XNamespace content = "URI";
                foreach (XElement value in _xml.Elements("channel").Elements("item"))
                {
                    rssItems _item = new rssItems();

                    _item.title = value.Element("title").Value;

                    _item.description = value.Element("description").Value;

                    _item.link = value.Element("link").Value;

                   // _item.enclosure = value.Element("enclosure").Value;

                    string url = (string)value.Element("enclosure").Attribute("url");
                    _item.url = url;
                    lstData.Add(_item);



                }

                // lstRSS is bound to the lstData: the final result of the RSS parsing 
                lstRSS = FindChildControl<ListView>(NewsHubSection, "lstRSS") as ListView;
                lstRSS.DataContext = lstData;
            }

        }

       /* public List<Uri> FetchLinksFromSource(string htmlSource)
        {
            List<Uri> links = new List<Uri>();
            string regexImgSrc = @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";
            MatchCollection matchesImgSrc = Regex.Matches(htmlSource, regexImgSrc, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            foreach (Match m in matchesImgSrc)
            {
                string href = m.Groups[1].Value;
                links.Add(new Uri(href));
            }
            return links;
        }*/

        public static bool IsInternetAvailable
        {

            get
            {
                var profiles = NetworkInformation.GetConnectionProfiles();
                var internetProfile = NetworkInformation.GetInternetConnectionProfile();
                return profiles.Any(s => s.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
                    || (internetProfile != null
                            && internetProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess);
            }
        }

        private async void MessageBox(string message)
        {
            var dialog = new MessageDialog(message.ToString());
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await dialog.ShowAsync());
        }

        private void ClosePopupClicked(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it 
            if (StandardPopup.IsOpen)
            {
                StandardPopup.IsOpen = false;
                Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-wifi:"));
            }
        }

        private void CancelPopup(object sender, RoutedEventArgs e)
        {
            if (StandardPopup.IsOpen) { StandardPopup.IsOpen = false; }
        }

    }
}
