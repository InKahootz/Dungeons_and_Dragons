using Interfaces;
using System;
using System.ServiceModel;

namespace DnDClient
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, InstanceContextMode = InstanceContextMode.PerSession, UseSynchronizationContext = false)]
    class ClientImplementation : IClient
    {
        private ClientMainWindow _mw;

        public ClientImplementation(ClientMainWindow window) {
            _mw = window;
        }

        public void ReceiveMessage(String message) {
            _mw.receiveMessage(message);
        }

        public void Refresh() {
            _mw.refreshPlayer();
        }

        public void Disconnect() {
            _mw.error("Serwer przestał odpowiadać.");
        }
    }
}
