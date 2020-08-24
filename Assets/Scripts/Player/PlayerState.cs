using System;
using System.Collections;
using UnityEngine;

namespace Fishnet {
    public class CharacterStatePushdown {
        Stack stateStack;

        public CharacterStatePushdown() {
            stateStack = new Stack();
        }

        public void PushState(CharacterState nextState) {
            stateStack.Push(nextState);
        }

        public void PopState() {
            stateStack.Pop();
        }

        public void Update() {
            CharacterState state = (CharacterState) stateStack.Peek();
            state.UpdateState();
            state.HandleInput();
        }
    }

    public abstract class CharacterState 
    {
        protected Player player;
        public CharacterState(Player player) {
            this.player = player;
        }
        public abstract void HandleInput();
        public virtual void UpdateState() { /* do nothing */ }
    }

    public class SailingState : CharacterState
    {
        public SailingState(Player player) : base(player) {
            player.animationChanger.SetSailing();
        }
        public override void HandleInput() {
            if (Input.GetKeyDown(KeyCode.F)) {
                player.animationChanger.SetIdle();
                player.movementStateStack.PopState();
                player.ChangeMover();
            }
        }
        public override void UpdateState() {
            player.LockToRaft();
        }  
    }

    public class WalkingState : CharacterState 
    {
        public WalkingState(Player player) : base(player) {
            player.animationChanger.SetWalking();
        }
        public override void HandleInput() {
            if (!player.InMotion()) {
                player.movementStateStack.PopState();
                player.movementStateStack.PushState(new IdleState(player));
            }
            if (Input.GetKeyDown(KeyCode.F)) {
                player.movementStateStack.PopState();
                player.movementStateStack.PushState(new SailingState(player));
                player.ChangeMover();
            }
        }
    }

    public class IdleState : CharacterState 
    {
        public IdleState(Player player) : base(player) {
            player.animationChanger.SetIdle();
        }
        public override void HandleInput() 
        {
            if (player.InMotion()) {
                player.movementStateStack.PopState();
                player.movementStateStack.PushState(new WalkingState(player));
            }
            if (Input.GetKeyDown(KeyCode.F)) {
                player.movementStateStack.PushState(new SailingState(player));
                player.ChangeMover();
            }
        }
    }

    public class ZoomedOutState : CharacterState
    {
        public ZoomedOutState(Player player) : base(player) {
            player.animationChanger.UnsetAiming();
        }
        public override void HandleInput() {
            if (Input.GetMouseButtonDown(1)) {
                player.cursorStateStack.PushState(new ZoomedInState(player));
            }
        }
    }

    public class ZoomedInState : CharacterState
    {
        public ZoomedInState(Player player) : base(player) {
            player.animationChanger.SetAiming();
        }
        public override void HandleInput() {
            if (Input.GetMouseButtonUp(1)) {
                player.animationChanger.UnsetAiming();
                player.cursorStateStack.PopState();
                player.ResetAim();
            } 
            if (Input.GetMouseButtonDown(0)) {
                player.animationChanger.SetCasting();
                player.CastLure();
                player.cursorStateStack.PushState(new CastingState(player));
            }
        }
        public override void UpdateState() {
            player.DrawCursor();
        }
    }

    public class CastingState : CharacterState {
        bool canGetInput = false;
        public CastingState(Player player) : base(player) {
            player.animationChanger.SetCasting();
        }
        public override void HandleInput() {
            if (Input.GetMouseButtonDown(0)) {
                player.animationChanger.SetAiming();
                player.cursorStateStack.PopState();
            }
        }
    }
}