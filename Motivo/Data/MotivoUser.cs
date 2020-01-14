using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Motivo.Data
{
	public class MotivoUser : IdentityUser
	{
		public ICollection<GoalDataModel> Goals { get; set; }
	}
}
