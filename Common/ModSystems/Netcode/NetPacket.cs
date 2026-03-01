using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergia.Common.ModSystems.Netcode
{
    abstract class NetPacket : IDisposable
    {
        public int Id { get; internal set; }

        protected BinaryWriter Writer { get; private set; }

        private MemoryStream stream;

        protected NetPacket()
        {
            Id = MultiplayerSystem.GetPacket(GetType()).Id;
            Writer = new BinaryWriter(stream = new MemoryStream());
        }

        public abstract void Read(BinaryReader reader, int sender);

        public void WriteAndDispose(BinaryWriter writer)
        {
            writer.Write(stream.ToArray());

            Dispose();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            Writer?.Dispose();
            stream?.Dispose();

            Writer = null!;
            stream = null!;
        }
    }
}
