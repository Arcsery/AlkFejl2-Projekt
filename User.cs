using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using System.Security.Cryptography;
using System.Text;
class User
{
    public string Guid { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public static void RegisterUser(string guid, string username, string password)

    {
        string hashedPassword = HashPassword(password);

        if (File.Exists("users.csv"))
        {
            var existingUsers = ReadUsersFromCsv();
            if (existingUsers.Exists(existingUsername => existingUsername == username))
            {
                Console.WriteLine("A felhasználónév már létezik. Kérlek válassz másik felhasználónevet.");
                return;
            }
        }

        User user = new User
        {
            Guid = guid,
            Username = username,
            Password = hashedPassword
        };

        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false, // Ne írjon fejlécet
            NewLine = Environment.NewLine // Az új sor karakter legyen a rendszerhez illeszkedő
        };

        using (var writer = new StreamWriter("users.csv", append: true))
        using (var csv = new CsvWriter(writer, csvConfig))
        {
            csv.WriteRecord(user);
            csv.NextRecord(); // Ugrás a következő sorba
        }

        Console.WriteLine("Sikeres regisztráció.");
    }

    private static List<string> ReadUsersFromCsv()
    {
        var users = new List<string>();

        using (var reader = new StreamReader("users.csv"))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                // A CSV fájlban csak az első vessző utáni adatot olvassuk be (a felhasználónevet)
                string[] parts = line.Split(',');
                if (parts.Length > 0)
                {
                    users.Add(parts[1]);
                }
            }
        }

        return users;
    }

    public static string GetUserIdByUsername(string username)
    {
        if (File.Exists("users.csv"))
        {
            string[] lines = File.ReadAllLines("users.csv");

            foreach (var line in lines)
            {
                string[] parts = line.Split(',');
                if (parts[1] == username)
                {
                    return parts[0];
                }
            }
        }

        return null; // Ha nincs ilyen felhasználó, visszaadunk null-t
    }

    public static bool Authentication(string username, string password)
    {
        string hashedPassowrd = HashPassword(password);
        if (File.Exists("users.csv"))
        {
            string[] lines = File.ReadAllLines("users.csv");

            foreach (var line in lines)
            {
                string[] parts = line.Split(',');
                if (parts[1] == username && parts[2] == hashedPassowrd) ;
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static void ListPasswords(string username) {
        string id = User.GetUserIdByUsername(username);
        Console.WriteLine("| Felhasználó név | Jelszó | Weboldal |");
        Console.WriteLine("----------------------------------------");

        using (var reader = new StreamReader("vault.csv"))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                // A CSV fájlban csak az első vessző utáni adatot olvassuk be (a felhasználónevet)
                string[] parts = line.Split(',');
                if (parts[0] == id)
                {
                    byte[] decodedBytes = Convert.FromBase64String(parts[2]);
                    string decodedPassword = Encoding.UTF8.GetString(decodedBytes);
                    Console.WriteLine(parts[1] + " " + decodedPassword + " " + parts[3]);
                }
            }
        }
    }

    static string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] jelszoBytes = Encoding.UTF8.GetBytes(password);

            // Hasheljük a byte tömböt
            byte[] hasheltBytes = sha256.ComputeHash(jelszoBytes);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hasheltBytes.Length; i++)
            {
                builder.Append(hasheltBytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
