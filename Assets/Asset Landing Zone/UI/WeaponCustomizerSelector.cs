using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCustomizerSelector : MonoBehaviour
{
	public WeaponCustomizerActivator [] m_BaseObjectList;
	private GameObject m_ActiveWeapon;
	public Transform m_ActiveWeaponLocation;
	public GameObject m_PartSlotPrefab;
	public GameObject m_PartSlotPanel;
	public GameObject m_ActiveProfileDisplay;
	public GameObject m_ActiveCoreDisplay;
	public GameObject m_ActiveForeDisplay;
	public GameObject m_ActiveRearDisplay;
	public GameObject m_ActiveAmmoDisplay;
	
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
	
	public void UpdateActive(PartSlotMonitor.PartSlotType modPart,string inName)
	{
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
				for(int i = 0; i < configurator.m_WeaponProfiles[activator.m_ProfileConfig].m_CoreParts.Length;i++)
				{
					if(configurator.m_WeaponProfiles[activator.m_ProfileConfig].m_CoreParts[i].m_PartName == inName)
					{
						activator.m_CoreConfig = i;
					}
				}
			}
		}
		
		
		var activatorSave = m_ActiveWeapon.GetComponent<WeaponCustomizerActivator>();
		
		var pickup = GameObject.Instantiate(m_ActiveWeapon,
			m_ActiveWeaponLocation.position,
			m_ActiveWeaponLocation.rotation);
			

			
		var activatorLoad = pickup.GetComponent<WeaponCustomizerActivator>();
		activatorLoad.m_ProfileConfig = activatorSave.m_ProfileConfig;
		activatorLoad.m_CoreConfig = activatorSave.m_CoreConfig;
		activatorLoad.m_ForeConfig = activatorSave.m_ForeConfig;
		activatorLoad.m_RearConfig = activatorSave.m_RearConfig;
		activatorLoad.m_AmmoConfig = activatorSave.m_AmmoConfig;
		activatorLoad.Activate();
	}
}
