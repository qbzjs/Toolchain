using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using PixelCrushers.DialogueSystem;

namespace BehaviorDesigner.Runtime.Tasks.DialugeSystem
{
    [TaskDescription("Gets the state of a quest entry in a quest.")]
    [TaskCategory("Dialogue System")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer/integrations/dialogue-system/")]
    [TaskIcon("Assets/Behavior Designer/Integrations/Dialogue System/Editor/DialogueSystemIcon.png")]
    public class GetQuestEntryState : Action
    {
        [Tooltip("The name of the quest")]
        public SharedString questEntryName;
        [Tooltip("The quest entry number (from 1)")]
        public SharedInt questEntryNumber;
        [Tooltip("Store the result in a String variable ('unassigned', 'active', 'success', or 'failure')")]
        public SharedString storeResult;

        public override TaskStatus OnUpdate()
        {
            if (questEntryName == null || string.IsNullOrEmpty(questEntryName.Value)) {
                Debug.LogWarning("EntryQuestState Task: Quest Entry Name is null or empty");
                return TaskStatus.Failure;
            } else if (questEntryNumber == null) {
                Debug.LogWarning("EntryQuestState Task: Quest Entry Number is null");
                return TaskStatus.Failure;
            }
            var questState = QuestLog.GetQuestEntryState(questEntryName.Value, Mathf.Max(1, questEntryNumber.Value));
            if (storeResult != null) {
                storeResult.Value = questState.ToString().ToLower();
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            questEntryName = "";
            questEntryNumber = 0;
            storeResult = "";
        }
    }
}