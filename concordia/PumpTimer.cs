/*
 * Crée par SharpDevelop.
 * Utilisateur: user
 * Date: 07/02/2017
 * Heure: 12:53
 * 
 * Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
 */
using System;
using WComp.Beans;
using System.Threading;

namespace WComp.Beans
{
	[Bean(Category="MyCategory")]
	public class PumpTimerBean:  IThreadCreator{
		
		private Thread t; 
		private int sleepValPump = 2000;
		private int sleepValValve = 2000;
		private volatile bool mutex_pump = false;
		private volatile bool mutex_valve = false;
		
		public void launchPumpTimer(int time) { 
			if(!mutex_pump){
					mutex_pump = true;
					sleepValPump = time;
					t = new Thread(new ThreadStart(waitPumpFunction)); 
					t.Start(); 
			}
		}
		
		public void launchValveVTimer(int time) { 
			if(!mutex_valve){
					mutex_valve = true;
					sleepValValve = time;
					t = new Thread(new ThreadStart(waitValveFunction)); 
					t.Start(); 
			}
		}
		
		public void waitPumpFunction(){
			Thread.Sleep(sleepValPump); 
			FirePumpTimerEvent();
			mutex_pump = false;
		}
		
		public void waitValveFunction(){
			Thread.Sleep(sleepValValve); 
			FireValveTimerEvent();
			mutex_valve = false;
		}
		
		public void Stop() {
			
		}
		
		public delegate void PumpTimerEventHandler();
		
		public event PumpTimerEventHandler pumpTimerChanged;
		
		private void FirePumpTimerEvent() {
			if (pumpTimerChanged != null)
				pumpTimerChanged();
		}
		
		public delegate void ValveTimerEventHandler();
		
		public event ValveTimerEventHandler valveTimerChanged;
		
		private void FireValveTimerEvent() {
			if (valveTimerChanged != null)
				valveTimerChanged();
		}
	}
}
