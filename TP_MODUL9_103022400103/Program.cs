using System;
using System.IO;
using System.Text.Json;

using System;
using System.IO;
using System.Text.Json;

public class CovidConfig
{
    public string satuan_suhu { get; set; }
    public int batas_hari_deman { get; set; }
    public string pesan_ditolak { get; set; }
    public string pesan_diterima { get; set; }

    private const string filePath = "covid_config.json";

    // ❌ JANGAN panggil LoadConfig di constructor
    public CovidConfig() { }

    public static CovidConfig Load()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<CovidConfig>(json);
        }
        else
        {
            var config = new CovidConfig
            {
                satuan_suhu = "celcius",
                batas_hari_deman = 14,
                pesan_ditolak = "Anda tidak diperbolehkan masuk ke dalam gedung ini",
                pesan_diterima = "Anda dipersilahkan untuk masuk ke dalam gedung ini"
            };

            config.Save();
            return config;
        }
    }

    public void Save()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(this, options);
        File.WriteAllText(filePath, json);
    }

    public void UbahSatuan()
    {
        satuan_suhu = (satuan_suhu == "celcius") ? "fahrenheit" : "celcius";
        Save();
    }
}

class Program
{
    static void Main()
    {
        CovidConfig config = CovidConfig.Load();

        Console.Write($"Berapa suhu badan anda saat ini? Dalam nilai {config.satuan_suhu}: ");
        double suhu = Convert.ToDouble(Console.ReadLine());

        Console.Write("Berapa hari yang lalu anda terakhir demam? ");
        int hari = Convert.ToInt32(Console.ReadLine());

        bool suhuValid = config.satuan_suhu == "celcius"
            ? (suhu >= 36 && suhu <= 37)
            : (suhu >= 97.7 && suhu <= 99.5);

        if (suhuValid && hari < config.batas_hari_deman)
            Console.WriteLine(config.pesan_diterima);
        else
            Console.WriteLine(config.pesan_ditolak);

        config.UbahSatuan();
        Console.WriteLine($"Satuan sekarang: {config.satuan_suhu}");


    }
}