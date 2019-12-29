using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Motivo.ApiModels
{
	/// <summary>
	/// The Response of all web API calls made
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ApiResponse<T>
	{
		public bool Successful => ErrorMessage == null;

		public string ErrorMessage { get; set; }

		public T Response { get; set; }
	}
}
