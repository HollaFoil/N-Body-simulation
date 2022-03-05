#version 330 core
in vec3 pos;
in vec3 normal;
out vec4 result;

void main() {
	vec3 lightdir = vec3(0, -1, 0);

	vec3 lightColor = vec3(1.0, 1.0, 1.0);
	vec3 objectColor = vec3(0.5 + pos.x/10, 0.5 + pos.y/2.5, 0.5 + pos.z/10);
	if (length(pos) - 0.0001 < 1) objectColor = vec3(0.15,0.50,0.98);

	float diff = max(dot(normal, lightdir), 0.0);
	vec3 diffuse = diff * lightColor;

	result = vec4((diffuse * objectColor), 1.0);
	//result = vec4(pos.x/2, pos.y/2, pos.z/2, 1.0);
}