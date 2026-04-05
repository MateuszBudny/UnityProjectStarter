using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Actions
{

    public class PlayTweenControllerNode : ActionTask
    {
        public TweenController tweenController;
        public bool waitForCompletion = true;

        protected override string info => $"<b>Play Tween Controller</b>{(tweenController == null ? "\n<b>TWEEN CONTROLLER IS MISSING!</b>" : "")}\nWait For Completion: {waitForCompletion}";

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            if(waitForCompletion)
            {
                tweenController.PlayTween(() => EndAction(true));
            }
            else
            {
                tweenController.PlayTween();
                EndAction(true);
            }
        }
    }
}