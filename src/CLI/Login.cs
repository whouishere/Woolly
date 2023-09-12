using Spectre.Console;
using System;
using TootNet.Exception;
using Woolly.User;

namespace Woolly.CLI
{
	public class Login
	{
		public static Session PromptLogin()
		{
			const string PromptPrefix = "[green]>[/]";
			bool wrong_credentials = false;
			Session? session = null;

			var instance = AnsiConsole.Ask<string>($"{PromptPrefix} Mastodon account instance:");

			do
			{
				wrong_credentials = false;
				var email = AnsiConsole.Ask<string>($"{PromptPrefix} Account e-mail:");
				var password = AnsiConsole.Prompt(
					new TextPrompt<string>($"{PromptPrefix} Account password:")
						.Secret()
				);

				try
				{
					AnsiConsole.Status()
						.Spinner(Spinner.Known.Dots)
						.Start("Logging in...", _ => {
							session = new Session(instance, email, password);
						});
				}
				catch (MastodonException e)
				{
					if (e.Message == "invalid_grant")
					{
						AnsiConsole.MarkupLine("[red]Incorrect e-mail or password.[/]");
						wrong_credentials = true;
					}
					else
					{
						AnsiConsole.MarkupLine($"[red]ERROR: {e.Message}[/]");
					}

					Console.WriteLine();
				}
			}
			while (wrong_credentials);

			if (session == null)
			{
				throw new NullReferenceException("Session kept as null.");
			}

			return session;
		}
	}
}
