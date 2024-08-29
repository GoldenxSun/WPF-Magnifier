using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;

namespace magnifier.MVVM.Model
{
    internal static class Utility
    {
        internal static void SortImages(ObservableCollection<ImageSource>? Pictures)
        {
            if (Pictures == null || Pictures.Count == 0)
            {
                return;
            }

            var sortedList = Pictures
                .OrderBy(image => ExtractNumericPrefix(image))
                .ToList();

            Pictures.Clear();

            foreach (var sortedImage in sortedList)
            {
                Pictures.Add(sortedImage);
            }
        }

        private static int ExtractNumericPrefix(ImageSource imageSource)
        {
            if (imageSource is BitmapImage bitmapImage)
            {
                string fileName = Path.GetFileName(bitmapImage.UriSource.LocalPath);

                string numericPrefix = new(fileName.TakeWhile(char.IsDigit).ToArray());

                if (int.TryParse(numericPrefix, out int number))
                {
                    return number;
                }
            }

            return 0;
        }
    }
}
