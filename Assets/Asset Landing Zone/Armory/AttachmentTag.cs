using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentTag : MonoBehaviour
{
	public enum AccessoryType
	{
		Sight,
		Barrel,
		Utility,
		Under,
	}
	
	public enum BayonetType
	{
		Classic,
		K98,
		M9,
		Rail
	}
	
	[SerializeField]public AccessoryType m_Type;
	[SerializeField]public BayonetType m_BayonetType;
	[SerializeField]public AccessoryRack.SightType m_SightType;
	[SerializeField]public AccessoryRack.UtilityType m_UtilityType;
	[SerializeField]public AccessoryRack.BarrelType m_BarrelType;
	[SerializeField]public AccessoryRack.UnderType m_UnderType;
	[SerializeField]public GameObject m_AttachBase;
	[SerializeField]public Transform m_SightPoint;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
