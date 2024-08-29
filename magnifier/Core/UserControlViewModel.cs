using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magnifier.Core
{
    internal class UserControlViewModel : ObservableObject, IUpdatable
    {
        private string? _title;

        public string? Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }
    }
}
