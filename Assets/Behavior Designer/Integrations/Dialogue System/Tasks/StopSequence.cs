using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using PixelCrushers.DialogueSystem;

namespace BehaviorDesigner.Runtime.Tasks.DialugeSystem
{
    [TaskDescription("Stops a sequence.")]
    [TaskCategory("Dialogue System")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer/integrations/dialogue-system/")]
    [TaskIcon("Assets/Behavior Designer/Integrations/Dialogue System/Editor/DialogueSystemIcon.png")]
    public class StopSequence : Action
    {
        [Tooltip("The sequencer object stored by a Start Sequence task")]
        public SharedObject sequencerHandle;

        public override TaskStatus OnUpdate()
        {
            var sequencer = sequencerHandle.Value as Sequencer;
            if (sequencer != null) {
                DialogueManager.StopSequence(sequencer);
            }
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            sequencerHandle = null;
        }
    }
}