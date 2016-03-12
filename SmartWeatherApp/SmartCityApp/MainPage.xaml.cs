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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Notifications;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SmartCityApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            pageFrame.Navigate(typeof(HomeFrame));

        }

        private void toastButton_Click(object sender, RoutedEventArgs e)
        {
            var toast1 = ToastTemplateType.ToastImageAndText01;
            var xml = ToastNotificationManager.GetTemplateContent(toast1);
            XmlNodeList toastTextElement = xml.GetElementsByTagName("text");
            toastTextElement[0].AppendChild(xml.CreateTextNode("Notified!"));

            XmlNodeList toastImageAttributes = xml.GetElementsByTagName("image");

            ((XmlElement)toastImageAttributes[0]).SetAttribute("src", "ms-appx:///assets/logo1.png");
            ((XmlElement)toastImageAttributes[0]).SetAttribute("alt", "red graphic");

            ToastNotification toast = new ToastNotification(xml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var storageFile =
              await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(
                new Uri("ms-appx:///VoiceCommandDictionary.xml"));
            await Windows.ApplicationModel.VoiceCommands.VoiceCommandDefinitionManager
                .InstallCommandDefinitionsFromStorageFileAsync(storageFile);
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            Mysplitview.IsPaneOpen = !Mysplitview.IsPaneOpen;
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (Mysplitview.IsPaneOpen)
            {
                Mysplitview.IsPaneOpen = !Mysplitview.IsPaneOpen;
            }
            pageFrame.Navigate(typeof(HomeFrame));
        }

        private void TextBlock_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            if(Mysplitview.IsPaneOpen)
            {
                Mysplitview.IsPaneOpen = !Mysplitview.IsPaneOpen;
            }
            pageFrame.Navigate(typeof(feedback));
        }
    }
}
