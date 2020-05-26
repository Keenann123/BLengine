layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTexCoord;

uniform vec3 cameraPosition;


out vec2 texCoord;

void main()
{
	texCoord = aTexCoord;

    gl_Position = vec4(aPosition, 1.0);
	

}