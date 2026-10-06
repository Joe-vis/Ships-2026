using Ga.Ships;
using GA.Ships.Pathfinding;
using Godot;
using System;
using System.Collections.Generic;
using static GA.Ships.Pathfinding.NavigationGrid;

public partial class PathfindingVisualizer : Node3D
{
	[Export] private NavigationGrid _grid;
	[Export] private int _maxSteps = 25;

	public override void _Ready()
	{
		ShowPathfinidingVisualizer();
	}

	private async void ShowPathfinidingVisualizer()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

		MeshInstance3D gridMesh;
		gridMesh = new MeshInstance3D();
		gridMesh.Name = "DebugGridMesh";
		AddChild(gridMesh);

		gridMesh.Visible = true;
		gridMesh.Mesh = BuildPathfindingGrid();
		gridMesh.MaterialOverride = CreateGridMaterial();
	}

	private StandardMaterial3D CreateGridMaterial()
	{
		var material = new StandardMaterial3D
		{
			ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded,
			VertexColorUseAsAlbedo = true,
			CullMode = BaseMaterial3D.CullModeEnum.Back,
			Transparency = BaseMaterial3D.TransparencyEnum.Alpha,
			AlphaAntialiasingMode = BaseMaterial3D.AlphaAntiAliasing.Off,
			DisableFog = true
		};
		return material;
	}

	private ArrayMesh BuildPathfindingGrid()
	{
		Cell startCell = _grid.GetCell(GlobalPosition);
		IList<Cell> reachableCells = Level.Current.Pathfinder.GetReachableCells(startCell, _maxSteps, false);
		
		var tool = new SurfaceTool();
		tool.Begin(Mesh.PrimitiveType.Triangles);

		foreach(Cell cell in reachableCells)
		{
			GD.Print(cell.WorldPosition);
			Color color = Colors.Violet;
			float half = _grid.CellSize * 0.4f;
			Vector3 offset = cell.WorldPosition - GlobalPosition;
			Vector3 a = new Vector3(offset.X - half, 0.02f, offset.Z - half);
			Vector3 b = new Vector3(offset.X + half, 0.02f, offset.Z - half);
			Vector3 c = new Vector3(offset.X + half, 0.02f, offset.Z + half);
			Vector3 d = new Vector3(offset.X - half, 0.02f, offset.Z + half);

			tool.SetColor(color);
			tool.AddVertex(a);
			tool.AddVertex(b);
			tool.AddVertex(c);
			tool.AddVertex(a);
			tool.AddVertex(c);
			tool.AddVertex(d);
		}

		return tool.Commit() as ArrayMesh;

	}

}
