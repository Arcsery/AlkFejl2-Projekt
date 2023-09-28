using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using static System.Net.Mime.MediaTypeNames;

class Vault
{
    public string userId { get; set; }
    public string webUsername { get; set; }
    public string webPassword { get; set; }
    public string webSite { get; set; }

    public static void addPassword(string username, string password, string webUsername, string webPassword, string website)
    {
        byte[] bytesToEncode = Encoding.UTF8.GetBytes(webPassword);
        string b64Passowrd = Convert.ToBase64String(bytesToEncode);

        if (User.Authentication(username, password))
        {
            string id = User.GetUserIdByUsername(username);
            Vault vault = new Vault
            {
                userId = id,
                webUsername = webUsername,
                webPassword = b64Passowrd,
                webSite = website
            };

            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                NewLine = Environment.NewLine
            };

            using (var writer = new StreamWriter("vault.csv", append: true))
            using (var csv = new CsvWriter(writer, csvConfig))
            {
                csv.WriteRecord(vault);
                csv.NextRecord();
            }
            Console.WriteLine("Sikeres jelszó hozzáadás.");
        }
        else
        {
            Console.WriteLine("Sikertelen autentikáció!");
        }
    }
}
