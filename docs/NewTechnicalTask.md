# New Technical Task

## Maybe

- Switching object with UI.

## Intro

Create a MRTK build with 

Main Scene introduction where you can select 3 other scenes. And where you come back after trying other scenes. Incide the application in every scene you can face you're hand and see scene swithing and decide to jump back to main scene. 

It might even heav my (Or generated voice welcoming messages)

There will be 3 scene. 

1. `Scene1` uses Shapes mesh based outline generation as primary solution.
2. `Scene2` uses ShaderGraph Sobel outline solution.
3. `Scene3` same ShaderGraph but set a little bit diffrent

## Core

In every scene 

You have objects with outline `B1040` and it's movable.


There are sliders to control outline properties.

- accuracy
- thickness
- color 
- segmentation


If you get really close to the object the outline become thick.

## Scene1 : Shapes based

I found out through my work that one of the best working solutions.
But it's still needed to be fully tested specially with:

- conus & Sphere objects
- complex objects

## Scene2 : Sobel Outline based solution

I found couple of video tutorials explaining how to do that. But I still need to do it.


## Plan

- [ ] A0 Generate outline with Shapes `16:15`

- [ ] A1 Generate outline with ShaderGraph `17:00`

- [ ] A2 Scene switching build to test on Oculus `17:15`

- [ ] A3 Working HL2 ARM64 build for Randolph `18:00`


1. 🐸 Quest2 build with Shapes and Shader graph till `17:00`
2. 🐸 HL2 build with the same things till `18:00`
3. 🐸 Participate in team call at `18:30`
4. 🐸 After call have money talk with Randolph `19:00`


## URP Edge Detection

1. [Article Solution](https://alexanderameye.github.io/notes/edge-detection-outlines/)
2. [More advenced article on URP Outline](https://kyriota.com/2022/08/02/Unity_Pixelated_Art_Style_In_URP/)
3. [Tree.js solution with intresting live demo](https://omar-shehata.medium.com/how-to-render-outlines-in-webgl-8253c14724f9)


At this point I still have my finger crossed that using **URP** I will still be able to get access to the debth buffer and normals.


- [ ] A0 Generate outline with Shapes `16:30`

- [ ] A1 Generate outline with ShaderGraph `17:00`

- [ ] A2 Scene switching build to test on Oculus `17:15`

- [ ] A3 Working HL2 ARM64 build for Randolph `18:00`
