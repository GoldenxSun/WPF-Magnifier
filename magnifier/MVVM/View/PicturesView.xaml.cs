using magnifier.MVVM.ViewModel;
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
using System.Windows.Threading;

namespace magnifier.MVVM.View
{
    /// <summary>
    /// Interaktionslogik für PicturesView.xaml
    /// </summary>
    public partial class PicturesView : UserControl
    {
        private readonly ImageManager imagingProviderIndexador = new ImageManager();
        private PicturesViewModel _viewModel;

        public PicturesView()
        {
            InitializeComponent();

            _viewModel = new PicturesViewModel();
            DataContext = _viewModel;


            // Load images and set the current picture
            var images = imagingProviderIndexador.LoadImagesFromResources();
            _viewModel.Pictures = images;
            _viewModel.CurrentPicture = _viewModel.Pictures.FirstOrDefault();


            imgObj.Source = _viewModel.CurrentPicture;
            imgMagnifier.Source = _viewModel.CurrentPicture;
            Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
            {
                ImageManager.FitToContentMagnifier(imgObj, imgTransformGroup, imgCanvas, imgCanvasMagnifier, imgMagnifier);
            }));
        }

        private void img_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            ImageManager.MouseWheel(imgCanvas, imgTransformGroup, e);
        }

        private void Img_MouseMove(object sender, MouseEventArgs e)
        {
            if (_viewModel.CurrentPicture != null)
            {
                imagingProviderIndexador.MouseMoveMagnifier(imgCanvas, imgObj, imgCanvasMagnifier, imgMagnifier, e);
            }
        }

        private void Img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel.CurrentPicture != null)
            {
                imagingProviderIndexador.MouseDown(imgCanvas, imgObj, imgTranslateTransform, e);
            }
        }

        private void Img_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel.CurrentPicture != null)
            {
                imagingProviderIndexador.MouseUp(imgObj, e);
            }
        }

        private void btnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            ImageManager.ZoomIn(imgTransformGroup);
        }

        private void btnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            ImageManager.ZoomOut(imgTransformGroup);
        }

        private void btnRotate_Click(object sender, RoutedEventArgs e)
        {
            ImageManager.Rotate(imgRotateTransform);
        }

        private void btnFTW_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.CurrentPicture != null)
            {
                ImageManager.FitToContentMagnifier(imgObj, imgTransformGroup, imgCanvas, imgCanvasMagnifier, imgMagnifier);
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ImageManager.SetScale(imgTransformGroup, sldZoom.Value);
        }

        private void btnNextPicture_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Pictures != null && _viewModel.CurrentPicture != null)
            {
                int currentIndex = _viewModel.Pictures.IndexOf(_viewModel.CurrentPicture);
                int nextIndex = (currentIndex + 1) % _viewModel.Pictures.Count;
                _viewModel.CurrentPicture = _viewModel.Pictures[nextIndex];
                imgObj.Source = _viewModel.CurrentPicture;
                imgMagnifier.Source = _viewModel.CurrentPicture;
                Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                {
                    ImageManager.FitToContentMagnifier(imgObj, imgTransformGroup, imgCanvas, imgCanvasMagnifier, imgMagnifier);
                }));
            }
        }
    }
}
