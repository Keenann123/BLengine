in vec2 texCoord;
uniform sampler2D LightingTexture;

layout(location = 0) out vec4 FragColour;

void main()
{
	FragColour = texture(LightingTexture, texCoord);
}