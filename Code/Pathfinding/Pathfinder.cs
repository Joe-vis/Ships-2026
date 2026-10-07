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


		/// <summary>
		/// finds every cell that is reachable from the start cell within the max steps
		/// </summary>
		/// <param name="start">center cell</param>
		/// <param name="maxSteps">how many steps away from the start cell</param>
		/// <param name="includeDiagonal">should diagonals be included</param>
		/// <returns>List of reachable cells</returns>
		public IList<Cell> GetReachableCells(Cell start, int maxSteps, bool includeDiagonal = false)
		{
			if(start == null)
			{
				throw new NullReferenceException($"{nameof(start)} cannot be null.");
			}

			if(maxSteps < 0)
			{
				throw new ArgumentOutOfRangeException($"{nameof(maxSteps)} must be above 0.");
			}
			// neighbouring cells
			IList<Cell> edgeCells = _grid.GetNeighbours(start, includeDiagonal);

			// cells weve already been through
			IList<Cell> exploredCells = [start];

			for(int i = 0; i < maxSteps; i++)
			{
				if(edgeCells.Count == 0)
				{
					break;
				}
				// cells to be added to edgeCells
				IList<Cell> newEdgeCells = [];

				for(int currentCellIdx = 0; currentCellIdx < edgeCells.Count; currentCellIdx++)
				{
					// Make sure the cell isnt already explored and its walkable
					if(exploredCells.Contains(edgeCells[currentCellIdx]) || !edgeCells[currentCellIdx].IsWalkable)
					{
						continue;
					}
					exploredCells.Add(edgeCells[currentCellIdx]);

					// Add new neighbouring cells to be checked next
					foreach(Cell newCell in _grid.GetNeighbours(edgeCells[currentCellIdx], includeDiagonal))
					{
						if(!newEdgeCells.Contains(newCell))
						{
							newEdgeCells.Add(newCell);
						}
					}
				}

				// Clear cells we just checked;
				edgeCells.Clear();

				// add new cells to edgeCells
				edgeCells = (IList<Cell>)newEdgeCells;
			}

			return exploredCells;
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