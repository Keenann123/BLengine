uniform mat4 world;
uniform vec3 cameraPosition;

out vec3 viewRay;
out vec2 texCoord;

void main()
{
	texCoord = aTexCoord;
    gl_Position = vec4(aPosition.xy,0.0,1.0);

	viewRay = ((world * vec4(aPosition, 0.0f)).rgb - cameraPosition).rgb;
} 