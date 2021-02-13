using LogicielNettoyagePC.Common;
using LogicielNettoyagePC.UI.Common;
using LogicielNettoyagePC.UI.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LogicielNettoyagePC.UI.ViewModels
{
    public class SettingPageVeiwModel : ViewModelBase, IPage
    {
        private IDirectoriesProvider directoriesProvider;
        private bool isAddNewElement;
        private DirectoryToDisplay editedItem;
        private string errorMessage;

        public SettingPageVeiwModel(IDirectoriesProvider directoriesProvider)
        {
            this.directoriesProvider = directoriesProvider;
            CanBeClosed = true;

            foreach (var dir in directoriesProvider.DirectoriesToAnalyse)
            {
                Directories.Add(new DirectoryToDisplay(dir.DirectoryPath, dir.DirectoryName));
            };

            AddNewElement = new RelayCommand<EventArgs>(ExecuteAddNewElement);
            SaveNewElement = new RelayCommand<EventArgs>(ExecuteSaveNewElement);
            CanceNewElement = new RelayCommand<EventArgs>(ExecuteCanceNewElement);
            DeleteDirectoryCommand = new RelayCommand<int>(ExecuteDeleteDirectory);

            EditedItem = new DirectoryToDisplay();
        }

        public PageKind PageKind => PageKind.Settings;
        public string Caption => ResourceFR.SettingsTxt;
        public bool IsEnabled { get; set; }

        public bool IsAddNewElement
        {
            get { return isAddNewElement; }
            set
            {
                SetProperty(ref isAddNewElement, value);
            }
        }

        public DirectoryToDisplay EditedItem
        {
            get
            {
                return editedItem;
            }
            set
            {
                SetProperty(ref editedItem, value);
            }
        }

        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
            set
            {
                SetProperty(ref errorMessage, value);
            }
        }

        public ObservableCollection<DirectoryToDisplay> Directories { get; } = new ObservableCollection<DirectoryToDisplay>();


        public ICommand AddNewElement { get; }
        public ICommand SaveNewElement { get; }
        public ICommand CanceNewElement { get; }
        public ICommand DeleteDirectoryCommand { get; }

        public bool CanBeClosed { get; private set; }

        public void Refreshe()
        {
            //DO
            //here we can realise the logic which will perform before the page will shown
        }

        #region Commands
        private void ExecuteDeleteDirectory(int index)
        {
            var directory = Directories[index];
            directoriesProvider.DeleteDirectory(directory.DirectoryPath, directory.DirectoryName);
            Directories.Remove(directory);
        }

        private void ExecuteCanceNewElement(EventArgs obj)
        {
            IsAddNewElement = false;
        }

        private void ExecuteSaveNewElement(EventArgs obj)
        {
            ErrorMessage = string.Empty;
            var result = directoriesProvider.AddDirectory(EditedItem.DirectoryPath, EditedItem.DirectoryName);
            if (result)
            {
                Directories.Add(EditedItem);
                IsAddNewElement = false;
            }
            else
            {
                ErrorMessage = ResourceFR.DirectoryAddedFail;
            }
         }

        private void ExecuteAddNewElement(EventArgs obj)
        {
            EditedItem = new DirectoryToDisplay();
            IsAddNewElement = true;
            ErrorMessage = string.Empty;
        }
        #endregion Commands

    }
}
