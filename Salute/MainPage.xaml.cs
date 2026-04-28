// C:\th1eros\Appletand\salute\MainPage.xaml.cs
namespace Salute;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnSobreClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SobrePage());
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Console.WriteLine("Página apareceu");
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Console.WriteLine("Página sumiu");
    }
}