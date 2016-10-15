using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace Economiste
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        List<Entite> ListEntite;
       // TextBlock feedStatus;
       // StackPanel statusPanel;
        Popup StandardPopup;
       // GridView lstRSS;
        private HttpClient httpClient;
        private HttpResponseMessage response;
        private String feedAddress = "http://www.leparisien.fr/economie/rss.xml";
        private String lePointFeed = "http://www.lepoint.fr/economie/rss.xml";
        public MainPage()
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
        /// Invoqué lorsque cette page est sur le point d'être affichée dans un frame.
        /// </summary>
        /// <param name="e">Données d’événement décrivant la manière dont l’utilisateur a accédé à cette page.
        /// Ce paramètre est généralement utilisé pour configurer la page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: préparer la page pour affichage ici.

            // TODO: si votre application comporte plusieurs pages, assurez-vous que vous
            // gérez le bouton Retour physique en vous inscrivant à l’événement
            // Événement Windows.Phone.UI.Input.HardwareButtons.BackPressed.
            // Si vous utilisez le NavigationHelper fourni par certains modèles,
            // cet événement est géré automatiquement.
        }

        private async void GridView_Loaded(object sender, RoutedEventArgs e)
        {
            /*ListEntite = new List<Entite>();
            ListEntite.Add(new Entite() { title = "Premier article", image = "/Assets/ecv.jpg", description = "Attentat fomenté par les services secrets, coup monté des medias, manigance des ennemis de l'islam... Le 7 janvier 2015, les théories conspirationnistes censées élucider l'attentat meurtrier contre Charlie Hebdo n'ont mis que quelques heures à se répandre sur internet. Comment reconnaître et lutter contre ces explications fantasques ? ➜ A lire également : Où sont passés les paranos ? ➜ Dossier web : Qu'est-ce que la rumeur ?" });
            ListEntite.Add(new Entite() { title = "Deuxième article", image = "/Assets/1735236-fleur-pellerin-se-dote-d-une-commission-de-terminologie-de-l-economie-numerique.jpg", description = "Attentat fomenté par les services secrets, coup monté des medias, manigance des ennemis de l'islam... Le 7 janvier 2015, les théories conspirationnistes censées élucider l'attentat meurtrier contre Charlie Hebdo n'ont mis que quelques heures à se répandre sur internet. Comment reconnaître et lutter contre ces explications fantasques ? ➜ A lire également : Où sont passés les paranos ? ➜ Dossier web : Qu'est-ce que la rumeur ?" });
            ListEntite.Add(new Entite() { title = "Troisième Article", image = "/Assets/economie3.jpg", description = "Attentat fomenté par les services secrets, coup monté des medias, manigance des ennemis de l'islam... Le 7 janvier 2015, les théories conspirationnistes censées élucider l'attentat meurtrier contre Charlie Hebdo n'ont mis que quelques heures à se répandre sur internet. Comment reconnaître et lutter contre ces explications fantasques ? ➜ A lire également : Où sont passés les paranos ? ➜ Dossier web : Qu'est-ce que la rumeur ?" });
            ListEntite.Add(new Entite() { title = "Premier article", image = "/Assets/statistique.jpg", description = "Attentat fomenté par les services secrets, coup monté des medias, manigance des ennemis de l'islam... Le 7 janvier 2015, les théories conspirationnistes censées élucider l'attentat meurtrier contre Charlie Hebdo n'ont mis que quelques heures à se répandre sur internet. Comment reconnaître et lutter contre ces explications fantasques ? ➜ A lire également : Où sont passés les paranos ? ➜ Dossier web : Qu'est-ce que la rumeur ?" });
           
            var list = sender as GridView;

            list.DataContext = ListEntite;*/
            if (IsInternetAvailable)
            {
                //feedStatus = FindChildControl<TextBlock>(NewsHubSection, "feedStatus") as TextBlock;
               // statusPanel = FindChildControl<StackPanel>(NewsHubSection, "statusPanel") as StackPanel;

                response = new HttpResponseMessage();

                // if 'feedAddress' value changed the new URI must be tested -------------------------------- 
                // if the new 'feedAddress' doesn't work, 'feedStatus' informs the user about the incorrect input. 

               // feedStatus.Text = "Test if URI is valid";

                Uri resourceUri;
                if (!Uri.TryCreate(feedAddress.Trim(), UriKind.Absolute, out resourceUri))
                {
                  //  feedStatus.Text = "Invalid URI, please re-enter a valid URI";
                    return;
                }
                if (resourceUri.Scheme != "http" && resourceUri.Scheme != "https")
                {
                   // feedStatus.Text = "Only 'http' and 'https' schemes supported. Please re-enter URI";
                    return;
                }
                // ---------- end of test--------------------------------------------------------------------- 

                string responseText;
              //  feedStatus.Text = "Waiting for response ...";

                try
                {
                    response = await httpClient.GetAsync(resourceUri);

                    response.EnsureSuccessStatusCode();

                    responseText = await response.Content.ReadAsStringAsync();
                  //  statusPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                }
                catch (Exception ex)
                {
                    // Need to convert int HResult to hex string 
                   // feedStatus.Text = "Error = " + ex.HResult.ToString("X") +
//"  Message: " + ex.Message;
                    responseText = "";
                }
               // feedStatus.Text = response.StatusCode + " " + response.ReasonPhrase;

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
              //  lstRSS = FindChildControl<ListView>(NewsHubSection, "lstRSS") as ListView;
                var list = sender as GridView;
                list.DataContext = lstData;
            }
            
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            
        }

        private void GridView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var list = sender as GridView;
            SharedEntite.shared = list.SelectedItem as rssItems;
            Frame.Navigate(typeof(Detail));
        }

        private async void GridView1_Loaded(object sender, RoutedEventArgs e)
        {
           /* ListEntite = new List<Entite>();
            ListEntite.Add(new Entite() { title = "Premier article", image = "/Assets/234555_economie-krach-boom-mue.jpg", description = "Attentat fomenté par les services secrets, coup monté des medias, manigance des ennemis de l'islam... Le 7 janvier 2015, les théories conspirationnistes censées élucider l'attentat meurtrier contre Charlie Hebdo n'ont mis que quelques heures à se répandre sur internet. Comment reconnaître et lutter contre ces explications fantasques ? ➜ A lire également : Où sont passés les paranos ? ➜ Dossier web : Qu'est-ce que la rumeur ?" });
            ListEntite.Add(new Entite() { title = "Deuxième article", image = "/Assets/89716_monde_cafe.jpg", description = "Attentat fomenté par les services secrets, coup monté des medias, manigance des ennemis de l'islam... Le 7 janvier 2015, les théories conspirationnistes censées élucider l'attentat meurtrier contre Charlie Hebdo n'ont mis que quelques heures à se répandre sur internet. Comment reconnaître et lutter contre ces explications fantasques ? ➜ A lire également : Où sont passés les paranos ? ➜ Dossier web : Qu'est-ce que la rumeur ?" });
            ListEntite.Add(new Entite() { title = "Troisième Article", image = "/Assets/croissance_invest22.jpg", description = "Attentat fomenté par les services secrets, coup monté des medias, manigance des ennemis de l'islam... Le 7 janvier 2015, les théories conspirationnistes censées élucider l'attentat meurtrier contre Charlie Hebdo n'ont mis que quelques heures à se répandre sur internet. Comment reconnaître et lutter contre ces explications fantasques ? ➜ A lire également : Où sont passés les paranos ? ➜ Dossier web : Qu'est-ce que la rumeur ?" });
            ListEntite.Add(new Entite() { title = "Premier article", image = "/Assets/economie-verde.png", description = "Attentat fomenté par les services secrets, coup monté des medias, manigance des ennemis de l'islam... Le 7 janvier 2015, les théories conspirationnistes censées élucider l'attentat meurtrier contre Charlie Hebdo n'ont mis que quelques heures à se répandre sur internet. Comment reconnaître et lutter contre ces explications fantasques ? ➜ A lire également : Où sont passés les paranos ? ➜ Dossier web : Qu'est-ce que la rumeur ?" });

            var list = sender as GridView;

            list.DataContext = ListEntite;*/
            if (IsInternetAvailable)
            {
                //feedStatus = FindChildControl<TextBlock>(NewsHubSection, "feedStatus") as TextBlock;
                // statusPanel = FindChildControl<StackPanel>(NewsHubSection, "statusPanel") as StackPanel;

                response = new HttpResponseMessage();

                // if 'feedAddress' value changed the new URI must be tested -------------------------------- 
                // if the new 'feedAddress' doesn't work, 'feedStatus' informs the user about the incorrect input. 

                // feedStatus.Text = "Test if URI is valid";

                Uri resourceUri;
                if (!Uri.TryCreate(lePointFeed.Trim(), UriKind.Absolute, out resourceUri))
                {
                    //  feedStatus.Text = "Invalid URI, please re-enter a valid URI";
                    return;
                }
                if (resourceUri.Scheme != "http" && resourceUri.Scheme != "https")
                {
                    // feedStatus.Text = "Only 'http' and 'https' schemes supported. Please re-enter URI";
                    return;
                }
                // ---------- end of test--------------------------------------------------------------------- 

                string responseText;
                //  feedStatus.Text = "Waiting for response ...";

                try
                {
                    response = await httpClient.GetAsync(resourceUri);

                    response.EnsureSuccessStatusCode();

                    responseText = await response.Content.ReadAsStringAsync();
                    //  statusPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                }
                catch (Exception ex)
                {
                    // Need to convert int HResult to hex string 
                    // feedStatus.Text = "Error = " + ex.HResult.ToString("X") +
                    //"  Message: " + ex.Message;
                    responseText = "";
                }
                // feedStatus.Text = response.StatusCode + " " + response.ReasonPhrase;

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
                //  lstRSS = FindChildControl<ListView>(NewsHubSection, "lstRSS") as ListView;
                var list = sender as GridView;
                list.DataContext = lstData;
            }

        }

        private void GridView1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var list = sender as GridView;
            SharedEntite.shared = list.SelectedItem as rssItems;
            Frame.Navigate(typeof(Detail));
        }

        private void GridView3_Loaded(object sender, RoutedEventArgs e)
        {
            ListEntite = new List<Entite>();
            ListEntite.Add(new Entite() { title = "Premier article", image = "/Assets/10940527_10202494361960653_7285173102864178019_n.jpg", description = "Attentat fomenté par les services secrets, coup monté des medias, manigance des ennemis de l'islam... Le 7 janvier 2015, les théories conspirationnistes censées élucider l'attentat meurtrier contre Charlie Hebdo n'ont mis que quelques heures à se répandre sur internet. Comment reconnaître et lutter contre ces explications fantasques ? ➜ A lire également : Où sont passés les paranos ? ➜ Dossier web : Qu'est-ce que la rumeur ?" });
            ListEntite.Add(new Entite() { title = "Deuxième article", image = "/Assets/statistique.jpg", description = "Attentat fomenté par les services secrets, coup monté des medias, manigance des ennemis de l'islam... Le 7 janvier 2015, les théories conspirationnistes censées élucider l'attentat meurtrier contre Charlie Hebdo n'ont mis que quelques heures à se répandre sur internet. Comment reconnaître et lutter contre ces explications fantasques ? ➜ A lire également : Où sont passés les paranos ? ➜ Dossier web : Qu'est-ce que la rumeur ?" });
            ListEntite.Add(new Entite() { title = "Troisième Article", image = "/Assets/image78.png", description = "Attentat fomenté par les services secrets, coup monté des medias, manigance des ennemis de l'islam... Le 7 janvier 2015, les théories conspirationnistes censées élucider l'attentat meurtrier contre Charlie Hebdo n'ont mis que quelques heures à se répandre sur internet. Comment reconnaître et lutter contre ces explications fantasques ? ➜ A lire également : Où sont passés les paranos ? ➜ Dossier web : Qu'est-ce que la rumeur ?" });
            ListEntite.Add(new Entite() { title = "Premier article", image = "/Assets/euro-billets-de-banque-17537597.jpg", description = "Attentat fomenté par les services secrets, coup monté des medias, manigance des ennemis de l'islam... Le 7 janvier 2015, les théories conspirationnistes censées élucider l'attentat meurtrier contre Charlie Hebdo n'ont mis que quelques heures à se répandre sur internet. Comment reconnaître et lutter contre ces explications fantasques ? ➜ A lire également : Où sont passés les paranos ? ➜ Dossier web : Qu'est-ce que la rumeur ?" });

            var list = sender as GridView;

            list.DataContext = ListEntite;
        }

        private void GridView3_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var list = sender as GridView;
            SharedEntite.sharedEntite = list.SelectedItem as Entite;
            Frame.Navigate(typeof(Detail));
        }

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
    }

}
