using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessoryRack : MonoBehaviour
{
	
	
	public enum SightType
	{
		Standard,
		Pistol_Dot,
		Red_Dot,
		ACOG,
		Scope16x,
		ScopeEnhanced,
	}
	
	public enum UtilityType
	{
		Standard,
		Light,
		Pistol_Light,
		Laser,
		Pistol_Laser,
		Light_Laser
	}
	
	public enum BarrelType
	{
		Standard,
		Suppressor,
		Suppressor_Pistol,
		Bayonet_Modern,
		Bayonet_Classic,
		Bayonet_Wedge,
		Bayonet_Spike,
	}
	
	public enum UnderType
	{
		Standard,
		Foregrip,
		Grenade,
		Energy_Biochem,
		Energy_Fission,
		Chemical,
	}
	
	[SerializeField]public SightType m_ActiveSight = SightType.Standard;
	[SerializeField]public GameObject m_SightBase;
	[SerializeField]public BarrelType m_ActiveBarrel = BarrelType.Standard;
	[SerializeField]public GameObject m_BarrelBase;
	[SerializeField]public UtilityType m_ActiveSideL = UtilityType.Standard;
	[SerializeField]public UtilityType m_ActiveSideR = UtilityType.Standard;
	[SerializeField]public GameObject m_UtilityBase;
	[SerializeField]public UnderType m_ActiveUnder = UnderType.Standard;
	[SerializeField]public GameObject m_UnderBase;
	
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
