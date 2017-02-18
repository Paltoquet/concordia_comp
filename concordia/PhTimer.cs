/*
 * Crée par SharpDevelop.
 * Utilisateur: user
 * Date: 07/02/2017
 * Heure: 13:07
 * 
 * Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
 */
using System;
using WComp.Beans;
using System.Threading;

namespace WComp.Beans
{
	[Bean(Category="MyCategory")]
	public class PhTimerBean: IThreadCreator{
			
		//time we shouldn t look at ph value for turning the pump on
		//we use it after changing the water 
		private Thread t; 
		private int sleepVal = 600;
		private volatile bool mutex = false;
		
		public void launchTimer() { 
			if(!mutex){
					mutex = true;
					t = new Thread(new ThreadStart(waitFunction)); 
					t.Start(); 
			}
		}
		
		public void waitFunction(){
			Thread.Sleep(sleepVal); 
			FirePhTimerEvent(true);
			mutex = false;
		}
		
		public void Stop() {
			
		}
		
		public delegate void PhTimerEventHandler(bool i);
		
		public event PhTimerEventHandler phTimerChanged;
		
		private void FirePhTimerEvent(bool i) {
			if (phTimerChanged != null)
				phTimerChanged(i);
		}
		
	}
}
