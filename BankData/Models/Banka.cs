namespace BankData.Models
{
    public class Banka
    {

        public string? Tarih { get; set; }
        public DateTime Saat { get; set; }
        public float Açılış { get; set; }
        public float Yüksek { get; set; }
        public float Düşük { get; set; }
        public float Kapanış { get; set; }
        public float Ortalama { get; set; }
        public double Hacim { get; set; }
        public double Lot { get; set; }
    }
}
