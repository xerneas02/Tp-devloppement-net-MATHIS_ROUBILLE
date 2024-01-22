using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gestion_contact
{
    // Classe abstraite de base pour d'autres objets
    public abstract class Objet
    {
        private string _nom;

        // Propriété pour le nom de l'objet avec un mécanisme pour mettre à jour la date de modification
        public string Nom
        {
            get { return _nom; }
            set { UpdateTime(); _nom = value; }
        }

        // Propriété pour la date de création de l'objet
        public DateTime DateCreation { get; set; }

        // Propriété pour la date de dernière modification de l'objet
        public DateTime DateModification { get; set; }

        // Méthode protégée pour mettre à jour la date de modification
        protected void UpdateTime()
        {
            DateModification = DateTime.Now;
        }

        // Constructeur de la classe Objet
        public Objet(string nom)
        {
            Nom = nom;
            DateCreation = DateTime.Now;
            DateModification = DateCreation;
        }

        // Méthode abstraite pour afficher l'objet avec une indentation et une indication s'il est le dernier dans une liste
        public abstract void Afficher(int niv = 0, bool last = true);

        // Méthode abstraite pour afficher les informations spécifiques de l'objet
        public abstract void Infos();
    }
}
