// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using Opsive.UltimateCharacterController.Traits;
using Opsive.UltimateCharacterController.Events;

namespace PixelCrushers.UCCSupport
{

    /// <summary>
    /// This replacement for CharacterRespawner reloads the last checkpoint save if available;
    /// otherwise it respawns as usual.
    /// </summary>
    [AddComponentMenu("Pixel Crushers/Common/Save System/Opsive/Checkpoint Character Respawner")]
    public class CheckpointCharacterRespawner : CharacterRespawner
    {
        [Tooltip("Slot where checkpoint saves are saved.")]
        public int checkpointSaveSlot = 1;

        protected override void Awake()
        {
            base.Awake();
            EventHandler.RegisterEvent(gameObject, "OnRespawn", OnRespawn);
        }

        protected override void OnDestroy()
        {
            EventHandler.UnregisterEvent(gameObject, "OnRespawn", OnRespawn);
            base.OnDestroy();
        }

        private void OnRespawn()
        {
            if (SaveSystem.HasSavedGameInSlot(checkpointSaveSlot))
            {
                SaveSystem.LoadFromSlot(checkpointSaveSlot);
            }
        }
    }
}
