in vec2 texCoord;
uniform sampler2D LightingBuffer;
uniform sampler2D GBufferDiffuse;

layout(location = 0) out vec4 FragColour;

void main()
{
	FragColour = texture(LightingBuffer, texCoord) * texture(GBufferDiffuse, texCoord);
}