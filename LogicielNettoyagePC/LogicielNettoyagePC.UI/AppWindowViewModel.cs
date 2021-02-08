using LogicielNettoyagePC.UI.Common;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using LogicielNettoyagePC.UI.Interfaces;
using LogicielNettoyagePC.UI.Helpers;
using LogicielNettoyagePC.Common;

namespace LogicielNettoyagePC.UI
{
    public class AppWindowViewModel : ViewModelBase
    {
        private string windowTitle = ResourceFR.WindowTitle;
        private string topTitle = ResourceFR.TopTitle;
        private string information = ResourceFR.NeedToCleanTxt;
        private bool canShowInformation;
        private string version;
        private string dateOfLastAnalises;

        private IPage selectedPage;
        private readonly IPageProvider pageProvider;
        private readonly ISettingsManager settingsManager;

        public AppWindowViewModel(IPageProvider pageProvider, ISettingsManager settingsManager)
        {
            WindowTitle = windowTitle;
            CanShowInformation = false;
            ExitCommand = new RelayCommand<EventArgs>(ExecuteExitCommand);
            ChangePageCommand = new RelayCommand<object>(ExecuteChangePageCommand);

            settingsManager.HistoryChanged += SettingsManager_HistoryChanged;

            this.pageProvider = pageProvider;
            this.settingsManager = settingsManager;

            ExecuteChangePageCommand(PageKind.Main);
            Version = "Version 1.0.0.";

            DateOfLastAnalises = settingsManager.ListHistories.FirstOrDefault().VerificationDate.ToString();
        }

        private void SettingsManager_HistoryChanged(object sender, HistoryChangedEventArgs e)
        {
            DateOfLastAnalises = e.LastVerification.ToString();
        }

        public IPage SelectedPage
        {
            get { return selectedPage; }
            set { SetProperty(ref selectedPage, value); }
        }

        public ICommand ChangePageCommand { get; }
        public ICommand ExitCommand { get; }

        public string WindowTitle
        {
            get { return windowTitle; }
            private set
            { SetProperty(ref windowTitle, value); }
        }

        public string TopTitle
        {
            get { return topTitle; }
            private set { SetProperty(ref topTitle, value); }
        }

        public string Version
        {
            get { return version; }
            private set { SetProperty(ref version, value); }
        }

        public string Information
        {
            get { return information; }
            set { SetProperty(ref information, value); }
        }

        public bool CanShowInformation
        {
            get { return canShowInformation; }
            set { SetProperty(ref canShowInformation, value); }
        }

        public string DateOfLastAnalises
        {
            get { return dateOfLastAnalises; }
            set { SetProperty(ref dateOfLastAnalises, value); }
        }

        #region Commands
        private void ExecuteChangePageCommand(object obj)
        {
            if (obj == null)
                return;

            PageKind pageKind = obj.ToString().ConvertToPageKind();

            if (pageKind != PageKind.Unknown)
            {
                SelectedPage = pageProvider.GetPage(pageKind);
                SelectedPage.Refreshe();
            }

            TopTitle = SelectedPage.Caption;
        }

        private void ExecuteExitCommand(object obj)
        {
            Application.Current.Shutdown();
        }

      
        #endregion Commands
    }
}
