using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Interfaces
{
    [DataContract]
    public class User
    {
        [DataMember]
        public String UserName { get; set; }

        [DataMember]
        public Int32 UserId { get; set; }

        [DataMember]
        public DateTime LogInTime { get; set; }

        [DataMember]
        public IClient Connection { get; set; }
    }
}
