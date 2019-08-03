using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartSlotMonitor : MonoBehaviour
{
	public enum PartSlotType
	{
		Profile,
		Core,
		Fore,
		Rear,
		Ammo,
	}
	
	private PartSlotType m_ButtonType;
	private GameObject m_ActivePartLabel;
	private WeaponCustomizerSelector m_Selector;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
	// Sets up what kind of button we're working with
	public void Setup(PartSlotType type, GameObject partLabel, WeaponCustomizerSelector selector)
	{
		m_ButtonType = type;
		m_ActivePartLabel = partLabel;
		m_Selector = selector;
	}
	
	public void OnClick()
	{
		Text displayText = m_ActivePartLabel.GetComponent<Text>();
		displayText.text = GetComponentInChildren<Text>().text;
		m_Selector.UpdateActive(m_ButtonType,displayText.text);
	}
	
}
