using System;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Nincs megadva parancs.");
            return;
        }

        string command = args[0];
        Guid guid = Guid.NewGuid();
        string username = null;
        string password = null;

        string webUsername = null;
        string webPassword = null;
        string website = null;

        switch (command)
        {
            case "register":
                for (int i = 1; i < args.Length; i++)
                {
                    string arg = args[i];
                    if (arg.StartsWith("--username="))
                    {
                        // Kivágjuk az egyenlőség utáni részt
                        username = arg.Substring("--username=".Length);
                    }
                    else if (arg.StartsWith("--password="))
                    {
                        // Kivágjuk az egyenlőség utáni részt
                        password = arg.Substring("--password=".Length);
                    }
                }

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    Console.WriteLine("Hiányzó felhasználónév vagy jelszó.");
                    return;
                }

                // Meghívjuk a RegisterUser metódust a kapott adatokkal
                User.RegisterUser(guid.ToString(), username, password);
                break;

            case "addPassword":
                for (int i = 1; i < args.Length; i++)
                {
                    string arg = args[i];
                    if (arg.StartsWith("--username="))
                    {
                        // Kivágjuk az egyenlőség utáni részt
                        username = arg.Substring("--username=".Length);
                    }
                    else if (arg.StartsWith("--password="))
                    {
                        // Kivágjuk az egyenlőség utáni részt
                        password = arg.Substring("--password=".Length);
                    }
                    else if (arg.StartsWith("--webUsername="))
                    {
                        webUsername = arg.Substring("--webUsername=".Length);
                    }
                    else if (arg.StartsWith("--webPassword="))
                    {
                        webPassword = arg.Substring("--webPassword=".Length);

                    }
                    else if (arg.StartsWith("--website="))
                    {
                        website = arg.Substring("--website=".Length);

                    }
                }
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(webUsername)
                    || string.IsNullOrEmpty(webPassword) || string.IsNullOrEmpty(website))
                {
                    Console.WriteLine("Hiányzó paraméterek.");
                    Console.WriteLine(website);
                    return;
                }
                Vault.addPassword(username, password, webUsername, webPassword, website);
                break;

            case "list":
                for (int i = 1; i < args.Length; i++)
                {
                    string arg = args[i];
                    if (arg.StartsWith("--username="))
                    {
                        // Kivágjuk az egyenlőség utáni részt
                        username = arg.Substring("--username=".Length);
                    }
                    else if (arg.StartsWith("--password="))
                    {
                        // Kivágjuk az egyenlőség utáni részt
                        password = arg.Substring("--password=".Length);
                    }
                }

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    Console.WriteLine("Hiányzó felhasználónév vagy jelszó.");
                    return;
                }

                if (User.Authentication(username, password))
                {
                    if (File.Exists("vault.csv"))
                    {
                        User.ListPasswords(username);
                    }
                    else
                    {
                        Console.WriteLine("Nem létezik még a vault.csv");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Sikertelen autentikáció!");
                    return;
                }
                break;

            default:
                Console.WriteLine($"Ismeretlen parancs: {command}");
                break;
        }
    }
}
