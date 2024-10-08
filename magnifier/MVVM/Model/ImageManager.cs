﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections;
using System.Reflection;
using System.Resources;
using magnifier.MVVM.Model;

namespace magnifier
{
    internal class ImageManager
    {
        private readonly ObservableCollection<ImageSource>? _pictures;
        private readonly Dictionary<ImageSource, string>? _pictureDescriptions;

        private static Point _start;
        private static Point _origin;

        public ImageManager()
        {
            _pictures = new ObservableCollection<ImageSource>();
            _pictureDescriptions = new Dictionary<ImageSource, string>();
        }

        public ObservableCollection<ImageSource> LoadImagesFromResources()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames = assembly.GetManifestResourceNames();

            string? gResourceName = Array.Find(resourceNames, s => s.EndsWith(".g.resources"));

            if (!string.IsNullOrEmpty(gResourceName))
            {
                using Stream? stream = assembly.GetManifestResourceStream(gResourceName);
                if (stream != null)
                {
                    using ResourceReader reader = new(stream);
                    foreach (DictionaryEntry entry in reader)
                    {
                        string resourceName = (string)entry.Key;

                        if (resourceName.StartsWith("images/3d%20pictures/", StringComparison.OrdinalIgnoreCase))
                        {
                            var imageUri = new Uri($"pack://application:,,,/{resourceName}", UriKind.Absolute);
                            var bitmap = new BitmapImage(imageUri);
                            _pictures!.Add(bitmap);
                        }
                    }
                }
            }
            Utility.SortImages(_pictures);

            return _pictures!;
        }

        public Dictionary<ImageSource, string> AssociateDescriptions()
        {
            foreach (var picture in _pictures!)
            {
                string fileName = GetImageFileName(picture);
                string description = GenerateDescription(fileName);
                _pictureDescriptions![picture] = description;
            }

            return _pictureDescriptions!;
        }

        private static string GetImageFileName(ImageSource picture)
        {
            return picture.ToString().Replace("pack://application:,,,/images/3d pictures/", "");
        }

        private static string GenerateDescription(string fileName)
        {
            return fileName switch
            {
                "00 hair drier.jpg" => "A hair drier, often used to dry hair quickly.",
                "01 lamp.jpg" => "A lamp that illuminates the room.",
                _ => "No description available."
            };
        }

        public static void LoadImage(Uri fileUri, Image frente)
        {
            TiffBitmapDecoder tiffBitmapDecoder = new TiffBitmapDecoder(fileUri,
                               BitmapCreateOptions.None,
                               BitmapCacheOption.Default);

            frente.Source = tiffBitmapDecoder.Frames[0];
        }

        public static void SetScale(TransformGroup tg, Double scale)
        {
            ScaleTransform st = (ScaleTransform)tg.Children[0];
            // scale - 0 a  100
            // zoom - 0.1 a 20
            st.ScaleX = scale / 5 + 0.1;
            st.ScaleY = scale / 5 + 0.1;

        }

        public static void ZoomIn(TransformGroup tg)
        {
            ScaleTransform st = (ScaleTransform)tg.Children[0];

            st.ScaleX *= 1.25;
            st.ScaleY *= 1.25;

        }

        public static void ZoomOut(TransformGroup tg)
        {
            ScaleTransform st = (ScaleTransform)tg.Children[0];
            st.ScaleX *= 0.75;
            st.ScaleY *= 0.75;
        }

        public static void FitToContentMagnifier(Image imgObject, TransformGroup tg, Canvas parentCanvas, Canvas canvasMagnifier, Image imgMagnifier)
        {
            ScaleTransform st = (ScaleTransform)tg.Children[0];
            TranslateTransform tt = (TranslateTransform)tg.Children[1];

            tt.X = 0;
            tt.Y = 0;
            st.CenterX = 0;
            st.CenterY = 0;


            Double auxX = parentCanvas.ActualWidth / imgObject.ActualWidth;
            Double auxY = parentCanvas.ActualHeight / imgObject.ActualHeight;

            st.ScaleX = auxX > auxY ? auxY : auxX;
            st.ScaleY = auxX > auxY ? auxY : auxX;

            //st.CenterX = imgObject.ActualWidth / 2;
            //st.CenterY = imgObject.ActualHeight / 2
        }

        public static void Rotate(RotateTransform rt)
        {
            if (rt.Angle == 270)
                rt.Angle = 0;
            else
                rt.Angle += 90;

            rt.CenterX = 100;
            rt.CenterY = 100;
        }

        public static void MouseDown(Canvas imgCanvas, Image imgObject, TranslateTransform tt, MouseButtonEventArgs e)
        {
            imgObject.CaptureMouse();
            _start = e.GetPosition(imgCanvas);
            _origin = new Point(tt.X, tt.Y);
        }

        public static void MouseMove(Canvas imgCanvas, Image imgObject, TranslateTransform tt, MouseEventArgs e)
        {
            if (!imgObject.IsMouseCaptured) return;

            Vector v = _start - e.GetPosition(imgCanvas);
            tt.X = _origin.X - v.X;
            tt.Y = _origin.Y - v.Y;
        }

        public static void MouseMoveMagnifier(Canvas imgCanvas, Image imgObject, Canvas imgCanvasMagnifier, Image imgMagnifier, MouseEventArgs e)
        {
            Magnifier(imgCanvas, imgObject, imgCanvasMagnifier, imgMagnifier, e);

            if (!imgObject.IsMouseCaptured) return;
            TranslateTransform tt = (TranslateTransform)((TransformGroup)imgObject.RenderTransform).Children[1];
            Vector v = _start - e.GetPosition(imgCanvas);
            tt.X = _origin.X - v.X;
            tt.Y = _origin.Y - v.Y;
        }

        public static void MouseUp(Image imgObject, MouseButtonEventArgs e)
        {
            imgObject.ReleaseMouseCapture();
        }

        public static void MouseWheel(Canvas c, TransformGroup tg, MouseWheelEventArgs e)
        {
            int deltaValue;
            deltaValue = e.Delta;
            TranslateTransform tt = (TranslateTransform)tg.Children[1];
            ScaleTransform st = (ScaleTransform)tg.Children[0];
            double xSpot = e.GetPosition(c).X;
            double ySpot = e.GetPosition(c).Y;

            Double x = e.GetPosition(c).X - tt.X;
            Double y = e.GetPosition(c).Y - tt.Y;

            Double centerX = st.CenterX * (st.ScaleX - 1);
            Double centerY = st.CenterY * (st.ScaleY - 1);

            st.CenterX = x;
            st.CenterY = y;



            if (deltaValue > 0)
            {
                st.ScaleX *= 1.25;
                st.ScaleY *= 1.25;
            }
            else
            {

                st.ScaleX *= 0.75;
                st.ScaleY *= 0.75;
            }
        }

        public static void Magnifier(Canvas imgCanvas, Image imgObject, Canvas imgCanvasMagnifier, Image imgMagnifier, MouseEventArgs e)
        {
            Double width = imgCanvasMagnifier.Width;
            Double height = imgCanvasMagnifier.Height;
            Int32 zoom = 5;

            String txtDebug = String.Empty;
            String txtZoom = String.Empty;

            if (imgMagnifier.Source != imgObject.Source)
            {
                imgMagnifier.Source = imgObject.Source;
            }

            Size size = imgObject.RenderSize;
            RotateTransform rt = (RotateTransform)imgObject.LayoutTransform;
            TranslateTransform tt = (TranslateTransform)((TransformGroup)imgObject.RenderTransform).Children[1];
            ScaleTransform st = (ScaleTransform)((System.Windows.Media.TransformGroup)(imgObject.RenderTransform)).Children[0];
            Double x = e.GetPosition(imgCanvas).X - tt.X;
            Double y = e.GetPosition(imgCanvas).Y - tt.Y;
            Point pos = e.MouseDevice.GetPosition(imgCanvas);
            var wnd = Canvas.GetTop(imgObject);

            TransformGroup transformGroup = new TransformGroup();
            ScaleTransform scale = new ScaleTransform();

            scale.ScaleX = st.ScaleX * zoom;
            scale.ScaleY = st.ScaleY * zoom;

            RotateTransform rotate = new RotateTransform();
            rotate.Angle = rt.Angle;

            TranslateTransform translate = new TranslateTransform();

            Double centerX = st.CenterX * (st.ScaleX - 1);
            Double centerY = st.CenterY * (st.ScaleY - 1);

            if (rt.Angle == 0)
            {
                translate.X = -(x + centerX) / st.ScaleX;
                translate.Y = -(y + centerY) / st.ScaleY;
                scale.CenterX = (x + centerX) / st.ScaleX;
                scale.CenterY = (y + centerY) / st.ScaleY;
            }
            if (rt.Angle == 90)
            {
                translate.X = -(x + centerX) / st.ScaleX;
                translate.Y = -(y + centerY) / st.ScaleY;
                translate.X += imgObject.ActualHeight * st.ScaleX * zoom;
                scale.CenterX = (x + centerX) / st.ScaleX;
                scale.CenterY = (y + centerY) / st.ScaleY;
            }

            if (rt.Angle == 180)
            {
                translate.X = -(x + centerX) / st.ScaleX;
                translate.Y = -(y + centerY) / st.ScaleY;
                translate.X += imgObject.ActualWidth * st.ScaleX * zoom;
                translate.Y += imgObject.ActualHeight * st.ScaleY * zoom;
                scale.CenterX = (x + centerX) / st.ScaleX;
                scale.CenterY = (y + centerY) / st.ScaleY;
            }

            if (rt.Angle == 270)
            {
                translate.X = -(x + centerX) / st.ScaleX;
                translate.Y = -(y + centerY) / st.ScaleY;
                translate.Y += imgObject.ActualWidth * st.ScaleX * zoom;
                scale.CenterX = (x + centerX) / st.ScaleX;
                scale.CenterY = (y + centerY) / st.ScaleY;
            }

            translate.X += width / 2;
            translate.Y += height / 2;

            transformGroup.Children.Add(rotate);
            transformGroup.Children.Add(scale);
            transformGroup.Children.Add(translate);


            imgMagnifier.RenderTransform = transformGroup;

        }
    }
}
