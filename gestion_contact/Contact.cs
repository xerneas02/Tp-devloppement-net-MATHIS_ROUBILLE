using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gestion_contact
{
    [Serializable] // Attribut indiquant que la classe peut être sérialisée
    public class Contact : Objet
    {
        // Propriétés spécifiques à la classe Contact
        private string _courriel;
        private string _societe;
        private string _lien;

        // Propriété pour le courriel avec un mécanisme pour mettre à jour la date de modification
        public string Courriel
        {
            get { return _courriel; }
            set { _courriel = value; UpdateTime(); }
        }

        // Propriété pour la société avec un mécanisme pour mettre à jour la date de modification
        public string Societe
        {
            get { return _societe; }
            set { _societe = value; UpdateTime(); }
        }

        // Propriété pour le lien avec un mécanisme pour mettre à jour la date de modification
        public string Lien
        {
            get { return _lien; }
            set { _lien = value; UpdateTime(); }
        }

        // Constructeur pour la classe Contact
        public Contact(string nom, string courriel, string societe, string lien) : base(nom)
        {
            Courriel = courriel;
            Societe = societe;
            Lien = lien;
        }

        // Constructeur par défaut pour la classe Contact
        public Contact() : this("Sans nom", "N/A", "N/A", "N/A")
        { }

        // Méthode pour afficher l'objet avec une indentation et une indication s'il est le dernier dans une liste
        public override void Afficher(int niv = 0, bool last = true)
        {
            for (int i = 1; i < niv; i++) Console.Write(" ");

            if (!last) Console.Write("├");
            else Console.Write("└");

            Console.WriteLine(Nom);
        }

        // Méthode pour afficher les informations spécifiques de l'objet
        public override void Infos()
        {
            Console.WriteLine(Nom + " : Creation " + DateCreation + ", Modification " + DateModification);
            Console.WriteLine(" Courriel " + Courriel + " Societe " + Societe + " Lien " + Lien);
        }
    }
}
