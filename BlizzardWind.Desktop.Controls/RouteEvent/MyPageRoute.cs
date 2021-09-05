using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BlizzardWind.Desktop.Controls.RouteEvent
{
    public class MyPageRoute
    {
        public string Route {  get; set; }

        public static readonly RoutedEvent MyPageRouteChangedEvent = EventManager.RegisterRoutedEvent(
            "MyPageRouteChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MyPageRoute));


    }
}
