using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BlizzardWind.Desktop.Controls.Buttons
{
    /// <summary>
    /// NoteCard.xaml 的交互逻辑
    /// </summary>
    public partial class NoteCard : UserControl
    {
        public NoteCard()
        {
            InitializeComponent();
        }

        public BitmapImage CoverImage
        {
            get { return (BitmapImage)GetValue(CoverImageProperty); }
            set { SetValue(CoverImageProperty, value); }
        }
        // Using a DependencyProperty as the backing store for CoverImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CoverImageProperty =
            DependencyProperty.Register("CoverImage", typeof(BitmapImage), typeof(NoteCard));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(NoteCard));

        public string Keys
        {
            get { return (string)GetValue(KeysProperty); }
            set { SetValue(KeysProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Keys.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeysProperty =
            DependencyProperty.Register("Keys", typeof(string), typeof(NoteCard));

        public string Time
        {
            get { return (string)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Time.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(string), typeof(NoteCard));

        public int FontCount
        {
            get { return (int)GetValue(FontCountProperty); }
            set { SetValue(FontCountProperty, value); }
        }
        // Using a DependencyProperty as the backing store for FontCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FontCountProperty =
            DependencyProperty.Register("FontCount", typeof(int), typeof(NoteCard), new PropertyMetadata(0));
    }
}
