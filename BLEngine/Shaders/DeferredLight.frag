in vec2 texCoord;
uniform sampler2D GBufferDiffuse;
uniform sampler2D GBufferNormal;
uniform sampler2D GBufferSpecular;
uniform sampler2D GBufferDepth;
uniform sampler2D GBufferPosition;
uniform sampler2D LightingBuffer;

uniform vec3 cameraPosition;
uniform vec3 lightColour;
uniform vec3 lightPosition;
uniform float lightIntensity;
uniform float lightRadius;

layout(location = 0) out vec4 FragColour;

const float PI = 3.14159265359;

vec3 wpos()
{
	return texture(GBufferPosition, texCoord).rgb;
}

vec3 fresnelSchlick(float cosTheta, vec3 F0)
{
    return F0 + (1.0 - F0) * pow(max(1.0 - cosTheta, 0.0), 5.0);
}  

float DistributionGGX(vec3 N, vec3 H, float roughness)
{
    float a      = roughness*roughness;
    float a2     = a*a;
    float NdotH  = max(dot(N, H), 0.0);
    float NdotH2 = NdotH*NdotH;
	
    float num   = a2;
    float denom = (NdotH2 * (a2 - 1.0) + 1.0);
    denom = PI * denom * denom;
	
    return num / denom;
}

float GeometrySchlickGGX(float NdotV, float roughness)
{
    float r = (roughness + 1.0);
    float k = (r*r) / 8.0;

    float num   = NdotV;
    float denom = NdotV * (1.0 - k) + k;
	
    return num / denom;
}
float GeometrySmith(vec3 N, vec3 V, vec3 L, float roughness)
{
    float NdotV = max(dot(N, V), 0.0);
    float NdotL = max(dot(N, L), 0.0);
    float ggx2  = GeometrySchlickGGX(NdotV, roughness);
    float ggx1  = GeometrySchlickGGX(NdotL, roughness);
	
    return ggx1 * ggx2;
}

void main()
{
    vec3 albedo = texture(GBufferDiffuse, texCoord).rgb;
    float roughness = 0.2f;//texture(GBufferDiffuse, texCoord).w;
    float metallic = texture(GBufferSpecular, texCoord).w;


	vec3 N = vec3(texture(GBufferNormal, texCoord).rgb * 2.0f - 1.0f);
    float mask = texture(GBufferNormal, texCoord).w;
	vec3 V = normalize(cameraPosition - wpos());
    
    float distance = 1.0f;

	vec3 F0 = vec3(0.04); 
    F0 = mix(F0, albedo, metallic);
	           
    // reflectance equation
    vec3 Lo = vec3(0.0);

	#ifdef LIT

        vec3 L = normalize(lightPosition - wpos());
        vec3 H = normalize(V + L);

        	    
        #ifdef LIGHT_DIRECTIONAL
            L = lightPosition;
        #endif 
        
        vec3 radiance;
        
        // cook-torrance brdf
        float NDF = DistributionGGX(N.rgb, H, roughness);        
        float G   = GeometrySmith(N.rgb, V, L, roughness);      
        vec3 F    = fresnelSchlick(max(dot(H, V), 0.0), F0);       
        
        vec3 kS = F;
        vec3 kD = vec3(1.0) - kS;
        kD *= 1.0 - metallic;	  
        

        vec3 numerator    = NDF * G * F;
        float denominator = 4.0 * max(dot(N, V), 0.0) * max(dot(N, L), 0.0);
        vec3 specular     = numerator / max(denominator, 0.001);  
            
        float NdotL = max(dot(N.rgb, L), 0.0);       
        
		#ifdef LIGHT_DIRECTIONAL
            radiance = lightColour * lightIntensity;        
            Lo += (kD * albedo / PI + specular) * radiance * NdotL; 
            FragColour = vec4(texture(LightingBuffer, texCoord).rgb + (Lo * mask), 1.0f);   
        #endif

		#ifdef LIGHT_POINT
            distance = length(lightPosition - wpos());
			float attenuation = 1.0f / (distance * distance);
            radiance = lightColour * clamp( attenuation * lightRadius, 0.0f, 1.0f) * lightIntensity;               
            Lo += (kD * albedo / PI + specular) * radiance * NdotL; 
			float light = clamp(dot(N.rgb, normalize(L)), 0.0f, 1.0f) * lightIntensity;
			FragColour = vec4(texture(LightingBuffer, texCoord).rgb + (Lo * mask), 1.0f);// * texture(GBufferDiffuse, texCoord).rgb), 1.0f);
		#endif
	
	#endif

}