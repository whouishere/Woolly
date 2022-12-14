using System.Threading.Tasks;
using TootNet;
using TootNet.Objects;

namespace Woolly.Client
{
	public class Toot
	{
		public static async Task<Status> PostAsync(Tokens tokens, string text)
		{
			return await tokens.Statuses.PostAsync(status => text, 
												   visibility => "public");
		}

		public static async Task<Status> ReplyAsync(Tokens tokens, long inReplyToId, string text)
		{
			return await tokens.Statuses.PostAsync(in_reply_to_id => inReplyToId, 
												   status => text, 
												   visibility => "public");
		}
	}
}
