using System;
using System.Linq;
using ServiceStack;

namespace App
{
	// Custom session handles adding group membership information to our session
	public class MyServiceUserSession : AuthUserSession
	{
		public int?[] Groups { get; set; }
		public bool IsAdministrator { get; set; }

		// The int value of our UserId is converted to a string? by ServiceStack, we want an int
		public new int UserAuthId { 
			get { return base.UserAuthId == null ? 0 : int.Parse(base.UserAuthId); }
			set { base.UserAuthId = value.ToString(); }
		}


		// Helper method to convert the int[] to int?[]
		// Groups needs to allow for null in Contains method check in permissions
		// Never set a member of Groups to null
		static T?[] ConvertArray<T>(T[] array) where T : struct
		{
			T?[] nullableArray = new T?[array.Length];
			for(int i = 0; i < array.Length; i++)
				nullableArray[i] = array[i];
			return nullableArray;
		}

		public override void OnAuthenticated(IServiceBase authService, ServiceStack.Auth.IAuthSession session, ServiceStack.Auth.IAuthTokens tokens, System.Collections.Generic.Dictionary<string, string> authInfo)
		{
			// Determine UserId from the Username that is in the session
			var userId = Db.Users.Where(u => u.Username == session.UserName).Select(u => u.Id).First();

			// Determine the Group Memberships of the User using the UserId
			var groups = Db.GroupMembers.Where(g => g.UserId == userId).Select(g => g.GroupId).ToArray();

			IsAdministrator = groups.Contains(1); // Set IsAdministrator (where 1 is the Id of the Administrator Group)

			Groups = ConvertArray<int>(groups);
			base.OnAuthenticated(authService, this, tokens, authInfo);
		}
	}
}