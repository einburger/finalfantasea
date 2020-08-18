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

    public interface ICharacterState 
    {
        void HandleInput(Player player);
        void UpdateState(Player player);
    }

    public class SailingState : ICharacterState
    {
        public void HandleInput(Player player) {
            if (Input.GetKeyDown(KeyCode.F)) {
                player.movementStateStack.PopState();
                player.animationController.ResetTrigger("boatTrigger");
                player.animationController.SetTrigger("idleTrigger");
                player.ChangeMover();
            }
        }
        public void UpdateState(Player player) {
            player.LockToRaft();
        }  
    }

    public class WalkingState : ICharacterState 
    {
        public void HandleInput(Player player) {
            if (!player.InMotion()) {
                player.movementStateStack.PopState();
                player.animationController.ResetTrigger("walkTrigger");
                player.animationController.SetTrigger("idleTrigger");
            }
            if (Input.GetKeyDown(KeyCode.F)) {
                player.movementStateStack.PopState();
                player.movementStateStack.PushState(new SailingState());
                player.animationController.ResetTrigger("walkTrigger");
                player.animationController.SetTrigger("boatTrigger");
                player.ChangeMover();
            }
        }
        public void UpdateState(Player player) {
            // process movement 
        }  
    }

    public class IdleState : ICharacterState 
    {
        public void HandleInput(Player player) 
        {
            if (player.InMotion()) {
                player.movementStateStack.PushState(new WalkingState());
                player.animationController.ResetTrigger("idleTrigger");
                player.animationController.SetTrigger("walkTrigger");
            }
            if (Input.GetKeyDown(KeyCode.F)) {
                player.movementStateStack.PushState(new SailingState());
                player.animationController.ResetTrigger("idleTrigger");
                player.animationController.SetTrigger("boatTrigger");
                player.ChangeMover();
            }
        }
        public void UpdateState(Player player) {
            // do nothing for idle state
        }  
    }

    public class ZoomedOutState : ICharacterState
    {
        public void HandleInput(Player player) {
            if (Input.GetMouseButtonDown(1)) {
                player.animationController.SetTrigger("aimTrigger");
                player.cursorStateStack.PushState(new ZoomedInState());
                player.Aim();
            }
        }
        public void UpdateState(Player player) {
            /* nothing for now */
        }
    }

    public class ZoomedInState : ICharacterState
    {
        public void HandleInput(Player player) {
            if (Input.GetMouseButtonUp(1)) {
                player.cursorStateStack.PopState();
                player.animationController.ResetTrigger("aimTrigger");
                player.animationController.SetTrigger("idleTrigger");
                player.ResetAim();
            } 
            if (Input.GetMouseButtonDown(0)) {
                player.animationController.SetTrigger("castTrigger");
                player.CastLure();
            }
        }
        public void UpdateState(Player player) {
            player.DrawCursor();
        }
    }

    public class InventoryState : ICharacterState 
    {
        public void HandleInput(Player player) {}
        public void UpdateState(Player player) {}  
    }
}