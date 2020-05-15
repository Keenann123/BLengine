in vec2 texCoord;
uniform sampler2D GBufferDiffuse;
uniform sampler2D GBufferNormal;
uniform sampler2D GBufferSpecular;
uniform sampler2D GBufferDepth;

layout(location = 0) out vec4 FragColour;

void main()
{
	vec4 diffuse = vec4(texture(GBufferDiffuse, texCoord * 2 - vec2(1, 0)).rgb, 1.0f);
	vec4 depth = vec4(texture(GBufferDepth, texCoord * 2 - 1).rgb, 1.0f);
	vec4 normal = vec4(texture(GBufferNormal, texCoord * 2 - vec2(0, 1)).rgb, 1.0f);
	vec4 specular = vec4(texture(GBufferSpecular, texCoord * 2).rgb, 1.0f);
	FragColour = normal + depth + diffuse + specular;
	//FragColour = vec4(texCoord.xy * 0.5, 0.0f, 1.0f);
}