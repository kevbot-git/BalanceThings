#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

sampler s0;
float lightness;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);
	float average = (color.r + color.g + color.b) / 3;
	float _lightness = lightness;
	if (_lightness > 1) _lightness = 1;
	if (_lightness < 0) _lightness = 0;
	color.rgb = pow(average, (1 - _lightness));
	return color;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}