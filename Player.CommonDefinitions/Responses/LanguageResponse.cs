using Connect4Sports.Coach.API.CommonDefinitions.Records;
using Reservation.CommonDefinitions.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.CommonDefinitions.Responses
{
	public class LanguageResponse : BaseResponse
	{
		public List<languageRecord> languageRecords { get; set; }
	}
}
