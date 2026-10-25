# XENEON TV — lanceur Edge pour Windows

## Utilisation sans PowerShell

1. Extraire l'archive dans un dossier durable.
2. Double-cliquer sur `XeneonTV.exe`.
3. Choisir le XENEON EDGE dans la liste des écrans. L'écran 2560 × 720 est présélectionné lorsqu'il est détecté. Les écrans doivent être configurés en bureau Windows étendu.
4. Cliquer sur F1 TV ou CANAL+. Le lanceur ouvre le site officiel dans une fenêtre Edge sans barre d'adresse et tente de la placer sur l'écran sélectionné.
5. Se connecter directement dans Edge. Aucun mot de passe n'est demandé par le lanceur.

Dans iCUE, utiliser le widget **Launch App / Lancer une application** et sélectionner `XeneonTV.exe`. Le widget ouvre le lanceur ; les sites s'affichent dans une fenêtre Windows sur l'écran. Pour accéder à cette fenêtre, le XENEON EDGE doit afficher le bureau Windows ; ce programme ne transforme pas Edge en widget vidéo intégré à iCUE. Selon la configuration iCUE, il peut être nécessaire de quitter son affichage de widgets.

## Sessions conservées

Les deux services partagent un profil Edge dédié, stocké dans `%LOCALAPPDATA%\XeneonTV\EdgeProfile`. Il est réutilisé à chaque lancement. Les connexions habituelles de votre autre profil Edge ne sont pas copiées : une première connexion est nécessaire dans chaque service. La durée de connexion dépend du fournisseur ; une nouvelle authentification peut être exigée après expiration ou déconnexion. Ne pas supprimer ce dossier pour conserver les cookies.

Le lanceur conserve aussi le choix de l'écran dans `%LOCALAPPDATA%\XeneonTV\display.txt`. Il ne transmet pas ces réglages à GitHub ni à un autre serveur. Fermer une fenêtre vidéo ne déconnecte pas automatiquement le compte. Pour fermer une fenêtre : Alt+F4. Pour le plein écran : F11 si disponible dans Edge.

## Prérequis et limites

Windows avec .NET Framework 4.x et Microsoft Edge installé. Aucun serveur local ni PowerShell à lancer. L'exécutable n'est pas signé. Le code source `Launcher.cs` est inclus pour inspection.

La détection et le placement des fenêtres utilisent les API Windows et l'identification du profil Edge. Si la fenêtre est réutilisée ou ne peut pas être identifiée, utiliser Windows + Maj + flèche pour la déplacer. Le lanceur affiche un message dans ce cas. Il n'impose pas de modification aux DRM, aux restrictions géographiques ou aux droits d'abonnement.

Cette version a été compilée, mais le placement réel sur XENEON EDGE, la lecture des vidéos et la persistance des comptes n'ont pas été validés sur votre matériel.
