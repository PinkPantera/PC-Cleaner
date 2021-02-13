using LogicielNettoyagePC.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace LogicielNettoyagePC.UI.Helpers
{
    public class WindowCloser
    {
        public static readonly DependencyProperty EnableWindowClosingProperty =
            DependencyProperty.RegisterAttached("EnableWindowClosing", typeof(bool), typeof(WindowCloser), new PropertyMetadata(false, OnEnableWindowClosingChanged));

        public static void SetEnableWindowClosing(DependencyObject dependencyObject, bool enableWindowClosing)
        {
            dependencyObject.SetValue(EnableWindowClosingProperty, enableWindowClosing);
        }

        public static bool GetEnableWindowClosing(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(EnableWindowClosingProperty);
        }

        private static void OnEnableWindowClosingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window window)
            {
                window.Loaded += (s, e) =>
                  {
                      if (window.DataContext is ICloseable wm)
                      {
                          wm.CloseWindow += () =>
                          {
                              window.Close();
                          };
                          window.Closing += (s, e) =>
                          {
                              e.Cancel = !wm.CanClose();

                              if (e.Cancel)
                              {
                                  MessageBox.Show($"{ResourceFR.CantCloseApplicationTxt} {ResourceFR.ProcessInProgressTxt}"
                                      , ResourceFR.CloseApplicationTxt, MessageBoxButton.OK, MessageBoxImage.Warning);
                              }
                          };
                      }
                  };
            }
        }
    }
}
