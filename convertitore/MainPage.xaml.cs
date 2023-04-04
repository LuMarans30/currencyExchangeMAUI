using System.Text.RegularExpressions;
using System.Globalization;
using System.Text.Json;
using System.Runtime.CompilerServices;

namespace convertitore;

public partial class MainPage : ContentPage
{ 
    public List<Valuta> Valute { get; set; }
    private double sorgente;

    public MainPage()
    {
        InitializeComponent();
        Valute = new();
        GetJson();
    }

    private void GetJson()
    {
        string json = new HttpClient().GetStringAsync("https://ec.europa.eu/budg/inforeuro/api/public/monthly-rates").Result;

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        Valute = JsonSerializer.Deserialize<List<Valuta>>(json, options);

        pickerSorg.ItemsSource = Valute;
        pickerSorg.SelectedIndex = 0;
        pickerDest.ItemsSource = Valute;
        //Search index in Valute by isoA3Code (e.g. USD)
        pickerDest.SelectedIndex = Valute.FindIndex(x => x.IsoA3Code == "USD");
    }

    private string Calcola(double input)
    {
        double output = input;

        Valuta sorg = (Valuta)pickerSorg.SelectedItem;

        Valuta dest = (Valuta)pickerDest.SelectedItem;

        if (sorg != null && dest != null)
            output = input * dest.Value / sorg.Value;

        return output.ToString("0.###");
    }

    private void valutaSorg_TextChanged(object sender, TextChangedEventArgs e)
    {
        string sorg = e.NewTextValue;

        sorg = FixNum(sorg);

        valutaSorg.Text = sorg;

        NumberFormatInfo numberFormatInfo = new()
        {
            NumberDecimalSeparator = "."
        };

        double input = Convert.ToDouble(sorg, numberFormatInfo);
        sorgente = input;

        valutaDest.Text = Calcola(input);
    }

    private static string FixNum(string s)
    {
        if (string.IsNullOrEmpty(s))
            return "0";

        s = s.Replace(",", ".");

        if (s.Count(f => f == '.') > 1)
            return "0";

        if (s.First() == '.') 
            s = $"0{s}";

        s = Regex.Replace(s, "[^0-9.]", "");

        return s;
    }

    private void pickerSorg_SelectedIndexChanged(object sender, EventArgs e)
    {
        valutaDest.Text = Calcola(sorgente);
    }

    private void pickerDest_SelectedIndexChanged(object sender, EventArgs e)
    {
        valutaDest.Text = Calcola(sorgente);
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        (pickerSorg.SelectedIndex, pickerDest.SelectedIndex) = (pickerDest.SelectedIndex, pickerSorg.SelectedIndex);
    }
}

