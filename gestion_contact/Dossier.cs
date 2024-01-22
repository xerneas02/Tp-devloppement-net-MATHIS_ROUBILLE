using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace gestion_contact
{
    [Serializable] // Attribut indiquant que la classe peut être sérialisée
    [XmlInclude(typeof(Contact))] // Permet à la sérialisation XML d'inclure le type Contact
    public class Dossier : Objet
    {
        // Liste pour stocker les objets (contacts ou sous-dossiers) contenus dans ce dossier
        public List<Objet> Contenus { get; private set; }

        // Constructeur pour la classe Dossier
        public Dossier(string nom) : base(nom)
        {
            Contenus = new List<Objet>();
        }

        // Constructeur par défaut pour la classe Dossier
        public Dossier() : this("Sans Nom")
        {

        }

        // Méthode pour ajouter un objet (contact ou sous-dossier) à ce dossier
        public void AjouterObj(Objet objet) { Contenus.Add(objet); UpdateTime(); }

        // Méthode pour afficher l'objet avec une indentation et une indication s'il est le dernier dans une liste
        public override void Afficher(int niv = 0, bool last = true)
        {
            int i;
            for (i = 0; i < niv - 1; i++) Console.Write(" ");

            if (niv > 0)
            {
                if (!last) Console.Write("├");
                else Console.Write("└");
            }

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(Nom);
            Console.ResetColor();

            if (Contenus.Count > 0)
            {
                for (i = 0; i < Contenus.Count - 1; i++)
                {
                    Contenus[i].Afficher(niv + 1, false);
                }
                Contenus[i].Afficher(niv + 1, true);
            }
        }

        // Méthode statique pour sauvegarder un dossier au format texte (méthode récursive)
        public static string SauvegarderDossier(Dossier dossier, int niveau = 0)
        {
            string indentation = new string(' ', niveau * 2);
            string texte = $"{indentation}Dossier: {dossier.Nom}";

            foreach (Objet objet in dossier.Contenus)
            {
                if (objet is Dossier sousDossier)
                {
                    texte += "\n" + SauvegarderDossier(sousDossier, niveau + 1);
                }
                else if (objet is Contact contact)
                {
                    texte += ($"\n{indentation}Contact: {contact.Nom},{contact.Courriel},{contact.Societe},{contact.Lien}");
                }
            }

            return texte;
        }

        // Méthode pour afficher les informations spécifiques de l'objet
        public override void Infos()
        {
            Console.WriteLine(Nom + " : Creation " + DateCreation + ", Modification " + DateModification);
        }

        // Méthode statique pour charger un dossier à partir d'une chaîne de texte
        public static Dossier? Charger(string texte)
        {
            Dossier? rootDossier = null;
            Dictionary<int, Dossier> dossiersParNiveau = new Dictionary<int, Dossier>();

            try
            {
                string[] lignes = texte.Split("\n");

                foreach (string ligne in lignes)
                {
                    string[] tokens = ligne.Split(':');

                    if (tokens.Length < 2)
                        continue;

                    string typeObjet = tokens[0].Trim();
                    string infos = tokens[1].Trim();

                    int niveau = 0;
                    while (tokens[0].Length > niveau * 2 && tokens[0][niveau * 2] == ' ')
                    {
                        niveau++;
                    }

                    if (typeObjet == "Dossier")
                    {
                        Dossier nouveauDossier = new Dossier(infos);

                        if (niveau == 0)
                        {
                            rootDossier = nouveauDossier;
                        }
                        else
                        {
                            if (dossiersParNiveau.TryGetValue(niveau - 1, out Dossier? parent))
                            {
                                if (parent != null) parent.AjouterObj(nouveauDossier);
                            }
                        }

                        dossiersParNiveau[niveau] = nouveauDossier;
                    }
                    else if (typeObjet == "Contact")
                    {
                        string[] contactInfos = infos.Split(',');
                        Contact nouveauContact = new Contact(contactInfos[0].Trim(), contactInfos[1].Trim(), contactInfos[2].Trim(), contactInfos[3].Trim());

                        if (dossiersParNiveau.TryGetValue(niveau, out Dossier? dossierActuel))
                        {
                            if (dossierActuel != null) dossierActuel.AjouterObj(nouveauContact);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement du fichier : {ex.Message}");
            }

            return rootDossier;
        }
    }
}
