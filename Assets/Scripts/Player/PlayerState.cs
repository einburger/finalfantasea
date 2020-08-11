using System;
using System.Collections;
using UnityEngine;

namespace Fishnet {
    public class CharacterStatePushdown {
        Stack stateStack;

        public CharacterStatePushdown() {
            stateStack = new Stack();
        }

        public void PushState(ICharacterState nextState) {
            stateStack.Push(nextState);
        }

        public void PopState() {
            stateStack.Pop();
        }

        public void Update(Player player) {
            ICharacterState state = (ICharacterState) stateStack.Peek();
            state.UpdateState(player);
            state.HandleInput(player);
        }
    }

    public abstract class ICharacterState 
    {
        public abstract void HandleInput(Player player);
        public abstract void UpdateState(Player player);
    }

    public class WalkingState : ICharacterState 
    {
        public override void HandleInput(Player player) {
            if (!player.InMotion()) {
                player.movementStateStack.PopState();
                player.animationController.ResetTrigger("walkTrigger");
                player.animationController.SetTrigger("idleTrigger");
            }
        }
        public override void UpdateState(Player player) {
            // process movement 
        }  
    }

    public class IdleState : ICharacterState 
    {
        public override void HandleInput(Player player) 
        {
            if (player.InMotion()) {
                player.movementStateStack.PushState(new WalkingState());
                player.animationController.ResetTrigger("idleTrigger");
                player.animationController.SetTrigger("walkTrigger");
            }
        }
        public override void UpdateState(Player player) {
            // do nothing for idle state
        }  
    }

    public class ZoomedOutState : ICharacterState
    {
        public override void HandleInput(Player player) {
            if (Input.GetMouseButtonDown(1)) {
                player.cursorStateStack.PushState(new ZoomedInState());
                player.Aim();
            }
        }
        public override void UpdateState(Player player) {
            /* nothing for now */
        }
    }

    public class ZoomedInState : ICharacterState
    {
        public override void HandleInput(Player player) {
            if (Input.GetMouseButtonUp(1)) {
                player.cursorStateStack.PopState();
                player.ResetAim();
            } 
        }
        public override void UpdateState(Player player) {
            player.DrawCursor();
        }
    }

    public class InventoryState : ICharacterState 
    {
        public override void HandleInput(Player player) {}
        public override void UpdateState(Player player) {}  
    }
}