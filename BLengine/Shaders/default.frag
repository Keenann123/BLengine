
uniform vec4 colour;
in vec2 texCoord;
in vec3 worldPosition;

uniform sampler2D diffuse;
uniform sampler2D normal;

out vec4 FragColor;

void main()
{
	//FragColor = vec4(worldPosition.rgb, 1.0f);
	vec4 test1 = vec4(1.0f, 0.0f, 0.0f, 1.0f);
	vec4 test2 = vec4(0.0f, 1.0f, 0.0f, 1.0f);

	#ifdef USE_TEST1
		test1 = vec4(texture(diffuse, texCoord).rgb, 1.0f);
	#endif

	#ifdef USE_TEST2
		test2 = vec4(texture(normal, texCoord).rgb, 1.0f);
	#endif
	
	FragColor = mix(test1, test2, 0.5);

}