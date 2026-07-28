shader_type canvas_item;

uniform vec4 hit_flash_color = vec4(1.0, 1.0, 1.0, 1.0);
uniform bool hit_flash_on = false;

varying vec4 modulate;

void vertex() {
	modulate = COLOR;
}

void fragment() {
	vec4 previous_color = texture(TEXTURE, UV);
	vec3 base_color = previous_color.rgb * modulate.rgb;
	vec3 new_color = hit_flash_on ? hit_flash_color.rgb : base_color;
	COLOR = vec4(new_color, previous_color.a * modulate.a);
}
