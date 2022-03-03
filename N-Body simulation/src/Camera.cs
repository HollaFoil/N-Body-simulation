using GlmSharp;
using N_Body_simulation.src.Event.Evenets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Body_simulation.src
{
    public class Camera
    {
        private vec3 directionPressed;
        private float speed = 0.005f;
        vec3 position;
        vec2 facing;
        public Camera()
        {
            DirectionalKeyPressEvent.Listeners += OnDirectionalKeyPress;
            MouseMovedEvent.Listeners += OnMouseMoved;
            position = new vec3();
            facing = new vec2();
            directionPressed = new vec3();
        }
        private void OnDirectionalKeyPress(DirectionalKeyPressEvent e)
        {
            directionPressed = e.GetDirection();
        }
        private void OnMouseMoved(MouseMovedEvent e)
        {
            facing += e.GetChange();
            if (facing.x <= -180.0f) facing.x += 360.0f;
            if (facing.x > 180.0f) facing.x -= 360.0f;
            if (facing.y < -90.0f) facing.y = -90.0f;
            if (facing.y > 90.0f) facing.y = 90.0f;
        }
        public void Update(int elapsedTime)
        {
            position += GetMovementDirection() * elapsedTime * speed;
            //Console.WriteLine((position + "\n") + GetYawPitch() + "\n");
        }
        private vec3 GetMovementDirection()
        {
            vec3 force = new vec3(0, 0, 0);
            force.y += directionPressed.y;
            force.z += (glm.Sin(glm.Radians(facing.x + 180)) * directionPressed.z +
                           glm.Cos(glm.Radians(facing.x)) * directionPressed.x);
            force.x += (glm.Sin(glm.Radians(facing.x)) * directionPressed.x +
                           glm.Cos(glm.Radians(facing.x)) * directionPressed.z);
            return force;
        }
        /*public static Camera GetInstance()
        {
            return this;
        }*/
        public vec2 GetYawPitch()
        {
            return facing;
        }
        public vec3 GetPosition()
        {
            return position;
        }

        vec3 up = new vec3(0, 1, 0);
        public vec3 GetDirection()
        {
            float yaw = -GetYawPitch().x;
            float pitch = GetYawPitch().y;
            vec3 direction = new vec3();
            direction.x = glm.Cos(glm.Radians(yaw)) * glm.Cos(glm.Radians(pitch));
            direction.y = glm.Sin(glm.Radians(pitch));
            direction.z = glm.Sin(glm.Radians(yaw)) * glm.Cos(glm.Radians(pitch));

            return glm.Normalized(direction);
        }
        public mat4 GetLookAtMatrix()
        {
            vec3 location = GetPosition();
            vec3 direction = GetDirection();
            return mat4.LookAt(location, location + direction, up);
        }
    }
}
