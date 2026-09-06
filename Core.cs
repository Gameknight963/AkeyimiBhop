using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using MelonLoader.Utils;
using UnityEngine;
using UnityEngine.UIElements;

[assembly: MelonInfo(typeof(AkeyimiBhop.Core), "AkeyimiBhop", "1.0.0", "Gameknight963", null)]
[assembly: MelonGame("Sonnick Games", "Akeyimi")]
[assembly: MelonAuthorColor(255, 86, 65, 157)]

namespace AkeyimiBhop
{
    public class Core : MelonMod
    {
        public override void OnInitializeMelon()
        {
        }
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName != "OutdoorsScene") return; // this check is redundant but future proof ig

        }

        private static class PlayerControllerPatches
        {
            private static Vector3 velocity;

            [HarmonyPatch(typeof(PlayerController), "Update")]
            private class UpdatePatch
            {
                private static bool Prefix(ref PlayerController __instance)
                {
                    Vector2 _moveInput = new Vector2(
                        Input.GetAxisRaw("Horizontal"),
                        Input.GetAxisRaw("Vertical")
                    );

                    Vector3 wishDir = __instance.transform.TransformDirection(
                        new Vector3(_moveInput.x, 0f, _moveInput.y)
                    ).normalized;

                    if (__instance.controller.isGrounded)
                    {
                        if (velocity.y < 0) velocity.y = -2f;
                        MovementFunctions.GroundMove(ref velocity, wishDir, true);
                    }
                    else
                    {
                        MovementFunctions.AirMove(ref velocity, wishDir);
                        velocity.y -= MovementConfig.Instance.GravityUnity * Time.deltaTime;
                    }

                    if ((Input.GetKeyDown(KeyCode.Space) || Input.GetAxis("Mouse ScrollWheel") != 0) && __instance.controller.isGrounded)
                    {
                        velocity.y = MovementConfig.Instance.JumpVelocityUnity;
                    }

                    __instance.controller.Move(velocity * Time.deltaTime);
                    __instance.Look();
                    return false;
                }
            }
        }
    }
}