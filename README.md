#WCOMP

Vous disposez du code source de nos différents composants WCOMP

-Process Bean contient toute la partie métier
-Timer permet d'éteindre la pompe après un certain délais
-PhTimer permet d'attendre la fin d'un cycle de changement d'eau

##Installation

-Ouvrir la solution dans SharpDevelop
-Cliquer sur `Build`
-Récuperer le bean concordia., ajouter le dans votre répertoire WCOMP.NET
-Ajouter le bean DivingConcord à votre répertoire, il s'agit du proxy UPnP
-Créer un nouveau container et ouvrez le fichier `tmp.wcc`
-Cliquer sur le Bean DivingConcord et remplacez l'ip par l'addresse de votre serveur tournant sur la rapsberry
