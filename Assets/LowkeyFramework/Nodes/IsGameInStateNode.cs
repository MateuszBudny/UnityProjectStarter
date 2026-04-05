using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;


namespace NodeCanvas.Tasks.Actions
{
    public class IsGameInStateNode : ActionTask
    {
        public BehaviourTree stateToCheck;

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            GameStateManager.Instance.IsGameInState(stateToCheck);

            EndAction(true);
        }
    }
}