using System;
using System.Threading.Tasks;
using Sharprompt;
using Sharprompt.Fluent;
using Woolly.Client;
using Woolly.User;

namespace Woolly.CLI
{
	public class Menu
	{
		private static readonly string[] MenuItems = { "Toot", "Exit" };

		private static Task WriteToot(Session session)
		{
			var content = Prompt.Input<string>("Write your Toot");
			return Toot.Post(session.GetAppTokens(), content);
		}

		public static async void Run(Session session)
		{
			bool isExiting = false;
			while (!isExiting)
			{
				Console.Clear();

				var choice = Prompt.Select<string>(o => o.WithMessage("What woolly you do?")
														 .WithItems(MenuItems));

				// TODO: come up with a better implementation
				// because i know a switch-case statement for 
				// every menu option will be a nightmare to maintain
				switch (choice)
				{
					case "Toot":
						await WriteToot(session);
						break;
					case "Exit":
						isExiting = true;
						break;
				}
			}
		}
	}
}
