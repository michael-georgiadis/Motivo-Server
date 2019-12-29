using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Motivo.Data
{
	public class GoalDataModel
	{
		[Key]
		public int Id { get; set; }

		[MaxLength(256)]
		public string Name { get; set; }

		public MotivoUser User { get; set; }
	}
}
