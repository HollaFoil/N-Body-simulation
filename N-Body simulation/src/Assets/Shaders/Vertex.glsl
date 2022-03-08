#version 330 core

layout (location = 0) in vec3 lPos;
layout (location = 1) in vec3 lNormal;
layout (location = 2) in vec3 lColor;

out vec3 pos;
out vec3 normal;
out vec3 color;
uniform mat4 transform;
uniform mat4 rotation;
uniform mat4 view;
uniform mat4 projection;

void main() {
	gl_Position = projection * view * transform * rotation * vec4(lPos, 1.0);
	pos = lPos;
	color = lColor;

	normal = normalize(vec3((rotation * vec4(lNormal, 1.0)).xyz));
}