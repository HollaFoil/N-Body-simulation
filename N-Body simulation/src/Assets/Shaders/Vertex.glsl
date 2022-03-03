#version 330 core

layout (location = 0) in vec3 lPos;

out vec3 pos;
uniform mat4 view;
uniform mat4 projection;

void main() {
	gl_Position = projection * view * vec4(lPos, 1.0);
	pos = lPos;
}