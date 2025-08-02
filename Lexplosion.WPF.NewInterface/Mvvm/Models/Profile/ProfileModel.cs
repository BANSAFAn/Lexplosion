using Lexplosion.WPF.NewInterface.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Media;

namespace Lexplosion.WPF.NewInterface.Mvvm.Models.Profile
{
    public readonly struct ProfileSocialMedia
    {
        public string Url { get; } = "";
        public string LogoName { get; } = "";
        public string Name { get; } = "";

        public ProfileSocialMedia(string name, string url, string logoName)
        {
            Url = url;
            LogoName = logoName;
        }
    }

    public readonly struct ProfileTitle 
    {
        public ProfileTitle(string name, uint color)
        {
            Name = name;
            Color = color;
        }

        public string Name { get; }
        public uint Color { get; } = 0x167FFC;
        public uint BackgroundColor { get; } = 0x0;
        public uint BorderColor { get; } = 0x66167FFC;
    }

    public struct Post 
    {
        public string AuthorName { get; }
        public ProfileTitle AuthorTitle { get; }
        public string Text { get; set; }
        public int ViewsCount { get; }
        public DateTime CreationDate { get; }
        public int LikesCount { get; set; }
    }

    public class PostComment 
    {
        public string AuthorName { get; }
        public string Content { get; }
        public DateTime CreationDate { get; }
        public List<PostComment> Answers { get; } = new();
    }

    public class ProfileModel : ObservableObject
    {
        public List<ProfileSocialMedia> SocialMedia { get; } = new();
        public string Name { get; }
        public ProfileTitle Title { get; }

        public int FriendsCount { get; }
        public float HoursCount { get; }
        public string AccountAge { get; }

        public string Summary { get; }
        public ActivityStatus Status { get; }


        public ObservableCollection<Post> Posts { get; } = new();

        public ProfileModel()
        {
            Name = "_Hel2x_";
            Title = new("dotnet", 0xFF00ffb5);
            Summary = "The European languages are members of the same family. Their separate existence is a myth. For science, music, sport, etc, Europe.";
            FriendsCount = 13;
            HoursCount = 50000.2f;
            AccountAge = "Долго";

            SocialMedia.Add(new ProfileSocialMedia("VK", "https://vk.com/idhel2x", "VKontakte"));
            SocialMedia.Add(new ProfileSocialMedia("Youtube", "https://vk.com/idhel2x", "Youtube"));
            SocialMedia.Add(new ProfileSocialMedia("Discord", "https://vk.com/idhel2x", "Discord"));

            Status = ActivityStatus.NotDisturb;
        }
    }
}
