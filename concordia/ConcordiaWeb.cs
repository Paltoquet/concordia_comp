/*
 * Crée par SharpDevelop.
 * Utilisateur: user
 * Date: 28/01/2017
 * Heure: 16:27
 * 
 * Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
 */
using System;
using System.Web.Services;

namespace concordia
{
	/// <summary>
	/// Description of ConcordiaWeb
	/// </summary>
	[WebService
 	 (	Name = "ConcordiaWeb",
  		Description = "ConcordiaWeb",
  		Namespace = "http://www.ConcordiaWeb.example"
 	 )
	]
	public class ConcordiaWeb : WebService
	{
		public ConcordiaWeb()
		{
		}
		
		[WebMethod]
		public string Status()
		{
			return "Bienvenue à bord du Diving Concordia";
		}
	}
}
