namespace Woolly.User
{
	public readonly struct LoginInfo
	{
		public LoginInfo(string instance, string email, string password)
		{
			this.instance = instance;
			this.email = email;
			this.password = password;
		}

		public readonly string instance { get; init; }
		public readonly string email { get; init; }
		public readonly string password { get; init; }
	}
}
