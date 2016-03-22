using System.Collections.Generic;

namespace Blokus
{
    public abstract class BlokusBasePlayer : IBlokusPlayer
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<BlokusMove> Moves { get; set; }
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
        List<BlokusMove> Moves {get;set;}
        BlokusMove PlayRound(IBlokusGameState gamestate);
    }

}
