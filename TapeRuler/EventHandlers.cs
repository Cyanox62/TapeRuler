using EXILED;
using EXILED.Extensions;
using MEC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace TapeRuler
{
	class EventHandlers
	{
		private Vector3 distStart = Vector3.zero;
		private Vector3 distEnd = Vector3.zero;

		Stopwatch stopWatch = new Stopwatch();

		private IEnumerator<float> Stopwatch(CommandSender sender)
		{
			for (int i = 3; i > 0; i--)
			{
				sender.RAMessage($"Stopwatch starting in {i} seconds...");
				yield return Timing.WaitForSeconds(1);
			}
			sender.RAMessage($"Stopwatch started!");
			stopWatch.Start();
		}

		public void OnRACommand(ref RACommandEvent ev)
		{
			string cmd = ev.Command.ToLower();
			if (cmd.StartsWith("measure"))
			{
				ev.Allow = false;

				ReferenceHub player = Player.GetPlayer(ev.Sender.SenderId);

				string[] args = cmd.Replace("measure", "").Trim().Split(' ');

				if (args.Length > 0)
				{
					if (args[0] == "dist")
					{
						if (args.Length > 1)
						{
							if (args[1] == "start")
							{
								distStart = player.transform.position;
								ev.Sender.RAMessage($"Start position saved at ({distStart.x}, {distStart.y}, {distStart.z}).");
								return;
							}
							else if (args[1] == "end")
							{
								distEnd = player.transform.position;
								ev.Sender.RAMessage($"End position saved at ({distEnd.x}, {distEnd.y}, {distEnd.z}).");
								ev.Sender.RAMessage($"Distance: {Vector3.Distance(distStart, distEnd)}.");
								return;
							}
						}
						ev.Sender.RAMessage($"USAGE: MEASURE DIST (START / END)", false);
					}
					else if (args[0] == "timer")
					{
						if (args.Length > 1)
						{
							if (args[1] == "start")
							{
								if (!stopWatch.IsRunning)
								{
									Timing.RunCoroutine(Stopwatch(ev.Sender));
									return;
								}
								else
								{
									ev.Sender.RAMessage("Stopwatch is already running.", false);
									return;
								}
							}
							else if (args[1] == "end")
							{
								stopWatch.Stop();
								ev.Sender.RAMessage("Stopwatch stopped!");
								TimeSpan ts = stopWatch.Elapsed;
								ev.Sender.RAMessage($"Elapsed time: {String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10)}.");
								stopWatch.Reset();
								return;
							}
						}
						ev.Sender.RAMessage($"USAGE: MEASURE TIMER (START / END)", false);
					}
					else
					{
						ev.Sender.RAMessage($"USAGE: MEASURE (DIST / TIMER)", false);
					}
				}
			}
		}
	}
}
