namespace Motivo.ApiModels.Login
{
	public class LoginResultApiModel
	{
		/// <summary>
		/// The authentication token used to stay authenticated through future requests
		/// </summary>
		public string Token { get; set; }

		/// <summary>
		/// The user's username
		/// </summary>
		public string Username { get; set; }
	}
}
