layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTexCoord;

uniform mat4 world;
uniform vec3 cameraPosition;

out vec2 texCoord;

void main()
{
	texCoord = aTexCoord;
    gl_Position = vec4(aPosition.xy,0.0,1.0);
}