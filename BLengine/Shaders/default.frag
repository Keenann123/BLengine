
uniform vec4 colour;
in vec2 texCoord;
in vec3 worldPosition;

uniform sampler2D diffuse;
uniform sampler2D normal;

out vec4 FragColor;

void main()
{
	//FragColor = vec4(worldPosition.rgb, 1.0f);
	vec4 tex1 = texture(diffuse, texCoord);
	vec4 tex2 = texture(normal, texCoord);
	vec4 test1 = vec4(1.0f, 0.0f, 0.0f, 1.0f);
	vec4 test2 = vec4(0.0f, 1.0f, 0.0f, 1.0f);

	#ifdef USE_DIFFUSE
		test1 *= tex1;
	#endif

	#ifdef USE_NORMAL
		test2 *= tex2;
	#endif
	
	FragColor = mix(test2, test1, 0.5);
}