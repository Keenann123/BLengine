in vec2 texCoord;
uniform sampler2D LightingBuffer;

layout(location = 0) out vec4 FragColour;

void main()
{ 
	
	vec4 output;
	
	output = vec4(texture(LightingBuffer, texCoord).rgb + vec3(light * lightColour.r, light * lightColour.g, light * lightColour.b), 1.0f);
	FragColour = output;
}