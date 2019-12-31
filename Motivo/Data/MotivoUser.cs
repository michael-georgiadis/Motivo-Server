using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Motivo.Data
{
	public class MotivoUser : IdentityUser
	{
		public List<GoalDataModel> Goals { get; set; }
	}
}
