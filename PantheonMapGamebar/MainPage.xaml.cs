using Microsoft.Gaming.XboxGameBar;
using Microsoft.Gaming.XboxGameBar.Authentication;
using Microsoft.Gaming.XboxGameBar.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Timers;
//using TextCopy;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PantheonMapGamebar
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private double locXCache = 0;
        private double locZCache = 0;
        private string baseUrl = "https://shalazam.info/maps/1";

        private XboxGameBarWidget widget = null;
        private XboxGameBarWidgetControl widgetControl = null;
        private XboxGameBarWebAuthenticationBroker gameBarWebAuth = null;
        private XboxGameBarAppTargetTracker appTargetTracker = null;
        private XboxGameBarWidgetNotificationManager notificationManager = null;
        private XboxGameBarHotkeyWatcher hotkeyWatcher = null;
        private XboxGameBarWidgetActivity widgetActivity = null;

        public MainPage()
        {
            this.InitializeComponent();


        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            widget = e.Parameter as XboxGameBarWidget;

            mapView.Source = new System.Uri(baseUrl);

            
            // causes crash, need to somehow ensure our window is visible before accessing the clipboard
            /*Windows.ApplicationModel.DataTransfer.Clipboard.ContentChanged += async (s, b) =>
            {
                //DataPackageView dataPackageView = Clipboard.GetContent();
                /*if (dataPackageView.Contains(StandardDataFormats.Text))
                {
                    string text = await dataPackageView.GetTextAsync();
                    // To output the text from this example, you need a TextBlock control
                    TextOutput.Text = "Clipboard now contains: " + text;
                }
                var text = ClipboardService.GetText();
                ParseMapData(text);
            };*/
        }

        private void ParseMapData(string clipboard)
        {
            Match match = Regex.Match(clipboard, @"\/jumploc\s+(-?\d+\.\d+)\s+-?\d+\.\d+\s(-?\d+\.\d+)");

            if (!match.Success)
                return;

            var locX = Math.Floor(float.Parse(match.Groups[1].Value));
            var locZ = Math.Floor(float.Parse(match.Groups[2].Value));

            /*if (locX == locXCache || locZ == locZCache)
                return;*/

            locXCache = locX;
            locZCache = locZ;

            // const url = `${currentMapUrl}?zoom=4&x=${x}&y=${z}&pin%5B%5D=${x}.${z}.&pin%5B%5D=${x}.${z}.Dropped+Pin`;

            var url = baseUrl + "?zoom=4&x=" +
                locX + "&y=" + locZ + "&pin%5B%5D=" +
                locX + "." + locZ + ".&pin%5B%5D=" +
                locX + "." + locZ + ".Dropped+Pin";

            mapView.Source = new System.Uri(url);
        }

        private async void btnUpdate_click(object sender, RoutedEventArgs e)
        {
            DataPackageView dataPackageView = Clipboard.GetContent();
            if (dataPackageView.Contains(StandardDataFormats.Text))
            {
                string text = await dataPackageView.GetTextAsync();
                ParseMapData(text);
            }
        }

        private void btnMain_click(object sender, RoutedEventArgs e)
        {
            baseUrl = "https://shalazam.info/maps/1";
            mapView.Source = new System.Uri(baseUrl);
        }

        private void btnHC_click(object sender, RoutedEventArgs e)
        {
            baseUrl = "https://shalazam.info/maps/2";
            mapView.Source = new System.Uri(baseUrl);
        }
        private void btnGC_click(object sender, RoutedEventArgs e)
        {
            baseUrl = "https://shalazam.info/maps/3";
            mapView.Source = new System.Uri(baseUrl);
        }
    }
}
