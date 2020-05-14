in vec2 texCoord;
uniform sampler2D GBufferDiffuse;
uniform sampler2D GBufferNormal;
uniform sampler2D GBufferSpecular;
uniform sampler2D GBufferDepth;

layout(location = 0) out vec4 FragColour;

void main()
{
	FragColour = vec4(texture(GBufferDiffuse, texCoord).rgb, 1.0f);
	//FragColour = vec4(texCoord.xy * 0.5, 0.0f, 1.0f);
}