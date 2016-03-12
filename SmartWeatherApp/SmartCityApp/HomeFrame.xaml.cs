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
using System.Runtime.Serialization.Json;
using Windows.Data.Xml.Dom;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SmartCityApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomeFrame : Page
    {
        private const string JSONFILENME = "data.json";
        public HomeFrame()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var v = await loadContent();
            progRing.IsActive = false;
            writeJsonAsync();
            //readJsonAsync();
        }

        private async Task<int> loadContent()
        {
            try
            {
                var position = await LocationManager.GetPosition();
                Rootobject myWeather = await OpenWeatherMapProxy.GetWeather(position.Coordinate.Latitude,
                    position.Coordinate.Longitude);
                scheduleNotify(position);
                string icon = String.Format("ms-appx:///Assets/Weather/{0}.png", myWeather.weather[0].icon);
                resultImage.Source = new BitmapImage(new Uri(icon));
                TempTextBlock.Text = ((int)myWeather.main.temp).ToString() + " *C";
                DescriptionTextBlock.Text = myWeather.weather[0].description;
                LocationTextBlock.Text = myWeather.name;
                MinValueTextblock.Text = myWeather.main.temp_min.ToString() + " *C";
                MaxvalueTextBlock.Text = myWeather.main.temp_max.ToString() + " *C";
                Pressuretextblock.Text += string.Format("Pressure\n{0}", myWeather.main.pressure.ToString());
                Humiditytextblock.Text += string.Format("Humidity\n{0} %", myWeather.main.humidity.ToString());
                humidityProgressBar.Value = myWeather.main.humidity;
                pressureProgressBar.Value = myWeather.main.pressure;
                tempImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/weather-thermometer-clip-art-9cRXkggce.png"));
                maxText.Text = "Maximum";
                minText.Text = "Minimum";
                //Schedule Upadte
                requestUpdate(position);

                return 1;
            }
            catch
            {
                LocationTextBlock.Text = "Unable to get weather at this time";
                return 0;
            }
        }

        public void requestUpdate(Windows.Devices.Geolocation.Geoposition position)
        {
            var uri = String.Format("http://owmweatherservicelivetile.azurewebsites.net/?lat={0}&lon={1}", position.Coordinate.Latitude,
                    position.Coordinate.Longitude);

            var tileContent = new Uri(uri);
            var requestedInterval = PeriodicUpdateRecurrence.HalfHour;

            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.StartPeriodicUpdate(tileContent, requestedInterval);
        }

        public async void scheduleNotify(Windows.Devices.Geolocation.Geoposition position)
        {
            var wForecast = await Forecast.WeatherForecast.GetWeatherForecast(position.Coordinate.Latitude,
                    position.Coordinate.Longitude);
            string writtenDate = await readJsonAsync();
            if(DateTime.Now.Date.ToString() != writtenDate)
            {
                bool exceed = false;
                DateTime current = DateTime.Now.Date;
                DateTime dayAfter = current.AddDays(1);
                foreach(var v in wForecast.list)
                {
                    foreach(var w in v.weather)
                    {
                        string s = w.id.ToString();
						if(s.StartsWith("5"))
                        {
                            ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText01;
                            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

                            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
                            toastTextElements[0].AppendChild(toastXml.CreateTextNode("It may be rain soon. Get an umbrella"));

                            XmlNodeList toastImageAttributes = toastXml.GetElementsByTagName("image");
                            ((XmlElement)toastImageAttributes[0]).SetAttribute("src", "ms-appx:///assets/redWide.png");
                            ((XmlElement)toastImageAttributes[0]).SetAttribute("alt", "red graphic");

                            DateTime dueTime = FromUnixTime(v.dt).AddDays(1).AddHours(4).AddMinutes(30);

                            ScheduledToastNotification scheduledToast = new ScheduledToastNotification(toastXml, dueTime);

                            scheduledToast.Id = "Future_Toast";
                            ToastNotificationManager.CreateToastNotifier().AddToSchedule(scheduledToast);
                        }

                        else if(s.StartsWith("3"))
                        {
                            ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText01;
                            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

                            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
                            toastTextElements[0].AppendChild(toastXml.CreateTextNode("It may be rain soon. Get an umbrella"));

                            XmlNodeList toastImageAttributes = toastXml.GetElementsByTagName("image");
                            ((XmlElement)toastImageAttributes[0]).SetAttribute("src", "ms-appx:///assets/redWide.png");
                            ((XmlElement)toastImageAttributes[0]).SetAttribute("alt", "red graphic");

                            DateTime dueTime = FromUnixTime(v.dt).AddDays(1).AddHours(4).AddMinutes(30);

                            ScheduledToastNotification scheduledToast = new ScheduledToastNotification(toastXml, dueTime);

                            scheduledToast.Id = "Future_Toast";
                            ToastNotificationManager.CreateToastNotifier().AddToSchedule(scheduledToast);
                        }

                        else if(s.StartsWith("90"))
                        {
                            ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText01;
                            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

                            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
                            toastTextElements[0].AppendChild(toastXml.CreateTextNode("Weather may be extremly adverse"));

                            XmlNodeList toastImageAttributes = toastXml.GetElementsByTagName("image");
                            ((XmlElement)toastImageAttributes[0]).SetAttribute("src", "ms-appx:///assets/redWide.png");
                            ((XmlElement)toastImageAttributes[0]).SetAttribute("alt", "red graphic");

                            DateTime dueTime = FromUnixTime(v.dt).AddDays(1).AddHours(4).AddMinutes(30);

                            ScheduledToastNotification scheduledToast = new ScheduledToastNotification(toastXml, dueTime);

                            scheduledToast.Id = "Future_Toast";
                            ToastNotificationManager.CreateToastNotifier().AddToSchedule(scheduledToast);
                        }

                        DateTime date1 = dayAfter.AddHours(8).AddMinutes(30);
                        //resultTextBlock.Text = date1.ToString();
                        //resultTextBlock.Text += FromUnixTime(v.dt).ToString();
                        if(date1 == FromUnixTime(v.dt).AddHours(5).AddMinutes(30))
                        {
                            ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText01;
                            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
                            string ids = w.id.ToString();
                            string text = "";
                            if(ids.StartsWith("2"))
                            {
                                text = "It may be thunderStrome Today. ";
                            }
                            else if (ids.StartsWith("3"))
                            {
                                text = "It may drizzle today.";
                            }
                            else if(ids.StartsWith("5"))
                            {
                                text = "It may rain today.Get an umbrella with you";
                            }
                            else if(ids.StartsWith("6"))
                            {
                                text = "It may snow today";
                            }
                            else if (ids.StartsWith("7"))
                            {
                                text = "Atmoshere may be misty today";
                            }
                            else if (ids.StartsWith("8"))
                            {
                                text = "The sky may be clear today";
                            }
                            else if(ids.StartsWith("90"))
                            {
                                text = "The conditions may be extreme adverse today";
                            }
                            else if (ids.StartsWith("9"))
                            {
                                text = "The weather may be pleasnt today";
                            }
                            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
                            toastTextElements[0].AppendChild(toastXml.CreateTextNode(text));

                            XmlNodeList toastImageAttributes = toastXml.GetElementsByTagName("image");
                            ((XmlElement)toastImageAttributes[0]).SetAttribute("src", "ms-appx:///assets/redWide.png");
                            ((XmlElement)toastImageAttributes[0]).SetAttribute("alt", "red graphic");

                            DateTime dueTime = DateTime.Now.Date.AddDays(1);
                            dueTime = dueTime.AddHours(7);

                            ScheduledToastNotification scheduledToast = new ScheduledToastNotification(toastXml, dueTime);

                            scheduledToast.Id = "Future_Toast";
                            ToastNotificationManager.CreateToastNotifier().AddToSchedule(scheduledToast);

                        }
                        
                    }
                }
            }
            
        }

        private async Task writeJsonAsync()
        {
            var data = DateTime.Now.Date;
            data = data.AddDays(5);
            string date = data.ToString();
            var serializer = new DataContractJsonSerializer(typeof(String));
            using (var stream = await Windows.Storage.ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(
                JSONFILENME, Windows.Storage.CreationCollisionOption.ReplaceExisting))
            {
                serializer.WriteObject(stream, date);
            }
            //resultTextBlock.Text = "Write Succeded";
        }

        private async Task<String> readJsonAsync()
        {
            string content = String.Empty;
            var stream = await Windows.Storage.ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(JSONFILENME);
            using (StreamReader reader = new StreamReader(stream))
            {
                content = await reader.ReadToEndAsync();
            }
            return content;
        }

        public DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }
    }
}
