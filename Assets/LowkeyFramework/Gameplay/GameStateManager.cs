using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using System;
using UnityEngine;

public class GameStateManager : SingleBehaviour<GameStateManager>
{
    [SerializeField]
    private FSMOwner fsmOwner;

    [Header("Signals")]
    [SerializeField]
    private SignalDefinition gameStateBTsInitializedSignal;
    [SerializeField]
    private SignalDefinition gameStateBTChangedSignal;

    public FSMState CurrentState => fsmOwner.GetCurrentState() as FSMState;

    private FSMState cachedCurrentState;

    private void OnEnable()
    {
        gameStateBTsInitializedSignal.onInvoke += OnGameStateBTsInitialized;
    }

    private void OnDisable()
    {
        gameStateBTsInitializedSignal.onInvoke -= OnGameStateBTsInitialized;
    }

    private void Update()
    {
        if(cachedCurrentState == null)
            return;

        if(cachedCurrentState != CurrentState)
        {
            gameStateBTChangedSignal.Invoke(transform, null, true, cachedCurrentState, CurrentState);
            cachedCurrentState = CurrentState;
        }
    }

    public bool IsGameInState(BehaviourTree btState) => CurrentState != null && (CurrentState as NestedBTState).subGraph.name == btState.name;

    private void OnGameStateBTsInitialized(Transform sender, Transform receiver, bool isGlobal, object[] args)
    {
        cachedCurrentState = CurrentState;
    }
}
