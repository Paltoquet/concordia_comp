/*
 * Crée par SharpDevelop.
 * Utilisateur: user
 * Date: 24/01/2017
 * Heure: 14:45
 * 
 * Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
 */
using System;
using System.Globalization;
using WComp.Beans;
using concordia;

namespace WComp.Beans
{
	[Bean(Category="MyCategory")]
	public class ProcessBean
	{
		/// <summary>
		/// Fill in private attributes here.
		/// </summary>
		public static float seuil_temperature_on = 21;
		public static float seuil_temperature_off = 24;
		public static float seuil_light = 250;
		public static int time_start = 3600; //54000 15h
		public static int time_end = 72000; //20h
		public static float seuil_ph = 9;
		
		private ConcordiaWeb web;
		
		private int property;
		private float temperature;
		private float light;
		private float ph;
		private bool light_is_on = false;
		private int current = 0;
	
		private bool ph_has_been_used = false;
		private float start_from_config = 22;
		
		public float Start_from_config {
			get { return start_from_config; }
			set { start_from_config = value; }
		}
		public bool Ph_has_been_used {
			get { return ph_has_been_used; }
			set { ph_has_been_used = value; }
		}
		
		private bool pump_enable = true;
		private bool pump_on = false;
		private bool valve_on = false;
		private bool heating_on = false;
		//between 0 and 300
		private float potar;
		
		public ProcessBean(){
			
		}
		public float Potar {
			get { return potar; }
			set {
				potar = value;
				process_potar();
			}
		}
		
		public bool Pump_enable {
			get { return pump_enable; }
			set { pump_enable = value; }
		}
		
		public bool Heating_on {
			get { return heating_on; }
			set { 
				heating_on = value;
			}
		}
		// Propriété
		public float Temperature {
			get { return temperature; }
			set { 
				temperature = value;
				process_temp();
			}
		}
		public float Ph {
			get { return ph; }
			set { 
				ph = value;
				process_ph();
				//AsynchronousSocketListener.ph = ph;
				
			}
		}
		public bool Pump_on {
			get { return pump_on; }
			set { 
				pump_on = value;
			}
		}

		public bool Valve_on {
			get { return valve_on; }
			set { valve_on = value; }
		}
		
		public float Light {
			get { return light; }
			set { light = value; }
		}
		
		public bool Light_is_on {
			get { return light_is_on; }
			set { light_is_on = value; }
		}
		
		public void process_temp(){
			start_from_config = seuil_temperature_on;
			if(heating_on == false){
				if(temperature<seuil_temperature_on){
					TurnOnHeaterEvent();
				}
			}
			else{
				if(temperature>seuil_temperature_off){
					TurnOffHeaterEvent();
				}
			}
		}
		
		public void process_ph(){
			//si j'ai le droit de diriger la pompe
			if(pump_enable){
				//si elle est eteinte
				if(pump_on == false && valve_on == false){
					//if(Math.Abs(7-ph) > 3){
					if(ph < 6){
						pump_enable = false;
						ph_has_been_used = true;
						TurnOnPumpEvent(5000);
						TurnOnValveEvent();
						PumpEnableEvent();
					}
				}
			}
		}
		public void process_light(){
			current = DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
			if(!Light_is_on){
				if(current > time_start && current < time_end && light > seuil_light){
					IncrementLightEvent();
				}
			}
			else{
				if(current < time_start || current > time_end){
					TurnOffLightEvent();
				}
			}
		}
		
		public void TurnOffPump(){
			TurnOffPumpEvent();
			pump_on = false;
		}
		
		//potar [0-300]
		public void process_potar(){
			//niveau trop bas
			if((potar > 130 && potar < 170)&& Pump_on == true){
				TurnOffPumpEvent();
			}
			//niveau trop haut
			else if((potar > 250 || potar < 50) && Pump_on == false){
				TurnOnPumpEvent(2000);
			}
			if(valve_on == true && (potar > 250 || potar < 50)){
				TurnOffValveEvent();
			}
			else if(valve_on == false && (potar > 130 && potar < 170)){
				TurnOnValveEvent();
			}
		}
		
		public void setTemp(string val){
			Temperature = float.Parse(val, CultureInfo.InvariantCulture);
		}
		
		public void setPh(string val){
			Ph = float.Parse(val, CultureInfo.InvariantCulture);
		}
		
		public void setPotar(string val){
			Potar = float.Parse(val, CultureInfo.InvariantCulture);
		}
		
		public void setLight(string val){
			Light = float.Parse(val, CultureInfo.InvariantCulture);
		}
		
		public int Current {
			get { return current; }
			set { current = value; }
		}
		
		public void start_WebServer(){
			AsynchronousSocketListener.StartListening();
		}
		/// <summary>
		/// Here are the delegate and his event.
		/// A function checking nullity should be used to fire events (like FireIntEvent).
		/// </summary>

		//allumer pompe
		public delegate void TurnOnPumpHandler(int seconds);
		public event TurnOnPumpHandler TurnPump_on;
		
		private void TurnOnPumpEvent(int l) {
			if (TurnPump_on != null)
				TurnPump_on(l);
		}
		
		//eteindre pompe
		public delegate void TurnOffPumpHandler();
		public event TurnOffPumpHandler TurnPump_off;
		
		private void TurnOffPumpEvent() {
			if (TurnPump_on != null)
				TurnPump_off();
		}
		
		//allumer la valve
		public delegate void TurnOnValveHandler();
		public event TurnOnValveHandler TurnValve_on;
		
		private void TurnOnValveEvent() {
			if (TurnPump_on != null)
				TurnValve_on();
		}
		
		//eteindre la valve
		public delegate void TurnOffValveHandler();
		public event TurnOffValveHandler TurnValve_off;
		
		private void TurnOffValveEvent() {
			if (TurnPump_off != null)
				TurnValve_off();
		}
		
		//allumer heater
		public delegate void TurnOnHeaterHandler();
		public event TurnOnHeaterHandler TurnHeater_on;
		
		private void TurnOnHeaterEvent() {
			if (TurnHeater_on != null)
				TurnHeater_on();
		}
		
		//eteindre heater
		public delegate void TurnOffHeaterHandler();
		public event TurnOffHeaterHandler TurnHeater_off;
		
		private void TurnOffHeaterEvent() {
			if (TurnHeater_off != null)
				TurnHeater_off();
		}
		//enable pump
		public delegate void PumpEnableHandler();
		public event PumpEnableHandler PumpEnable;
		
		private void PumpEnableEvent() {
			if (PumpEnable != null)
				PumpEnable();
		}
		
		//eteindre heater
		public delegate void TurnOffLightHandler();
		public event TurnOffLightHandler TurnLight_off;
		
		private void TurnOffLightEvent() {
			if (TurnLight_off != null)
				TurnLight_off();
		}
		
		public delegate void IncrementLightHandler();
		public event IncrementLightHandler incrementLight;
		
		private void IncrementLightEvent() {
			if (incrementLight != null)
				incrementLight();
		}
		
		public delegate void DecrementLightHandler();
		public event DecrementLightHandler decrementLight;
		
		private void DecrementLightEvent() {
			if (decrementLight != null)
				decrementLight();
		}
	}
}
