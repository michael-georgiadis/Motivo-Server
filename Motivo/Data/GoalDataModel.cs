using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Motivo.Data
{
	public class GoalDataModel
	{
		[Key]
		public int Id { get; set; }

		[MaxLength(256), Required]
		public string Title { get; set; }

		[ForeignKey("User")]
		public string UserRefId { get; set; }
		public MotivoUser User { get; set; }

		[MaxLength(256), Required]
		public int NumericGoal { get; set; }

		[MaxLength(256), Required]
		public int NumericCurrent { get; set; }

		[Required, MaxLength(20)]
		public int AddBy { get; set; }
	}
}
