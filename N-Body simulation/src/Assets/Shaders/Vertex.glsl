﻿#version 330 core

layout (location = 0) in vec3 lPos;
layout (location = 1) in vec3 lNormal;

out vec3 pos;
out vec3 normal;
uniform mat4 view;
uniform mat4 projection;

void main() {
	gl_Position = projection * view * vec4(lPos, 1.0);
	pos = lPos;

	normal = lNormal;
}