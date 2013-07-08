using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Interfaces
{
    public interface IClient
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveMessage(String message);

        [OperationContract(IsOneWay = true)]
        void Disconnect();
    }
}
