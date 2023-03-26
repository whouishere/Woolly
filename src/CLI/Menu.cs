using System;
using System.Threading.Tasks;
using TootNet.Objects;
using Sharprompt;
using Sharprompt.Fluent;
using Woolly.Client;
using Woolly.User;

namespace Woolly.CLI
{
	public class Menu
	{
		private static readonly string[] MenuItems = { "Toot", "Exit" };

		private static async Task<Status> WriteTootAsync(Session session)
		{
			// TODO: broken?
			var content = Prompt.Input<string>("Write your Toot");
			return await Toot.PostAsync(session.GetAppTokens(), content);
		}

		public static void Run(Session session)
		{
			bool isExiting = false;
			bool haveTooted = false;
			Task<Status>? status = null;

			while (!isExiting)
			{
				Console.Clear();

				if (haveTooted)
				{	
					try
					{
						if (status != null) 
						{
							Task.WaitAll(status);
							var post = status.Result;
							Console.WriteLine($"Toot posted at {post.Url}.");
							haveTooted = false;
						} 
						else 
						{
							throw new NullReferenceException();
						}
					}
					catch
					{
						Console.WriteLine("Something went wrong. Toot again.");
					}
				}

				var choice = Prompt.Select<string>(o => o.WithMessage("What woolly you do?")
														 .WithItems(MenuItems));

				// TODO: come up with a better implementation
				// because i know a switch-case statement for 
				// every menu option will be a nightmare to maintain
				switch (choice)
				{
					case "Toot":
						status = WriteTootAsync(session);
						haveTooted = true;
						break;
					case "Exit":
						isExiting = true;
						break;
				}
			}
		}
	}
}
