# XENEON TV — GitHub Pages

Interface statique pour le widget web/iFrame de CORSAIR iCUE : boutons F1 TV et CANAL+, affichage des sites officiels et lien de secours vers chaque service. Aucun serveur local, PowerShell ou Node.js nécessaire pour l'utilisation.

## Publication sans terminal

1. Ajouter `index.html`, `app.js` et `.nojekyll` à la racine d'un dépôt GitHub via « Add file / Upload files », puis enregistrer les modifications.
2. Dans Settings → Pages, sélectionner « Deploy from a branch », la branche contenant ces fichiers et le dossier « / (root) », puis Save.
3. Attendre le déploiement et copier l'URL indiquée par GitHub Pages.
4. Ajouter cette URL au widget web d'iCUE. Si iCUE demande du HTML, remplacer `URL_GITHUB_PAGES` dans ce code :

```html
<iframe src="URL_GITHUB_PAGES" style="width:100%;height:100%;border:0" allow="autoplay; encrypted-media; fullscreen; picture-in-picture" allowfullscreen></iframe>
```

## Connexion et vidéo

Se connecter uniquement sur les pages officielles des services. Aucun identifiant ou cookie n'est collecté par cette interface ou envoyé au dépôt GitHub.

GitHub Pages fournit l'hébergement HTTPS de l'interface. Il ne supprime pas les restrictions d'intégration des fournisseurs, ne fournit pas les DRM au navigateur d'iCUE et ne garantit pas la conservation des cookies tiers. Le changement d'adresse du prototype ne transfère pas les sessions du navigateur ni celles de l'ancienne version locale.

La connexion, la lecture vidéo et la conservation de la session après redémarrage d'iCUE restent à tester sur l'appareil. Si l'intégration est bloquée, le lien vers le site officiel peut ouvrir le navigateur, si iCUE l'autorise. Cette version n'est pas un lecteur vidéo autonome ni une extension officielle des services.
