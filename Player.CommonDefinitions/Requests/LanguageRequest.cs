using Connect4Sports.Coach.API.CommonDefinitions.Records;
using Reservation.CommonDefinitions.MysqlRequests;
using Reservation.CommonDefinitions.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.CommonDefinitions.Requests
{
	public class LanguageRequest : MysqlBaseRequest
	{
		public languageRecord languageRecord { get; set; }
	}
}
