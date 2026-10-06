# Problems with diagonal pathfinding

* Cost of diagonal movement  
	*  When moving diagonally the cost of a single space should be 1,5x, otherwise the distance you could move would be higher than wanted.  
	here movement is 7 cells which matches the distance traveled.  
	<img src="./moveVertical.png" alt="move vertical" height="100">    
	here movement is also 7 cells but the distance traveled is more.  
	<img src="./moveDiagonal.png" alt="move diagonal" height="100">   
* Jumping past colliders
	* Moving diagonally allows you to go past 2 unwalkable spaces if said spaces are diagonally adjecent to eachother.  
	here the black tiles represent a wall which the pathfinding would ignore on the diagonal space.  
	<img src="./unwantedDiagonal.png" alt="unwanted diagonal" height="150">   


## What would the reachable cells look like? 
if no cost is considered and no changes are made to the current pathfinding algorithm the reachable cells from a center point would become a square.

Usually in games would assume movement of for example 20 spaces meant 20 spaces in a radius around the center not in a 20 x 20 square.