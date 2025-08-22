using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using LexplosionAvalonia.Tools;
using LexplosionAvalonia.Mvvm.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using Lexplosion.Core.Objects;
using Lexplosion.WindowComponents.Header;
using Lexplosion.Core;
using Lexplosion.Core.Tools;
using Lexplosion.Global;

namespace LexplosionAvalonia.Mvvm.Views.Windows
{
    public interface IScalable
    {
        double ActualWidth { get; }
        double ActualHeight { get; }
        double ScalingFactor { get; }
    }

    /// <summary>
    /// Логика взаимодействия для MainWindow.axaml
    /// </summary>
    public partial class MainWindow : Window, IScalable
    {
        private readonly Animation _defaultChangeThemeAnimation = new Animation
        {
            Duration = TimeSpan.FromSeconds(0.35 * 3),
            Easing = new Avalonia.Animation.Easings.SineEaseInOut(),
            Children =
            {
                new KeyFrame
                {
                    Setters = { new Setter(OpacityProperty, 1.0) },
                    Cue = 0d
                },
                new KeyFrame
                {
                    Setters = { new Setter(OpacityProperty, 0.0) },
                    Cue = 1d
                }
            }
        };

        private readonly AppCore _appCore;
        private bool _isScalled = false;
        private Gallery _gallery;


        public string currentLang = "ru";

        public double ScalingKeff { get; private set; } = 1;
        public double ScalingFactor { get; private set; } = 1;

        public MainWindow()
        {
            InitializeComponent();

            _appCore = AppCore.Instance;

            PrepareAnimationForThemeService();

            PointerPressed += OnPointerPressed;

            HeaderContainer.DataContext = new WindowHeaderArgs(GlobalData.GeneralSettings.AppHeaderTemplateName, Close, Maximized, Minimized);

            _appCore.Settings.ThemeService.AppHeaderTemplateNameChanged += () =>
            {
                HeaderContainer.DataContext = new WindowHeaderArgs(_appCore.Settings.ThemeService.SelectedAppHeaderTemplateName, Close, Maximized, Minimized);
            };

            _gallery = _appCore.GalleryManager;
            InitGallery();
        }

        private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                BeginMoveDrag(e);
            }
        }

        private void PrepareAnimationForThemeService()
        {
            var themeService = _appCore.Settings.ThemeService;

            themeService.BeforeAnimations.Add(() =>
            {
                PaintArea.Opacity = 1;
                PaintArea.IsVisible = true;
                // TODO: Implement CreateBrushFromVisual for Avalonia
            });

            themeService.Animations.Add(async () =>
            {
                await _defaultChangeThemeAnimation.RunAsync(PaintArea);
                // Adapt radius animations if needed
            });

            // Adapt welcome page animations similarly
        }

        private void InitGallery()
        {
            ImageViewer.IsVisible = _gallery.HasSelectedImage;

            _gallery.StateChanged += OnGalleryStateChanged;

            CloseImage.PointerPressed += OnCloseImageClicked;
            // Adapt button clicks for Avalonia
            //NextImage.PointerPressed += OnNextImageClicked;
            //PrevImage.PointerPressed += OnPrevImageClicked;

            // Adapt bindings for Avalonia
        }

        private void OnGalleryStateChanged()
        {
            ImageViewer.IsVisible = _gallery.HasSelectedImage;

            if (!_gallery.HasSelectedImage)
                Image.Source = null;
            if (Image.Source == null /* adapt condition */)
            {
                IBitmap image = null;

                Runtime.TaskRun(async () =>
                {
                    if (_gallery.SelectedImageSource is byte[] byteImage)
                    {
                        image = await Bitmap.DecodeToWidth(byteImage, 400); // Example, adapt
                    }
                    else if (_gallery.SelectedImageSource is string stringImage)
                    {
                        image = new Bitmap(stringImage);
                    }

                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        Image.Source = image;
                    });
                });

            }
        }

        #region Image Viewer


        private void OnPrevImageClicked(object? sender, PointerPressedEventArgs e)
        {
            _gallery.PrevImage();
        }

        private void OnNextImageClicked(object? sender, PointerPressedEventArgs e)
        {
            _gallery.NextImage();
        }

        private void OnCloseImageClicked(object? sender, PointerPressedEventArgs e)
        {
            _gallery.CloseImage();
        }


        #endregion ImageViewer


        private void Scalling()
        {
            double factor = 0.25;

            if (_isScalled)
            {
                factor *= -1;
            }

            // Adapt scaling for Avalonia
            this.Width += Width * factor;
            this.Height += Height * factor;
            ScalingFactor = factor + 1; // Adjust as needed
            // Center window
            var screen = Screens.Primary;
            if (screen != null)
            {
                var screenHeight = screen.WorkingArea.Height;
                var screenWidth = screen.WorkingArea.Width;
                Position = new PixelPoint((screenWidth - (int)Width) / 2, (screenHeight - (int)Height) / 2);
            }

            _isScalled = !_isScalled;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Environment.Exit(0); // Adapt for Avalonia
        }


        private void Grid_PointerEntered(object? sender, PointerEventArgs e)
        {
            if (sender is Grid grid)
            {
                for (int i = 0; i < grid.ColumnDefinitions.Count; i++)
                {
                    //Runtime.DebugWrite(i.ToString() + " " + grid.ColumnDefinitions[i].ActualWidth.ToString());
                }
            }
        }


        #region Window State Buttons


        private void Close()
        {
            this.Close();
        }

        private void Maximized()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                // Runtime.DebugWrite
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void Minimized()
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseWindow_Click(object? sender, PointerPressedEventArgs e)
        {
            this.Close();
        }

        private void MaximizedWindow_Click(object? sender, PointerPressedEventArgs e)
        {
            Maximized();
        }

        private void MinimizedWindow_Click(object? sender, PointerPressedEventArgs e)
        {
            Minimized();
        }


        #endregion Window State


        private void Button_Click_2(object? sender, PointerPressedEventArgs e)
        {
            if (Application.Current is { Resources: { } resources })
            {
                resources.MergedDictionaries.Clear();

                resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("avares://LexplosionAvalonia/Resources/Fonts.axaml") });
                resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("avares://LexplosionAvalonia/Resources/Icons.axaml") });
                resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("avares://LexplosionAvalonia/Resources/Styles/TextBlock.axaml") });
                resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("avares://LexplosionAvalonia/Resources/Styles/Buttons.axaml") });
                resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("avares://LexplosionAvalonia/Resources/Styles/TextBox.axaml") });
                resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("avares://LexplosionAvalonia/Resources/Styles/CheckBox.axaml") });

                var langUri = currentLang == "ru" ? "avares://LexplosionAvalonia/Assets/langs/ru-RU.axaml" : "avares://LexplosionAvalonia/Assets/langs/en-US.axaml";
                resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(langUri) });
            }
        }


        // TODO : Сделать для подобных махинаций отдельный класс
        /// <summary>
        /// Creates a brush based on the current appearance of a visual element. 
        /// The brush is an ImageBrush and once created, won't update its look
        /// </summary>
        /// <param name="v">The visual element to take a snapshot of</param>
        private IBrush CreateBrushFromVisual(Visual v)
        {
            if (v == null)
                throw new ArgumentNullException(nameof(v));

            // Adapt for Avalonia: Use RenderTargetBitmap
            var pixelSize = new PixelSize((int)this.Width, (int)this.Height);
            var dpi = new Vector(96, 96); // Default DPI
            using var rtb = new RenderTargetBitmap(pixelSize, dpi);
            rtb.Render(v);
            return new ImageBrush(rtb);
        }


        public static bool Contains(IEnumerable collection, string key)
        {
            if (collection == null) return false;

            foreach (var item in collection)
            {
                if (item is string str && str.Contains(key))
                {
                    return true;
                }
            }
            return false;
        }

        private void ChangeTheme_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            var themeService = _appCore.Settings.ThemeService;
            Theme selectedTheme = null;
            if (themeService.SelectedTheme.Name == "Light Punch")
            {
                selectedTheme = themeService.Themes.FirstOrDefault(t => t.Name == "Open Space");
            }
            else
            {
                selectedTheme = themeService.Themes.FirstOrDefault(t => t.Name == "Light Punch");
            }

            themeService.ChangeTheme(selectedTheme);
        }

        private void Dba_Completed(object? sender, EventArgs e, Border border)
        {
            PaintArea.IsVisible = false;
            border.IsEnabled = true;
            // GC.Collect() not recommended
        }

        private void ChangeLanguage_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            var langUri = currentLang == "ru" ? "avares://LexplosionAvalonia/Assets/langs/en-US.axaml" : "avares://LexplosionAvalonia/Assets/langs/ru-RU.axaml";
            currentLang = currentLang == "ru" ? "en" : "ru";
            if (Application.Current is { Resources: { } resources })
            {
                // Remove old lang if needed, add new
                resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(langUri) });
            }
        }

        private void Border_PointerPressed_2(object? sender, PointerPressedEventArgs e)
        {
            ChangeWHPHorizontalOrintationAnimation();
        }


        private void ChangeWHPHorizontalOrintationAnimation()
        {
            //var opacityAdditionalFuncsHideAnimation = new DoubleAnimation()
            //{
            //    Duration = TimeSpan.FromSeconds(0.35 / 2),
            //    To = 0
            //};

            //var opacityHideAnimation = new DoubleAnimation()
            //{
            //    Duration = TimeSpan.FromSeconds(0.35 / 2),
            //    To = 0
            //};

            //var opacityShowAnimation = new DoubleAnimation()
            //{
            //    Duration = TimeSpan.FromSeconds(0.35 / 2),
            //    To = 1
            //};

            //// перемещаем кнопки и панель в нужную сторону.
            //opacityHideAnimation.Completed += (object sender, EventArgs e) =>
            //{
            //    ChangeWHPHorizontalOrintation();
            //    WindowHeaderPanelButtonsGrid.BeginAnimation(OpacityProperty, opacityShowAnimation);
            //    AddtionalFuncs.BeginAnimation(OpacityProperty, opacityShowAnimation);
            //};

            //// скрываем 
            //WindowHeaderPanelButtonsGrid.BeginAnimation(OpacityProperty, opacityHideAnimation);
            //AddtionalFuncs.BeginAnimation(OpacityProperty, opacityAdditionalFuncsHideAnimation);
        }

        private void ChangeWHPHorizontalOrintation()
        {
            //if (WindowHeaderPanelButtonsGrid.HorizontalAlignment == HorizontalAlignment.Left)
            //{
            //    WindowHeaderPanelButtons.RenderTransform = new RotateTransform(180);
            //    WindowHeaderPanelButtonsGrid.HorizontalAlignment = HorizontalAlignment.Right;

            //    AddtionalFuncs.HorizontalAlignment = HorizontalAlignment.Left;

            //    Grid.SetColumn(DebugPanel, 0);
            //    Grid.SetColumn(WindowHeaderPanelButtons, 1);

            //    RuntimeApp.HeaderState = HeaderState.Right;
            //}
            //else
            //{
            //    WindowHeaderPanelButtons.RenderTransform = new RotateTransform(360);
            //    WindowHeaderPanelButtonsGrid.HorizontalAlignment = HorizontalAlignment.Left;

            //    AddtionalFuncs.HorizontalAlignment = HorizontalAlignment.Right;

            //    Grid.SetColumn(DebugPanel, 1);
            //    Grid.SetColumn(WindowHeaderPanelButtons, 0);

            //    RuntimeApp.HeaderState = HeaderState.Left;
            //}
        }

        private void ScaleFit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Scalling();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var grid = (Grid)sender;
            //Runtime.DebugWrite(grid.ActualWidth.ToString() + "x" + grid.ActualHeight.ToString());
        }
    }
}
