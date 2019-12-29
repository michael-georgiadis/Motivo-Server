using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Motivo.ApiModels
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
		public string FirstName { get; set; }

		/// <summary>
		/// The user's email
		/// </summary>
		public string Email { get; set; }
	}
}
