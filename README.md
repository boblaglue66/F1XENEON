# XENEON TV — lanceur Edge pour Windows

La version actuelle remplace l'intégration iFrame bloquée par F1 TV par un lanceur Windows avec deux boutons F1 TV et CANAL+.

Télécharger [XeneonTV.exe](native/XeneonTV.exe), puis suivre les [instructions](native/LIRE-MOI.md). Le [code source C#](native/Launcher.cs) est inclus.

Le lanceur ouvre les sites officiels dans Edge, réutilise un profil dédié sur ce PC et tente de placer la fenêtre sur l'écran sélectionné. Une première connexion est nécessaire aux deux services. La durée des sessions dépend des fournisseurs.

Dans iCUE : utiliser **Launch App / Lancer une application** avec cet exécutable. La vidéo s'affiche sur le bureau Windows du XENEON EDGE, et non dans un widget web intégré. Aucun PowerShell ni serveur local nécessaire.

Compilation vérifiée ; placement sur le matériel XENEON EDGE et lecture des services à valider sur l'appareil. Exécutable non signé.
