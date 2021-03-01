in vec2 texCoord;

uniform sampler2D GBufferDiffuse;
uniform sampler2D GBufferNormal;
uniform sampler2D GBufferSpecular;
uniform sampler2D GBufferDepth;
uniform sampler2D GBufferPosition;
uniform sampler2D LightingBuffer;


layout(location = 0) out vec4 FragColour;

void main()
{ 
	vec4 output;
	
	output = texture(LightingBuffer, texCoord);// * vec4(texture(GBufferDiffuse, texCoord).rgb, 1.0f); // only want RGB because alpha is roughness as declared in GBuffer.frag :)
	FragColour = output;
}