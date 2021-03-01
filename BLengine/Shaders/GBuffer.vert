layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTexCoord;
layout (location = 2) in vec3 aNormal;
uniform mat4 model;
uniform mat4 viewProjection;
uniform mat4 inverseProjection;
uniform vec3 cameraPosition;

out vec2 texCoord;
out vec3 worldNormal;
out vec3 worldPosition;
out float depth;

void main()
{
	texCoord = aTexCoord;
	worldPosition = (vec4(aPosition, 1.0f) * model).rgb;

    worldNormal = normalize(vec4(aNormal.rgb, 0.0f) * model).rgb;
    gl_Position = vec4(aPosition, 1.0) * model * viewProjection;
    depth = gl_Position.z;// / gl_Position.w;
}

