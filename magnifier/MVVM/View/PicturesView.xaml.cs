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
        public PicturesView()
        {
            InitializeComponent();

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
            if (ViewModel.CurrentPicture != null)
            {
                ImageManager.MouseMoveMagnifier(imgCanvas, imgObj, imgCanvasMagnifier, imgMagnifier, e);
            }
        }

        private void Img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ViewModel.CurrentPicture != null)
            {
                ImageManager.MouseDown(imgCanvas, imgObj, imgTranslateTransform, e);
            }
        }

        private void Img_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ViewModel.CurrentPicture != null)
            {
                ImageManager.MouseUp(imgObj, e);
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
            if (ViewModel.CurrentPicture != null)
            {
                ImageManager.FitToContentMagnifier(imgObj, imgTransformGroup, imgCanvas, imgCanvasMagnifier, imgMagnifier);
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ImageManager.SetScale(imgTransformGroup, sldZoom.Value);
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.CurrentPicture != null)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                {
                    ImageManager.FitToContentMagnifier(imgObj, imgTransformGroup, imgCanvas, imgCanvasMagnifier, imgMagnifier);
                }));
            }
        }
    }
}
