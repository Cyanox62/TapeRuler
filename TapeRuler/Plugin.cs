using EXILED;

namespace TapeRuler
{
	public class Plugin : EXILED.Plugin
	{
		private EventHandlers ev;

		public override void OnEnable()
		{
			ev = new EventHandlers();

			Events.RemoteAdminCommandEvent += ev.OnRACommand;
		}

		public override void OnDisable()
		{
			Events.RemoteAdminCommandEvent -= ev.OnRACommand;

			ev = null;
		}

		public override void OnReload() { }

		public override string getName { get; } = "TapeRuler";
	}
}
