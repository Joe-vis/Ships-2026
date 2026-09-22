# Pathfinding node cost calculations
## Costs
```c#
Impassable = -1,
Water = 1,
Currents = 2,
SwampWater = 4,
RockyWater = 8,
StormyWater = 12,
```
## Water Detection
There are several ways to detect what type of terrain (water in this case) your nodes are on.
The easiest method to set up would be either doing a collision check to see if anything is colliding with the node or
if we used physics in our game we could also raycast downwards to get the physics material from the terrain

### Collision check with shape casts
first we need to make sure all the impassable objects have collision setup and that we are using collision layers to control what we are checking.  
We need to add Area3Ds to the level to create different water zones

> #### Required Godot Nodes
> 1. ShapeCast3D for collision check   
> 2. Shape3D shape for the ShapeCast3D     
> 3. Area3D for different water areas
> 4. StaticBody3D for islands 
 


```c#
for each cell in cells do:
    Spawn ShapeCast3D at cell position;
    ShapeCast3D collide_with_areas = true;
    Force update ShapeCast3D

    cell cost = 1
    for collider in ShapeCast3D collisions:
        if collider is not Area3D:
            cell cost = -1
            break;
        if GetCostFromCollider(collider) > cell cost:
            cell cost = GetCostFromCollider(collider)
```

To get the cost from a collider we can either match the colliders collision layer to a set of values or we can define some other value for them like a custom data script that lets you select different costs from an enum.

#### Other consideration
1. You could replace the shape casts with area3ds, but that comes with its own problems
    * area3Ds can't be forced to update meaning that the collider checks would need to be done after a physics frame has passed

2. You don't need to use godots own ShapeCast3D instead you can directly query the physics server.  
    * The setup for that is slightly more complicated, but as an upside you wouldn't have to spawn new objects for every cell.

