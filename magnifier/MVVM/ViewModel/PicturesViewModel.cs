using magnifier.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;

namespace magnifier.MVVM.ViewModel
{
    internal class PicturesViewModel : UserControlViewModel
    {
        public ICommand? NextPicture { get; }
        public ICommand? PrevPicture { get; }

        private readonly ImageManager? _imageManager;
        private ObservableCollection<ImageSource>? _pictures;
        private readonly Dictionary<ImageSource, string>? _pictureDescriptions;
        private ImageSource? _currentPicture;
        private int? _currentPictureIndex;
        private string? _currentDescription;

        public PicturesViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;

            _pictures = new ObservableCollection<ImageSource>();
            _pictureDescriptions = new Dictionary<ImageSource, string>();
            _imageManager = new ImageManager();

            _pictures = _imageManager!.LoadImagesFromResources();
            _pictureDescriptions = _imageManager!.AssociateDescriptions();

            _currentPictureIndex = 0;
            UpdateCurrentPicture();

            NextPicture = new DelegateCommand<object>(o =>
            {
                ExecuteNextPicture();
            });

            PrevPicture = new DelegateCommand<object>(o =>
            {
                ExecutePrevPicture();
            });

            Title = "Picture Magnifier";
        }

        public override void UpdateProperties()
        {
            if (_pictures!.Count > 0)
            {
                CurrentPicture = _pictures[(int)_currentPictureIndex!];
                CurrentDescription = _pictureDescriptions![CurrentPicture];
            }
        }

        private void UpdateCurrentPicture()
        {
            if (_pictures!.Count > 0)
            {
                CurrentPicture = _pictures[(int)_currentPictureIndex!];
                CurrentDescription = _pictureDescriptions![CurrentPicture];
            }
            else
            {
                Debug.WriteLine(_pictures!.Count);
            }
        }

        private void ExecuteNextPicture()
        {
            if (CanExecuteNextPicture())
            {
                _currentPictureIndex++;
            }
            else
            {
                _currentPictureIndex = 0;
            }

            UpdateCurrentPicture();
            UpdateProperties();
        }

        private bool CanExecuteNextPicture()
        {
            return _currentPictureIndex < _pictures!.Count - 1;
        }

        private void ExecutePrevPicture()
        {
            if (CanExecutePrevPicture())
            {
                _currentPictureIndex--;
            }
            else
            {
                _currentPictureIndex = _pictures!.Count - 1;
            }

            UpdateCurrentPicture();
        }

        private bool CanExecutePrevPicture()
        {
            return _currentPictureIndex > 0;
        }

        public ImageSource? CurrentPicture
        {
            get => _currentPicture;
            set
            {
                _currentPicture = value;
                RaisePropertyChanged(nameof(CurrentPicture));
            }
        }
        public string? CurrentDescription
        {
            get => _currentDescription;
            private set
            {
                _currentDescription = value;
                RaisePropertyChanged(nameof(CurrentDescription));
            }
        }

        public ObservableCollection<ImageSource>? Pictures
        {
            get => _pictures;
            set
            {
                _pictures = value;
                RaisePropertyChanged(nameof(Pictures));
            }
        }
    }
}