using System;
using ServiceStack;
using ServiceStack.Auth;

namespace App
{
	class MainClass
	{
		public static void Main()
		{
			// Very simple console host
			var appHost = new AppHost(500);
			appHost.Init();
			appHost.Start("http://*:9000/");
			Console.ReadKey();
		}
	}

	public class AppHost : AppHostHttpListenerPoolBase
	{
		public AppHost(int poolSize) : base("Test Service", poolSize, typeof(DocumentService).Assembly) {}

		public override void Configure(Funq.Container container)
		{
			// Add credentials provider and our custom session
			Plugins.Add(new AuthFeature(() => new MyServiceUserSession(), new IAuthProvider[] { new CredentialsAuthProvider() }));

			// Add registered users to the system
			var userRep = new InMemoryAuthRepository();
			foreach(Model.User user in Db.Users)
				userRep.CreateUserAuth(new UserAuth { Id = user.Id, UserName = user.Username }, user.Password);
			container.Register<IUserAuthRepository>(userRep);

			SetConfig(new HostConfig { 
				DebugMode = true,
			});
		}
	}
}