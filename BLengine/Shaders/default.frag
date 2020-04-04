#version 330 core

uniform vec4 colour;
in vec2 texCoord;
in vec3 worldPosition;

uniform sampler2D diffuse;
uniform sampler2D normal;

out vec4 FragColor;

void main()
{
	//FragColor = vec4(worldPosition.rgb, 1.0f);
    FragColor = FragColor = mix(texture(diffuse, texCoord), texture(normal, texCoord), 0.5);
}