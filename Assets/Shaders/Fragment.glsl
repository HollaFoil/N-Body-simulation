#version 330 core
in vec3 pos;
in vec3 normal;
in float height;
in vec3 uv;
out vec4 result;
//uniform sampler1D uTexture;
uniform samplerCube cubeTexture;

void main() {
	float heightCapped = height;
	if (height > 0 && height < 0.05) heightCapped = 0.05;
	vec3 ambient = vec3(0.5, 0.5, 0.5);
	vec3 normalnorm = normalize(normal);
	vec3 lightPos = vec3(150,150,150);
	vec3 lightdir = normalize(lightPos - pos);  

	vec3 lightColor = vec3(1.0, 1.0, 1.0);
	vec3 objectColor = texture(cubeTexture, uv).xyz;

	float diff = max(dot(normalnorm, lightdir), 0.0);
	vec3 diffuse = diff * lightColor;

	result = vec4(((ambient + diffuse) * objectColor), 1.0);
}