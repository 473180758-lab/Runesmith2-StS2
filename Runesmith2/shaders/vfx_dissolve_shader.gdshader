shader_type canvas_item;
render_mode blend_mix;

uniform sampler2D lut;

uniform vec2 pivot_offset = vec2(0.0, 0.0);

uniform sampler2D erosion_curve;
uniform float erosion_offset = 0.25;

uniform float erosion_time = 0.5;

uniform float v_mask = 0.5;
uniform float v_mask_offset = 0.1;

varying vec4 vertex_color;

void vertex() {
	vertex_color = COLOR;
	
	VERTEX.x *= TEXTURE_PIXEL_SIZE.x/TEXTURE_PIXEL_SIZE.y;
	VERTEX += pivot_offset * (1.0/TEXTURE_PIXEL_SIZE);
}

void fragment() {
	vec4 texture_color =  texture(TEXTURE, UV);

	vec4 lut_color = texture(lut, texture_color.rr);

	float erosion_curve_val = texture(erosion_curve, vec2(erosion_time)).r;
	float erosion_val = smoothstep(erosion_curve_val, erosion_curve_val + erosion_offset, texture_color.g);
	
	float v_mask_val = 1.0 - smoothstep(UV.y, UV.y + v_mask_offset, 1.0 - v_mask);

	COLOR = vec4(lut_color.rgb, erosion_val * lut_color.a * v_mask_val) * vertex_color;
}