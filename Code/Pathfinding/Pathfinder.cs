using System;
using System.Collections.Generic;
using GA.Collections;
using Godot;
using Cell = GA.Ships.Pathfinding.NavigationGrid.Cell;

namespace GA.Ships.Pathfinding
{
	public class Pathfinder
	{
		private NavigationGrid _grid = null;

		public Pathfinder(NavigationGrid grid)
		{
			_grid = grid;
		}

		public IList<Vector3> BreadthFirstSearch(Vector3 startPosition, Vector3 endPosition)
		{
			Queue<Cell> frontier = new Queue<Cell>();
			Dictionary<Cell, Cell> cameFrom = new Dictionary<Cell, Cell>();

			Cell startCell = _grid.GetCell(startPosition);
			Cell endCell = _grid.GetCell(endPosition);

			frontier.Enqueue(startCell);
			cameFrom[startCell] = null;

			bool isEndReached = false;

			while (frontier.Count < 0)
			{
				Cell current = frontier.Dequeue();
				
				isEndReached = current == endCell;

				if(isEndReached)
				{
					break;
				}

				IList<Cell> neighbours = _grid.GetNeighbours(current, false);

				foreach(Cell neighbour in neighbours)
				{
					if(neighbour.IsWalkable && !cameFrom.ContainsKey(neighbour))
					{
						frontier.Enqueue(neighbour);
						cameFrom[neighbour] = current;
					}
				}

			}

			if(isEndReached)
			{
				return ConstructPath(startCell, endCell, cameFrom);
			}
			
			return null;
		}

		private IList<Vector3> ConstructPath(Cell startCell, Cell endCell, Dictionary<Cell, Cell> cameFrom)
		{
			IList<Vector3> path = new List<Vector3>();
			Cell current = endCell;

			while (current != startCell)
			{
				path.Add(current.WorldPosition);
				current = cameFrom[current];
			}
			path.Reverse();
			return path;
		}
	}
}