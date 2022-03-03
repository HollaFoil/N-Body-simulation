#version 330 core
in vec3 pos;
out vec4 result;
void main() {
	result = vec4(pos.x/2, pos.y/2, pos.z/2, 1.0);
}