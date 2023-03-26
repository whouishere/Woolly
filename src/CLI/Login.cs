using Kurukuru;
using Sharprompt;
using System;
using Woolly.User;

namespace Woolly.CLI
{
	public class Login
	{
		public static Session PromptLogin()
		{
			var instance = Prompt.Input<string>("Mastodon account instance");
			var email = Prompt.Input<string>("Account e-mail");
			var password = Prompt.Password("Account password");
			Session? session = null;

			// TODO: handle invalid_grant Exception
			Spinner.Start("Logging in...", spinner => {
				session = new Session(instance, email, password);
			}, Patterns.Dots);
			
			if (session == null) {
				throw new NullReferenceException("Session kept as null.");
			}

			return session;
		}
	}
}
