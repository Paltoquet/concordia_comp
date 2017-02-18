/*
 * Crée par SharpDevelop.
 * Utilisateur: user
 * Date: 07/02/2017
 * Heure: 17:25
 * 
 * Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
 */
using System;
using System.Net;
using System.Globalization;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WComp.Beans;
using concordia;

namespace WComp.Beans
{
	[Bean(Category="MyCategory")]
	public class LightBean : IThreadCreator
	{
		
		
		public static int level = 10;
		
		private int delta;
		private int sleepVal;
		private Thread t;
		private volatile bool mutex = false;
		
		public LightBean(){
			configure();
		}
		
		public void configure(){
			var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://192.168.1.163/api/config");
	        httpWebRequest.Method = "GET";
	        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
	        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
	        {
	            var responseText = streamReader.ReadToEnd();
	            //set the config
	            JObject obj = JObject.Parse(responseText);
	            Config conf = JsonConvert.DeserializeObject<Config>(obj["config"].ToString());
	            delta = Int32.Parse(conf.lightDuration);
	            ProcessBean.seuil_temperature_on = Int32.Parse(conf.thermoTempStart);
	            ProcessBean.seuil_temperature_off = Int32.Parse(conf.thermoTempEnd);
	        }
		}
		public void launchThread() { 
			if(!mutex){
					mutex = true;
					sleepVal = delta/level;
					t = new Thread(new ThreadStart(waitFunction)); 
					t.Start(); 
			}
		}
		
		public void waitFunction(){
			for(int i = 0; i < level; i++){
				Thread.Sleep(sleepVal); 
				FireLightTimerEvent();
			}
			mutex = false;
		}
		
		public void Stop() {
			
		}
		
		public delegate void LightEventHandler();
		
		public event LightEventHandler LightTimerChanged;
		
		private void FireLightTimerEvent() {
			if (LightTimerChanged != null)
				LightTimerChanged();
		}
	}
}
