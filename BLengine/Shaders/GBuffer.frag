uniform vec3 viewPos;
uniform float FogEndDistance;
in vec2 texCoord;
in vec3 worldNormal;
in vec3 worldPosition;
in float depth;
uniform sampler2D diffuseTexture;
uniform sampler2D normalTexture;
uniform sampler2D specularTexture;
uniform sampler2D roughnessTexture;
uniform float roughness;
uniform float metallic;

layout(location = 0) out vec4 DiffuseColour;
layout(location = 1) out vec4 NormalColour;
layout(location = 2) out vec4 SpecularColour;
layout(location = 3) out float DepthOut;
layout(location = 4) out vec4 PositionOut;

//remove this code when we realise we don't need it :)
float linearize_depth(float d, float nearz, float farz)
{
    return (d-nearz)/(farz-nearz);
}

void main()
{
    DiffuseColour = vec4(texture(diffuseTexture, texCoord).rgb, 1f);
    NormalColour = vec4(worldNormal * 0.5 + 0.5, 1.0f);
    SpecularColour = vec4(1.0f, 0.0f, 1.0f, 1.0f);
    DepthOut = linearize_depth(depth, 1.0f, 10000.0f);
    PositionOut = vec4(worldPosition.rgb, 1.0f);
}