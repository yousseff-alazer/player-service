namespace Reservation.Helpers
{
    public class RabbitMqConsts
    {
        public const string RabbitMqRootUri = "amqps://b-283c476b-82e2-4d1d-bcef-4bf16603cdfc.mq.us-east-1.amazonaws.com:5671";
        public const string RabbitMqUri = "amqps://b-283c476b-82e2-4d1d-bcef-4bf16603cdfc.mq.us-east-1.amazonaws.com:5671/paymentQueue";
        public const string UserName = "admin";
        public const string Password = "C4S-admin@2022";
        public const string RabbitMqUri_PlayerReview = "amqps://b-283c476b-82e2-4d1d-bcef-4bf16603cdfc.mq.us-east-1.amazonaws.com:5671/playerReviewQueue";
        public const string RabbitMqUri_PlayerGroup = "amqps://b-283c476b-82e2-4d1d-bcef-4bf16603cdfc.mq.us-east-1.amazonaws.com:5671/playerGroupQueue";
        public const string RabbitMqUri_PlayerChallenge = "amqps://b-283c476b-82e2-4d1d-bcef-4bf16603cdfc.mq.us-east-1.amazonaws.com:5671/playerChallengeQueue";
        public const string RabbitMqUri_PlayerReservation = "amqps://b-283c476b-82e2-4d1d-bcef-4bf16603cdfc.mq.us-east-1.amazonaws.com:5671/playerReservationQueue";
        public const string RabbitMqUri_PlayerOrder = "amqps://b-283c476b-82e2-4d1d-bcef-4bf16603cdfc.mq.us-east-1.amazonaws.com:5671/playerOrderQueue";

    }
}
