using BankData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;

namespace BankData.Controllers
{
    public class BankDataController : Controller
    {
        private readonly ILogger<BankDataController> _logger;

        public BankDataController(ILogger<BankDataController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string connectionString = "Server=SEDANUR\\SQLEXPRESS;Database=BankData;Trusted_Connection=True;";
            List<Banka> bankData = new List<Banka>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Tarih, Saat, Açılış, Yüksek, Düşük, Kapanış, Ortalama, Hacim, Lot FROM dbo.Banka";
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    _logger.LogInformation("Veritabanı bağlantısı açıldı.");

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        _logger.LogInformation("Sorgu başarıyla çalıştırıldı.");

                        if (!reader.HasRows)
                        {
                            _logger.LogWarning("Sorgu sonuç döndürmedi.");
                            return Content("Sorgu sonuç döndürmedi.");
                        }

                        while (reader.Read())
                        {
                            _logger.LogInformation("Veri okundu.");

                            Banka banka = new Banka
                            {
                                Tarih = reader.GetString(0),
                                Saat = reader.GetDateTime(1),
                                Açılış = Convert.ToSingle(reader.GetDouble(2)), // double'dan float'a dönüştürme
                                Yüksek = Convert.ToSingle(reader.GetDouble(3)),  // double'dan float'a dönüştürme
                                Düşük = Convert.ToSingle(reader.GetDouble(4)),   // double'dan float'a dönüştürme
                                Kapanış = Convert.ToSingle(reader.GetDouble(5)), // double'dan float'a dönüştürme
                                Ortalama = Convert.ToSingle(reader.GetDouble(6)), // double'dan float'a dönüştürme
                                Hacim = reader.GetDouble(7), // double olarak okundu
                                Lot = reader.GetDouble(8)  // long olarak okundu
                            };
                            bankData.Add(banka);
                        }
                    }
                }
                catch (SqlException sqlEx)
                {
                    _logger.LogError(sqlEx, "SQL Veritabanı hatası: {Message}", sqlEx.Message);
                    return Content("Veritabanı hatası: " + sqlEx.Message); // Hata mesajını görünümde göster
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Genel hata: {Message}", ex.Message);
                    return Content("Genel hata: " + ex.Message); // Hata mesajını görünümde göster
                }
            }

            if (bankData == null || bankData.Count == 0)
            {
                _logger.LogWarning("Veri bulunamadı.");
                return Content("Veri bulunamadı.");
            }

            return View(bankData);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
