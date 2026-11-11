using Godot;

namespace GA.Ships
{
	public enum ProcessMode
	{
		None,
		Frame,
		PhysicsFrame,
	}
	public abstract partial class AIActionBase : Resource
	{
		[Export] public ProcessMode ProcessMode { get; private set; }
		public abstract void Act(EnemyShip ship, float deltaTime = 0);
	}
}