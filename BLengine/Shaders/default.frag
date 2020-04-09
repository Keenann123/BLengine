uniform vec4 colour;
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

void main()
{
    //FragColor = vec4(worldPosition.rgb, 1.0f);
    vec4 diff = vec4(0.5f, 0.5f, 0.5f, 1.0f);
    vec3 norm = vec3(0.0f, 0.0f, 1.0f);
    vec3 lightDir = normalize(vec3(0.0f, 10.0f, 10.0f));
    vec3 viewDir = normalize(worldPosition - viewPos);
    vec3 halfwayDir = normalize(lightDir + viewDir);
    float kShininess = 2;

    vec4 result = vec4(DiffuseColour, 1.0f);
    #ifdef USE_DIFFUSE_TEXTURE
        diff = vec4(texture(diffuseTexture, texCoord).rgb, 1.0f);
        result = vec4(diff.rgb, 1.0f);
    #endif

    #ifdef USE_NORMAL_TEXTURE
        norm = vec3(texture(normalTexture, texCoord).rgb) * 2 - 1;
        result = vec4(norm.rgb * 2 - 1, 1.0f);
    #endif

    float lighting = max(dot(norm, lightDir), 0.0f);
    vec3 lightcolour = vec3(0.5f, 1.0f, 0.9f);
    vec4 colouredlight = vec4(lighting * lightcolour.r, lighting * lightcolour.g, lighting * lightcolour.b, 1.0f);
    float kPi = 3.14159265;
    float kEnergyConservation = ( 8.0 + kShininess ) / ( 8.0 * kPi ); 
    //vec3 halfwayDir = normalize(lightDir + viewDir); 
    float spec = kEnergyConservation * pow(max(dot(norm, halfwayDir), 0.0), kShininess);

    vec3 specular = spec * lightcolour;

    #ifdef LIT
        result = vec4(vec3(diff * colouredlight) + specular.rgb, 1.0f);
    #endif

    #ifdef DEBUG_WORLDPOSITION
        result = vec4(worldPosition.rgb, 1.0);// / 50, 0.0f, 0.0f, 1.0f);
    #endif

    #ifdef DEBUG_VIEWPOS
        result = vec4(viewPos.rgb, 1.0f);
    #endif

    #ifdef DEBUG_FOG
        float fog = distance(viewPos.rgb, worldPosition.rgb) / FogEndDistance;
        result = vec4(fog, fog, fog, 1.0f);
    #endif

    #ifdef DEBUG_VIEWDIRECTION
        result = vec4(viewDir, 1.0f);
    #endif

    #ifdef DEBUG_LIGHTING
        result = vec4(colouredlight.rgb + specular, 1.0f);
        result = vec4(viewDir.rgb, 1.0f);
        result = vec4(specular.rgb, 1.0f);
    #endif

    FragColor = result;
    //FragColor = vec4(worldNormal * 0.5 + 0.5, 1.0f);

}