using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCustomizerConfigurator : MonoBehaviour
{
	// this class manages the various weapon configurations
	public enum WeaponType
	{
		Shootable1h,
		Shootable2h,
		GunbladeRifleStance,
		GunbladeMeleeStance,
		MeleeCutter1h,
		MeleeStriker1h,
		MeleeCutter2h,
		MeleeStriker2h,
		MeleePolearm,
		MeleeOversized,
	}
	
	[System.Serializable]
	public class Part
	{
		[SerializeField]public GameObject m_Part;
		[SerializeField]public string [] m_SubParts;
		[SerializeField]public bool m_SightMount;
		[SerializeField]public bool m_ScopeMount;
		[SerializeField]public bool m_BarrelMount;
		[SerializeField]public bool m_GripMount;
		[SerializeField]public bool m_UnderMount;
		[SerializeField]public bool m_SideMount;
		[SerializeField]public bool m_BarrelMountPistol;
		[SerializeField]public bool m_SightMountPistol;
		[SerializeField]public bool m_UnderMountPistol;
	}
	
	[System.Serializable]
	public class CorePart
	{
		[SerializeField]public string m_PartName;
		[SerializeField]public Part m_Part;
		// Include gameplay stuff here, not important yet
		//[SerializeField]public Transform m_ShellOffset;
		[Tooltip("The mode in which the weapon fires multiple shots.")]
		[SerializeField] 
		public Opsive.UltimateCharacterController.Items.Actions.ShootableWeapon.FireMode m_FireMode = 
			Opsive.UltimateCharacterController.Items.Actions.ShootableWeapon.FireMode.SemiAuto;
		[Tooltip("Specifies when the weapon should be fired.")]
		[SerializeField] 
		public Opsive.UltimateCharacterController.Items.Actions.ShootableWeapon.FireType m_FireType = 
			Opsive.UltimateCharacterController.Items.Actions.ShootableWeapon.FireType.Instant;
		[Tooltip("If using a charge FireType, the minimum amount of time that the weapon must be charged for in order to be fired.")]
		[SerializeField] public float m_MinChargeLength;
		[Tooltip("If using a charge FireType, the amount of time that the weapon must be charged for in order to be fully fired.")]
		[SerializeField] public float m_FullChargeLength;
		[Tooltip("The number of rounds to fire in a single shot.")]
		[SerializeField] public int m_FireCount = 1;
		[Tooltip("If using the Burst FireMode, specifies the number of bursts the weapon can fire.")]
		[SerializeField] public int m_BurstCount = 5;
		[Tooltip("If using the Burst FireMode, specifies the delay before the next burst can occur.")]
		[SerializeField] public float m_BurstDelay = 0.25f;
		
	}
	
	[System.Serializable]
	public class ForePart
	{
		[SerializeField]public string m_PartName;
		[SerializeField]public Part m_Part;
		// Include gameplay stuff here, not important yet
		[SerializeField]public Transform m_FirePointOffset;
		[SerializeField]public Transform m_MuzzleFlashOffset;
		//[SerializeField]public Transform m_SmokeOffset;
		//[SerializeField]public Transform m_TracerOffset;
		[Tooltip("The random spread of the bullets once they are fired.")]
		[Range(0, 360)] [SerializeField] public float m_Spread = 0.01f;
		[Tooltip("The maximum distance in which the hitscan fire can reach.")]
		[SerializeField] public float m_HitscanFireRange = float.MaxValue;
		[Tooltip("A reference to the muzzle flash prefab.")]
		[SerializeField] public GameObject m_MuzzleFlash;
		//store Minimum Recoil vector here
		//store Melee main attack values here
	}
	
	[System.Serializable]
	public class RearPart
	{
		[SerializeField]public string m_PartName;
		[SerializeField]public Part m_Part;
		// Include gameplay stuff here, not important yet
		//store Maximum Recoil vector here
		//store Melee rear attack values here
	}
	
	[System.Serializable]
	public class AmmoPart
	{
		[SerializeField]public string m_PartName;
		[SerializeField]public Part m_Part;
		// Include gameplay stuff here, not important yet
		[SerializeField]public int m_MaxAmmo;
		[SerializeField]public Transform m_ClipOffset;
		[SerializeField]public Transform m_ClipParent;
		[SerializeField]public Transform m_Projectile;
		[SerializeField]public Transform m_ProjectileParent;
	}
	
	[System.Serializable]
	public class AmmoSet
	{
		[SerializeField]public string m_SetName;
		[SerializeField]public AmmoPart [] m_AmmoParts;
		[Tooltip("The amount of damage to apply to the hit object.")]
		[SerializeField]public float m_DamageAmount = 10;
		[Tooltip("The amount of force to apply to the hit object.")]
		[SerializeField]public float m_ImpactForce = 2;
		[Tooltip("Specifies when the item should be automatically reloaded.")]
		[SerializeField]public Opsive.UltimateCharacterController.Character.Abilities.Items.Reload.AutoReloadType m_AutoReload = 
			Opsive.UltimateCharacterController.Character.Abilities.Items.Reload.AutoReloadType.Pickup | 
			Opsive.UltimateCharacterController.Character.Abilities.Items.Reload.AutoReloadType.Empty;
		[Tooltip("Specifies how the clip should be reloaded.")]
		[SerializeField] public Opsive.UltimateCharacterController.Items.Actions.ShootableWeapon.ReloadClipType m_ReloadType = 
			Opsive.UltimateCharacterController.Items.Actions.ShootableWeapon.ReloadClipType.Full;
	}
	
	[System.Serializable]
	public class WeaponProfile
	{
		[SerializeField]public string m_ProfileName;
		[SerializeField]public GameObject m_ProfileRoot;
		[SerializeField]public WeaponType m_WeaponType;
		[Tooltip("Unique ID used for item identification within the animator.")]
		[SerializeField]public int m_AnimatorID;
		[SerializeField]public CorePart [] m_CoreParts;
		[SerializeField]public ForePart [] m_ForeParts;
		[SerializeField]public RearPart [] m_RearParts;
		[SerializeField]public AmmoSet [] m_AmmoSets;
		[SerializeField]public AccessoryRack m_AccessoryRack;
		
		// Item variables
		[Tooltip("Specifies the inventory slot/spawn location of the item.")]
		[SerializeField]public int m_SlotID;
		[Tooltip("The movement set ID used for within the animator.")]
		[SerializeField]public int m_AnimatorMovementSetID;
		[Tooltip("Does the item control the movement and the UI shown?")]
		[SerializeField]public bool m_DominantItem = true;
		[Tooltip("The ID of the UI Monitor that the item should use.")]
		[SerializeField]public int m_UIMonitorID;
		[Tooltip("Should the crosshairs be shown when the item aims?")]
		[SerializeField]public bool m_ShowCrosshairsOnAim = true;
		
	}
	
	[SerializeField]public string m_BaseName;
	[SerializeField]public WeaponProfile [] m_WeaponProfiles;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
	public void SetActiveConfig(GameObject baseObj, WeaponCustomizerActivator settings)
	{
		bool findConfigBase = false;
		GameObject useObj = baseObj;
		
		if(baseObj != m_WeaponProfiles[settings.m_ProfileConfig].m_ProfileRoot)
		{
			findConfigBase = true;
		}
		
		for(int i = 0;i < useObj.transform.childCount; i++)
		{
			int partStatus;
			GameObject current = useObj.transform.GetChild(i).gameObject;
			// if we don't have the weaponParent, check for it and delete the others
			// Gonna save my ass when I get to the lever action and the AK
			if(findConfigBase && current.CompareTag("Weapon - ConfigBase"))
			{
				if(current != m_WeaponProfiles[settings.m_ProfileConfig].m_ProfileRoot)
				{
					Destroy(current);
				}
				else
				{
					// call the function recursively, so that everything loads from here
					// and the other configs get destroyed
					SetActiveConfig(current,settings);
				}
			}
			else if(current.CompareTag("Weapon - Ammo"))
			{
				if(m_WeaponProfiles[settings.m_ProfileConfig].m_AmmoSets.Length > 0)
				{
					partStatus = 0;
					for(int j = 0; j < m_WeaponProfiles[settings.m_ProfileConfig].
						m_AmmoSets[settings.m_AmmoConfig].m_AmmoParts.Length; j++)
					{
						partStatus = ValidatePart(current,"Weapon - Ammo",
							m_WeaponProfiles[settings.m_ProfileConfig].
							m_AmmoSets[settings.m_AmmoConfig].m_AmmoParts[j].m_Part);
						// With AmmoSet, make the active mag visible, the rest invisible
						if(partStatus == 1)
						{
							// set the active mag
							if(j != settings.m_ActiveMag)
							{
								current.SetActive(false);
							}
							break;
						}
					}
					if(partStatus == -1)
					{
						Destroy(current);
					}
					
				}
			}
			else if(!current.CompareTag("Weapon - Integral"))
			{
				bool isLoading = true;
				bool checkCore = true;
				bool checkFore = true;
				bool checkRear = true;
				bool checkAccessory = true;
				
				while((checkCore||checkFore||checkRear||checkAccessory)&&(isLoading))
				{
					string currentTag = null;
					Part currentPart = null;
					if(checkCore && m_WeaponProfiles[settings.m_ProfileConfig].m_CoreParts.Length > 0)
					{
						currentTag = "Weapon - Core";
						currentPart = m_WeaponProfiles[settings.m_ProfileConfig].m_CoreParts[settings.m_CoreConfig].m_Part;
						checkCore = false;
					}
					else if(checkFore && m_WeaponProfiles[settings.m_ProfileConfig].m_ForeParts.Length > 0)
					{
						currentTag = "Weapon - Fore";
						currentPart = m_WeaponProfiles[settings.m_ProfileConfig].m_ForeParts[settings.m_ForeConfig].m_Part;
						checkFore = false;
					}
					else if(checkRear && m_WeaponProfiles[settings.m_ProfileConfig].m_RearParts.Length > 0)
					{
						currentTag = "Weapon - Rear";
						currentPart = m_WeaponProfiles[settings.m_ProfileConfig].m_RearParts[settings.m_RearConfig].m_Part;
						checkRear = false;
					}
					//else if(checkAccessory && m_WeaponProfiles[settings.m_ProfileConfig].m_AccessoryRack.Length > 0)
					//{
					//	currentTag = "Weapon - Accessory";
					//	currentPart = m_WeaponProfiles[settings.m_ProfileConfig].m_AccessoryRack[settings.m_AccessoryConfig].m_Part;
					//	checkAccessory = false;
					//}
					else
					{
						isLoading = false;
					}
					
					if(isLoading)
					{
						partStatus = ValidatePart(current,currentTag,currentPart);
						if(partStatus == -1)
						{
							// it's the right kind of part, but not the configured one, so delete it or turn it off
							Destroy(current);
							break;
						}
						else if(partStatus == 1)
						{
							// it's the right part, break the flow
							break;
						}
					}
				}
			}
		}
	}
	
	private int ValidatePart(GameObject partObj, string tagToCompare, Part partToCompare)
	{
		int retVal = 0;
		if(partObj.CompareTag(tagToCompare))
		{
			if(partObj == partToCompare.m_Part)
			{
				if(partToCompare.m_SubParts.Length > 0)
				{
					loadSubParts(partObj,partToCompare.m_SubParts,retVal);
				}
				retVal = 1;
			}
			else
			{
				retVal = -1;	
			}
		}
		return retVal;
	}
	
	private int loadSubParts(GameObject basePart, string [] subParts, int index)
	{
		int retVal = index;
		for(int i = 0; i < basePart.transform.childCount; i++)
		{
			GameObject current = basePart.transform.GetChild(i).gameObject;
			// if we have integral subparts along with selective subparts, we only want to destroy unnecessary ones
			if(!current.CompareTag("Weapon - Integral"))
			{
				if(current.name == subParts[retVal])
				{
					retVal++;
					if(retVal < subParts.Length)
					{
						retVal = loadSubParts(current, subParts, retVal);
					}
					if(retVal >= subParts.Length)
					{
						break;
					}
				}
				else
				{
					Destroy(current);
				}
			}
		}
		return retVal;
	}
}
