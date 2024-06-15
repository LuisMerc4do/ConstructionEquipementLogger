using System.ComponentModel.DataAnnotations;

namespace ConstructionEquipementLogger.Models
{
	public class ConstructionEquipementModel
	{
		public int Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Range(0, Int32.MaxValue, ErrorMessage = "Value for {0} must be positive.")]
        public int Quantity { get; set; }
	}
}
