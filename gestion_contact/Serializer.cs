using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace gestion_contact
{
    // Interface pour la sérialisation et la désérialisation d'un Dossier
    public interface ISerializer
    {
        // Méthode pour sérialiser un Dossier de manière récursive
        string SerializeRecursive(Dossier root);

        // Méthode pour désérialiser une chaîne de caractères en un Dossier
        Dossier? Deserialize(string serializedData);
    }

    // Implémentation de l'interface ISerializer pour la sérialisation en XML
    public class XmlSerializer2 : ISerializer
    {
        // Méthode pour sérialiser un Dossier en format XML
        public string SerializeRecursive(Dossier root)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Dossier));
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, root);
                return writer.ToString();
            }
        }

        // Méthode pour désérialiser une chaîne XML en un Dossier
        public Dossier? Deserialize(string serializedData)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Dossier));
            using (StringReader reader = new StringReader(serializedData))
            {
                try
                {
                    return (Dossier?)serializer.Deserialize(reader);
                }
                catch
                {
                    // En cas d'erreur lors de la désérialisation XML
                    Console.WriteLine("Erreur lors de la désérialisation XML.");
                    return null;
                }
            }
        }
    }

    // Implémentation de l'interface ISerializer par défaut
    public class DefaultSerializer : ISerializer
    {
        // Méthode pour sérialiser un Dossier de manière récursive
        public string SerializeRecursive(Dossier root)
        {
            // Utilisation d'une méthode de la classe Dossier pour sauvegarder le Dossier en format personnalisé
            return Dossier.SauvegarderDossier(root, 0);
        }

        // Méthode pour désérialiser une chaîne en un Dossier
        public Dossier? Deserialize(string serializedData)
        {
            // Utilisation d'une méthode de la classe Dossier pour charger un Dossier à partir de la chaîne
            return Dossier.Charger(serializedData);
        }
    }
}
