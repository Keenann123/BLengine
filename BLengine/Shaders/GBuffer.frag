uniform vec3 viewPos;
uniform float FogEndDistance;
in vec2 texCoord;
in vec3 worldNormal;
in vec3 worldPosition;
in float depth;
uniform vec3 DiffuseColour;
uniform sampler2D diffuseTexture;
uniform sampler2D normalTexture;

out vec4 FragColor;
out vec4 FragNormal;
out vec4 FragSpecular;
out float Depth;

void main()
{
    //FragColor = vec4(worldPosition.rgb, 1.0f);
    vec4 diff = vec4(DiffuseColour, 1.0f);
    vec4 spec = vec4(SpecularColour, SpecularRoughness);
    vec3 norm = worldNormal;

    vec4 resultDiffuse = vec4(DiffuseColour.rgb, 1.0f);
    vec4 resultSpecular = vec4(SpecularColour, SpecularRoughness);
    vec4 resultNormal = vec4(worldNormal, 1.0f);

    #ifdef USE_DIFFUSE_TEXTURE
        diff = vec4(texture(diffuseTexture, texCoord).rgb, 1.0f);
        resultDiffuse = vec4(diff.rgb, 1.0f);
    #endif

    #ifdef USE_NORMAL_TEXTURE
        norm = vec3(texture(normalTexture, texCoord).rgb) * 2 - 1;
        resultNormal = vec4(norm, 1.0f);
    #endif

    gl_FragData[0] = resultDiffuse;
    gl_FragData[1] = resultNormal;
    gl_FragData[2] = resultSpecular;
    gl_FragData[3] = resultDepth;

}