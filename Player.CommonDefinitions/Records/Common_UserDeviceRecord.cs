using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Reservation.DAL.DB;
using System.Linq;
using Reservation.Helpers;

namespace Reservation.CommonDefinitions.Records
{
    public class Common_UserDeviceRecord
    {
       



        public long ID { get; set; }


        public string UserID { get; set; }


        public string DeviceName { get; set; }


        public string DeviceIMEI { get; set; }


        public string DeviceType { get; set; }


        public string DeviceOSVersion { get; set; }


        public string DeviceToken { get; set; }


        public string DeviceEmail { get; set; }


        public bool? EnableNotification { get; set; }


        public string AuthToken { get; set; }


        public string AuthIP { get; set; }
		
		[IgnoreDataMember]		
        public DateTime? AuthCreationDate { get; set; }
		
     

		
		[IgnoreDataMember]		
        public DateTime? AuthExpirationDate { get; set; }
		
   



        public bool IsLoggedIn { get; set; }


        public string DeviceMobileNumber { get; set; }
		
        public DateTime? LastActiveDate { get; set; }
		
      



        public string Lang { get; set; }


        public long? CreatedBy { get; set; }


        public long? ModifiedBy { get; set; }
		
        public DateTime? LastUpdateDate { get; set; }
		
        

		
		[IgnoreDataMember]		
        public DateTime CreationDate { get; set; }
		


        public bool IsDeleted { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
        public string ImageUrl { get; set; }
        public bool? IsGoogleSupport { get; set; }
    }

}
