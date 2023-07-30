using Kurukuru;
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
							Spinner.Start("Tooting in process...", spinner => {
								Task.WaitAll(status);
								spinner.Succeed("Tooted successfully!");
							}, Patterns.Dots);

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

			while (!onEnd)
			{
				string? line = Prompt.Input<string>("");
				if (line is null)
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

			if (content == "")
			{
				return await Task.FromException<Status>(new TaskCanceledException());
			}

			return await Toot.PostAsync(session.GetAppTokens(), content);
		}

		private static async Task<Status> WriteTootAsync(Session session)
		{
			var content = Prompt.Input<string>("Write your Toot");
			return await Toot.PostAsync(session.GetAppTokens(), content);
		}
	}
}
