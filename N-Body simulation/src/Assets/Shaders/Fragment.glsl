#version 330 core
in vec3 pos;
in vec3 normal;
in vec3 color;
out vec4 result;

void main() {
	
	vec3 ambient = vec3(0.05, 0.05, 0.05);
	vec3 normalnorm = normalize(normal);
	vec3 lightPos = vec3(150,150,150);
	vec3 lightdir = normalize(lightPos - pos);  

	vec3 lightColor = vec3(1.0, 1.0, 1.0);
	vec3 objectColor = color;

	float diff = max(dot(normalnorm, lightdir), 0.0);
	vec3 diffuse = diff * lightColor;

	result = vec4(((ambient + diffuse) * objectColor), 1.0);
}