using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Interfaces;

namespace Interfaces
{
    [ServiceContract(CallbackContract = typeof(IClient))]
    public interface IService
    {
        User[] LoggedInUsers {
            [OperationContract]
            get;
        }

        User User {
            [OperationContract]
            get;
        }

        [OperationContract]
        Boolean Login(String userName, String userPass);

        [OperationContract(IsOneWay = true)]
        void Logout();

        [OperationContract(IsOneWay = true)]
        void Disconnect();

        [OperationContract(IsOneWay = true)]
        void SendGlobalMessage(String message);
    }
}
