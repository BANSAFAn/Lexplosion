using Lexplosion.WPF.NewInterface.Core;
using Lexplosion.WPF.NewInterface.Mvvm.Models.Profile;

namespace Lexplosion.WPF.NewInterface.Mvvm.ViewModels.Profile
{
    public sealed class ProfileViewModel : ViewModelBase
    {
        public ProfileModel Model { get; }

        public ProfileViewModel()
        {
            Model = new();
        }
    }
}
