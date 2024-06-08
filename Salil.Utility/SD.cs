using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salil.Utility
{
    public static class SD
    {
        public const string managerRole = "Manger";
        public const string kitchenRole = "Kitchen";
        public const string frontDeskRole = "FrontDesk";
        public const string customerRole = "Customer";

		public const string statusPending = "Pending_Payment";
		public const string statusSubmitted = "Submitted_PaymentApproved";
		public const string statusRejected = "Payment_Rejected";
		public const string statusInProcess = "Being Prepared";
		public const string statusReady = "Ready for Pickup";
		public const string statusCompleted = "Completed";
		public const string statusCancelled = "Cancelled";
		public const string statusRefunded = "Refunded";
        public const string sessionCart = "sessionCart";
    }
}
