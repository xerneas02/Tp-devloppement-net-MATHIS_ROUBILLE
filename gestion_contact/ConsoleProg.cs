using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace gestion_contact
{
    internal class ConsoleProg
    {
        private string Key = "2b7e151628aed2a6abf7158809cf4f3c"; // Clé privée utilisée pour une opération quelconque

        private ISerializer serializer; // Interface pour la sérialisation et la désérialisation d'un Dossier

        public delegate void Methode(string param); // Définition d'un délégué pour une méthode prenant une chaîne de caractères en paramètre

        public Dictionary<string, Methode> Actions { get; } // Dictionnaire associant des noms d'actions à des méthodes correspondantes

        public Dossier Root { get; private set; } // Dossier racine de la structure hiérarchique

        public List<Dossier> Path { get; private set; } // Liste représentant le chemin actuel dans la structure hiérarchique

        public Dossier Courant { get { return Path[Path.Count - 1]; } } // Dossier actuellement sélectionné (dernier élément du chemin)

        public bool Running { get; private set; } // Indique si le programme est en cours d'exécution



        public ConsoleProg()
        {
            // Initialize the hierarchical structure with a root folder named "/"
            Root = new Dossier("/");

            // Set the initial path to the root folder
            Path = new List<Dossier> { Root };

            // Initialize the dictionary of actions mapped to corresponding methods
            Actions = new Dictionary<string, Methode>();

            // Add available commands with associated methods to the actions dictionary
            Actions.Add("help", help);
            Actions.Add("afficher", afficher);
            Actions.Add("ls", ls);
            Actions.Add("cd", cd);
            Actions.Add("mkdir", mkdir);
            Actions.Add("mkcont", mkcont);
            Actions.Add("rm", rm);
            Actions.Add("infos", infos);
            Actions.Add("rename", rename);
            Actions.Add("save", save);
            Actions.Add("load", load);
            Actions.Add("exit", exit);
        }

        public void Start()
        {
            // Set the program state to running
            Running = true;

            // Generate a key based on the current Windows user identity
            Key = WindowsIdentity.GetCurrent().User.Value.Substring(0, 32);

            // Main program loop
            while (Running)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("$ ");
                Console.ResetColor();

                // Read the user input as a command
                string? command = Console.ReadLine();

                // Check if the command is not null
                if (command == null) continue;

                // Split the command into parameters
                string[] param = command.Split(' ');

                // Check if the command is a valid action
                if (Actions.ContainsKey(param[0]))
                {
                    // Execute the corresponding action method with the parameters
                    Actions[param[0]](string.Join(" ", param.Skip(1)));
                }
            }
        }

        // Permet de sortir du programme
        public void exit(string param = "")
        {
            Running = false;
        }

        // Affiche la liste des commandes ou si le nom d'une commande est donner affiche l'aide de cette commande
        public void help(string param = "")
        {
            if (string.IsNullOrEmpty(param))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                foreach (var item in Actions)
                {
                    Console.WriteLine(item.Key);
                }
                Console.ResetColor();
            }
            else
            {
                string command = param.Trim().ToLower();
                if (Actions.ContainsKey(command))
                {
                    AfficherAideCommande(command);
                }
                else
                {
                    Console.WriteLine($"Erreur: La commande '{command}' n'existe pas.");
                }
            }
        }

        private void AfficherAideCommande(string command)
        {
            switch (command)
            {
                case "afficher":
                    Console.WriteLine("Affiche l'arborescence actuelle.");
                    break;

                case "mkdir":
                    Console.WriteLine("Crée un nouveau dossier dans le dossier courant.");
                    Console.WriteLine("Utilisation: mkdir <nom_du_dossier>");
                    break;

                case "mkcont":
                    Console.WriteLine("Crée un nouveau contact dans le dossier courant.");
                    Console.WriteLine("Utilisation: mkcont <nom_du_contact> [courriel] [societe] [lien]");
                    break;

                case "cd":
                    Console.WriteLine("Change le dossier courant.");
                    Console.WriteLine("Utilisation: cd <chemin_du_dossier>");
                    break;

                case "help":
                    Console.WriteLine("Affiche toutes les commandes disponibles ou des informations sur une commande spécifique.");
                    Console.WriteLine("Utilisation: help [nom_de_la_commande]");
                    break;

                case "exit":
                    Console.WriteLine("Quitte le programme.");
                    break;

                case "ls":
                    Console.WriteLine("Affiche le contenu du dossier courant.");
                    break;

                case "infos":
                    Console.WriteLine("Affiche les informations d'un objet (dossier ou contact) dans le dossier courant.");
                    Console.WriteLine("Utilisation: infos <nom_de_l'objet>");
                    break;

                case "save":
                    Console.WriteLine("Sauvegarde la structure actuelle dans un fichier avec la possibilité de spécifier le format.");
                    Console.WriteLine("Utilisation: save <nom_du_fichier> [format]");
                    Console.WriteLine("Formats disponibles : 'xml' (XML Serialization), 'default' (Default Serialization)");
                    break;

                case "load":
                    Console.WriteLine("Charge une structure à partir d'un fichier.");
                    Console.WriteLine("Utilisation: load <nom_du_fichier>");
                    break;

                case "rm":
                    Console.WriteLine("Supprime un dossier ou un contact du dossier courant.");
                    Console.WriteLine("Utilisation: rm <nom_de_l'objet>");
                    break;

                case "rename":
                    Console.WriteLine("Renomme un dossier ou un contact dans le dossier courant.");
                    Console.WriteLine("Utilisation: rename <ancien_nom> <nouveau_nom>");
                    break;

                default:
                    Console.WriteLine($"Erreur: Aide non disponible pour la commande '{command}'.");
                    break;
            }
        }

        //Affiche l'arborescence dpuis le fichier racine
        public void afficher(string param = "")
        {
            Root.Afficher();
        }

        //Affiche l'arborescence dpuis le fichier courant
        public void ls(string param = "")
        {
            Courant.Afficher();
        }

        //Creer un dossier
        public void mkdir(string nom)
        {
            if (ObjetExisteDeja(nom))
            {
                Console.WriteLine($"Erreur : Un objet avec le nom '{nom}' existe déjà.");
                return;
            }

            Courant.AjouterObj(new Dossier(nom));
        }

        //Creer un contact
        public void mkcont(string param)
        {
            string[] mots = param.Split(' ');

            if (mots.Length >= 1)
            {
                string nom = mots[0];

                if (ObjetExisteDeja(nom))
                {
                    Console.WriteLine($"Erreur : Un objet avec le nom '{nom}' existe déjà.");
                    return;
                }

                string couriel = (mots.Length >= 2) ? mots[1] : "N/A";
                string societe = (mots.Length >= 3) ? mots[2] : "N/A";
                string lien = (mots.Length >= 4) ? mots[3] : "N/A";

                if (IsValidEmail(couriel))
                {
                    mkcont(nom, couriel, societe, lien);
                }
                else
                {
                    Console.WriteLine("Erreur : Format de l'adresse e-mail invalide.");
                }
            }
            else
            {
                Console.WriteLine("La chaîne de paramètres est trop courte.");
            }
        }
        
        public void mkcont(string nom, string couriel = "N/A", string societe = "N/A", string lien = "N/A")
        {
            Courant.AjouterObj(new Contact(nom, couriel, societe, lien));
        }

        //Verifie si un email est valide
        private bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }

        //Verifie si le nom est déjà utiliser dans le repertoire courant
        private bool ObjetExisteDeja(string nom)
        {
            return Courant.Contenus.Any(obj => obj.Nom == nom);
        }

        //Affiche les informations de l'objet demandé
        public void infos(string nom)
        {
            foreach (Objet obj in Courant.Contenus)
            {
                if (obj.Nom == nom)
                {
                    obj.Infos();
                    return;
                }
            }
        }

        //Permet de changer de dossier
        public void cd(string nom)
        {
            string[] parties = nom.Split('/');
            foreach (string partie in parties)
            {
                if (string.IsNullOrEmpty(partie))
                {
                    continue;
                }

                if (partie == ".." && Courant != Root)
                {
                    Path.Remove(Courant);
                }
                else
                {
                    Objet dossier = Courant.Contenus.FirstOrDefault(obj => obj.GetType() == typeof(Dossier) && obj.Nom == partie);

                    if (dossier != null)
                    {
                        Path.Add((Dossier)dossier);
                    }
                    else
                    {
                        Console.WriteLine($"Erreur: Le dossier '{partie}' n'a pas été trouvé.");
                        return;
                    }
                }
            }
        }

        //Sauvegarde l'etat actuel au format demandé
        public void save(string parameters)
        {
            string[] paramsArray = parameters.Split(' ');
            string fileName = paramsArray[0];
            string format = "default";

            switch (paramsArray.Length > 1 ? paramsArray[1].ToLower() : "default")
            {
                case "xml":
                    serializer = new XmlSerializer2();
                    format = "xml";
                    break;

                default:
                    serializer = new DefaultSerializer();
                    break;
            }

            string file = serializer.SerializeRecursive(Root);
            string encryptedData = EncryptStringAES($"{format} {file}", Key);

            File.WriteAllText(fileName, encryptedData);
            Console.WriteLine($"Sauvegarde réussie dans le fichier {fileName}");
        }

        //Charge un etat depuis un fichier
        public void load(string parameters)
        {
            string[] paramsArray = parameters.Split(' ');
            string fileName = paramsArray[0];

            string encryptedData = File.ReadAllText(fileName);
            string decryptedData = DecryptStringAES(encryptedData, Key);

            string[] dataParts = decryptedData.Split(' ');

            if (dataParts.Length >= 2)
            {
                string format = dataParts[0];
                string file = string.Join(' ', dataParts.Skip(1));

                switch (format.ToLower())
                {

                    case "xml":
                        serializer = new XmlSerializer2();
                        break;

                    default:
                        serializer = new DefaultSerializer();
                        break;
                }

                Dossier? loadedRoot = serializer.Deserialize(file);

                if (loadedRoot != null)
                {
                    Root = loadedRoot;
                    Path = new List<Dossier> { Root };
                    Console.WriteLine($"Chargement réussi depuis le fichier {fileName}");
                }
                else
                {
                    Console.WriteLine($"Erreur lors du chargement du fichier {fileName}");
                }
            }
            else
            {
                Console.WriteLine($"Erreur: Le fichier {fileName} ne contient pas d'information de format.");
            }
        }

        //Crypte une chaine de caractère à partir d'une clé    
        private string EncryptStringAES(string plainText, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[16]; // Utilisez un vecteur d'initialisation unique (IV)

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }

                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        //Decrypte une chaine de caractère à partir d'une clé
        private string DecryptStringAES(string cipherText, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[16]; // Utilisez le même IV que celui utilisé pour le chiffrement

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        //Supprime le fichier voulu
        public void rm(string param)
        {
            string[] mots = param.Split(' ');

            if (mots.Length == 1)
            {
                string nom = mots[0];

                Objet objToDelete = Courant.Contenus.FirstOrDefault(obj => obj.Nom == nom);

                if (objToDelete != null)
                {
                    if (objToDelete is Dossier)
                    {
                        if (ConfirmationSuppressionDossier((Dossier)objToDelete))
                        {
                            Courant.Contenus.Remove(objToDelete);
                            Console.WriteLine($"Dossier '{nom}' supprimé avec succès.");
                        }
                        else
                        {
                            Console.WriteLine($"Suppression du dossier '{nom}' annulée.");
                        }
                    }
                    else if (objToDelete is Contact)
                    {
                        Courant.Contenus.Remove(objToDelete);
                        Console.WriteLine($"Contact '{nom}' supprimé avec succès.");
                    }
                }
                else
                {
                    Console.WriteLine($"Erreur: L'objet '{nom}' n'a pas été trouvé.");
                }
            }
            else
            {
                Console.WriteLine("La commande 'rm' prend un seul argument (nom du dossier ou du contact).");
            }
        }

        //Renome le fichier voulu
        public void rename(string parameters)
        {
            string[] paramsArray = parameters.Split(' ');

            if (paramsArray.Length == 2)
            {
                string oldName = paramsArray[0];
                string newName = paramsArray[1];

                if (!ObjetExisteDeja(newName))
                {
                    Objet objToRename = Courant.Contenus.FirstOrDefault(obj => obj.Nom == oldName);

                    if (objToRename != null)
                    {
                        objToRename.Nom = newName;
                        Console.WriteLine($"Renommage réussi. '{oldName}' est maintenant '{newName}'.");
                    }
                    else
                    {
                        Console.WriteLine($"Erreur: L'objet '{oldName}' n'a pas été trouvé.");
                    }
                }
                else
                {
                    Console.WriteLine($"Erreur: Le nom '{newName}' est déjà pris.");
                }
            }
            else
            {
                Console.WriteLine("La commande 'rename' prend deux arguments (ancien_nom et nouveau_nom).");
            }
        }

        private bool ConfirmationSuppressionDossier(Dossier dossier)
        {
            Console.WriteLine($"Attention: La suppression du dossier '{dossier.Nom}' entraînera la suppression de tout son contenu.");
            Console.Write("Voulez-vous continuer? (O/N): ");
            string confirmation = Console.ReadLine()?.Trim().ToLower();

            return confirmation == "o" || confirmation == "oui";
        }
    }
}
