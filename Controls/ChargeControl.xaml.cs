using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf.Ui.Controls;


namespace CANSAT_UI.Controls;

/// <summary>
/// Interaction logic for ChargeControl.xaml
/// </summary>
public partial class ChargeControl : UserControl
{
    public static readonly DependencyProperty IsTriggeredProperty =
            DependencyProperty.Register("IsTriggered", typeof(bool), typeof(ChargeControl), new PropertyMetadata(false, OnStateChanged));

    public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(ChargeControl), new PropertyMetadata(""));

    public static readonly DependencyProperty ChangeStateCommandProperty =
            DependencyProperty.Register("ChangeStateCommand", typeof(ICommand), typeof(ChargeControl));

    public bool IsTriggered
    {
        get => (bool)GetValue(IsTriggeredProperty);
        set => SetValue(IsTriggeredProperty, value);
    }

    public ICommand ChangeStateCommand
    {
        get => (ICommand)GetValue(ChangeStateCommandProperty);
        set => SetValue(ChangeStateCommandProperty, value);
    }

    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public ChargeControl()
    {
        InitializeComponent();

        label.Text = IsTriggered ? "Ligado" : "Desligado";
        symbolIcon.Symbol = IsTriggered ? SymbolRegular.LightbulbFilament24 : SymbolRegular.Lightbulb24;
        switchControl.IsChecked = IsTriggered;
    }

    private static void OnStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var chargeControl = (ChargeControl)d;

        chargeControl.switchControl.IsChecked = chargeControl.IsTriggered;
        chargeControl.label.Text = chargeControl.IsTriggered ? "Ligado" : "Desligado";
        chargeControl.symbolIcon.Symbol = chargeControl.IsTriggered ? SymbolRegular.LightbulbFilament24 : SymbolRegular.Lightbulb24;
    }

    private void switchControl_Checked(object sender, RoutedEventArgs e)
    {
        ToggleSwitch toggleSwitch = (ToggleSwitch)sender;
        IsTriggered = (bool)toggleSwitch.IsChecked!;
        label.Text = IsTriggered ? "Ligado" : "Desligado";
        symbolIcon.Symbol = IsTriggered ? SymbolRegular.LightbulbFilament24 : SymbolRegular.Lightbulb24;

        ExecuteCommand();
    }

    private void ExecuteCommand()
    {
        if (ChangeStateCommand != null && ChangeStateCommand.CanExecute(this))
            ChangeStateCommand.Execute(this);
    }
}
