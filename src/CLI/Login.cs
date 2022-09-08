using System.Threading.Tasks;
using TootNet;
using Sharprompt;
using Woolly.User;

namespace Woolly.CLI
{
	public class Login
	{
		private readonly LoginInfo user;
		private Tokens? tokens;
        
        private Authorize oauth;

        public Login(LoginInfo user)
        {
            this.user = user;
            oauth = new Authorize();
			Task.WaitAll(GetAuth());
        }

        public Login(string instance, string email, string password)
		{
			this.user = new LoginInfo(instance, email, password);
			oauth = new Authorize();
			Task.WaitAll(GetAuth());
		}

        public static Login PromptLogin()
        {
			var instance = Prompt.Input<string>("Mastodon account instance");
			var email = Prompt.Input<string>("Account e-mail");
			var password = Prompt.Password("Account password");

			return new Login(instance, email, password);
        }

		public Tokens? GetAppTokens() => tokens;

		private async Task GetAuth()
		{
			await oauth.CreateApp(user.instance, "Woolly", Scope.Read | Scope.Write);
			tokens = await oauth.AuthorizeWithEmail(user.email, user.password);
		}
	}
}
