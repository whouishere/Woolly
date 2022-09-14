using System.Threading.Tasks;
using TootNet;
using Sharprompt;
using Woolly.User;

namespace Woolly.CLI
{
	public class Session
	{
		private readonly LoginInfo user;
		private Tokens tokens;
        
        private Authorize oauth;

        public Session(LoginInfo user)
        {
            this.user = user;
			var authTask = GetAuth();
			Task.WaitAll(authTask);

			(oauth, tokens) = authTask.Result;
        }

        public Session(string instance, string email, string password)
		{
			user = new LoginInfo(instance, email, password);
			var authTask = GetAuth();
			Task.WaitAll(authTask);

			(oauth, tokens) = authTask.Result;
		}

		public Tokens GetAppTokens() => tokens;

		private async Task<(Authorize, Tokens)> GetAuth()
		{
			var oauth = new Authorize();
			await oauth.CreateApp(user.instance, "Woolly", Scope.Read | Scope.Write);
			var tokens = await oauth.AuthorizeWithEmail(user.email, user.password);

			return (oauth, tokens);
		}
	}
}
