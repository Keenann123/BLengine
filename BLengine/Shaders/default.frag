#version 330 core

uniform vec4 colour;
in vec2 texCoord;
uniform sampler2D texture0;

out vec4 FragColor;

void main()
{
    FragColor = vec4(texCoord.rg, 0, 1);
}