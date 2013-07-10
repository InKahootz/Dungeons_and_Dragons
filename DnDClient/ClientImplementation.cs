using Interfaces;
using System;
using System.ServiceModel;

namespace DnDClient
{
    [ServiceBehavior(ConcurrencyMode=ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    class ClientImplementation : IClient
    {
        private ClientMainWindow _mw;

        public ClientImplementation(ClientMainWindow window) {
            _mw = window;
        }

        public void ReceiveMessage(String name, String message) {
            _mw.receiveMessage(name, message);
        }

        public void Refresh() {
            _mw.refreshPlayer();
        }

        public void Disconnect() {
            _mw.error("Serwer przestał odpowiadać.");
        }
    }
}
