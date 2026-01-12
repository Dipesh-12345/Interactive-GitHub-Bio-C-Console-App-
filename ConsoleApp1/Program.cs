using System;
using System.IO;
using System.Linq;       
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GitHubBio
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Bio bio = new Bio();
            await bio.Run();
        }
    }

    class Bio
    {
        // Data for the bio (now loaded from/saved to JSON for persistence)
        private string[] skills;
        private string[] interests;
        private string[] learning;
        private string githubUrl;
        private string facebookUrl;
        private string tryhackmeUrl;
        private const string DataFile = "bio_data.json";

        public Bio()
        {
            LoadData();
        }

        public async Task Run()
        {
            while (true)
            {
                Console.Clear();
                await Display();
                Console.WriteLine("\nOptions:");
                Console.WriteLine("1. Edit Skills");
                Console.WriteLine("2. Edit Interests");
                Console.WriteLine("3. Edit Learning");
                Console.WriteLine("4. Edit Contacts");
                Console.WriteLine("5. Save and Exit");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        EditArray(ref skills, "Skills");
                        break;
                    case "2":
                        EditArray(ref interests, "Interests");
                        break;
                    case "3":
                        EditArray(ref learning, "Learning");
                        break;
                    case "4":
                        EditContacts();
                        break;
                    case "5":
                        SaveData();
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private async Task Display()
        {
            await PrintBanner();
            await PrintSection("Skills & Expertise:", skills, ConsoleColor.Yellow);
            await PrintSection("Interests:", interests, ConsoleColor.Green);
            await PrintSection("Currently Learning:", learning, ConsoleColor.Magenta);
            PrintContact();
            PrintFooter();
        }

        private async Task PrintBanner()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            // Custom ASCII art for "Dipesh" with a fun twist (insired by "Johnny Johnny Yes Papa" meme style)
            string banner = @"
           Jonny Jonny Yes Papa eating sugar no papa !! 
My name is Dipesh. Full time free no job but try to became .........
Iam a ......................
            ";
            await TypewriterEffect(banner, 100);
            Console.ForegroundColor = ConsoleColor.Red;
            string nameBanner = @"
   _____  _     _     _    
Welcome to my Bio section where you can see my profile 
  |  
";
            await TypewriterEffect(nameBanner, 20);
            Console.ResetColor();
        }

        private async Task PrintSection(string title, string[] items, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"\n{title}");
            Console.ResetColor();
            foreach (var item in items)
            {
                Console.WriteLine($"- {item}");
                await Task.Delay(100); // Async delay for smooth effect
            }
        }

        private void PrintContact()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n📫 Connect with me:");
            Console.ResetColor();
            Console.WriteLine($"- GitHub: {githubUrl}");
            Console.WriteLine($"- Facebook: {facebookUrl}");
            Console.WriteLine($"- Tryhackme: {tryhackmeUrl}");
        }

        private void PrintFooter()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n=======================");
            Console.ResetColor();
        }

        private async Task TypewriterEffect(string text, int delayMs)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                await Task.Delay(delayMs);
            }
        }

        private void EditArray(ref string[] array, string name)
        {
            Console.WriteLine($"\nCurrent {name}:");
            for (int i = 0; i < array.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {array[i]}");
            }
            Console.Write("Enter new items separated by commas (or press Enter to keep current): ");
            string input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
            {
                array = input.Split(',').Select(s => s.Trim()).ToArray();
            }
        }

        private void EditContacts()
        {
            Console.Write($"GitHub URL ({githubUrl}): ");
            string input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input)) githubUrl = input;

            Console.Write($"Facebook URL ({facebookUrl}): ");
            input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input)) facebookUrl = input;

            Console.Write($"Tryhackme URL ({tryhackmeUrl}): ");
            input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input)) tryhackmeUrl = input;
        }

        private void LoadData()
        {
            if (File.Exists(DataFile))
            {
                string json = File.ReadAllText(DataFile);
                var data = JsonConvert.DeserializeObject<BioData>(json);
                skills = data.Skills;
                interests = data.Interests;
                learning = data.Learning;
                githubUrl = data.GitHubUrl;
                facebookUrl = data.FacebookUrl;
                tryhackmeUrl = data.TryhackmeUrl;
            }
            else
            {
                // Default data
                skills = new[] { "C", "C#", "Python", "JavaScript", "Operating System & Linux Administration", "Networking", "Ethical Hacking & Cybersecurity" };
                interests = new[] {
                    "Developing secure and efficient applications",
                    "Linux automation scripts",
                    "Exploring network security",
                    "Ethical hacking techniques",
                    "Interest in Learning new technology"
                };
                learning = new[] {
                    "Advanced C# patterns",
                    "Network security & penetration testing",
                    "Cloud integration and DevOps"
                };
                githubUrl = "https://github.com/Dipesh-12345";
                facebookUrl = "https://www.facebook.com/james.rubby.464720";
                tryhackmeUrl = "https://tryhackme.com/p/Toraxa";
            }
        }

        private void SaveData()
        {
            var data = new BioData
            {
                Skills = skills,
                Interests = interests,
                Learning = learning,
                GitHubUrl = githubUrl,
                FacebookUrl = facebookUrl,
                TryhackmeUrl = tryhackmeUrl
            };
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(DataFile, json);
            Console.WriteLine("Data saved!");
        }
    }

    class BioData
    {
        public string[] Skills { get; set; }
        public string[] Interests { get; set; }
        public string[] Learning { get; set; }
        public string GitHubUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string TryhackmeUrl { get; set; }
    }
}