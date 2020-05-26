in vec2 texCoord;
uniform sampler2D GBufferDiffuse;
uniform sampler2D GBufferNormal;
uniform sampler2D GBufferSpecular;
uniform sampler2D GBufferDepth;

uniform vec3 lightColour;
unifrom vec3 lightDirection;

layout(location = 0) out vec4 FragColour;

void main()
{
    vec3 lightDir = vec3(0.5, 0.5, 1.0); //= normalize(lightDirection);
    vec4 diff;
    vec3 norm;

    diff = vec4(texture(GBufferDiffuse, texCoord).rgb, 1.0f);
    norm = vec3(texture(GBufferNormal, texCoord).rgb) * 2 - 1;


    float lighting = max(dot(norm, lightDir), 0.0f);
    vec3 lightcolour = lightColour;
    vec4 colouredlight = vec4(lighting * lightcolour.r, lighting * lightcolour.g, lighting * lightcolour.b, 1.0f);
    
    result = vec4(vec3(diff * colouredlight), 1.0f);

    FragColour = vec4(1.0f, 0.0f, 0.0f, 1.0f);
}