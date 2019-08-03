using PixelCrushers.DialogueSystem;

namespace BehaviorDesigner.Runtime.Tasks.DialugeSystem
{
    [TaskDescription("Is a conversation active?")]
    [TaskCategory("Dialogue System")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer/integrations/dialogue-system/")]
    [TaskIcon("Assets/Behavior Designer/Integrations/Dialogue System/Editor/DialogueSystemIcon.png")]
    public class IsConversationActive : Conditional
    {
        public override TaskStatus OnUpdate()
        {
            return DialogueManager.IsConversationActive ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}