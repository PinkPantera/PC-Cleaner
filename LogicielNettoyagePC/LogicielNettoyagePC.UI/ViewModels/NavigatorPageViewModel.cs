﻿using LogicielNettoyagePC.UI.Common;
using LogicielNettoyagePC.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;

namespace LogicielNettoyagePC.UI.ViewModels
{
    public class NavigatorPageViewModel : ViewModelBase, IPage
    {
        private string sourceNavigator;

        public NavigatorPageViewModel()
        {
            WebCommand = new RelayCommand<EventArgs>(ExecuteWebCommand);
            SourceNavigator = ResourceFR.UrlTxt;
            CanBeClosed = true;
        }

        public ICommand WebCommand { get; }

        public PageKind PageKind => PageKind.Navigator;

        public string Caption => ResourceFR.NavigatorTxt;

        public bool IsEnabled { get; set; }

        public string SourceNavigator
        {
            get { return sourceNavigator; }
            set { SetProperty(ref sourceNavigator, value); }
        }

        public bool CanBeClosed { get; private set; }

        public void Refreshe()
        {

        }

        private void ExecuteWebCommand(EventArgs obj)
        {
            try
            {
                Process.Start(new ProcessStartInfo(ResourceFR.UrlTxt)
                {
                    UseShellExecute = true
                });

            }
            catch (Exception ex)
            {
                //
                //TODO
                //write in log file
            }
        }
    }
}
