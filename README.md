**Introduction:**

Cette application permet de créer, manipuler, et sauvegarder une structure hiérarchique de dossiers et de contacts. Utilisez la commande `help` pour obtenir des informations sur les commandes disponibles.

**Commandes Disponibles:**

1. **`help`** - Affiche toutes les commandes disponibles ou fournit des informations détaillées sur une commande spécifique.

   - **Utilisation:** `help [nom_de_la_commande]`

2. **`afficher`** - Affiche l'arborescence depuis le dossier racine.

   - **Utilisation:** `afficher`

3. **`ls`** - Affiche le contenu du dossier courant.

   - **Utilisation:** `ls`

4. **`cd`** - Change le dossier courant.

   - **Utilisation:** `cd <chemin_du_dossier>`

5. **`mkdir`** - Crée un nouveau dossier dans le dossier courant.

   - **Utilisation:** `mkdir <nom_du_dossier>`

6. **`mkcont`** - Crée un nouveau contact dans le dossier courant.

   - **Utilisation:** `mkcont <nom_du_contact> [courriel] [societe] [lien]`

7. **`rm`** - Supprime un dossier ou un contact du dossier courant.

   - **Utilisation:** `rm <nom_de_l'objet>`

8. **`infos`** - Affiche les informations d'un objet (dossier ou contact) dans le dossier courant.

   - **Utilisation:** `infos <nom_de_l'objet>`

9. **`rename`** - Renomme un dossier ou un contact dans le dossier courant.

   - **Utilisation:** `rename <ancien_nom> <nouveau_nom>`

10. **`save`** - Sauvegarde la structure actuelle dans un fichier avec la possibilité de spécifier le format.

   - **Utilisation:** `save <nom_du_fichier> [format]`
   - **Formats disponibles:** 'xml' (XML Serialization), 'default' (Default Serialization)

11. **`load`** - Charge une structure à partir d'un fichier.

   - **Utilisation:** `load <nom_du_fichier>`

12. **`exit`** - Quitte le programme.

   - **Utilisation:** `exit`

**Notes Importantes:**

- Le programme démarre avec un dossier racine nommé `/`. Vous pouvez naviguer entre les dossiers en utilisant la commande `cd`.
- Pour créer un nouveau dossier, utilisez la commande `mkdir <nom_du_dossier>`.
- Pour créer un nouveau contact, utilisez la commande `mkcont <nom_du_contact> [courriel] [societe] [lien]`.
- Assurez-vous d'utiliser la commande `save` pour sauvegarder vos données dans un fichier avant de quitter l'application.
- Les adresses e-mail sont validées lors de la création d'un nouveau contact.
- Avant de supprimer un dossier, le programme demande une confirmation, car cela entraînera la suppression de tout son contenu.
