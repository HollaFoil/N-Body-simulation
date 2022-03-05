#version 330 core
in vec3 pos;
in vec3 normal;
in vec3 color;
out vec4 result;

void main() {
	vec3 lightdir = vec3(0, -1, -1);

	vec3 lightColor = vec3(1.0, 1.0, 1.0);
	vec3 objectColor = color;

	float diff = max(dot(normal, lightdir), 0.0);
	vec3 diffuse = diff * lightColor;

	result = vec4((diffuse * objectColor), 1.0);
	//result = vec4(pos.x/2, pos.y/2, pos.z/2, 1.0);
}