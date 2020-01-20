#version 330
 
in vec3 a_Position;
in  vec3 a_Colour;
out vec4 v_Color;
uniform mat4 WorldViewProj;
 
void main()
{
    gl_Position = WorldViewProj * vec4(a_Position, 1.0);
 
    v_Color = vec4( a_Colour, 1.0 );
}