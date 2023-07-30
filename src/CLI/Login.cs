using Kurukuru;
using Sharprompt;
using System;
using TootNet.Exception;
using Woolly.User;

namespace Woolly.CLI
{
	public class Login
	{
		public static Session PromptLogin()
		{
			bool wrong_credentials = false;
			Session? session = null;

			var instance = Prompt.Input<string>("Mastodon account instance");

			do
			{
				wrong_credentials = false;
				var email = Prompt.Input<string>("Account e-mail");
				var password = Prompt.Password("Account password");

				try
				{
					Spinner.Start("Logging in...", spinner => {
						session = new Session(instance, email, password);
					}, Patterns.Dots);
				}
				catch (MastodonException e)
				{
					Console.ForegroundColor = ConsoleColor.Red;

					if (e.Message == "invalid_grant")
					{
						Console.WriteLine("Incorrect e-mail or password.");
						wrong_credentials = true;
					}
					else
					{
						Console.WriteLine($"ERROR: {e.Message}");
					}

					Console.ResetColor();
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
