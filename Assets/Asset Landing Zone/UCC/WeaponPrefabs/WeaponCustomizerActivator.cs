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
	public int m_ActiveMag;
	public int m_AccessoryConfig;
	public bool m_ActivateOnStart;
	
	private WeaponCustomizerConfigurator m_Configurator;
	private Opsive.UltimateCharacterController.Items.Item m_Item;
	private Opsive.UltimateCharacterController.Items.Actions.ShootableWeapon m_Shootable;
	private Opsive.UltimateCharacterController.Items.Actions.MeleeWeapon m_Melee;
	private Opsive.UltimateCharacterController.ThirdPersonController.Items.ThirdPersonPerspectiveItem m_ItemGrip;
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
			m_Weapon.name = m_Configurator.m_WeaponProfiles[m_ProfileConfig].m_ProfileName;

			m_Item = GetComponent<Opsive.UltimateCharacterController.Items.Item>();
			if(m_Item != null)
			{
				SetItem();
			}
			m_Shootable = GetComponent<Opsive.UltimateCharacterController.Items.Actions.ShootableWeapon>();
			if(m_Shootable != null)
			{
				SetShootable();
			}
			m_TPCShootable = GetComponent<Opsive.UltimateCharacterController.ThirdPersonController.Items.ThirdPersonShootableWeaponProperties>();
			if(m_TPCShootable != null)
			{
				SetTPCShootable();									
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
				SetPickup();
			}
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
    
	void SetItem()
	{
		m_Item.SlotID = m_Configurator.m_WeaponProfiles[m_ProfileConfig].m_SlotID;
		m_Item.AnimatorItemID = m_Configurator.m_WeaponProfiles[m_ProfileConfig].m_AnimatorID;
		m_Item.AnimatorMovementSetID = m_Configurator.m_WeaponProfiles[m_ProfileConfig].m_AnimatorMovementSetID;
		m_Item.DominantItem = m_Configurator.m_WeaponProfiles[m_ProfileConfig].m_DominantItem;
		m_Item.UIMonitorID = m_Configurator.m_WeaponProfiles[m_ProfileConfig].m_UIMonitorID;
		m_Item.ShowCrosshairsOnAim = m_Configurator.m_WeaponProfiles[m_ProfileConfig].m_ShowCrosshairsOnAim;
	}
    
	void SetShootable()
	{
		// Set Shootable Values from the Core
		m_Shootable.Mode = m_Configurator.m_WeaponProfiles[m_ProfileConfig].
			m_CoreParts[m_CoreConfig].m_FireMode;
		m_Shootable.Type = m_Configurator.m_WeaponProfiles[m_ProfileConfig].
			m_CoreParts[m_CoreConfig].m_FireType;
		m_Shootable.MinChargeLength = m_Configurator.m_WeaponProfiles[m_ProfileConfig].
			m_CoreParts[m_CoreConfig].m_MinChargeLength;
		m_Shootable.FullChargeLength = m_Configurator.m_WeaponProfiles[m_ProfileConfig].
			m_CoreParts[m_CoreConfig].m_FullChargeLength;
		m_Shootable.FireCount = m_Configurator.m_WeaponProfiles[m_ProfileConfig].
			m_CoreParts[m_CoreConfig].m_FireCount;
		m_Shootable.BurstCount = m_Configurator.m_WeaponProfiles[m_ProfileConfig].
			m_CoreParts[m_CoreConfig].m_BurstCount;
		m_Shootable.BurstDelay = m_Configurator.m_WeaponProfiles[m_ProfileConfig].
			m_CoreParts[m_CoreConfig].m_BurstDelay;
		
		m_Shootable.Spread = m_Configurator.m_WeaponProfiles[m_ProfileConfig].
			m_ForeParts[m_ForeConfig].m_Spread;
		m_Shootable.HitscanFireRange = m_Configurator.m_WeaponProfiles[m_ProfileConfig].
			m_ForeParts[m_ForeConfig].m_HitscanFireRange;
		m_Shootable.MuzzleFlash = m_Configurator.m_WeaponProfiles[m_ProfileConfig].
			m_ForeParts[m_ForeConfig].m_MuzzleFlash;
		
		m_Shootable.AutoReload = m_Configurator.m_WeaponProfiles[m_ProfileConfig].
			m_AmmoSets[m_AmmoConfig].m_AutoReload;
		m_Shootable.ReloadType = m_Configurator.m_WeaponProfiles[m_ProfileConfig].
			m_AmmoSets[m_AmmoConfig].m_ReloadType;
	}
	
	void SetTPCShootable()
	{
		// I may need to set the Action ID here as well
		m_TPCShootable.FirePointLocation = m_Configurator.m_WeaponProfiles[m_ProfileConfig].
			m_ForeParts[m_ForeConfig].m_FirePointOffset;
		m_TPCShootable.MuzzleFlashLocation = m_Configurator.m_WeaponProfiles[m_ProfileConfig].
			m_ForeParts[m_ForeConfig].m_MuzzleFlashOffset;
			
		// Set Tracer, Smoke, Shell, Reload Clip/Projectile
		
	}
    
	void SetMelee()
	{
		
	}
	
	void SetTPCMelee()
	{
		
	}
	
	void SetPickup()
	{
		var pickupItem = m_Pickup.ItemPickupSet[0];
		var pickupActivator = pickupItem.Item.GetComponentInChildren<WeaponCustomizerActivator>();
		
		pickupActivator.m_ProfileConfig = m_ProfileConfig;
		pickupActivator.m_CoreConfig = m_CoreConfig;
		pickupActivator.m_ForeConfig = m_ForeConfig;
		pickupActivator.m_RearConfig = m_RearConfig;
		pickupActivator.m_AmmoConfig = m_AmmoConfig;
		pickupActivator.m_ActiveMag = m_ActiveMag;
		pickupActivator.m_ActivateOnStart = true;
		
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
