using System.Threading.Tasks;
using Sharprompt;
using Sharprompt.Fluent;
using Woolly.User;

namespace Woolly.CLI
{
	public class Menu
	{
		private readonly static string[] MenuItems = { "Toot", "Exit" };

		public static void Run(Session session)
		{
			bool isExiting = false;
			while (!isExiting)
			{
				var choice = Prompt.Select<string>(o => o.WithMessage("What woolly you do?")
														.WithItems(MenuItems));

				switch (choice)
				{
					case "Toot":
						var content = Prompt.Input<string>("Write your Toot");
						Task.WaitAll(Toot.Post(session.GetAppTokens(), content));
						break;
					case "Exit":
						isExiting = true;
						break;
				}
			}
		}
	}
}
