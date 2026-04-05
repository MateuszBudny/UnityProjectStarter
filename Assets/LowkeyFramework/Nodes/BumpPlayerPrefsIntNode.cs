using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
    public class BumpPlayerPrefsIntNode : ActionTask
    {
        public string key;
        public int bumpByNum = 1;

        protected override string info => $"<b>Bump PlayerPrefs Int</b>\nKey: {key}\nBump By: {bumpByNum}";

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            PlayerPrefs.SetInt(key, PlayerPrefs.GetInt(key) + bumpByNum);

            EndAction(true);
        }
    }
}