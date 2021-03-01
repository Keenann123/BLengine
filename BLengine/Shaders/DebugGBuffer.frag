in vec2 texCoord;
uniform sampler2D GBufferDiffuse;
uniform sampler2D GBufferNormal;
uniform sampler2D GBufferSpecular;
uniform sampler2D GBufferDepth;
uniform sampler2D GBufferPosition;
uniform sampler2D LightingBuffer;


uniform mat4 view;
uniform mat4 projection;
uniform vec3 cameraUp;
uniform vec3 cameraForward;

uniform vec3 cameraPosition;

layout(location = 0) out vec4 FragColour;

vec2 Resolution = vec2(1920, 1080);

vec3 wpos()
{
	return texture(GBufferPosition, texCoord).rgb;
}

void main()
{
	vec4 diffuse = vec4(texture(GBufferDiffuse, texCoord * 2 - vec2(1, 0)).rgb, 1.0f);
	vec4 depth = vec4(texture(GBufferDepth, texCoord * 2 - 1).rgb, 1.0f);
	vec4 normal = texture(GBufferNormal, texCoord * 2 - vec2(0, 1));
	vec4 specular = vec4(texture(GBufferSpecular, texCoord * 2).rgb, 1.0f);
	vec4 lighting = vec4(texture(LightingBuffer, texCoord * 2).rgb, 1.0f);//* texture(GBufferNormal, texCoord*2).w, 1.0f);

	FragColour = normal + vec4(depth.r * 10f, depth.r * 10f, depth.r * 10f, 1.0f) + diffuse + lighting;
	//FragColour = vec4(texCoord.xy * 0.5, 0.0f, 1.0f);

	#ifdef DEBUG_DIFFUSE_ONLY
		FragColour = vec4(texture(GBufferDiffuse, texCoord).rgb, 1.0f);
	#endif

	#ifdef DEBUG_NORMAL_ONLY
		FragColour = vec4(texture(GBufferNormal, texCoord).rgb, 1.0f);
	#endif

	#ifdef DEBUG_SPECULAR_ONLY
		FragColour = vec4(texture(GBufferSpecular, texCoord).rgb, 1.0f);
	#endif
	
	#ifdef DEBUG_DEPTH_ONLY
		float d1 = texture(GBufferDepth, texCoord).r;
		FragColour = vec4(d1 * 10f, d1 * 10f, d1 * 10f, 1.0f);
	#endif

	#ifdef DEBUG_WORLD_POSITION
		float d1 = texture(GBufferDepth, texCoord).r;
		FragColour.rgb = wpos() / 102.4f;
	#endif
}