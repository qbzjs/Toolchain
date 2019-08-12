using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCustomizerSelector : MonoBehaviour
{
	public WeaponCustomizerActivator [] m_BaseObjectList;
	private GameObject m_ActiveWeapon;
	private GameObject m_LoadedWeapon;
	public Transform m_ActiveWeaponLocation;
	public GameObject m_PartSlotPrefab;
	public GameObject m_PartSlotPanel;
	public GameObject m_ActiveProfileDisplay;
	public GameObject m_ActiveCoreDisplay;
	public GameObject m_ActiveForeDisplay;
	public GameObject m_ActiveRearDisplay;
	public GameObject m_ActiveAmmoDisplay;
	public GameObject m_CoreButton;
	public GameObject m_ForeButton;
	public GameObject m_RearButton;
	public GameObject m_AmmoButton;
	
	/// <summary>
	/// Remove any controls from the panels
	/// </summary>
	private void Cleanup()
	{
		//var displayText = m_ActiveProfileDisplay.GetComponentInChildren<Text>();
		//displayText = "Default";
		//displayText = m_ActiveCoreDisplay.GetComponentInChildren<Text>();
		//displayText = "Default";
		//displayText = m_ActiveForeDisplay.GetComponentInChildren<Text>();
		//displayText = "Default";
		//displayText = m_ActiveRearDisplay.GetComponentInChildren<Text>();
		//displayText = "Default";
		//displayText = m_ActiveAmmoDisplay.GetComponentInChildren<Text>();
		//displayText = "Default";

		foreach (Transform t in m_PartSlotPanel.transform)
		{
			Destroy(t.gameObject);
		}
	}
	
	public void ProfileClick()
	{
		Cleanup();
		foreach(WeaponCustomizerActivator weaponBase in m_BaseObjectList)
		{
			var configurator = weaponBase.GetBaseConfigurator();
			var profiles = configurator.m_WeaponProfiles;
			foreach(var profile in profiles)
			{
				GameObject go = GameObject.Instantiate(m_PartSlotPrefab);
				PartSlotMonitor monitor = go.GetComponent<PartSlotMonitor>();
				monitor.Setup(PartSlotMonitor.PartSlotType.Profile,m_ActiveProfileDisplay,this);
				Text txt = go.GetComponentInChildren<Text>();
				txt.text = profile.m_ProfileName;
				go.transform.SetParent(m_PartSlotPanel.transform);
			}
		}
	}
	
	public void CoreClick()
	{
		Cleanup();
		if(m_ActiveWeapon != null)
		{
			foreach(WeaponCustomizerActivator weaponBase in m_BaseObjectList)
			{
				var activator = m_ActiveWeapon.GetComponent<WeaponCustomizerActivator>();
				var configurator = activator.GetBaseConfigurator();
				var cores = configurator.m_WeaponProfiles[activator.m_ProfileConfig].m_CoreParts;
				foreach(var core in cores)
				{
					GameObject go = GameObject.Instantiate(m_PartSlotPrefab);
					PartSlotMonitor monitor = go.GetComponent<PartSlotMonitor>();
					monitor.Setup(PartSlotMonitor.PartSlotType.Core,m_ActiveCoreDisplay,this);
					Text txt = go.GetComponentInChildren<Text>();
					txt.text = core.m_PartName;
					go.transform.SetParent(m_PartSlotPanel.transform);
				}
			}
		}
	}
	
	public void ForeClick()
	{
		Cleanup();
		if(m_ActiveWeapon != null)
		{
			foreach(WeaponCustomizerActivator weaponBase in m_BaseObjectList)
			{
				var activator = m_ActiveWeapon.GetComponent<WeaponCustomizerActivator>();
				var configurator = activator.GetBaseConfigurator();
				var fores = configurator.m_WeaponProfiles[activator.m_ProfileConfig].m_ForeParts;
				foreach(var fore in fores)
				{
					GameObject go = GameObject.Instantiate(m_PartSlotPrefab);
					PartSlotMonitor monitor = go.GetComponent<PartSlotMonitor>();
					monitor.Setup(PartSlotMonitor.PartSlotType.Fore,m_ActiveForeDisplay,this);
					Text txt = go.GetComponentInChildren<Text>();
					txt.text = fore.m_PartName;
					go.transform.SetParent(m_PartSlotPanel.transform);
				}
			}
		}
	}
	
	public void RearClick()
	{
		Cleanup();
		if(m_ActiveWeapon != null)
		{
			foreach(WeaponCustomizerActivator weaponBase in m_BaseObjectList)
			{
				var activator = m_ActiveWeapon.GetComponent<WeaponCustomizerActivator>();
				var configurator = activator.GetBaseConfigurator();
				var rears = configurator.m_WeaponProfiles[activator.m_ProfileConfig].m_RearParts;
				foreach(var rear in rears)
				{
					GameObject go = GameObject.Instantiate(m_PartSlotPrefab);
					PartSlotMonitor monitor = go.GetComponent<PartSlotMonitor>();
					monitor.Setup(PartSlotMonitor.PartSlotType.Rear,m_ActiveRearDisplay,this);
					Text txt = go.GetComponentInChildren<Text>();
					txt.text = rear.m_PartName;
					go.transform.SetParent(m_PartSlotPanel.transform);
				}
			}
		}
	}
	
	
	public void AmmoClick()
	{
		Cleanup();
		if(m_ActiveWeapon != null)
		{
			foreach(WeaponCustomizerActivator weaponBase in m_BaseObjectList)
			{
				var activator = m_ActiveWeapon.GetComponent<WeaponCustomizerActivator>();
				var configurator = activator.GetBaseConfigurator();
				var ammoSets = configurator.m_WeaponProfiles[activator.m_ProfileConfig].m_AmmoSets;
				foreach(var ammoSet in ammoSets)
				{
					var ammos = ammoSet.m_AmmoParts;
					foreach(var ammo in ammos)
					{
						GameObject go = GameObject.Instantiate(m_PartSlotPrefab);
						PartSlotMonitor monitor = go.GetComponent<PartSlotMonitor>();
						monitor.Setup(PartSlotMonitor.PartSlotType.Ammo,m_ActiveAmmoDisplay,this);
						Text txt = go.GetComponentInChildren<Text>();
						txt.text = ammo.m_PartName;
						go.transform.SetParent(m_PartSlotPanel.transform);
					}
				}
			}
		}
	}
	
	public void EquipClick()
	{
		Cleanup();
		m_LoadedWeapon.GetComponent<Opsive.UltimateCharacterController.Objects.CharacterAssist.ItemPickup>().enabled = true;

	}
	
	public void UpdateActive(PartSlotMonitor.PartSlotType modPart,string inName)
	{
		if(m_LoadedWeapon != null)
		{
			Destroy(m_LoadedWeapon);
		}
		
		if(modPart == PartSlotMonitor.PartSlotType.Profile)
		{
			foreach(WeaponCustomizerActivator activator in m_BaseObjectList)
			{
				var configurator = activator.GetBaseConfigurator();
				for(int i = 0; i < configurator.m_WeaponProfiles.Length;i++)
				{
					if(configurator.m_WeaponProfiles[i].m_ProfileName == inName)
					{
						m_ActiveWeapon = activator.gameObject;
						activator.m_ProfileConfig = i;
						activator.m_CoreConfig = 0;
						activator.m_ForeConfig = 0;
						activator.m_RearConfig = 0;
						activator.m_AmmoConfig = 0;
						activator.m_ActiveMag = 0;
						// turn on configurator buttons based on having more than one option
						if(configurator.m_WeaponProfiles[i].m_CoreParts.Length > 1 )
						{
							m_CoreButton.SetActive(true);
						}
						else
						{
							m_CoreButton.SetActive(false);
							var text = m_ActiveCoreDisplay.GetComponentInChildren<Text>();
							text.text = configurator.m_WeaponProfiles[i].m_CoreParts[0].m_PartName;
						}
						
						if(configurator.m_WeaponProfiles[i].m_ForeParts.Length > 1 )
						{
							m_ForeButton.SetActive(true);
						}
						else
						{
							m_ForeButton.SetActive(false);
							var text = m_ActiveForeDisplay.GetComponentInChildren<Text>();
							text.text = configurator.m_WeaponProfiles[i].m_ForeParts[0].m_PartName;
						}
						
						if(configurator.m_WeaponProfiles[i].m_RearParts.Length > 1 )
						{
							m_RearButton.SetActive(true);
						}
						else
						{
							m_RearButton.SetActive(false);
							var text = m_ActiveForeDisplay.GetComponentInChildren<Text>();
							text.text = configurator.m_WeaponProfiles[i].m_RearParts[0].m_PartName;
						}
						
						if(configurator.m_WeaponProfiles[i].m_AmmoSets.Length > 1)
						{
							m_AmmoButton.SetActive(true);
						}
						else if (configurator.m_WeaponProfiles[i].m_AmmoSets[0].m_AmmoParts.Length > 1)
						{
							m_AmmoButton.SetActive(true);
						}
						else
						{
							m_AmmoButton.SetActive(false);
							var text = m_ActiveForeDisplay.GetComponentInChildren<Text>();
							text.text = configurator.m_WeaponProfiles[i].m_AmmoSets[0].m_AmmoParts[0].m_PartName;
						}
					}
				}
			}
		}
		
		if(modPart== PartSlotMonitor.PartSlotType.Core)
		{
			if(m_ActiveWeapon != null)
			{
				var activator = m_ActiveWeapon.GetComponent<WeaponCustomizerActivator>();
				var configurator = activator.GetBaseConfigurator();
				var cores = configurator.m_WeaponProfiles[activator.m_ProfileConfig].m_CoreParts;
				for(int i = 0; i < cores.Length;i++)
				{
					if(cores[i].m_PartName == inName)
					{
						activator.m_CoreConfig = i;
					}
				}
			}
		}
		
		if(modPart== PartSlotMonitor.PartSlotType.Fore)
		{
			if(m_ActiveWeapon != null)
			{
				var activator = m_ActiveWeapon.GetComponent<WeaponCustomizerActivator>();
				var configurator = activator.GetBaseConfigurator();
				var fores = configurator.m_WeaponProfiles[activator.m_ProfileConfig].m_ForeParts;
				for(int i = 0; i < fores.Length;i++)
				{
					if(fores[i].m_PartName == inName)
					{
						activator.m_ForeConfig = i;
					}
				}
			}
		}
		
		if(modPart== PartSlotMonitor.PartSlotType.Rear)
		{
			if(m_ActiveWeapon != null)
			{
				var activator = m_ActiveWeapon.GetComponent<WeaponCustomizerActivator>();
				var configurator = activator.GetBaseConfigurator();
				var rears = configurator.m_WeaponProfiles[activator.m_ProfileConfig].m_RearParts;
				for(int i = 0; i < rears.Length;i++)
				{
					if(rears[i].m_PartName == inName)
					{
						activator.m_RearConfig = i;
					}
				}
			}
		}
		
		
		if(modPart== PartSlotMonitor.PartSlotType.Ammo)
		{
			if(m_ActiveWeapon != null)
			{
				var activator = m_ActiveWeapon.GetComponent<WeaponCustomizerActivator>();
				var configurator = activator.GetBaseConfigurator();
				var ammo = configurator.m_WeaponProfiles[activator.m_ProfileConfig].m_AmmoSets;
				for(int i = 0; i < ammo.Length;i++)
				{
					for(int j = 0; j < ammo[i].m_AmmoParts.Length; j++)
					{
						if(ammo[i].m_AmmoParts[j].m_PartName == inName)
						{
							activator.m_AmmoConfig = i;
							activator.m_ActiveMag = j;
						}
					}
				}
			}
		}
		
		var activatorSave = m_ActiveWeapon.GetComponent<WeaponCustomizerActivator>();
		
		m_LoadedWeapon = GameObject.Instantiate(m_ActiveWeapon,
			m_ActiveWeaponLocation.position,
			m_ActiveWeaponLocation.rotation);
			

			
		var activatorLoad = m_LoadedWeapon.GetComponent<WeaponCustomizerActivator>();
		activatorLoad.m_ProfileConfig = activatorSave.m_ProfileConfig;
		activatorLoad.m_CoreConfig = activatorSave.m_CoreConfig;
		activatorLoad.m_ForeConfig = activatorSave.m_ForeConfig;
		activatorLoad.m_RearConfig = activatorSave.m_RearConfig;
		activatorLoad.m_AmmoConfig = activatorSave.m_AmmoConfig;
		activatorLoad.m_ActiveMag = activatorSave.m_ActiveMag;
		activatorLoad.Activate();

	}
}
