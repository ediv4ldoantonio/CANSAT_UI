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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CANSAT_UI.Controls
{
    /// <summary>
    /// Interação lógica para TrashLevelControl.xam
    /// </summary>
    public partial class TrashLevelControl : UserControl
    {
        // Dependency property for TrashLevel
        public static readonly DependencyProperty TrashLevelProperty =
            DependencyProperty.Register("TrashLevel", typeof(double), typeof(TrashLevelControl),
                new PropertyMetadata(0.0, OnTrashLevelChanged));

        // Dependency property for AnimationDuration
        public static readonly DependencyProperty AnimationDurationProperty =
            DependencyProperty.Register("AnimationDuration", typeof(TimeSpan), typeof(TrashLevelControl),
                new PropertyMetadata(TimeSpan.FromMilliseconds(500)));

        public TrashLevelControl()
        {
            InitializeComponent();
            UpdateDisplay();
        }

        /// <summary>
        /// Gets or sets the trash level (0-100)
        /// </summary>
        public double TrashLevel
        {
            get { return (double)GetValue(TrashLevelProperty); }
            set { SetValue(TrashLevelProperty, Math.Max(0, Math.Min(100, value))); }
        }

        /// <summary>
        /// Gets or sets the animation duration for level changes
        /// </summary>
        public TimeSpan AnimationDuration
        {
            get { return (TimeSpan)GetValue(AnimationDurationProperty); }
            set { SetValue(AnimationDurationProperty, value); }
        }

        private static void OnTrashLevelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TrashLevelControl;
            control?.UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (FillLevel == null || PercentageText == null) return;

            var level = TrashLevel;
            var fillHeight = (level / 100.0) * 80; // 80 is the max height of the fill area

            // Animate the fill level
            var heightAnimation = new DoubleAnimation
            {
                To = fillHeight,
                Duration = AnimationDuration,
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            FillLevel.BeginAnimation(FrameworkElement.HeightProperty, heightAnimation);

            // Update percentage text
            PercentageText.Text = $"{level:F0}%";

            // Change color based on fill level
            var fillColor = GetFillColor(level);
            var colorAnimation = new ColorAnimation
            {
                To = fillColor,
                Duration = AnimationDuration
            };

            var brush = FillLevel.Fill as SolidColorBrush ?? new SolidColorBrush();
            brush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
            FillLevel.Fill = brush;

            // Update text color for better visibility when trash is full
            PercentageText.Foreground = level > 80 ? Brushes.White : Brushes.Black;
        }

        private Color GetFillColor(double level)
        {
            if (level <= 25)
                return Color.FromRgb(76, 175, 80);   // Green
            else if (level <= 50)
                return Color.FromRgb(255, 193, 7);   // Yellow
            else if (level <= 75)
                return Color.FromRgb(255, 152, 0);   // Orange
            else
                return Color.FromRgb(244, 67, 54);   // Red
        }

        /// <summary>
        /// Sets the trash level with optional custom animation duration
        /// </summary>
        /// <param name="level">Level from 0 to 100</param>
        /// <param name="animationDuration">Optional custom animation duration</param>
        public void SetLevel(double level, TimeSpan? animationDuration = null)
        {
            if (animationDuration.HasValue)
            {
                var originalDuration = AnimationDuration;
                AnimationDuration = animationDuration.Value;
                TrashLevel = level;
                AnimationDuration = originalDuration;
            }
            else
            {
                TrashLevel = level;
            }
        }

        /// <summary>
        /// Gets the current status description based on the trash level
        /// </summary>
        public string GetStatusDescription()
        {
            var level = TrashLevel;
            if (level <= 25)
                return "Vazio";
            else if (level <= 50)
                return "Quase no meio";
            else if (level <= 75)
                return "Meio Cheio";
            else if (level <= 90)
                return "Quase Cheio";
            else
                return "Cheio";
        }
    }
}
