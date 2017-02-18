/*
 * Crée par SharpDevelop.
 * Utilisateur: user
 * Date: 07/02/2017
 * Heure: 14:11
 * 
 * Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
 */
using System;
using System.Net;
using System.IO;
using WComp.Beans;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using concordia;

namespace WComp.Beans
{
	
	[Bean(Category="MyCategory")]
	public class CloudBean
	{
		
		public static string url = "http://192.168.1.163:2909/api/sensor/";
			
		public void configure(){
			var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://192.168.1.111:2909/api/config");
	        httpWebRequest.Method = "GET";
	        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
	        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
	        {
	            var responseText = streamReader.ReadToEnd();
	            //set the config
	            JObject obj = JObject.Parse(responseText);
	            Config conf = JsonConvert.DeserializeObject<Config>(obj["config"].ToString());
	            int delta = Int32.Parse(conf.lightDuration);
	            //ProcessBean.seuil_temperature_on = Int32.Parse(conf.thermoTempStart);
	            //ProcessBean.seuil_temperature_off = Int32.Parse(conf.thermoTempEnd);
	           	
	            int temperature_on = Int32.Parse(conf.thermoTempStart);
	            int temperature_off = Int32.Parse(conf.thermoTempEnd);
	        }
		}
		
		public void sendTemperature(string temp){
			string my_url = url + "temperature";
			var httpWebRequest = (HttpWebRequest)WebRequest.Create(my_url);
	        httpWebRequest.ContentType = "application/json";
	        httpWebRequest.Method = "PUT";
	
	        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
	        {
	        	Captor captor_temp = new Captor();
    			captor_temp.value = float.Parse(temp, CultureInfo.InvariantCulture.NumberFormat);;
    			captor_temp.name = "temperature";
    			string json = JsonConvert.SerializeObject(captor_temp);
	            streamWriter.Write(json);
	        }
	        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
		}
		public void sendPh(string ph){
			string my_url = url + "ph";
			var httpWebRequest = (HttpWebRequest)WebRequest.Create(my_url);
	        httpWebRequest.ContentType = "application/json";
	        httpWebRequest.Method = "PUT";
	
	        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
	        {
	        	Captor captor_ph = new Captor();
    			captor_ph.value = float.Parse(ph, CultureInfo.InvariantCulture.NumberFormat);;
    			captor_ph.name = "ph";
    			string json = JsonConvert.SerializeObject(captor_ph);
	            streamWriter.Write(json);
	        }
	        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
		}
		public void sendPotar(string potar){
			string my_url = url + "potar";
			var httpWebRequest = (HttpWebRequest)WebRequest.Create(my_url);
	        httpWebRequest.ContentType = "application/json";
	        httpWebRequest.Method = "PUT";
	
	        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
	        {
	        	Captor captor_potar = new Captor();
    			captor_potar.value = float.Parse(potar, CultureInfo.InvariantCulture.NumberFormat);;
    			captor_potar.name = "potar";
    			string json = JsonConvert.SerializeObject(captor_potar);
	            streamWriter.Write(json);
	        }
	        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
		}
		public void sendLight(string light){
			string my_url = url + "light";
			var httpWebRequest = (HttpWebRequest)WebRequest.Create(my_url);
	        httpWebRequest.ContentType = "application/json";
	        httpWebRequest.Method = "PUT";
	
	        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
	        {
	        	Captor captor_light = new Captor();
    			captor_light.value = float.Parse(light, CultureInfo.InvariantCulture.NumberFormat);;
    			captor_light.name = "light";
    			string json = JsonConvert.SerializeObject(captor_light);
	            streamWriter.Write(json);
	        }
	        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
		}
	}
}
