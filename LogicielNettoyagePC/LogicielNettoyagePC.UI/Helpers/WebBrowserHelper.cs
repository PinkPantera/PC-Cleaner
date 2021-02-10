using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace LogicielNettoyagePC.UI.Helpers
{
    public class WebBrowserHelper
    {
        public static readonly DependencyProperty UrlProperty =
            DependencyProperty.RegisterAttached("Url", typeof(string), typeof(WebBrowserHelper), new UIPropertyMetadata(null, OnUrlChanged));

        public static string GetUrl(DependencyObject dependencyObject)
        {
            return (string)dependencyObject.GetValue(UrlProperty);
        }

        public static void SetUrl(DependencyObject dependencyObject, string url)
        {
            dependencyObject.SetValue(UrlProperty, url);
        }

        public static void OnUrlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var webBrowser = d as WebBrowser;

            if (webBrowser == null)
                return;

            Uri uri = null;

            if (e.NewValue is string)
            {
                var uriStr = e.NewValue as string;
                if (!string.IsNullOrEmpty(uriStr))
                {
                    uri = new Uri(uriStr);
                }
            }
            else if (e.NewValue is Uri)
            {
                uri = e.NewValue as Uri;
            }

            webBrowser.Source = uri;
        }
    }
}
