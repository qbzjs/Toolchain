using Opsive.UltimateCharacterController.Camera;
using Opsive.UltimateCharacterController.Events;
using Opsive.UltimateCharacterController.Utility;
using UnityEngine;

namespace PixelCrushers.UCCSupport
{

    /// <summary>
    /// Utility script to handle the cursor nicely with UCC during menus.
    /// </summary>
    public class UCCMenuUtility : MonoBehaviour
    {
        private GameObject m_character = null;
        public GameObject character
        {
            get
            {
                if (m_character == null)
                {
                    var camera = UnityEngineUtility.FindCamera(null);
                    if (camera != null)
                    {
                        m_character = camera.GetComponent<Opsive.UltimateCharacterController.Camera.CameraController>().Character;
                    }
                }
                return m_character;
            }
        }

        public void OnOpenMenu()
        {
            EventHandler.ExecuteEvent(character, "OnEnableGameplayInput", false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void OnCloseMenu()
        {
            EventHandler.ExecuteEvent(character, "OnEnableGameplayInput", true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}