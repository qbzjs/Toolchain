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
		Gunblade1h,
		Gunblade2h,
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
		[SerializeField]public bool m_allowSightAttach;
		[SerializeField]public bool m_allowScopeAttach;
		[SerializeField]public bool m_allowBarrelAttach;
		[SerializeField]public bool m_allowUnderBarrelAttach;
		[SerializeField]public bool m_allowSideAttach;
		[SerializeField]public bool m_allowPistolSightAttach;
		[SerializeField]public bool m_allowPistolUnderBarrelAttach;
	}
	
	[System.Serializable]
	public class CorePart
	{
		[SerializeField]public string m_PartName;
		[SerializeField]public Part m_Part;
		// Include gameplay stuff here, not important yet
		[SerializeField]public GameObject m_ScopeCam;
		//[SerializeField]public Transform m_ShellOffset;
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
	}
	
	[System.Serializable]
	public class RearPart
	{
		[SerializeField]public string m_PartName;
		[SerializeField]public Part m_Part;
		// Include gameplay stuff here, not important yet
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
	}
	
	[System.Serializable]
	public class AccessoryPart
	{
		[SerializeField]public string m_PartName;
		[SerializeField]public Part m_Part;
		// Include gameplay stuff here, not important yet
	}
	
	[System.Serializable]
	public class WeaponProfile
	{
		[SerializeField]public string m_ProfileName;
		[SerializeField]public GameObject m_ProfileRoot;
		[SerializeField]public WeaponType m_WeaponType;
		[SerializeField]public int m_AnimatorID;
		[SerializeField]public CorePart [] m_CoreParts;
		[SerializeField]public ForePart [] m_ForeParts;
		[SerializeField]public RearPart [] m_RearParts;
		[SerializeField]public AmmoSet [] m_AmmoSets;
		[SerializeField]public AccessoryPart [] m_AccessoryParts;
		
		// Include gameplay stuff here, not important yet
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
						partStatus = ValidatePart(current,"Weapon - Ammo",m_WeaponProfiles[settings.m_ProfileConfig].
							m_AmmoSets[settings.m_AmmoConfig].m_AmmoParts[j].m_Part);
						// With AmmoSet, we don't want to delete any of the mags in the set, leave the default
						if(partStatus == 1)
						{
							if(j != 0)
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
					else if(checkAccessory && m_WeaponProfiles[settings.m_ProfileConfig].m_AccessoryParts.Length > 0)
					{
						currentTag = "Weapon - Accessory";
						currentPart = m_WeaponProfiles[settings.m_ProfileConfig].m_AccessoryParts[settings.m_AccessoryConfig].m_Part;
						checkAccessory = false;
					}
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
