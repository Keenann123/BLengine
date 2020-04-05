layout (location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoord;
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform vec3 cameraPosition;

out vec2 texCoord;
out vec3 worldPosition;
out float depth;

void main()
{
	texCoord = aTexCoord;
	//worldPosition = (model * vec4(aPosition, 1.0f)).rgb;

    gl_Position = view * model * vec4(aPosition, 1.0f);

}