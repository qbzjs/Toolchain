using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentTag : MonoBehaviour
{
	[SerializeField]public WeaponCustomizerConfigurator.AccessoryType m_Type;
	[SerializeField]public WeaponCustomizerConfigurator.SightType m_SightType;
	[SerializeField]public WeaponCustomizerConfigurator.UtilityType m_UtilityType;
	[SerializeField]public WeaponCustomizerConfigurator.BarrelType m_BarrelType;
	[SerializeField]public WeaponCustomizerConfigurator.UnderType m_UnderType;
	[SerializeField]public bool m_UseBase = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
