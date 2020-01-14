using System.Collections.Generic;
using Motivo.ApiModels.Goals.Outbound;

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

		public List<T> Response { get; set; }
    }


	/// <summary>
	///  The response that doesn't need to return an object.
    /// </summary>
    public class ApiResponse
    {
        public bool Successful => ErrorMessage == null;
		public string ErrorMessage { get; set; }
    }
}
