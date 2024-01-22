**Introduction:**

Cette application permet de créer, manipuler, et sauvegarder une structure hiérarchique de dossiers et de contacts. Utilisez la commande help pour obtenir des informations sur les commandes disponibles.

**Commandes Disponibles:**

help - Affiche toutes les commandes disponibles ou fournit des informations détaillées sur une commande spécifique.

Utilisation: help [nom_de_la_commande]
afficher - Affiche l'arborescence depuis le dossier racine.

Utilisation: afficher
ls - Affiche le contenu du dossier courant.

Utilisation: ls
cd - Change le dossier courant.

Utilisation: cd <chemin_du_dossier>
mkdir - Crée un nouveau dossier dans le dossier courant.

Utilisation: mkdir <nom_du_dossier>
mkcont - Crée un nouveau contact dans le dossier courant.

Utilisation: mkcont <nom_du_contact> [courriel] [societe] [lien]
rm - Supprime un dossier ou un contact du dossier courant.

Utilisation: rm <nom_de_l'objet>
infos - Affiche les informations d'un objet (dossier ou contact) dans le dossier courant.

Utilisation: infos <nom_de_l'objet>
rename - Renomme un dossier ou un contact dans le dossier courant.

Utilisation: rename <ancien_nom> <nouveau_nom>
save - Sauvegarde la structure actuelle dans un fichier avec la possibilité de spécifier le format.

Utilisation: save <nom_du_fichier> [format]
Formats disponibles: 'xml' (XML Serialization), 'default' (Default Serialization)
load - Charge une structure à partir d'un fichier.
Utilisation: load <nom_du_fichier>
exit - Quitte le programme.
Utilisation: exit
