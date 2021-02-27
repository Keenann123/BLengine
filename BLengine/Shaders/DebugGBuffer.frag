in vec2 texCoord;
uniform sampler2D GBufferDiffuse;
uniform sampler2D GBufferNormal;
uniform sampler2D GBufferSpecular;
uniform sampler2D GBufferDepth;
uniform sampler2D LightingBuffer;

uniform vec3 cameraPosition;

in vec3 viewRay;
layout(location = 0) out vec4 FragColour;

void main()
{
	vec4 diffuse = vec4(texture(GBufferDiffuse, texCoord * 2 - vec2(1, 0)).rgb, 1.0f);
	vec4 depth = vec4(texture(GBufferDepth, texCoord * 2 - 1).rgb, 1.0f);
	vec4 normal = texture(GBufferNormal, texCoord * 2 - vec2(0, 1));
	vec4 specular = vec4(texture(GBufferSpecular, texCoord * 2).rgb, 1.0f);
	vec4 lighting = vec4(texture(LightingBuffer, texCoord * 2).rgb, 1.0f);

	vec4 pixelPosition = vec4(cameraPosition - (normalize(viewRay) * depth.r), 0.0f) * normal.a;
	FragColour = normal + vec4(depth.r * 100, depth.r * 100, depth.r * 100, 1.0f) + diffuse + lighting;
	//FragColour = vec4(texCoord.xy * 0.5, 0.0f, 1.0f);

	#ifdef DEBUG_DIFFUSE_ONLY
	FragColour = vec4(texture(GBufferDiffuse, texCoord).rgb, 1.0f);
	#endif

	#ifdef LIT
	FragColour = vec4(texture(LightingBuffer, texcoord).rgb, 1.0f);
	#endif

}