using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Opsive.UltimateCharacterController;

public class WeaponCustomizerActivator : MonoBehaviour
{
	
	public GameObject m_Weapon;
	public int m_ProfileConfig;
	public int m_CoreConfig;
	public int m_ForeConfig;
	public int m_RearConfig;
	public int m_AmmoConfig;
	public int m_AccessoryConfig;
	public bool m_ActivateOnStart;
	
	private WeaponCustomizerConfigurator m_Configurator;
	private Opsive.UltimateCharacterController.Items.Item m_Item;
	private Opsive.UltimateCharacterController.Items.Actions.ShootableWeapon m_Shootable;
	private Opsive.UltimateCharacterController.Items.Actions.MeleeWeapon m_Melee;
	private Opsive.UltimateCharacterController.ThirdPersonController.Items.ThirdPersonShootableWeaponProperties m_TPCShootable;
	private Opsive.UltimateCharacterController.ThirdPersonController.Items.ThirdPersonMeleeWeaponProperties m_TPCMelee;
	private Opsive.UltimateCharacterController.Objects.CharacterAssist.ItemPickup m_Pickup;
	
    // Start is called before the first frame update
    void Start()
	{
		if(m_ActivateOnStart)
		{
			Activate();
		}
	}
    
	public void Activate()
	{
		if(m_Weapon != null)
		{
			m_Configurator = m_Weapon.GetComponent<WeaponCustomizerConfigurator>();
		}
		if(m_Configurator != null)
		{
			m_Configurator.SetActiveConfig(m_Weapon,this);
			m_Weapon.name = m_Configurator.m_BaseName + " " + m_Configurator.m_WeaponProfiles[m_ProfileConfig].m_ProfileName;
			//this.name = m_Weapon.name;
		}
		m_Item = GetComponent<Opsive.UltimateCharacterController.Items.Item>();
		if(m_Item != null)
		{
			if(m_Configurator != null)
			{
				m_Item.AnimatorItemID = m_Configurator.m_WeaponProfiles[m_ProfileConfig].m_AnimatorID;
			}
		}
		m_Shootable = GetComponent<Opsive.UltimateCharacterController.Items.Actions.ShootableWeapon>();
		if(m_Shootable != null)
		{
			
		}
		m_TPCShootable = GetComponent<Opsive.UltimateCharacterController.ThirdPersonController.Items.ThirdPersonShootableWeaponProperties>();
		if(m_TPCShootable != null)
		{
			m_TPCShootable.FirePointLocation = m_Configurator.m_WeaponProfiles[m_ProfileConfig].
												m_ForeParts[m_ForeConfig].m_FirePointOffset;
			m_TPCShootable.MuzzleFlashLocation = m_Configurator.m_WeaponProfiles[m_ProfileConfig].
												m_ForeParts[m_ForeConfig].m_MuzzleFlashOffset;									
		}
		m_Melee = GetComponent<Opsive.UltimateCharacterController.Items.Actions.MeleeWeapon>();
		if(m_Melee != null)
		{
			
		}
		m_TPCMelee = GetComponent<Opsive.UltimateCharacterController.ThirdPersonController.Items.ThirdPersonMeleeWeaponProperties>();
		if(m_TPCMelee != null)
		{
			
		}
		m_Pickup = GetComponent<Opsive.UltimateCharacterController.Objects.CharacterAssist.ItemPickup>();
		if(m_Pickup != null)
		{
			var pickupItem = m_Pickup.ItemPickupSet[0];
			var pickupActivator = pickupItem.Item.GetComponentInChildren<WeaponCustomizerActivator>();
			pickupActivator.m_ProfileConfig = m_ProfileConfig;
			pickupActivator.m_CoreConfig = m_CoreConfig;
			pickupActivator.m_ForeConfig = m_ForeConfig;
			pickupActivator.m_RearConfig = m_RearConfig;
			pickupActivator.m_AmmoConfig = m_AmmoConfig;
			pickupActivator.m_ActivateOnStart = true;
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
    
	void SetShootable()
	{
		
	}
	
	void SetTPCShootable()
	{
		
	}
    
	void SetMelee()
	{
		
	}
	
	void SetTPCMelee()
	{
		
	}
	
	public WeaponCustomizerConfigurator GetBaseConfigurator()
	{
		if(m_Weapon != null)
		{
			m_Configurator = m_Weapon.GetComponent<WeaponCustomizerConfigurator>();
		}
		return m_Configurator;
	}
	
}
