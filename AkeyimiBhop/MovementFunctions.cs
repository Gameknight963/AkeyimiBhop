using UnityEngine;

namespace AkeyimiBhop
{
    public static class MovementFunctions
    {
        private static MovementConfig Config => MovementConfig.Instance;

        public static void ApplyFriction(ref Vector3 velocity)
        {
            float speed = velocity.magnitude;
            if (speed < 0.1f) return;

            float drop = speed * Config.Friction * Time.fixedDeltaTime;
            float newSpeed = Mathf.Max(speed - drop, 0.0f);

            velocity *= newSpeed / speed;
        }
        public static void Accelerate(ref Vector3 velocity, Vector3 wishDir, float wishSpeed, float accel)
        {
            float currentSpeed = Vector3.Dot(velocity, wishDir);
            float addSpeed = wishSpeed - currentSpeed;
            if (addSpeed <= 0) return;

            float accelSpeed = accel * Time.fixedDeltaTime * wishSpeed;
            if (accelSpeed > addSpeed) accelSpeed = addSpeed;

            velocity += wishDir * accelSpeed;
        }

        public static void AirMove(ref Vector3 velocity, Vector3 wishDir)
        {
            float wishSpeed = Config.MaxGroundSpeedUnity;

            if (wishSpeed > Config.AirCapUnity)
            {
                wishSpeed = Config.AirCapUnity;
            }

            Accelerate(ref velocity, wishDir, wishSpeed, Config.AirAccelUnity);
        }

        public static void GroundMove(ref Vector3 velocity, Vector3 wishDir, bool applyFriction)
        {
            if (applyFriction)
            {
                ApplyFriction(ref velocity);
            }

            float wishSpeed = Config.MaxGroundSpeedUnity;
            Accelerate(ref velocity, wishDir, wishSpeed, Config.GroundAccelUnity);
        }
    }
}
