#version 330 core

layout (location = 0) in vec3 lPos;
layout (location = 1) in vec3 lNormal;
layout (location = 2) in float lHeight;

out vec3 pos;
out vec3 normal;
out float height;
out vec3 uv;
uniform mat4 transform;
uniform mat4 rotation;
uniform mat4 view;
uniform mat4 projection;

void main() {
	uv = lPos;
	vec4 worldpos = transform * rotation * vec4(lPos, 1.0);
	gl_Position = projection * view * worldpos;
	pos = vec3(worldpos.xyz);
	height = lHeight;

	normal = vec3((rotation * vec4(lNormal, 1.0)).xyz);
}