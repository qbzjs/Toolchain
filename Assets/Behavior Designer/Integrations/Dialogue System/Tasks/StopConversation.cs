using UnityEngine;
using PixelCrushers.DialogueSystem;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.DialugeSystem
{
    [TaskDescription("Stops the current conversation.")]
    [TaskCategory("Dialogue System")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer/integrations/dialogue-system/")]
    [TaskIcon("Assets/Behavior Designer/Integrations/Dialogue System/Editor/DialogueSystemIcon.png")]
    public class StopConversation : Action
    {
        public override TaskStatus OnUpdate()
        {
            DialogueManager.StopConversation();
            return TaskStatus.Success;
        }
    }
}