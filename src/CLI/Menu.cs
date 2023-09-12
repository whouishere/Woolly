using Spectre.Console;
using Status = TootNet.Objects.Status;
using System;
using System.Threading.Tasks;
using Woolly.Client;
using Woolly.User;

namespace Woolly.CLI
{
	public class Menu
	{
		private static readonly string[] MenuItems = { "Toot", "Multiline Toot", "Exit" };

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
							AnsiConsole.Status()
								.Spinner(Spinner.Known.Dots)
								.Start("Tooting in process...", _ => {
									Task.WaitAll(status);
								});

							Task.WaitAll(status);
							var post = status.Result;
							AnsiConsole.MarkupLine($"[green]✓[/] Toot posted at {post.Url}.");
							haveTooted = false;
						} 
						else 
						{
							throw new NullReferenceException();
						}
					}
					catch
					{
						AnsiConsole.MarkupLine("[red]Something went wrong.[/] Try tooting again.");
					}
				}

				var choice = AnsiConsole.Prompt(
					new SelectionPrompt<string>()
						.Title("What woolly you do?")
						.AddChoices(MenuItems)
				);

				switch (choice)
				{
					case "Toot":
						status = WriteTootAsync(session);
						haveTooted = true;
						break;
					case "Multiline Toot":
						status = WriteMultilineTootAsync(session);
						haveTooted = true;
						break;
					case "Exit":
						isExiting = true;
						break;
				}
			}
		}

		private static async Task<Status> WriteMultilineTootAsync(Session session)
		{
			string content = "";
			bool onEnd = false;

			AnsiConsole.Clear();
			AnsiConsole.WriteLine("Write your toot:\n");

			while (!onEnd)
			{
				string? line = AnsiConsole.Prompt(
					new TextPrompt<string>("")
						.AllowEmpty()
				);

				if (string.IsNullOrEmpty(line))
				{
					line = "\n";
				}

				// if this is the second empty line
				if (line == "\n" && content.EndsWith('\n'))
				{
					onEnd = true;
					break;
				}

				content += line;
			}

			if (string.IsNullOrEmpty(content))
			{
				return await Task.FromException<Status>(new TaskCanceledException());
			}

			return await Toot.PostAsync(session.GetAppTokens(), content);
		}

		private static async Task<Status> WriteTootAsync(Session session)
		{
			AnsiConsole.Clear();
			var content = AnsiConsole.Ask<string>("Write your toot:\n");
			return await Toot.PostAsync(session.GetAppTokens(), content);
		}
	}
}
