using Sharprompt;
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

			return new Session(instance, email, password);
		}
	}
}
