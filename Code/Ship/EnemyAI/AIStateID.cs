using Godot;

namespace GA.Ships
{
	/// <summary>
	/// Identifies an AIState.
	/// Create one .tres per state;
	/// it has no references to other resources.
	/// </summary>
	[GlobalClass]
	public partial class AIStateID : Resource
	{
		[Export(PropertyHint.MultilineText)]
		public string Description { get; private set; } = "";
	}
}