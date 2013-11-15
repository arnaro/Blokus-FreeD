using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blokus
{
    public abstract class BlokusBasePlayer : IBlokusPlayer
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public void Initialize(int playernumber)
        {
            Id = playernumber;
        }

        public abstract BlokusMove PlayRound(IBlokusGameState gamestate);
    }

    public interface IBlokusPlayer
    {
        string Name { get; set; }
        int Id { get; set; }
        void Initialize(int playernumber);

        BlokusMove PlayRound(IBlokusGameState gamestate);
    }

}
