shader_type canvas_item;

uniform float vitesse : hint_range(0.1, 2.0) = 0.5;
uniform float intensite : hint_range(0.1, 5.0) = 2.0;
uniform vec4 couleur_bleu : source_color = vec4(0.149, 0.098, 0.471, 1.0);
uniform vec4 couleur_rouge : source_color = vec4(0.471, 0.098, 0.129, 1.0);

void fragment() {
    vec2 uv = UV;
    float t = TIME * vitesse;
    
    float mouvement = sin(uv.x * 10.0 + t) + sin(uv.y * 8.0 + t * 0.5);
    mouvement += sin((uv.x + uv.y) * 5.0 + t * 0.8);
    mouvement *= 0.5;
    
    vec3 final_color = mix(couleur_bleu.rgb, couleur_rouge.rgb, mouvement * 0.5 + 0.5);
    
    final_color += 0.05 * sin(t + uv.x * 20.0);
    
    COLOR = vec4(final_color, 1.0);
}