namespace convertitore
{
    public class Valuta
    {
        public string Country { get; set; }
        public string Currency { get; set; }
        public string IsoA3Code { get; set; }
        public string IsoA2Code { get; set; }
        public double Value { get; set; }
        public string Comment { get; set; }

        public Valuta()
        {
            Country = "";
            Currency = "";
            IsoA3Code = "";
            IsoA2Code = "";
            Value = 0;
            Comment = "";
        }

        public Valuta(string Country, string Currency, string IsoA3Code, string IsoA2Code, double Value, string Comment) : this()
        {
            this.Country = Country;
            this.Currency = Currency;
            this.IsoA3Code = IsoA3Code;
            this.IsoA2Code = IsoA2Code;
            this.Value = Value;
            this.Comment = Comment;
        }

        public override string ToString()
        {
            return Currency + " (" + IsoA3Code + ") - " + Value;
        }
    }
}
