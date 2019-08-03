using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using PixelCrushers.DialogueSystem;

namespace BehaviorDesigner.Runtime.Tasks.DialugeSystem
{
    [TaskDescription("Starts a cutscene sequence.")]
    [TaskCategory("Dialogue System")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer/integrations/dialogue-system/")]
    [TaskIcon("Assets/Behavior Designer/Integrations/Dialogue System/Editor/DialogueSystemIcon.png")]
    public class StartSequence : Action
    {
        [Tooltip("The sequence to play")]
        public SharedString sequence;
        [Tooltip("The speaker, if the sequence references 'speaker' (optional)")]
        public SharedGameObject speaker;
        [Tooltip("The listener (optional)")]
        public SharedGameObject listener;
        [Tooltip("Should the behavior tree wait for the sequence to finish before moving onto the next task?")]
        public SharedBool waitForSequenceCompletion = true;
        [Tooltip("Store the resulting sequence handler in an Object variable")]
        public SharedObject storeResult;

        // The return status of the sequence after it has finished executing
        private TaskStatus status;

        public override void OnStart()
        {
            var speakerTransform = (speaker.Value != null) ? speaker.Value.transform : null;
            var listenerTransform = (listener.Value != null) ? listener.Value.transform : null;
            status = TaskStatus.Failure; // assume failure
            if (!waitForSequenceCompletion.Value || BehaviorManager.instance.MapObjectToTask(speakerTransform != null ? speakerTransform : gameObject.transform, this, BehaviorManager.ThirdPartyObjectType.DialogueSystem)) {
                storeResult.Value = DialogueManager.PlaySequence(sequence.Value, speakerTransform, listenerTransform, waitForSequenceCompletion.Value);
                status = waitForSequenceCompletion.Value ? TaskStatus.Running : TaskStatus.Success;
            }
        }

        public override TaskStatus OnUpdate()
        {
            // We are returning the same status until we hear otherwise.
            return status;
        }

        // SequenceComplete will be called after the Dialogue System finishes the sequence. 
        public void SequenceComplete(TaskStatus taskStatus)
        {
            // Update the status when the Dialogue System completes
            status = taskStatus;
        }

        public override void OnReset()
        {
            sequence = "";
            speaker = null;
            listener = null;
            waitForSequenceCompletion = true;
            storeResult = null;
        }
    }
}