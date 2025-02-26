Shader "Hidden/Shader/Noise"
{
    Properties
    {
        // This property is necessary to make the CommandBuffer.Blit bind the source texture to _MainTex
        _MainTex("Main Texture", 2DArray) = "grey" {}
    }

    HLSLINCLUDE

    #pragma target 4.5
    #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch

    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/FXAA.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/RTUpscale.hlsl"

    struct Attributes
    {
        uint vertexID : SV_VertexID;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct Varyings
    {
        float4 positionCS : SV_POSITION;
        float2 texcoord   : TEXCOORD0;
        UNITY_VERTEX_OUTPUT_STEREO
    };

    Varyings Vert(Attributes input)
    {
        Varyings output;
        UNITY_SETUP_INSTANCE_ID(input);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
        output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
        output.texcoord = GetFullScreenTriangleTexCoord(input.vertexID);
        return output;
    }

    // List of properties to control your post process effect
    float _Intensity;
    float _noise_seed;
    float _noise_range;
    TEXTURE2D_X(_MainTex);


    float random(float2 co){
        // float random = dot(co, float3(12.9898, 78.233, 37.719));
        // random = frac(random * 143758.5453);
        // return random;


        return frac(sin(dot(co, float2(_noise_seed * 12.9898 , 78.233))) * 43758.5453123);
    }


    float Remap (float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }


    float4 CustomPostProcess(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        // noise effect
        float range = _noise_range;
        float3 noise_color = float3(random(input.texcoord), random(input.texcoord), random(input.texcoord));
        noise_color = (noise_color * float3(range, range, range));
        
        
        float3 sourceColor = SAMPLE_TEXTURE2D_X(_MainTex, s_linear_clamp_sampler, ClampAndScaleUVForBilinearPostProcessTexture(input.texcoord.xy)).xyz;

        
        //float3 color = lerp(sourceColor, Luminance(sourceColor), _Intensity);

        float3 Intensity = float3(_Intensity,_Intensity,_Intensity);
        //float3 color = (noise_color * Intensity) + (sourceColor * (float3(1,1,1) - Intensity));

        sourceColor.x = Remap(sourceColor.x, 0, 1, 0.05, 2);
        sourceColor.y = Remap(sourceColor.y, 0, 1, 0.05, 2);
        sourceColor.z = Remap(sourceColor.z, 0, 1, 0.05, 2);
        // sourceColor = Remap(sourceColor, 0, 1, 0.05, 2);
        float3 color = (noise_color * Intensity) + (sourceColor * (float3(1,1,1) - Intensity));


        return float4(color, 1);
    }

    ENDHLSL

    SubShader
    {
        Tags{ "RenderPipeline" = "HDRenderPipeline" }
        Pass
        {
            Name "Noise"

            ZWrite Off
            ZTest Always
            Blend Off
            Cull Off

            HLSLPROGRAM
                #pragma fragment CustomPostProcess
                #pragma vertex Vert
            ENDHLSL
        }
    }
    Fallback Off
}
