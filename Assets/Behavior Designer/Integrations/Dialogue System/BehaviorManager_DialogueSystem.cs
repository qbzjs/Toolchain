using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.DialugeSystem;
using PixelCrushers.DialogueSystem;

namespace BehaviorDesigner.Runtime
{
    public static class BehaviorManager_DialogueSystem
    {
        public static void DialogueSystemFinished(this BehaviorManager behaviorManager, Transform actor, TaskStatus status, string lastLine)
        {
            if (behaviorManager == null) {
                return;
            }
            var task = behaviorManager.TaskForObject(actor);
            if (task is StartConversation) {
                var startConversationTask = task as StartConversation;
                startConversationTask.ConversationComplete(status, lastLine);
            } else if (task is StartSequence) {
                var startSequenceTask = task as StartSequence;
                startSequenceTask.SequenceComplete(status);
            }
        }

        public static bool StopDialogueSystem(Task dialogueSystemTask)
        {
            if (dialogueSystemTask != null) {
                if (dialogueSystemTask is StartConversation) {
                    DialogueManager.StopConversation();
                } else if (dialogueSystemTask is StartSequence) {
                    DialogueManager.StopSequence((dialogueSystemTask as StartSequence).storeResult.Value as Sequencer);
                }
            }

            return true;
        }
    }
}