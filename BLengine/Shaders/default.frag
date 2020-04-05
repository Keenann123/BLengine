uniform vec4 colour;
in vec2 texCoord;
in vec3 worldPosition;
in float depth;

uniform sampler2D diffuse;
uniform sampler2D normal;

out vec4 FragColor;

void main()
{
	//FragColor = vec4(worldPosition.rgb, 1.0f);
	vec4 diff = vec4(0.5f, 0.5f, 0.5f, 1.0f);
	vec3 norm = vec3(0.5f, 0.5f, 1.0f);

	vec4 result = vec4(1.0f, 0.0f, 0.0f, 1.0f);
	#ifdef USE_DIFFUSE
		diff = vec4(texture(diffuse, texCoord).rgb, 1.0f);
		result = vec4(diff.rgb, 1.0f);
	#endif

	#ifdef USE_NORMAL
		norm = vec3(texture(normal, texCoord).rgb);
		result = vec4(norm.rgb, 1.0f);
	#endif
	
	float lighting = dot(norm * 2 - 1, normalize(vec3(0.0f, 10.0f, 10.0f) - worldPosition));

	#ifdef DEBUG_LIGHTING
	result = vec4(lighting, lighting, lighting, 1.0f);
	#endif

	#ifdef LIT
	result = vec4(vec3(lighting, lighting, lighting) * diff.rgb, 1.0f);
	#endif

	#ifdef DEBUG_WORLDPOSITION
	result = vec4(worldPosition.rgb, 1.0f);
	#endif

	FragColor = result;
	//FragColor = vec4(depth, 0.0f, 0.0f, 1.0f);

}