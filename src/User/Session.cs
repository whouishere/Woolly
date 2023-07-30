using System;
using System.Threading.Tasks;
using TootNet;

namespace Woolly.User
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
			Task.WhenAll(authTask);

			(oauth, tokens) = authTask.Result;
        }

        public Session(string instance, string email, string password)
		{
			user = new LoginInfo(instance, email, password);
			var authTask = GetAuth();
			Task.WhenAll(authTask);

			try
			{
				(oauth, tokens) = authTask.Result;
			}
			catch (AggregateException e)
			{
				throw e.InnerException!;
			}
		}

		public Tokens GetAppTokens() => tokens;

		private async Task<(Authorize, Tokens)> GetAuth()
		{
			var oauth = new Authorize();
			await oauth.CreateApp(user.instance, "Woolly", Scope.Read | Scope.Write, "https://codeberg.org/whou/Woolly");
			var tokens = await oauth.AuthorizeWithEmail(user.email, user.password);

			return (oauth, tokens);
		}
	}
}
