in vec2 texCoord;
uniform sampler2D GBufferDiffuse;
uniform sampler2D LightingBuffer;

layout(location = 0) out vec4 FragColour;

void main()
{ 
	vec4 output;
	
	output = texture(LightingBuffer, texCoord);// * texture(GBufferDiffuse, texCoord);
	FragColour = output;
}