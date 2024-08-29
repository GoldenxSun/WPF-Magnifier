using magnifier.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace magnifier.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        private string? _caption;
        private ObservableObject? _currentChildView;

        public MainViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;

            _currentChildView = new PicturesViewModel();
            Caption = (CurrentChildView as PicturesViewModel)?.Title;
        }

        public ObservableObject? CurrentChildView
        {
            get => _currentChildView;
            set
            {
                _currentChildView = value;
                RaisePropertyChanged(nameof(CurrentChildView));
            }
        }

        public string? Caption
        {
            get => _caption;
            set
            {
                _caption = value;
                RaisePropertyChanged(nameof(Caption));
            }
        }
    }
}
