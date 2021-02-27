in vec2 texCoord;
uniform sampler2D GBufferDiffuse;
uniform sampler2D GBufferNormal;
uniform sampler2D GBufferSpecular;
uniform sampler2D GBufferDepth;
uniform sampler2D LightingBuffer;

uniform vec3 cameraPosition;
uniform vec3 lightColour;
uniform vec3 lightPosition;
in vec3 viewRay;
layout(location = 0) out vec4 FragColour;

void main()
{
	vec4 diffuse = vec4(texture(GBufferDiffuse, texCoord * 3 - vec2(2, 0)).rgb, 1.0f);
	vec4 depth = vec4(texture(GBufferDepth, texCoord * 3 - 2).rgb, 1.0f);
	vec4 normal = texture(GBufferNormal, texCoord * 3 - vec2(0, 2));
	vec4 normal2 = vec4(texture(GBufferNormal, texCoord).rgb * 2.0f - 1.0f, texture(GBufferNormal, texCoord).w);
	vec4 specular = vec4(texture(GBufferSpecular, texCoord * 3).rgb, 1.0f);
	vec4 lighting = vec4(texture(LightingBuffer, texCoord * 3).rgb, 1.0f);

	vec4 pixelPosition = vec4(cameraPosition - (normalize(viewRay) * depth.r), 0.0f) / 10.0f;
	FragColour = normal + vec4(depth.r * 100, depth.r * 100, depth.r * 100, 1.0f) + diffuse + lighting;
	//FragColour = vec4(texCoord.xy * 0.5, 0.0f, 1.0f);

	#ifdef LIT
		vec3 diff = texture(GBufferDiffuse, texCoord).rgb;
		float light = dot(normal2.rgb, normalize(lightPosition));
		FragColour = texture(LightingBuffer, texCoord) + vec4(vec3(light * lightColour.r * normal2.w, light * lightColour.g * normal2.w, light * lightColour.b * normal2.w), 1.0f);
	#endif

}