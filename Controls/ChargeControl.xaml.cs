using System.Windows;
using System.Windows.Controls;


namespace CANSAT_UI.Controls;

/// <summary>
/// Interaction logic for ChargeControl.xaml
/// </summary>
public partial class ChargeControl : UserControl
{
    public static readonly DependencyProperty IsTriggeredProperty =
    DependencyProperty.Register("IsTriggered", typeof(bool), typeof(ChargeControl), new PropertyMetadata(false, OnDependencyPropertyChanged));

    public bool IsTriggered
    {
        get => (bool)GetValue(IsTriggeredProperty);
        set => SetValue(IsTriggeredProperty, value);
    }

    private static void OnDependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var chargeControl = (ChargeControl)d;
    }

    public ChargeControl()
    {
        InitializeComponent();
    }
}
