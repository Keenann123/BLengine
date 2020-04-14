uniform vec3 viewPos;
uniform float FogEndDistance;
in vec2 texCoord;
in vec3 worldNormal;
in vec3 worldPosition;
in float depth;
uniform vec3 DiffuseColour;
uniform sampler2D diffuseTexture;
uniform sampler2D normalTexture;
uniform sampler2D specularTexture;
uniform sampler2D depthTexture;

uniform vec3 lightColour;
unifrom vec3 lightDirection;

out vec4 FragColor;

void main()
{
    vec4 diff = vec4(0.5f, 0.5f, 0.5f, 1.0f);
    vec3 norm = vec3(0.0f, 0.0f, 1.0f);
    vec3 lightDir = normalize(lightDirection);
    vec3 viewDir = normalize(worldPosition - viewPos);
    vec3 halfwayDir = normalize(lightDir + viewDir);
    float kShininess = 32;

    diff = vec4(texture(diffuseTexture, texCoord).rgb, 1.0f);
    norm = vec3(texture(normalTexture, texCoord).rgb) * 2 - 1;


    float lighting = max(dot(norm, lightDir), 0.0f);
    vec3 lightcolour = vec3(0.5f, 1.0f, 0.9f);
    vec4 colouredlight = vec4(lighting * lightcolour.r, lighting * lightcolour.g, lighting * lightcolour.b, 1.0f);
    float kPi = 3.14159265;
    float kEnergyConservation = ( 8.0 + kShininess ) / ( 8.0 * kPi ); 
    float spec = kEnergyConservation * pow(max(0.0, dot(reflect(lightDir, norm), viewDir)), kShininess);// pow(max(dot(norm, halfwayDir), 0.0), kShininess);
    spec *= lighting;
    vec3 specular = spec * lightcolour;

    result = vec4(vec3(diff * colouredlight) + specular.rgb, 1.0f);

    FragColor = result;
    //FragColor = vec4(worldNormal * 0.5 + 0.5, 1.0f);

}